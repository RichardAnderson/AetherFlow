using AetherFlow.Xrm.Framework.Tests.Example.Interfaces;

namespace AetherFlow.Xrm.Framework.Tests.Example.Implementations
{
    public class BoolConverter : IConverter<bool, string>
    {
        public string From(bool input) { return input.ToString(); }
        public bool To(string input) { return true; }
    }
}
