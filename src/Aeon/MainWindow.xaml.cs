﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Aeon.DiskImages;
using Aeon.DiskImages.Archives;
using Aeon.Emulator.Dos.VirtualFileSystem;
using Aeon.Emulator.Launcher.Configuration;
using Aeon.Presentation.Dialogs;
using Microsoft.Win32;

namespace Aeon.Emulator.Launcher
{
    public sealed partial class MainWindow : Window
    {
        private PerformanceWindow performanceWindow;
        private AeonConfiguration currentConfig;
        private bool hasActivated;
        private PaletteDialog paletteWindow;

        public MainWindow() => this.InitializeComponent();

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (!this.hasActivated)
            {
                var args = App.Current.Args;

                if (args.Count > 0)
                    QuickLaunch(args[0]);

                this.hasActivated = true;
            }

            if (emulatorDisplay != null && this.WindowState != WindowState.Minimized)
                emulatorDisplay.Focus();
        }

        private void ApplyConfiguration(Configuration.AeonConfiguration config)
        {
            this.emulatorDisplay.ResetEmulator(config.PhysicalMemorySize ?? 16);

            foreach (var (letter, info) in config.Drives)
            {
                var driveLetter = ParseDriveLetter(letter);

                var vmDrive = this.emulatorDisplay.EmulatorHost.VirtualMachine.FileSystem.Drives[driveLetter];
                vmDrive.DriveType = info.Type;
                vmDrive.VolumeLabel = info.Label;
                if (info.FreeSpace != null)
                    vmDrive.FreeSpace = info.FreeSpace.GetValueOrDefault();

                if (config.Archive == null)
                {
                    if (!string.IsNullOrEmpty(info.HostPath))
                        vmDrive.Mapping = info.ReadOnly ? new MappedFolder(info.HostPath) : new WritableMappedFolder(info.HostPath);
                    else if (!string.IsNullOrEmpty(info.ImagePath))
                        vmDrive.Mapping = new ISOImage(info.ImagePath);
                    else
                        throw new FormatException();
                }
                else
                {
                    if (string.IsNullOrEmpty(info.ImagePath))
                    {
                        if (info.ReadOnly)
                        {
                            vmDrive.Mapping = new MappedArchive(driveLetter, config.Archive);
                        }
                        else
                        {
                            var rootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Aeon Emulator", "Files", config.Id, letter.ToUpperInvariant());
                            vmDrive.Mapping = new DifferencingFolder(driveLetter, config.Archive, rootPath);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                vmDrive.HasCommandInterpreter = vmDrive.DriveType == DriveType.Fixed;
            }

            this.emulatorDisplay.EmulatorHost.VirtualMachine.FileSystem.WorkingDirectory = new VirtualPath(config.StartupPath);

            var vm = this.emulatorDisplay.EmulatorHost.VirtualMachine;
            var handle = new System.Windows.Interop.WindowInteropHelper(this).Handle;

            vm.RegisterVirtualDevice(new Sound.PCSpeaker.InternalSpeaker(handle));
            vm.RegisterVirtualDevice(new Sound.Blaster.SoundBlaster(vm, handle));
            vm.RegisterVirtualDevice(new Sound.FM.FmSoundCard(handle));
            vm.RegisterVirtualDevice(new Sound.GeneralMidi());

            emulatorDisplay.EmulationSpeed = config.EmulationSpeed ?? 20_000_000;
            emulatorDisplay.MouseInputMode = config.IsMouseAbsolute ? Presentation.MouseInputMode.Absolute : Presentation.MouseInputMode.Relative;
            toolBar.Visibility = config.HideUserInterface ? Visibility.Collapsed : Visibility.Visible;
            mainMenu.Visibility = config.HideUserInterface ? Visibility.Collapsed : Visibility.Visible;
            if (!string.IsNullOrEmpty(config.Title))
                this.Title = config.Title;

            static DriveLetter ParseDriveLetter(string s)
            {
                if (string.IsNullOrEmpty(s))
                    throw new ArgumentNullException(nameof(s));
                if (s.Length != 1)
                    throw new FormatException();

                return new DriveLetter(s[0]);
            }
        }
        private void LaunchCurrentConfig()
        {
            ApplyConfiguration(this.currentConfig);
            if (!string.IsNullOrEmpty(this.currentConfig.Launch))
            {
                var launchTargets = this.currentConfig.Launch.Split(new char[] { ' ', '\t' }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (launchTargets.Length == 1)
                    this.emulatorDisplay.EmulatorHost.LoadProgram(launchTargets[0]);
                else
                    this.emulatorDisplay.EmulatorHost.LoadProgram(launchTargets[0], launchTargets[1]);
            }
            else
            {
                this.emulatorDisplay.EmulatorHost.LoadProgram("COMMAND.COM");
            }

            this.emulatorDisplay.EmulatorHost.Run();
        }
        private void QuickLaunch(string fileName)
        {
            bool hasConfig = fileName.EndsWith(".AeonConfig", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".AeonPack", StringComparison.OrdinalIgnoreCase);
            if (hasConfig)
                this.currentConfig = Configuration.AeonConfiguration.Load(fileName);
            else
                this.currentConfig = Configuration.AeonConfiguration.GetQuickLaunchConfiguration(Path.GetDirectoryName(fileName), Path.GetFileName(fileName));

            this.LaunchCurrentConfig();
        }
        private TaskDialogItem ShowTaskDialog(string title, string caption, params TaskDialogItem[] items)
        {
            var taskDialog = new TaskDialog { Owner = this, Items = items, Icon = this.Icon, Title = title, Caption = caption };
            if (taskDialog.ShowDialog() == true)
                return taskDialog.SelectedItem;
            else
                return null;
        }

        private void QuickLaunch_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "Programs (*.exe,*.com;*.AeonConfig;*.AeonPack)|*.exe;*.com;*.AeonConfig;*.AeonPack|All files (*.*)|*.*",
                Title = "Run DOS program..."
            };

            if (fileDialog.ShowDialog(this) == true)
                this.QuickLaunch(fileDialog.FileName);
        }
        private void CommandPrompt_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Presentation.Browsers.FolderBrowserDialog
            {
                Title = "Select folder for C:\\ drive..."
            };

            if (dialog.ShowDialog(this))
            {
                this.currentConfig = Configuration.AeonConfiguration.GetQuickLaunchConfiguration(dialog.Path, null);
                this.LaunchCurrentConfig();
            }
        }
        private void Close_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void Close_Executed(object sender, ExecutedRoutedEventArgs e) => this.Close();
        private void MapDrives_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this.emulatorDisplay != null)
            {
                var state = this.emulatorDisplay.EmulatorState;
                e.CanExecute = state == EmulatorState.Running || state == EmulatorState.Paused;
            }
        }
        private void EmulatorDisplay_EmulatorStateChanged(object sender, RoutedEventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
            if (this.emulatorDisplay.EmulatorState == EmulatorState.ProgramExited && this.currentConfig != null && this.currentConfig.HideUserInterface)
                Close();
        }
        private void SlowerButton_Click(object sender, RoutedEventArgs e)
        {
            if (emulatorDisplay != null)
            {
                int newSpeed = Math.Max(EmulatorHost.MinimumSpeed, emulatorDisplay.EmulationSpeed - 100_000);
                if (newSpeed != emulatorDisplay.EmulationSpeed)
                    emulatorDisplay.EmulationSpeed = newSpeed;
            }
        }
        private void FasterButton_Click(object sender, RoutedEventArgs e)
        {
            if (emulatorDisplay != null)
            {
                int newSpeed = emulatorDisplay.EmulationSpeed + 100_000;
                if (newSpeed != emulatorDisplay.EmulationSpeed)
                    emulatorDisplay.EmulationSpeed = newSpeed;
            }
        }
        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (emulatorDisplay != null && emulatorDisplay.DisplayBitmap != null)
                e.CanExecute = true;
        }
        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (emulatorDisplay != null)
            {
                var bmp = emulatorDisplay.DisplayBitmap;
                if (bmp != null)
                    Clipboard.SetImage(bmp);
            }
        }
        private void FullScreen_Executed(object sener, ExecutedRoutedEventArgs e)
        {
            if (this.WindowStyle != WindowStyle.None)
            {
                this.menuContainer.Visibility = Visibility.Collapsed;
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                this.SetCurrentValue(BackgroundProperty, Brushes.Black);
            }
            else
            {
                this.menuContainer.Visibility = Visibility.Visible;
                this.WindowState = WindowState.Normal;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.SetCurrentValue(BackgroundProperty, this.FindResource("backgroundGradient"));
            }
        }
        private void EmulatorDisplay_EmulationError(object sender, Presentation.EmulationErrorRoutedEventArgs e)
        {
            var end = new TaskDialogItem("End Program", "Terminates the current emulation session.");
            var debug = new TaskDialogItem("Debug", "View the current emulation session in the Aeon debugger.");

            var selection = ShowTaskDialog("Emulation Error", "An error occurred which caused the emulator to halt: " + e.Message + " What would you like to do?", end, debug);

            if (selection == end || selection == null)
            {
                emulatorDisplay.ResetEmulator();
            }
            else if (selection == debug)
            {
                var debuggerWindow = new DebuggerWindow { Owner = this, EmulatorHost = this.emulatorDisplay.EmulatorHost };
                debuggerWindow.Show();
                debuggerWindow.UpdateDebugger();
            }
        }
        private void EmulatorDisplay_CurrentProcessChanged(object sender, RoutedEventArgs e)
        {
            if (this.currentConfig == null || string.IsNullOrEmpty(this.currentConfig.Title))
            {
                var process = emulatorDisplay.CurrentProcess;
                if (process != null)
                    this.Title = $"{process} - Aeon";
                else
                    this.Title = "Aeon";
            }
        }
        private void PerformanceWindow_Click(object sender, RoutedEventArgs e)
        {
            if (performanceWindow != null)
                performanceWindow.Activate();
            else
            {
                performanceWindow = new PerformanceWindow();
                performanceWindow.Closed += this.PerformanceWindow_Closed;
                performanceWindow.Owner = this;
                performanceWindow.EmulatorDisplay = emulatorDisplay;
                performanceWindow.Show();
            }
        }
        private void PerformanceWindow_Closed(object sender, EventArgs e)
        {
            if (performanceWindow != null)
            {
                performanceWindow.Closed -= this.PerformanceWindow_Closed;
                performanceWindow = null;
            }
        }
        private void ShowDebugger_Click(object sender, RoutedEventArgs e)
        {
            var debuggerWindow = new DebuggerWindow { Owner = this, EmulatorHost = this.emulatorDisplay.EmulatorHost };
            debuggerWindow.Show();
            debuggerWindow.UpdateDebugger();
        }
        private void ShowPalette_Click(object sender, RoutedEventArgs e)
        {
            if (this.paletteWindow != null)
            {
                this.paletteWindow.Activate();
            }
            else
            {
                this.paletteWindow = new PaletteDialog { Owner = this, EmulatorDisplay = this.emulatorDisplay, Icon = this.Icon };
                this.paletteWindow.Closed += PaletteWindow_Closed;
                paletteWindow.Show();
            }
        }
        private void OpenInstructionLog_Click(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog
            {
                Filter = "Log files (*.AeonLog)|*.AeonLog|All files (*.*)|*.*",
                Title = "Open Log File..."
            };

            if (openFile.ShowDialog(this) == true)
            {
                var log = LogAccessor.Open(openFile.FileName);
                InstructionLogWindow.ShowDialog(log);
            }
        }
        private void PaletteWindow_Closed(object sender, EventArgs e)
        {
            if (this.paletteWindow != null)
            {
                this.paletteWindow.Closed -= this.PaletteWindow_Closed;
                this.paletteWindow = null;
            }
        }
    }
}