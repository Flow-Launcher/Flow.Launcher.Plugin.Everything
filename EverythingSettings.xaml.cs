using System.Windows;
using System.Windows.Controls;
using Flow.Launcher.Plugin.Everything.ViewModels;
using Microsoft.Win32;

namespace Flow.Launcher.Plugin.Everything
{
    public partial class EverythingSettings : UserControl
    {
        public Settings settings { get; }

        private SettingsViewModel vm;

        public EverythingSettings(Settings settings, SettingsViewModel vm)
        {
            this.settings = settings;
            this.vm = vm;
            DataContext = vm;
            InitializeComponent();
        }

        private void View_Loaded(object sender, RoutedEventArgs re)
        {
            UseLocationAsWorkingDir.IsChecked = settings.UseLocationAsWorkingDir;

            UseLocationAsWorkingDir.Checked += (o, e) =>
            {
                settings.UseLocationAsWorkingDir = true;
            };

            UseLocationAsWorkingDir.Unchecked += (o, e) =>
            {
                settings.UseLocationAsWorkingDir = false;
            };

            LaunchHidden.IsChecked = settings.LaunchHidden;

            LaunchHidden.Checked += (o, e) =>
            {
                settings.LaunchHidden = true;
            };

            LaunchHidden.Unchecked += (o, e) =>
            {
                settings.LaunchHidden = false;
            };

            EditorPath.Content = settings.EditorPath;
        }

        private void EditorPath_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Executable File(*.exe)| *.exe";
            if (!string.IsNullOrEmpty(settings.EditorPath))
                openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(settings.EditorPath);

            if (openFileDialog.ShowDialog() == true)
            {
                settings.EditorPath = openFileDialog.FileName;
            }

            EditorPath.Content = settings.EditorPath;
        }


        private void onSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            // on load, tbFastSortWarning control will not have been loaded yet
            if (tbFastSortWarning is not null)
            {
                tbFastSortWarning.Visibility = vm.FastSortWarningVisibility;
                tbFastSortWarning.Text = vm.GetSortOptionWarningMessage;
            }
        }
    }
}
