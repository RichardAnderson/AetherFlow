using System.Linq;
using Microsoft.Xrm.Sdk.Metadata;

namespace AetherFlow.XrmToolBox.CreateModels.Builder
{
    public class EntityFactory : BaseFactory
    {
        public EntityFactory(EntityMetadata metadata, string defaultNamespace = null)
            : base(defaultNamespace)
        {
            // Get attributes
            var attributes = GetAttributes(metadata);

            var labelFactory = new LabelFactory(metadata.DisplayName.LocalizedLabels, Tab, defaultNamespace);
            var fieldDefinitionFactory = new FieldDefinitionFactory(attributes, defaultNamespace);
            var fieldAttributeFactory = new FieldAttributeFactory(attributes, defaultNamespace);

            AddMergeData("ClassName", AlphanumericOnly(metadata.DisplayName.UserLocalizedLabel.Label));
            AddMergeData("LogicalName", metadata.LogicalName);
            AddMergeData("SchemaName", metadata.SchemaName);
            AddMergeData("IdAttribute", metadata.PrimaryIdAttribute);
            AddMergeData("PrimaryAttribute", metadata.PrimaryNameAttribute);
            AddMergeData("CollectionName", metadata.LogicalCollectionName);

            // Larger section replacement
            AddMergeData("ClassLabels", labelFactory.ToString());
            AddMergeData("FieldDefinitionContent", fieldDefinitionFactory.ToString());
            AddMergeData("FieldAttributeContent", fieldAttributeFactory.ToString());
        }

        public override string ToString()
        {
            return MergeTemplate("EntityTemplate").Trim();
        }

        private AttributeMetadata[] GetAttributes(EntityMetadata metadata)
        {
            return metadata.Attributes
                .Where(a => a != null)
                .Where(a => a.DisplayName != null)
                .Where(a => a.DisplayName.UserLocalizedLabel != null)
                .Where(a => a.DisplayName.UserLocalizedLabel.Label != null)
                .Where(a => a.AttributeType != AttributeTypeCode.Virtual)
                .Where(a => a.AttributeType != AttributeTypeCode.ManagedProperty)
                .Where(a => a.AttributeType != AttributeTypeCode.Uniqueidentifier)
                .OrderBy(a => a.DisplayName.UserLocalizedLabel.Label)
                .ToArray();
        }
    }
}
