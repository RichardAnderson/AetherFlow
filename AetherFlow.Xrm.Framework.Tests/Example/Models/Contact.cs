using System;
using AetherFlow.Xrm.Framework.Core;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Xrm.Framework.Tests.Example.Models
{
    public class Contact : EntityBase
    {
        public static readonly string LogicalName = "contact";
        public static readonly string SchemaName = "Contact";
        public static readonly string IdAttribute = "contactid";
        public static readonly string PrimaryAttribute = "fullname";
        public static readonly string CollectionName = "contacts";

        public Contact(Entity record, IOrganizationService service) : base(LogicalName, record, service) { }

        public Contact(IOrganizationService service = null) : base(LogicalName, service) { }

        public Contact(Guid id, IOrganizationService service) : base(LogicalName, id, service) { }

        public static class Fields
        {
            public const string FirstName = "firstname";
        }
    }
}
