using Flow.Launcher.Plugin.Everything.Everything;
using System.Windows;

namespace Flow.Launcher.Plugin.Everything.ViewModels
{
    public class SettingsViewModel
    {
        private readonly IEverythingApi api;

        private readonly PluginInitContext context;

        private Settings settings;

        public SettingsViewModel(IEverythingApi api, Settings settings, PluginInitContext context)
        {
            this.api = api;
            this.settings = settings;
            this.context = context;
        }

        public Visibility FastSortWarningVisibility
        {
            get 
            {
                try
                {
                    return api.IsFastSortOption(settings.SortOption) ? Visibility.Collapsed : Visibility.Visible;
                }
                catch (IPCErrorException)
                {
                    // this error occurs if the Everything service is not running, in this instance show the warning and
                    // update the message to let user know in the settings panel.
                    return Visibility.Visible;
                }
            }
        }

        public string GetSortOptionWarningMessage
        {
            get
            {
                try
                {
                    // this method is used to determine if Everything service is running because as at Everything v1.4.1
                    // the sdk does not provide a dedicated interface to determine if it is running.
                    return api.IsFastSortOption(settings.SortOption) ? string.Empty
                        : context.API.GetTranslation("flowlauncher_plugin_everything_nonfastsort_warning");
                }
                catch (IPCErrorException)
                {
                    return context.API.GetTranslation("flowlauncher_plugin_everything_is_not_running");
                }
            }
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
