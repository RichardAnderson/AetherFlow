using System;
using System.Collections.Generic;
using AetherFlow.Xml.Framework.Core.Net.Models;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Xml.Framework.Core.Net.Interfaces
{
    public interface IContactDal
    {
        List<Contact> GetAllContacts();
        Contact GetContact(Guid contactId);
        Contact FromTarget(Entity e);
    }
}