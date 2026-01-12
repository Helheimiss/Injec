#include <windows.h>


extern "C" {
    __declspec(dllexport) __cdecl
    bool InjectModuleToProcess(int PID, const wchar_t* modulePath) {
        HANDLE process = OpenProcess(PROCESS_ALL_ACCESS, true, PID);
        if (process == INVALID_HANDLE_VALUE || !process) {
            return false;
        }


        size_t modulePathLength = (wcslen(modulePath) + 1) * sizeof(wchar_t);
        void *lpBaseAddress = VirtualAllocEx(process, nullptr, modulePathLength, MEM_COMMIT, PAGE_READWRITE);
        if (!lpBaseAddress) {
            CloseHandle(process);
            return false;
        }


        if (!WriteProcessMemory(process, lpBaseAddress, modulePath, modulePathLength, nullptr)) {
            VirtualFreeEx(process, lpBaseAddress, 0, MEM_RELEASE);
            CloseHandle(process);
            return false;
        }


        HMODULE kernel32base = GetModuleHandleA("kernel32.dll");
        if (!kernel32base) {
            VirtualFreeEx(process, lpBaseAddress, 0, MEM_RELEASE);
            CloseHandle(process);
            return false;
        }


        HANDLE thread = CreateRemoteThread(process, nullptr, 0, reinterpret_cast<LPTHREAD_START_ROUTINE>(GetProcAddress(kernel32base, "LoadLibraryW")), lpBaseAddress, 0, nullptr);
        if (thread == INVALID_HANDLE_VALUE) {
            VirtualFreeEx(process, lpBaseAddress, 0, MEM_RELEASE);
            CloseHandle(process);
            return false;
        }
        
        
        WaitForSingleObject(thread, INFINITE);
	    DWORD exitCode = 0;
        GetExitCodeThread(thread, &exitCode);


        VirtualFreeEx(process, lpBaseAddress, 0, MEM_RELEASE);
        CloseHandle(thread);
        CloseHandle(process);

        return true;
    }
}