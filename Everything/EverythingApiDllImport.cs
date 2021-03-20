using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Flow.Launcher.Plugin.Everything.Everything
{
    public sealed class EverythingApiDllImport
    {
        [DllImport(Main.DLL, CharSet = CharSet.Unicode)]
        internal static extern int Everything_SetSearchW(string lpSearchString);

        [DllImport(Main.DLL)]
        internal static extern void Everything_SetMatchPath(bool bEnable);

        [DllImport(Main.DLL)]
        internal static extern void Everything_SetMatchCase(bool bEnable);

        [DllImport(Main.DLL)]
        internal static extern void Everything_SetMatchWholeWord(bool bEnable);

        [DllImport(Main.DLL)]
        internal static extern void Everything_SetRegex(bool bEnable);

        [DllImport(Main.DLL)]
        internal static extern void Everything_SetMax(int dwMax);

        [DllImport(Main.DLL)]
        internal static extern void Everything_SetOffset(int dwOffset);

        [DllImport(Main.DLL)]
        internal static extern bool Everything_GetMatchPath();

        [DllImport(Main.DLL)]
        internal static extern bool Everything_GetMatchCase();

        [DllImport(Main.DLL)]
        internal static extern bool Everything_GetMatchWholeWord();

        [DllImport(Main.DLL)]
        internal static extern bool Everything_GetRegex();

        [DllImport(Main.DLL)]
        internal static extern uint Everything_GetMax();

        [DllImport(Main.DLL)]
        internal static extern uint Everything_GetOffset();

        [DllImport(Main.DLL, CharSet = CharSet.Unicode)]
        internal static extern string Everything_GetSearchW();

        [DllImport(Main.DLL)]
        internal static extern EverythingApi.StateCode Everything_GetLastError();

        [DllImport(Main.DLL, CharSet = CharSet.Unicode)]
        internal static extern bool Everything_QueryW(bool bWait);

        [DllImport(Main.DLL)]
        internal static extern void Everything_SortResultsByPath();

        [DllImport(Main.DLL)]
        internal static extern int Everything_GetNumFileResults();

        [DllImport(Main.DLL)]
        internal static extern int Everything_GetNumFolderResults();

        [DllImport(Main.DLL)]
        internal static extern int Everything_GetNumResults();

        [DllImport(Main.DLL)]
        internal static extern int Everything_GetTotFileResults();

        [DllImport(Main.DLL)]
        internal static extern int Everything_GetTotFolderResults();

        [DllImport(Main.DLL)]
        internal static extern int Everything_GetTotResults();

        [DllImport(Main.DLL)]
        internal static extern bool Everything_IsVolumeResult(int nIndex);

        [DllImport(Main.DLL)]
        internal static extern bool Everything_IsFolderResult(int nIndex);

        [DllImport(Main.DLL)]
        internal static extern bool Everything_IsFileResult(int nIndex);

        [DllImport(Main.DLL, CharSet = CharSet.Unicode)]
        internal static extern void Everything_GetResultFullPathNameW(int nIndex, StringBuilder lpString, int nMaxCount);

        [DllImport(Main.DLL)]
        internal static extern void Everything_Reset();
        
        // Everything 1.4
		[DllImport(Main.DLL)]
		public static extern void Everything_SetSort(uint dwSortType);
		[DllImport(Main.DLL)]
		public static extern uint Everything_GetSort();
		[DllImport(Main.DLL)]
		public static extern uint Everything_GetResultListSort();
		[DllImport(Main.DLL)]
		public static extern void Everything_SetRequestFlags(uint dwRequestFlags);
		[DllImport(Main.DLL)]
		public static extern uint Everything_GetRequestFlags();
		[DllImport(Main.DLL)]
		public static extern uint Everything_GetResultListRequestFlags();
		[DllImport(Main.DLL, CharSet = CharSet.Unicode)]
		public static extern IntPtr Everything_GetResultExtension(uint nIndex);
		[DllImport(Main.DLL)]
		public static extern bool Everything_GetResultSize(uint nIndex, out long lpFileSize);
		[DllImport(Main.DLL)]
		public static extern bool Everything_GetResultDateCreated(uint nIndex, out long lpFileTime);
		[DllImport(Main.DLL)]
		public static extern bool Everything_GetResultDateModified(uint nIndex, out long lpFileTime);
		[DllImport(Main.DLL)]
		public static extern bool Everything_GetResultDateAccessed(uint nIndex, out long lpFileTime);
		[DllImport(Main.DLL)]
		public static extern uint Everything_GetResultAttributes(uint nIndex);
		[DllImport(Main.DLL, CharSet = CharSet.Unicode)]
		public static extern IntPtr Everything_GetResultFileListFileName(uint nIndex);
		[DllImport(Main.DLL)]
		public static extern uint Everything_GetResultRunCount(uint nIndex);
		[DllImport(Main.DLL)]
		public static extern bool Everything_GetResultDateRun(uint nIndex, out long lpFileTime);
		[DllImport(Main.DLL)]
		public static extern bool Everything_GetResultDateRecentlyChanged(uint nIndex, out long lpFileTime);
		[DllImport(Main.DLL, CharSet = CharSet.Unicode)]
		public static extern IntPtr Everything_GetResultHighlightedFileName(uint nIndex);
		[DllImport(Main.DLL, CharSet = CharSet.Unicode)]
		public static extern IntPtr Everything_GetResultHighlightedPath(uint nIndex);
		[DllImport(Main.DLL, CharSet = CharSet.Unicode)]
		public static extern IntPtr Everything_GetResultHighlightedFullPathAndFileName(uint nIndex);
		[DllImport(Main.DLL)]
		public static extern uint Everything_GetRunCountFromFileName(string lpFileName);
		[DllImport(Main.DLL)]
		public static extern bool Everything_SetRunCountFromFileName(string lpFileName, uint dwRunCount);
		[DllImport(Main.DLL)]
		public static extern uint Everything_IncRunCountFromFileName(string lpFileName);
    }
}