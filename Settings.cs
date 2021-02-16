using System.Collections.Generic;

namespace Flow.Launcher.Plugin.Everything
{
    public class Settings
    {
        public const int DefaultMaxSearchCount = 50;

        public string EditorPath { get; set; } = "";

        internal const string Explorer = "explorer";

        internal const string alternativeExplorerPath = "explorer.exe";

        internal string explorerPath = Explorer;

        public string ExplorerPath
        {
            get => explorerPath.Trim() switch
            {
                "" => Explorer,
                alternativeExplorerPath => Explorer,
                _ => explorerPath
            }; set => explorerPath = value;
        }
        public string ExplorerArgs { get; set; } = DirectoryPathPlaceHolder;

        internal const string DirectoryPathPlaceHolder = "%s";

        internal const string FilePathPlaceHolder = "%f";

        internal const string DefaultExplorerArgsWithFilePath = "/select, %f";

        public List<ContextMenu> ContextMenus = new List<ContextMenu>();

        public int MaxSearchCount { get; set; } = DefaultMaxSearchCount;

        public bool UseLocationAsWorkingDir { get; set; } = false;

        public bool LaunchHidden { get; set; } = false;

        public string EverythingInstalledPath { get; set; }
    }

    public class ContextMenu
    {
        public string Name { get; set; }
        public string Command { get; set; }
        public string Argument { get; set; }
        public string ImagePath { get; set; }
    }
}