using Flow.Launcher.Plugin.Everything.Everything;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Launcher.Plugin.Everything
{
    public class Settings
    {
        public const int DefaultMaxSearchCount = 50;

        public string EditorPath { get; set; } = "";

        [Obsolete]
        internal const string Explorer = "explorer";

        [Obsolete]
        internal const string alternativeExplorerPath = "explorer.exe";

        [Obsolete]
        internal string explorerPath = Explorer;

        [Obsolete]
        public string ExplorerPath
        {
            get => explorerPath.Trim() switch
            {
                "" => Explorer,
                alternativeExplorerPath => Explorer,
                _ => explorerPath
            }; set => explorerPath = value;
        }
        [Obsolete]
        public string ExplorerArgs { get; set; } = DirectoryPathPlaceHolder;

        [Obsolete]
        internal const string DirectoryPathPlaceHolder = "%s";

        [Obsolete]
        internal const string FilePathPlaceHolder = "%f";

        [Obsolete]
        internal const string DefaultExplorerArgsWithFilePath = "/select, %f";

        public List<ContextMenu> ContextMenus = new List<ContextMenu>();

        public int MaxSearchCount { get; set; } = DefaultMaxSearchCount;

        public bool UseLocationAsWorkingDir { get; set; } = false;

        public bool LaunchHidden { get; set; } = false;

        public string EverythingInstalledPath { get; set; }

        public SortOption[] SortOptions { get; set; } = Enum.GetValues<SortOption>().Cast<SortOption>().ToArray();

        public SortOption SortOption { get; set; } = SortOption.NAME_ASCENDING;
    }

    public class ContextMenu
    {
        public string Name { get; set; }
        public string Command { get; set; }
        public string Argument { get; set; }
        public string ImagePath { get; set; }
    }
}