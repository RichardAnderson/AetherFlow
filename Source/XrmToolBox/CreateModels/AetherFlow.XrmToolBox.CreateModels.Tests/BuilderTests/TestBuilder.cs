using System.Reflection;
using AetherFlow.XrmToolBox.CreateModels.Builder;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using NUnit.Framework;

namespace AetherFlow.XrmToolBox.CreateModels.Tests.BuilderTests
{
    [TestFixture]
    public class TestBuilder
    {
        private string _content; 

        [OneTimeSetUp]
        public void Setup()
        {
            var metadata = new DemoConnection().GetMetadata();
            var factory = new ClassFactory(
                "AetherFlow.Xrm.Core.Models", 
                "AetherFlow.Xrm.Core",
                metadata,
                "AetherFlow.XrmToolBox.CreateModels.Tests"
            );
            _content = factory.ToString();
        }

        [Test]
        public void EnsureContentHasData()
        {
            Assert.That(_content, Is.Not.Null);
            TestContext.WriteLine(_content);
        }

        [Test]
        public void GetManifestResourceNames()
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                TestContext.WriteLine(resourceName);
            }
        }
    }

    public class DemoConnection
    {
        private string url = "";
        private string clientId = "";
        private string clientSecret = "";
        public ServiceClient Client;

        public DemoConnection()
        {
            string connectionString = $"AuthType=ClientSecret;Url={url};ClientId={clientId};ClientSecret={clientSecret}";
            Client = new ServiceClient(connectionString);
        }

        public EntityMetadata GetMetadata()
        {
            var metaDataRequest = new RetrieveEntityRequest();
            metaDataRequest.LogicalName = "";
            metaDataRequest.EntityFilters = EntityFilters.All;

            // Execute the request.
            var metaDataResponse = (RetrieveEntityResponse)Client.Execute(metaDataRequest);
            return metaDataResponse.EntityMetadata;
        }
    }
}
