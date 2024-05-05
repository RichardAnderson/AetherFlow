using AetherFlow.Xrm.Framework.Tests.Example.Interfaces;

namespace AetherFlow.Xrm.Framework.Tests.Example.Implementations
{
    public class ExampleMapper : IMapper<InstanceWithNoConstructor>
    {
        public string Serialize(InstanceWithNoConstructor record)
        {
            return "InstanceWithNoConstructor";
        }

        public InstanceWithNoConstructor Deserialize(string data)
        {
            return new InstanceWithNoConstructor();
        }
    }
}
