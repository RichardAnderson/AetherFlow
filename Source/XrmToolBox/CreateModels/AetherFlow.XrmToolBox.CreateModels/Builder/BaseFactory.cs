using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AetherFlow.XrmToolBox.CreateModels.Builder
{
    public abstract class BaseFactory
    {
        public static string Tab = "\t";
        public static string TwoTabs = $"{Tab}{Tab}";
        public static string ThreeTabs = $"{TwoTabs}{Tab}";
        public static string FourTabs = $"{TwoTabs}{TwoTabs}";
        public static string NewLine = "\r\n";

        protected IDictionary<string, string> MergeData = new Dictionary<string, string>();
        protected string Namespace = "AetherFlow.XrmToolBox.CreateModels.Presentation";

        protected BaseFactory(string defaultNamespace = null)
        {
            if (defaultNamespace != null)
                Namespace = defaultNamespace;
        }

        protected string GetTemplateContent(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = Namespace + ".builder.templates." + name + ".txt";
            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null) return "";
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        protected void AddMergeData(string key, string value)
        {
            MergeData.Add(key, value);
        }

        protected string MergeTemplate(string templateName)
        {
            var template = GetTemplateContent(templateName);
            foreach (var merge in MergeData)
                template = template.Replace("{{" + merge.Key + "}}", merge.Value);

            return template;
        }

        protected string AlphanumericOnly(string s)
        {
            return Regex.Replace(s, @"\W", "");
        }
    }
}
