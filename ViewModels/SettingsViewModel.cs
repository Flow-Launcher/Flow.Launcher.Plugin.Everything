using Flow.Launcher.Plugin.Everything.Everything;
using System.Windows;

namespace Flow.Launcher.Plugin.Everything.ViewModels
{
    public class SettingsViewModel
    {
        private readonly IEverythingApi api;

        private Settings settings;
        public SettingsViewModel(IEverythingApi api, Settings settings)
        {
            this.api = api;
            this.settings = settings;
        }

        public Visibility FastSortWarningVisibility
        {
            get => api.IsFastSortOption(settings.SortOption)? Visibility.Hidden : Visibility.Visible;
        }

        public SortOption[] GetSortOptions
        {
            get => settings.SortOptions;
        }

        public SortOption SortOption
        {
            get => settings.SortOption;
            set => settings.SortOption = value;
        }
    }
}
