using Microsoft.Xrm.Sdk.Metadata;

namespace AetherFlow.XrmToolBox.CreateModels.Builder
{
    public class ClassFactory : BaseFactory
    {
        public ClassFactory(
            string primaryNamespace, 
            string aetherFlowNamespace, 
            EntityMetadata metadata,
            string currentNamespace = null
        )
            :base(currentNamespace)
        {
            var entityFactory = new EntityFactory(metadata, currentNamespace);

            AddMergeData("AetherFlowNamespace", aetherFlowNamespace);
            AddMergeData("CoreNamespace", primaryNamespace);
            AddMergeData("EntityContent", entityFactory.ToString());
        }

        public override string ToString()
        {
            return MergeTemplate("ClassTemplate").Trim();
        }
    }
}
