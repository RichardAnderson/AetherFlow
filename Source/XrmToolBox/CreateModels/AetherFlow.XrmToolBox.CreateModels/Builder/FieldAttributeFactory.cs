using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk.Metadata;
using System.Text;

namespace AetherFlow.XrmToolBox.CreateModels.Builder
{
    public class FieldAttributeData
    {
        public string TemplateType { get; set; }
        public string AttributeType { get; set; }
        public List<string> AllowedEntities { get; set; } = new List<string>();
        public bool IsGlobalOptionSet { get; set; } = false;
        public string Name { get; set; }

        public string GetTemplateName()
        {
            switch (TemplateType)
            {
                case "standard":
                    return "FieldStandardTemplate";
                case "lookup":
                    return "FieldLookupTemplate";
                case "Optionset":
                    return "FieldOptionsetTemplate";
                default:
                    return null;
            }
        }
    }

    public class FieldAttributeFactory : BaseFactory
    {
        private AttributeMetadata[] _metadata;

        public FieldAttributeFactory(AttributeMetadata[] attributes, string defaultNamespace)
            : base(defaultNamespace)
        {
            _metadata = attributes;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var attributes = new List<FieldAttributeData>();
            foreach (var attribute in _metadata)
            {
                var process = true;
                var attrData = new FieldAttributeData {
                    Name = AlphanumericOnly(attribute?.DisplayName?.UserLocalizedLabel?.Label)
                };

                switch (attribute?.AttributeType)
                {
                    case AttributeTypeCode.Boolean:
                        attrData.TemplateType = "standard";
                        attrData.AttributeType = "bool";
                        break;
                    case AttributeTypeCode.DateTime:
                        attrData.TemplateType = "standard";
                        attrData.AttributeType = "DateTime?";
                        break;
                    case AttributeTypeCode.BigInt:
                    case AttributeTypeCode.Integer:
                        attrData.TemplateType = "standard";
                        attrData.AttributeType = "int?";
                        break;
                    case AttributeTypeCode.String:
                    case AttributeTypeCode.Memo:
                        attrData.TemplateType = "standard";
                        attrData.AttributeType = "string";
                        break;
                    case AttributeTypeCode.Customer:
                    case AttributeTypeCode.Lookup:
                    case AttributeTypeCode.Owner:
                    case AttributeTypeCode.PartyList:
                        var customerAttr = (LookupAttributeMetadata)attribute;
                        attrData.TemplateType = "lookup";
                        attrData.AllowedEntities.AddRange(customerAttr.Targets);
                        break;
                    case AttributeTypeCode.Decimal:
                    case AttributeTypeCode.Double:
                        attrData.TemplateType = "standard";
                        attrData.AttributeType = "decimal?";
                        break;
                    case AttributeTypeCode.Money:
                        attrData.TemplateType = "standard";
                        attrData.AttributeType = "Money";
                        break;
                    case AttributeTypeCode.Picklist:
                        var osvAttribute = (PicklistAttributeMetadata)attribute;
                        attrData.TemplateType = "optionset";
                        attrData.AttributeType = AlphanumericOnly(osvAttribute.DisplayName.UserLocalizedLabel.Label);
                        attrData.IsGlobalOptionSet = osvAttribute.OptionSet.IsGlobal ?? false;
                        break;
                    case AttributeTypeCode.State:
                        var stateAttribute = (StateAttributeMetadata)attribute;
                        attrData.TemplateType = "optionset";
                        attrData.AttributeType = AlphanumericOnly(stateAttribute.DisplayName.UserLocalizedLabel.Label);
                        break;
                    case AttributeTypeCode.Status:
                        var statusAttribute = (StatusAttributeMetadata)attribute;
                        attrData.TemplateType = "optionset";
                        attrData.AttributeType = AlphanumericOnly(statusAttribute.DisplayName.UserLocalizedLabel.Label);
                        break;
                    default:
                        process = false;
                        break;
                }

                if (process) attributes.Add(attrData);
            }

            // Now loop over every attribute to process
            foreach (var attributeData in attributes)
            {
                var template = GetTemplateContent(attributeData.GetTemplateName());
                template = template.Replace("{{Name}}", attributeData.Name);
                template = template.Replace("{{Type}}", attributeData.AttributeType);
                template = template.Replace("{{Global}}", attributeData.IsGlobalOptionSet ? "Global" : "");
                template = template.Replace("{{Optionset}}", attributeData.Name + "Values");
                template = template.Replace("{{LookupTypes}}", string.Join(", ", attributeData.AllowedEntities.Select(a => "\"" + a + "\"").ToArray()));
                sb.AppendLine(template);
            }

            return sb.ToString().Trim();
        }
    }
}
