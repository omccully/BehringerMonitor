using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace BehringerMonitor;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    // Import Win32 functions to manipulate windows
    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private const int SW_RESTORE = 9; // Restore window if minimized
    private Mutex? _mutex;

    protected override void OnStartup(StartupEventArgs e)
    {
        bool createdNew;
        _mutex = new Mutex(true, "BehringerMonitorMutex", out createdNew);

        if (!createdNew)
        {
            // Another instance exists — bring it to front
            BringExistingInstanceToFront();
            Shutdown(); // Exit this instance
            return;
        }

        this.DispatcherUnhandledException += OnDispatcherUnhandledException;
        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        if (_mutex != null)
        {
            _mutex.ReleaseMutex();
            _mutex.Dispose();
            _mutex = null;
        }
        base.OnExit(e);
    }


    private static void BringExistingInstanceToFront()
    {
        try
        {
            Process current = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            {
                if (process.Id != current.Id)
                {
                    IntPtr handle = process.MainWindowHandle;
                    if (handle != IntPtr.Zero)
                    {
                        ShowWindow(handle, SW_RESTORE); // Restore if minimized
                        SetForegroundWindow(handle);    // Bring to front
                    }
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Could not bring existing instance to front: " + ex.Message);
        }
    }

    private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show($"An unhandled exception occurred: {e.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
