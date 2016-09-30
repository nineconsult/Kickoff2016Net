using Abp.Application.Features;
using Abp.Runtime.Validation;
using Abp.UI.Inputs;

namespace Abp.TestBase.SampleApplication.ContacLists
{
    public class SampleFeatureProvider : FeatureProvider
    {
        public static class Names
        {
            public const string Contacts = "ContactManager.Contacts";
            public const string MaxContactCount = "ContactManager.MaxContactCount";
        }

        public override void SetFeatures(IFeatureDefinitionContext context)
        {
            var contacts = context.Create(Names.Contacts, "false",null,null,FeatureScopes.All,null);
            contacts.CreateChildFeature(Names.MaxContactCount, "100",null,null,FeatureScopes.All, new SingleLineStringInputType(new NumericValueValidator(1, 10000)));
        }
    }
}