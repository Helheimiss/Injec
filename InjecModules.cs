using System;
using System.Diagnostics;

namespace Injec
{
    internal class InjecModules
    {
        public string ModuleName { get; }
        public string FileName { get; }
        public IntPtr BaseAddres { get; }
        public int ModuleMemorySize { get; }
        public IntPtr EntryPointAddress { get; }
        public FileVersionInfo FileVersionInfo { get; }

        public InjecModules(string moduleName, string fileName, IntPtr baseAddres, int moduleMemorySize, IntPtr entryPointAddress, FileVersionInfo fileVersionInfo)
        {
            ModuleName = moduleName;
            FileName = fileName;
            BaseAddres = baseAddres;
            ModuleMemorySize = moduleMemorySize;
            EntryPointAddress = entryPointAddress;
            FileVersionInfo = fileVersionInfo;
        }
    }
}
