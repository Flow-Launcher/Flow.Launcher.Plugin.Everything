using System.Windows;
using System.Windows.Controls;
using Flow.Launcher.Plugin.Everything.Everything;
using Microsoft.Win32;

namespace Flow.Launcher.Plugin.Everything
{
    public partial class EverythingSettings : UserControl
    {
        public Settings _settings { get; }

        public SortOption SortOption { get; set; }
        public SortOption[] SortOptions { get; set; }

        public string SortText { get; set; } = "SortBy";

        public EverythingSettings(Settings settings)
        {
            _settings = settings;
            SortOption = settings.SortOption;
            SortOptions = settings.SortOptions;
            DataContext = this;
            InitializeComponent();
        }

        private void View_Loaded(object sender, RoutedEventArgs re)
        {
            UseLocationAsWorkingDir.IsChecked = _settings.UseLocationAsWorkingDir;

            UseLocationAsWorkingDir.Checked += (o, e) =>
            {
                _settings.UseLocationAsWorkingDir = true;
            };

            UseLocationAsWorkingDir.Unchecked += (o, e) =>
            {
                _settings.UseLocationAsWorkingDir = false;
            };

            LaunchHidden.IsChecked = _settings.LaunchHidden;

            LaunchHidden.Checked += (o, e) =>
            {
                _settings.LaunchHidden = true;
            };

            LaunchHidden.Unchecked += (o, e) =>
            {
                _settings.LaunchHidden = false;
            };

            EditorPath.Content = _settings.EditorPath;
            CustomizeExplorerBox.Text = _settings.ExplorerPath;
            CustomizeArgsBox.Text = _settings.ExplorerArgs;
        }

        private void EditorPath_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Executable File(*.exe)| *.exe";
            if (!string.IsNullOrEmpty(_settings.EditorPath))
                openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(_settings.EditorPath);

            if (openFileDialog.ShowDialog() == true)
            {
                _settings.EditorPath = openFileDialog.FileName;
            }

            EditorPath.Content = _settings.EditorPath;
        }

        private void CustomizeExplorer(object sender, TextChangedEventArgs e)
        {
            _settings.ExplorerPath = CustomizeExplorerBox.Text;
        }

        private void CustomizeExplorerArgs(object sender, TextChangedEventArgs e)
        {
            _settings.ExplorerArgs = CustomizeArgsBox.Text;
        }
    }
}
