﻿using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Flow.Launcher.Infrastructure.Storage;

namespace Flow.Launcher.Plugin.Everything
{
    public class Settings
    {
        public const int DefaultMaxSearchCount = 50;

        public string EditorPath { get; set; } = "";

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