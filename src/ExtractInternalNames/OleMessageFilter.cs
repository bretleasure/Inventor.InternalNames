using System.Runtime.InteropServices;

namespace ExtractInternalNames;

// Inventor can temporarily reject out-of-process calls while loading documents or add-ins.
[ComVisible(true)]
internal sealed class OleMessageFilter : IOleMessageFilter, IDisposable
{
    private readonly IOleMessageFilter previous;
    private readonly int retryMilliseconds;

    public OleMessageFilter(int timeoutSeconds)
    {
        retryMilliseconds = timeoutSeconds * 1000;
        Marshal.ThrowExceptionForHR(CoRegisterMessageFilter(this, out previous));
    }

    public int HandleInComingCall(int callType, IntPtr caller, int tickCount, IntPtr interfaceInfo) => 0;
    public int RetryRejectedCall(IntPtr callee, int tickCount, int rejectType) =>
        (rejectType == 1 || rejectType == 2) && tickCount < retryMilliseconds ? 200 : -1;
    public int MessagePending(IntPtr callee, int tickCount, int pendingType) => 2;
    public void Dispose() => Marshal.ThrowExceptionForHR(CoRegisterMessageFilter(previous, out _));

    [DllImport("ole32.dll")]
    private static extern int CoRegisterMessageFilter(IOleMessageFilter filter, out IOleMessageFilter previousFilter);
}

[ComImport, Guid("00000016-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IOleMessageFilter
{
    [PreserveSig] int HandleInComingCall(int callType, IntPtr caller, int tickCount, IntPtr interfaceInfo);
    [PreserveSig] int RetryRejectedCall(IntPtr callee, int tickCount, int rejectType);
    [PreserveSig] int MessagePending(IntPtr callee, int tickCount, int pendingType);
}
