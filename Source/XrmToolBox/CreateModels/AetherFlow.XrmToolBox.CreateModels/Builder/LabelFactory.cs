using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.XrmToolBox.CreateModels.Builder
{
    public class LabelFactory : BaseFactory
    {
        private Dictionary<int, string> _labels;
        private string _prefix;

        public LabelFactory(LocalizedLabelCollection labels, string prefix, string defaultNamespace = null)
            : base(defaultNamespace)
        {
            _labels = labels.ToDictionary(a => a.LanguageCode, b => b.Label);
            _prefix = prefix;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var label in _labels)
            {
                var template = GetTemplateContent("LabelTemplate");
                template = template.Replace("{{Code}}", label.Key.ToString());
                template = template.Replace("{{Value}}", label.Value);
                sb.AppendLine(_prefix + template);
            }

            return sb.ToString().Trim();
        }
    }
}
