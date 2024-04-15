using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace AetherFlow.Xrm.FakeXrmEasy.Services
{
    public class InvoiceInitializerService : IEntityInitializerService
    {
        public const string EntityLogicalName = "invoice";

        public Entity Initialize(Entity e, Guid gCallerId, XrmFakedContext ctx, bool isManyToManyRelationshipEntity = false)
        {
            if (string.IsNullOrEmpty(e.GetAttributeValue<string>("invoicenumber")))
            {
                //first AetherFlow.Xrm.FakeXrmEasy auto-numbering emulation
                e["invoicenumber"] = "INV-" + DateTime.Now.Ticks;
            }

            return e;
        }

        public Entity Initialize(Entity e, XrmFakedContext ctx, bool isManyToManyRelationshipEntity = false)
        {
            return this.Initialize(e, Guid.NewGuid(), ctx, isManyToManyRelationshipEntity);
        }
    }
}
