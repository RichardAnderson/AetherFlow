using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk.Metadata;

namespace AetherFlow.XrmToolBox.CreateModels.Builder
{
    public class FieldDefinitionFactory : BaseFactory
    {
        private AttributeMetadata[] _metadata;

        public FieldDefinitionFactory(AttributeMetadata[] metadata, string defaultNamespace = null)
            : base(defaultNamespace)
        {
            _metadata = metadata;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var attribute in _metadata)
            {
                var template = GetTemplateContent("FieldDefinitionTemplate");
                var labels = new LabelFactory(attribute.DisplayName.LocalizedLabels, ThreeTabs, Namespace);
                template = template.Replace("{{Labels}}", labels.ToString());
                template = template.Replace("{{FieldName}}",
                    AlphanumericOnly(attribute.DisplayName.UserLocalizedLabel.Label));
                template = template.Replace("{{AttributeName}}", attribute.LogicalName);
                sb.AppendLine(template);
            }

            return sb.ToString().Trim();
        }
    }
}
