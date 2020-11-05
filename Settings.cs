using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Flow.Launcher.Infrastructure.Storage;

namespace Flow.Launcher.Plugin.Everything
{
    public class Settings
    {
        public const int DefaultMaxSearchCount = 50;

        public string EditorPath { get; set; } = "";

        private string explorerPath = "explorer";

        public string ExplorerPath
        {
            get => explorerPath.Trim() switch
            {
                "" => "explorer",
                "explorer.exe" => "explorer",
                _ => explorerPath
            }; set => explorerPath = value;
        }
        private string explorerArgs = "";
        public string ExplorerArgs
        {
            get => ExplorerPath switch
            {
                "explorer" => explorerArgs.Trim() switch
                {
                    "" => "/select,%f",
                    _ => explorerArgs
                },
                _ => explorerArgs
            };
            set => explorerArgs = value;
        }

        public List<ContextMenu> ContextMenus = new List<ContextMenu>();

        public int MaxSearchCount { get; set; } = DefaultMaxSearchCount;

        public bool UseLocationAsWorkingDir { get; set; } = false;
    }

    public class ContextMenu
    {
        public string Name { get; set; }
        public string Command { get; set; }
        public string Argument { get; set; }
        public string ImagePath { get; set; }
    }
}