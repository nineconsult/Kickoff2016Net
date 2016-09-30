using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace Abp.Net.Mail
{
    /// <summary>
    /// Defines settings to send emails.
    /// <see cref="EmailSettingNames"/> for all available configurations.
    /// </summary>
    internal class EmailSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
                   {
                       new SettingDefinition(EmailSettingNames.Smtp.Host, "127.0.0.1", L("SmtpHost"), null,null, SettingScopes.Application | SettingScopes.Tenant,false,true,null),
                       new SettingDefinition(EmailSettingNames.Smtp.Port, "25", L("SmtpPort"), null,null, SettingScopes.Application | SettingScopes.Tenant,false,true,null),
                       new SettingDefinition(EmailSettingNames.Smtp.UserName, "", L("Username"), null,null, SettingScopes.Application | SettingScopes.Tenant,false,true,null),
                       new SettingDefinition(EmailSettingNames.Smtp.Password, "", L("Password"), null,null, SettingScopes.Application | SettingScopes.Tenant,false,true,null),
                       new SettingDefinition(EmailSettingNames.Smtp.Domain, "", L("DomainName"), null,null, SettingScopes.Application | SettingScopes.Tenant,false,true,null),
                       new SettingDefinition(EmailSettingNames.Smtp.EnableSsl, "false", L("UseSSL"), null,null, SettingScopes.Application | SettingScopes.Tenant,false,true,null),
                       new SettingDefinition(EmailSettingNames.Smtp.UseDefaultCredentials, "true", L("UseDefaultCredentials"), null,null, SettingScopes.Application | SettingScopes.Tenant,false,true,null),
                       new SettingDefinition(EmailSettingNames.DefaultFromAddress, "", L("DefaultFromSenderEmailAddress"), null,null, SettingScopes.Application | SettingScopes.Tenant,false,true,null),
                       new SettingDefinition(EmailSettingNames.DefaultFromDisplayName, "", L("DefaultFromSenderDisplayName"), null,null, SettingScopes.Application | SettingScopes.Tenant,false,true,null)
                   };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, AbpConsts.LocalizationSourceName);
        }
    }
}