using Flow.Launcher.Plugin.Everything.Everything;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.Everything.Helper
{
    public static class SortOptionTranlationHelper
    {
        public static IPublicAPI API { get; internal set; }

        public static string GetTranslatedName(this SortOption sortOption)
        {
            const string prefix = "flowlauncher_plugin_everything_sort_by_";

            var enumName = Enum.GetName(sortOption);
            var splited = enumName.Split('_');
            var name = string.Join('_', splited[..^1]);
            var direction = splited[^1];

            return $"{API.GetTranslation(prefix + name.ToLower())} {API.GetTranslation(prefix + direction.ToLower())}";
        }
    }
}
