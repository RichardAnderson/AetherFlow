using AetherFlow.Xrm.Framework.Core.Interfaces;

namespace AetherFlow.Xrm.Framework.Core.Configuration
{
    public class TraceConfiguration : ITraceConfiguration
    {
        public bool ShouldLogInfo { get; set; } = false;
        public bool ShouldLogDebug { get; set; } = true;
        public bool ShouldLogError { get; set; } = true;
        public bool ShouldLogFatal { get; set; } = true;
    }
}
