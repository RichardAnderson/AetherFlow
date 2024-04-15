using System.Collections.Generic;
using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;
using Microsoft.Xrm.Sdk.Messages;

namespace AetherFlow.Xrm.Framework.Core
{
    public abstract class EntityBase
    {
        private IDictionary<string, object> _changes = new Dictionary<string, object>();
        private Guid? _entityGuid;
        private readonly string _logicalName;
        private readonly IOrganizationService _service;
        private readonly IDictionary<string, object> _values = new Dictionary<string, object>();

        protected bool PreOperation = false;
        protected Entity TargetEntity = null;

        /// <summary>
        ///     Constructor used to setup an existing entity
        /// </summary>
        /// <param name="record"></param>
        /// <param name="service"></param>
        protected EntityBase(string logicalName, Entity record, IOrganizationService service)
        {
            // Process ID
            _entityGuid = record.Id;
            _logicalName = logicalName;
            _service = service;

            if (_logicalName != record.LogicalName)
                throw new Exception("An invalid entity type was provided.");

            // Process attributes
            foreach (var attribute in record.Attributes)
                _values.Add(attribute);
        }

        /// <summary>
        ///     Constructor for a new entity.
        /// </summary>
        /// <param name="logicalName"></param>
        /// <param name="service"></param>
        protected EntityBase(string logicalName, IOrganizationService service = null)
        {
            _logicalName = logicalName;
            _service = service;
        }

        /// <summary>
        ///     Constructor for existing entity - however, get no attributes!
        /// </summary>
        /// <param name="logicalName"></param>
        /// <param name="id"></param>
        /// <param name="service"></param>
        protected EntityBase(string logicalName, Guid id, IOrganizationService service)
        {
            _logicalName = logicalName;
            _entityGuid = id;
            _service = service;
        }

        public Guid? Id
        {
            get => _entityGuid;
            set => _entityGuid = value;
        }

        public object this[string attributeName]
        {
            get =>
                _values.ContainsKey(attributeName)
                    ? _values[attributeName]
                    : null;
            set
            {
                // Set the current value & changes dictionaries 
                _values[attributeName] = value;
                _changes[attributeName] = value;
            }
        }

        public bool IsDirty() => _changes.Count > 0;

        public void RegisterAsPreOperation(Entity target)
        {
            TargetEntity = target;
            PreOperation = true;
        }

        /// <summary>
        ///     Save the record - be this a Create or Update, only changes sent
        ///     will be pushed to the server.
        /// </summary>
        public void Save()
        {
            // Validate service object
            if (_service == null)
                throw new Exception("Unable to save an entity object without a service object");

            // 21.10.2015, RA:  Updated to allow for preoperation plugins
            //                  to correctly update the target entity
            if (PreOperation && TargetEntity != null)
            {
                foreach (var c in _changes)
                    TargetEntity[c.Key] = c.Value;
                return;
            }

            // Create the new Entity object
            var obj = new Entity(_logicalName);

            // Add all changes attributes;
            obj.Attributes.AddRange(_changes);

            // Check if this is an Update
            if (_entityGuid != null)
            {
                // RA:  27.11.2016  -   Ensure there are changes!
                if (_changes.Count == 0)
                    return;
                // END

                obj.Id = (Guid)_entityGuid;
                _service.Update(obj);
            }
            else
            {
                // This is an Create
                _entityGuid = _service.Create(obj);
            }
            _changes = new Dictionary<string, object>();
        }

        public void Delete()
        {
            // Check the id 
            if (_entityGuid != null)
                _service.Delete(_logicalName, (Guid)_entityGuid);
        }

        /// <summary>
        ///     Get multiple attributes on the current entity.
        /// </summary>
        /// <param name="attributes">String array of attributes</param>
        public EntityBase Get(string[] attributes)
        {
            // If we have no id, do nothing
            if (_entityGuid == null)
                return null;

            // Get the attributes;
            Entity entity;
            try
            {
                entity = _service.Retrieve(
                    _logicalName,
                    (Guid)_entityGuid,
                    new ColumnSet(attributes)
                );
            }
            catch
            {
                return null;
            }

            // Foreach attribute, add to the entity;
            foreach (var attribute in attributes)
                if (_values.ContainsKey(attribute))
                    _values[attribute] =
                        entity.Contains(attribute)
                            ? entity[attribute]
                            : null;
                else
                    _values.Add(
                        attribute,
                        entity.Contains(attribute)
                            ? entity[attribute]
                            : null
                    );

            // We have updated the entity.. 
            // Remove all changes relating to the fields updated
            var tmp = _changes;
            _changes = new Dictionary<string, object>();
            foreach (var change in tmp.Where(change => !attributes.Contains(change.Key)))
                _changes.Add(change);

            // Finally return this object
            return this;
        }

        /// <summary>
        ///     Returns a delete request for the current object
        /// </summary>
        /// <returns></returns>
        public DeleteRequest DeleteRequest()
        {
            if (_entityGuid == null)
                return null;

            // return the delete request
            return new DeleteRequest
            {
                Target = GetReference()
            };
        }

        /// <summary>
        ///     Returns an update request for the current object
        /// </summary>
        /// <returns></returns>
        public UpdateRequest UpdateRequest()
        {
            if (_entityGuid == null)
                return null;

            // RA:  27.11.2016  -   Again, ensure there are changes!
            if (_changes.Count == 0)
                return null;
            // END

            // Create the new Entity object
            var obj = new Entity(_logicalName) { Id = (Guid)_entityGuid };

            // Add all changes attributes;
            obj.Attributes.AddRange(_changes);

            // Return the update, this can then be bulk updated
            return new UpdateRequest { Target = obj };
        }

        /// <summary>
        ///     Returns a create request for the current object
        /// </summary>
        /// <returns></returns>
        public CreateRequest CreateRequest()
        {
            // Create the new Entity object
            var obj = new Entity(_logicalName);

            // Add all changes attributes;
            obj.Attributes.AddRange(_changes);

            // Add the ID
            if (_entityGuid != null)
                obj.Id = (Guid)_entityGuid;

            // Return the update, this can then be bulk updated
            return new CreateRequest { Target = obj };
        }

        public EntityReference GetReference()
        {
            if (Id == null)
                throw new Exception("Unable to get Reference of a non created entity");
            return new EntityReference(_logicalName, Id.Value);
        }

        protected T? GetOptionSetValue<T>(string attributeName) where T : struct, Enum
        {
            if (!_values.ContainsKey(attributeName))
                return null;

            var value = this[attributeName];
            if (value is OptionSetValue osv)
                return (T)Convert.ChangeType(osv.Value, typeof(T));

            return null;
        }

        protected void SetOptionSetValue<T>(string attributeName, T? value) where T : struct, Enum
        {
            var optionSetValue = value == null ? null : new OptionSetValue(Convert.ToInt32(value));
            this[attributeName] = optionSetValue;
        }

        protected void SetLookup(string attributeName, EntityReference value, string[] allowedEntities)
        {
            if (value == null)
            {
                this[attributeName] = null;
                return;
            }

            if (!allowedEntities.Contains(value.LogicalName))
                throw new Exception($"Entity {value.LogicalName} is not valid for attribute {attributeName}");
            
            this[attributeName] = value;
        }

    }
}