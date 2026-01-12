using System;
using System.Runtime.InteropServices;

namespace Injec
{
    internal class InjecProcess
    {
        public string ProcessName { get; }
        public int PID { get; }

        public InjecProcess(string ProcessName, int PID)
        {
            this.ProcessName = ProcessName;
            this.PID = PID;
        }


        [DllImport("libInjec.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern bool InjectModuleToProcess(int PID,  string modulePath);
    }
}
