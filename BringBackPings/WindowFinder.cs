using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BringBackPings;

internal class WindowFinder
{
    private const int SW_RESTORE = 9;

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    public void FocusWindow(string windowTitle)
    {
        var processes = Process.GetProcessesByName(windowTitle);

        if (processes.Length > 0)
        {
            var hWnd = processes[0].MainWindowHandle;

            if (hWnd != IntPtr.Zero)
            {
                // Show the window if it's minimized
                ShowWindow(hWnd, SW_RESTORE);

                // Set the window to the foreground
                SetForegroundWindow(hWnd);
            }
            else
            {
                Console.WriteLine("Main window handle not found.");
            }
        }
        else
        {
            Console.WriteLine($"Process '{windowTitle}' not found.");
        }
    }

    public bool Checkprocess(string windowTitle)
    {
        var processes = Process.GetProcessesByName(windowTitle);

        if (processes.Length > 0)
        {
            var hWnd = processes[0].MainWindowHandle;

            if (hWnd != IntPtr.Zero) return true;
        }

        return false;
    }
}