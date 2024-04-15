﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.IO.Compression;
using System.Runtime.Serialization;

#if NETFRAMEWORK
using Microsoft.Xrm.Tooling.Connector;
#elif NETSTANDARD
using Microsoft.PowerPlatform.Dataverse;
#endif

namespace AetherFlow.Xrm.FakeXrmEasy
{
    /// <summary>
    /// Reuse unit test syntax to test against a real CRM organisation
    /// It uses a real CRM organisation service instance
    /// </summary>
    public class XrmRealContext : XrmFakedContext, IXrmContext
    {
        public string ConnectionStringName { get; set; } = "fakexrmeasy-connection";

        public XrmRealContext()
        {
            //Don't setup fakes in this case.
        }

        public XrmRealContext(string connectionStringName)
        {
            ConnectionStringName = connectionStringName;
            //Don't setup fakes in this case.
        }

        public XrmRealContext(IOrganizationService organizationService)
        {
            Service = organizationService;
            //Don't setup fakes in this case.
        }

        public override IOrganizationService GetOrganizationService()
        {
            if (Service != null)
                return Service;

            Service = GetOrgService();
            return Service;
        }

        public override void Initialize(IEnumerable<Entity> entities)
        {
            //Does nothing...  otherwise it would create records in a real org db
        }

        protected IOrganizationService GetOrgService()
        {
            var connection = ConfigurationManager.ConnectionStrings[ConnectionStringName];

            // In case of missing connection string in configuration,
            // use ConnectionStringName as an explicit connection string
            var connectionString = connection == null ? ConnectionStringName : connection.ConnectionString;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("The ConnectionStringName property must be either a connection string or a connection string name");
            }

            // Connect to the CRM web service using a connection string.
#if NETFRAMEWORK
            var client = new Microsoft.Xrm.Tooling.Connector.CrmServiceClient(connectionString);
#elif NETSTANDARD
            var client = new Microsoft.PowerPlatform.Dataverse.Client.ServiceClient(connectionString);
#endif
            return client;
        }

        public XrmFakedPluginExecutionContext GetContextFromSerialisedCompressedProfile(string sCompressedProfile)
        {
            byte[] data = Convert.FromBase64String(sCompressedProfile);

            using (var memStream = new MemoryStream(data))
            {
                using (var decompressedStream = new DeflateStream(memStream, CompressionMode.Decompress, false))
                {
                    byte[] buffer = new byte[0x1000];

                    using (var tempStream = new MemoryStream())
                    {
                        int numBytesRead = decompressedStream.Read(buffer, 0, buffer.Length);
                        while (numBytesRead > 0)
                        {
                            tempStream.Write(buffer, 0, numBytesRead);
                            numBytesRead = decompressedStream.Read(buffer, 0, buffer.Length);
                        }

                        //tempStream has the decompressed plugin context now
                        var decompressedString = Encoding.UTF8.GetString(tempStream.ToArray());
                        var xlDoc = XDocument.Parse(decompressedString);

                        var contextElement = xlDoc.Descendants().Elements()
                            .Where(x => x.Name.LocalName.Equals("Context"))
                            .FirstOrDefault();

                        var pluginContextString = contextElement.Value;

                        XrmFakedPluginExecutionContext context = null;
                        using (var reader = new MemoryStream(Encoding.UTF8.GetBytes(pluginContextString)))
                        {
                            var dcSerializer = new DataContractSerializer(typeof(XrmFakedPluginExecutionContext));
                            context = (XrmFakedPluginExecutionContext)dcSerializer.ReadObject(reader);
                        }

                        return context;
                    }
                }
            }
        }
    }
}