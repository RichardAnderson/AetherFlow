namespace AetherFlow.Xrm.Framework.Core.Interfaces
{
    public interface ITraceConfiguration
    {
        bool ShouldLogInfo { get; set; }
        bool ShouldLogDebug { get; set; }
        bool ShouldLogError { get; set; }
        bool ShouldLogFatal { get; set; }
    }
}
