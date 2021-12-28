using Droplex;
using Flow.Launcher.Plugin.Everything.Everything;
using Flow.Launcher.Plugin.Everything.Helper;
using Flow.Launcher.Plugin.Everything.ViewModels;
using Flow.Launcher.Plugin.SharedCommands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Flow.Launcher.Plugin.Everything
{
    public class Main : IPlugin, ISettingProvider, IPluginI18n, IContextMenu
    {
        public const string DLL = "Everything.dll";
        private readonly IEverythingApi _api = new EverythingApi();

        internal static PluginInitContext _context;

        private Settings _settings;
        private CancellationTokenSource _cancellationTokenSource;

        public List<Result> Query(Query query)
        {
            _cancellationTokenSource?.Cancel(); // cancel if already exist
            var cts = _cancellationTokenSource = new CancellationTokenSource();
            var results = new List<Result>();
            if (!string.IsNullOrEmpty(query.Search))
            {
                var keyword = query.Search;

                try
                {
                    var searchList = _api.Search(keyword, cts.Token, _settings.SortOption, maxCount: _settings.MaxSearchCount);
                    if (searchList == null)
                    {
                        return results;
                    }

                    foreach (var searchResult in searchList)
                    {
                        var r = CreateResult(keyword, searchResult);
                        results.Add(r);
                    }
                }
                catch (IPCErrorException)
                {
                    results.Add(new Result
                    {
                        Title = _context.API.GetTranslation("flowlauncher_plugin_everything_is_not_running"),
                        SubTitle = _context.API.GetTranslation("flowlauncher_plugin_everything_run_service"),
                        IcoPath = "Images\\warning.png",
                        Action = _ =>
                        {
                            if (FilesFolders.FileExists(_settings.EverythingInstalledPath))
                                FilesFolders.OpenPath(_settings.EverythingInstalledPath);

                            return true;
                        }
                    });
                }
                catch (Exception e)
                {
                    _context.API.LogException("EverythingPlugin", "Query Error", e);
                    results.Add(new Result
                    {
                        Title = _context.API.GetTranslation("flowlauncher_plugin_everything_query_error"),
                        SubTitle = e.Message,
                        Action = _ =>
                        {
                            Clipboard.SetDataObject(e.Message + "\r\n" + e.StackTrace);
                            _context.API.ShowMsg(_context.API.GetTranslation("flowlauncher_plugin_everything_copied"), null, string.Empty);
                            return false;
                        },
                        IcoPath = "Images\\error.png"
                    });
                }
            }

            return results;
        }

        private Result CreateResult(string keyword, SearchResult searchResult)
        {
            var path = searchResult.FullPath;

            string workingDir = null;
            if (_settings.UseLocationAsWorkingDir)
                workingDir = Path.GetDirectoryName(path);

            var r = new Result
            {
                Title = Path.GetFileName(path),
                SubTitle = path,
                IcoPath = path,
                TitleHighlightData = _context.API.FuzzySearch(keyword, Path.GetFileName(path)).MatchData,
                Action = c =>
                {
                    bool hide;
                    try
                    {
                        switch (searchResult.Type)
                        {
                            case ResultType.Folder:
                                _context.API.OpenDirectory(path);
                                break;
                            case ResultType.Volume:
                            case ResultType.File:
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = path,
                                    UseShellExecute = true,
                                    WorkingDirectory = workingDir
                                });
                                break;
                            default:
                                break;
                        }

                        hide = true;
                    }
                    catch (Win32Exception)
                    {
                        var name = $"Plugin: {_context.CurrentPluginMetadata.Name}";
                        var message = "Can't open this file";
                        _context.API.ShowMsg(name, message, string.Empty);
                        hide = false;
                    }

                    return hide;
                },
                ContextData = searchResult,
                SubTitleHighlightData = _context.API.FuzzySearch(keyword, path).MatchData
            };
            return r;
        }

        private List<ContextMenu> GetDefaultContextMenu()
        {
            List<ContextMenu> defaultContextMenus = new List<ContextMenu>();
            ContextMenu openFolderContextMenu = new ContextMenu
            {
                Name = _context.API.GetTranslation("flowlauncher_plugin_everything_open_containing_folder"),
                Command = _settings.ExplorerPath,
                Argument = $"{_settings.ExplorerArgs}",
                ImagePath = "Images\\folder.png"
            };

            defaultContextMenus.Add(openFolderContextMenu);

            string editorPath = string.IsNullOrEmpty(_settings.EditorPath) ? "notepad.exe" : _settings.EditorPath;

            ContextMenu openWithEditorContextMenu = new ContextMenu
            {
                Name = string.Format(_context.API.GetTranslation("flowlauncher_plugin_everything_open_with_editor"), Path.GetFileNameWithoutExtension(editorPath)),
                Command = editorPath,
                Argument = $" {Settings.FilePathPlaceHolder}",
                ImagePath = editorPath
            };

            defaultContextMenus.Add(openWithEditorContextMenu);

            return defaultContextMenus;
        }

        public void Init(PluginInitContext context)
        {
            _context = context;
            _settings = context.API.LoadSettingJsonStorage<Settings>();
            SortOptionTranlationHelper.API = context.API;

            if (_settings.MaxSearchCount <= 0)
                _settings.MaxSearchCount = Settings.DefaultMaxSearchCount;

            if (!_settings.EverythingInstalledPath.FileExists())
            {
                var installedLocation = Utilities.GetInstalledPath();

                if (string.IsNullOrEmpty(installedLocation) &&
                    System.Windows.Forms.MessageBox.Show(
                        string.Format(context.API.GetTranslation("flowlauncher_plugin_everything_installing_select"), Environment.NewLine),
                        context.API.GetTranslation("flowlauncher_plugin_everything_installing_title"),
                        System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    // Solves single thread apartment (STA) mode requirement error when using OpenFileDialog
                    Thread t = new Thread(() =>
                    {
                        var dlg = new System.Windows.Forms.OpenFileDialog
                        {
                            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
                        };

                        var result = dlg.ShowDialog();
                        if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dlg.FileName))
                            installedLocation = dlg.FileName;

                    });

                    // Run your code from a thread that joins the STA Thread
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();
                    t.Join();
                }

                if (string.IsNullOrEmpty(installedLocation))
                {
                    Task.Run(async delegate
                    {
                        context.API.ShowMsg(context.API.GetTranslation("flowlauncher_plugin_everything_installing_title"),
                            context.API.GetTranslation("flowlauncher_plugin_everything_installing_subtitle"), "", useMainWindowAsOwner: false);

                        await DroplexPackage.Drop(App.Everything1_4_1_1009).ConfigureAwait(false);

                        context.API.ShowMsg(context.API.GetTranslation("flowlauncher_plugin_everything_installing_title"),
                            context.API.GetTranslation("flowlauncher_plugin_everything_installationsuccess_subtitle"), "", useMainWindowAsOwner: false);

                        _settings.EverythingInstalledPath = "C:\\Program Files\\Everything\\Everything.exe";

                        FilesFolders.OpenPath(_settings.EverythingInstalledPath);

                    }).ContinueWith(t =>
                    {
                        _context.API.LogException("Everything.Main", $"Failed to install Everything service", t.Exception.InnerException, "DroplexPackage.Drop");
                        MessageBox.Show(context.API.GetTranslation("flowlauncher_plugin_everything_installationfailed_subtitle"),
                            context.API.GetTranslation("flowlauncher_plugin_everything_installing_title"));
                    }, TaskContinuationOptions.OnlyOnFaulted);
                }
                else
                {
                    _settings.EverythingInstalledPath = installedLocation;
                }
            }

            var pluginDirectory = context.CurrentPluginMetadata.PluginDirectory;
            const string sdk = "EverythingSDK";
            var bundledSdkDirectory = Path.Combine(pluginDirectory, sdk, CpuType());

            var sdkPath = Path.Combine(bundledSdkDirectory, DLL);
            _api.Load(sdkPath);
        }

        private static string CpuType()
        {
            return Environment.Is64BitOperatingSystem ? "x64" : "x86";
        }

        public string GetTranslatedPluginTitle()
        {
            return _context.API.GetTranslation("flowlauncher_plugin_everything_plugin_name");
        }

        public string GetTranslatedPluginDescription()
        {
            return _context.API.GetTranslation("flowlauncher_plugin_everything_plugin_description");
        }

        public List<Result> LoadContextMenus(Result selectedResult)
        {
            if (selectedResult.ContextData is not SearchResult record)
                return new List<Result>();

            var icoPath = record.Type == ResultType.File ? "Images\\file.png" : "Images\\folder.png";

            List<Result> contextMenus = new List<Result>
            {
                new()
                {
                    Title = _context.API.GetTranslation("flowlauncher_plugin_everything_open_containing_folder"),
                    Action = _ =>
                    {
                        _context.API.OpenDirectory(Path.GetDirectoryName(record.FullPath), record.FullPath);
                        return true;
                    },
                    IcoPath = icoPath,
                    Glyph = new GlyphInfo("/Resources/#Segoe Fluent Icons", "\ue8de")
                }
            };

            if (record.Type is ResultType.File)
            {
                var notepadPath = string.IsNullOrEmpty(_settings.EditorPath) ? "notepad.exe" : _settings.EditorPath;
                contextMenus.Add(new Result
                {
                    Title = string.Format(
                        _context.API.GetTranslation("flowlauncher_plugin_everything_open_with_editor"),
                        Path.GetFileNameWithoutExtension(notepadPath)
                    ),
                    Action = state =>
                    {
                        using var notepad = new Process();
                        notepad.StartInfo = new ProcessStartInfo()
                        {
                            FileName = notepadPath,
                            Arguments = record.FullPath
                        };
                        if (state.SpecialKeyState.CtrlPressed && state.SpecialKeyState.ShiftPressed)
                        {
                            notepad.StartInfo.Verb = "runAs";
                        }
                        Task.Run(() => notepad.Start());
                        return true;
                    },
                    Glyph = new GlyphInfo("/Resources/#Segoe Fluent Icons", "\ue932")
                });
            }

            contextMenus.Add(
                new()
                {
                    Title = _context.API.GetTranslation("flowlauncher_plugin_everything_copy_path"),
                    Action = _ =>
                    {
                        Clipboard.SetDataObject(record.FullPath);
                        return true;
                    },
                    IcoPath = icoPath,
                    Glyph = new GlyphInfo("/Resources/#Segoe Fluent Icons", "\ue8c8")
                });
            contextMenus.Add(
                new Result
                {
                    Title = _context.API.GetTranslation("flowlauncher_plugin_everything_copy"),
                    Action = _ =>
                    {
                        Clipboard.SetFileDropList(new System.Collections.Specialized.StringCollection
                        {
                            record.FullPath
                        });
                        return true;
                    },
                    IcoPath = icoPath,
                    Glyph = new GlyphInfo("/Resources/#Segoe Fluent Icons", "\ue8c8")
                });

            if (record.Type is ResultType.File or ResultType.Folder)
                contextMenus.Add(new Result
                {
                    Title = _context.API.GetTranslation("flowlauncher_plugin_everything_delete"),
                    Action = _ =>
                    {
                        try
                        {
                            if (record.Type == ResultType.File)
                                File.Delete(record.FullPath);
                            else
                                Directory.Delete(record.FullPath);
                        }
                        catch
                        {
                            _context.API.ShowMsg(string.Format(_context.API.GetTranslation("flowlauncher_plugin_everything_canot_delete"), record.FullPath), string.Empty, string.Empty);
                            return false;
                        }

                        return true;
                    },
                    IcoPath = icoPath,
                    Glyph = new("/Resources/#Segoe Fluent Icons", "\ue74d")
                });
            return contextMenus;
        }

        public Control CreateSettingPanel()
        {
            return new EverythingSettings(_settings, new SettingsViewModel(_api, _settings, _context));
        }
    }

}
