using System;
using System.Collections.Generic;

namespace ConsoleTNKDxf
{
    internal class Settings
    {
        public Settings()
        {
        }

        public string FileExtension { get; internal set; }
        public string FilePrefix { get; internal set; }
        public string FileSuffix { get; internal set; }
        public string FileVersion { get; internal set; }
        public bool OpenFolderWhenDone { get; internal set; }
        public bool SnapshotToModelSpace { get; internal set; }
        public string SnapshotScale { get; internal set; }
        public object EmbedImages { get; internal set; }
        public bool WithoutBlocks { get; internal set; }
        public string OutputDirectory { get; internal set; }
        public bool UpdateExisting { get; internal set; }
        public string CoordinateStringToUse { get; internal set; }
        public object Rules { get; internal set; }

        internal static string GetRealPath(string outputDirectory)
        {
            throw new NotImplementedException();
        }

        internal IEnumerable<string> DataFolders()
        {
            throw new NotImplementedException();
        }

        internal (bool flag, string text) Load(string settingsFileFromInput)
        {
            throw new NotImplementedException();
        }

        internal IEnumerable<string> SettingFiles()
        {
            throw new NotImplementedException();
        }
    }
}