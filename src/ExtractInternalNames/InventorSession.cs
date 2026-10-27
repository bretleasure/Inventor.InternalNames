using System.Diagnostics;
using System.Runtime.InteropServices;
using Application = Inventor.Application;

namespace ExtractInternalNames;

internal sealed class InventorSession : IDisposable
{
    public Application Application { get; private set; }
    private readonly bool ownsApplication;
    private readonly OleMessageFilter messageFilter;

    private InventorSession(Application application, bool ownsApplication, OleMessageFilter messageFilter)
    {
        Application = application;
        this.ownsApplication = ownsApplication;
        this.messageFilter = messageFilter;
    }

    public static InventorSession Connect(string mode, int timeoutSeconds)
    {
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            throw new PlatformNotSupportedException("Live extraction requires Windows with Inventor installed. Use --from-catalog for offline generation.");
        }

        if (mode != "new" && Process.GetProcessesByName("Inventor").Length > 1)
        {
            throw new InvalidOperationException("Multiple Inventor processes are running. Leave one running or use --connect new to start a dedicated session.");
        }

        var filter = new OleMessageFilter(timeoutSeconds);
        try { return ConnectCore(mode, timeoutSeconds, filter); }
        catch { filter.Dispose(); throw; }
    }

    private static InventorSession ConnectCore(string mode, int timeoutSeconds, OleMessageFilter filter)
    {
        Application application = null;
        var owns = false;
        if (mode != "new")
        {
            try
            {
                Marshal.ThrowExceptionForHR(CLSIDFromProgID("Inventor.Application", out var clsid));
                GetActiveObject(ref clsid, IntPtr.Zero, out var active);
                application = (Application)active;
            }
            catch (COMException error) when (error.HResult == unchecked((int)0x800401E3))
            {
                if (mode == "existing")
                {
                    throw new InvalidOperationException("No running Inventor instance was found. Start Inventor or use --connect auto.", error);
                }
            }
            catch (COMException error) when (error.HResult == unchecked((int)0x80040154) || error.HResult == unchecked((int)0x800401F3))
            {
                throw new InvalidOperationException("Inventor is not installed or its COM registration is missing.", error);
            }
        }
        if (application == null)
        {
            var type = Type.GetTypeFromProgID("Inventor.Application");
            if (type == null)
            {
                throw new InvalidOperationException("Inventor is not installed or its COM registration is missing.");
            }

            application = (Application)Activator.CreateInstance(type);
            owns = true;
        }
        var session = new InventorSession(application, owns, filter);
        try
        {
            if (owns)
            {
                application.Visible = true;
            }

            var watch = Stopwatch.StartNew();
            while (true)
            {
                try { if (application.Ready)
                    {
                        break;
                    }
                }
                catch (COMException error) when (error.HResult == unchecked((int)0x80010001) ||
                                                error.HResult == unchecked((int)0x8001010A)) { }
                if (watch.Elapsed.TotalSeconds >= timeoutSeconds)
                {
                    throw new TimeoutException($"Inventor did not become ready within {timeoutSeconds} seconds. Close any modal dialogs and try again.");
                }

                Thread.Sleep(200);
            }
            return session;
        }
        catch
        {
            // The caller restores the message filter if connection fails.
            if (owns)
            {
                application.Quit();
            }

            throw;
        }
    }

    public void Dispose()
    {
        if (Application == null)
        {
            return;
        }

        try { if (ownsApplication)
            {
                Application.Quit();
            }
        }
        finally
        {
            Application = null;
            messageFilter.Dispose();
        }
    }

    [DllImport("ole32.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
    private static extern int CLSIDFromProgID(string progId, out Guid clsid);

    [DllImport("oleaut32.dll", PreserveSig = false)]
    private static extern void GetActiveObject(ref Guid clsid, IntPtr reserved, [MarshalAs(UnmanagedType.IUnknown)] out object active);
}
