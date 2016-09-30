using System.Collections.Generic;
using Abp.Configuration;

namespace Abp.Localization
{
    public class LocalizationSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(LocalizationSettingNames.DefaultLanguage, null, L("DefaultLanguage"), null,null,SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User,false,true,null)
            };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, AbpConsts.LocalizationSourceName);
        }
    }
}