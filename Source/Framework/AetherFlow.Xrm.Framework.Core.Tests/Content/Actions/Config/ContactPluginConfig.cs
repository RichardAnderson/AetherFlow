using System.Runtime.Serialization;
using AetherFlow.Xrm.Framework.Core.Interfaces;

namespace AetherFlow.Xrm.Framework.Tests.Content.Actions.Config
{
    [DataContract]
    public class ContactPluginConfig : IConfiguration
    {
        [DataMember]
        public string FirstName { get; set; }
    }
}
