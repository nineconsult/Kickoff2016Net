using System.Linq;
using System.Reflection;
using Abp.Runtime.Session;
using Newtonsoft.Json;

namespace Abp.Auditing
{
    public static class AuditingHelper
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="methodInfo">The info method</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="abpSession">Session</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns></returns>
        public static bool ShouldSaveAudit(MethodInfo methodInfo, IAuditingConfiguration configuration, IAbpSession abpSession, bool defaultValue)
        {
            ShouldSaveAudit(methodInfo, configuration, abpSession);
            return defaultValue;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="methodInfo">The info method</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="abpSession">Session</param>
        /// <returns></returns>
        public static bool ShouldSaveAudit(MethodInfo methodInfo, IAuditingConfiguration configuration, IAbpSession abpSession)
        {
            if (configuration == null || !configuration.IsEnabled)
            {
                return false;
            }

            if (!configuration.IsEnabledForAnonymousUsers && (abpSession == null || !abpSession.UserId.HasValue))
            {
                return false;
            }

            if (methodInfo == null)
            {
                return false;
            }

            if (!methodInfo.IsPublic)
            {
                return false;
            }

            if (methodInfo.IsDefined(typeof(AuditedAttribute)))
            {
                return true;
            }

            if (methodInfo.IsDefined(typeof(DisableAuditingAttribute)))
            {
                return false;
            }

            var classType = methodInfo.DeclaringType;
            if (classType != null)
            {
                if (classType.IsDefined(typeof(AuditedAttribute)))
                {
                    return true;
                }

                if (classType.IsDefined(typeof(DisableAuditingAttribute)))
                {
                    return false;
                }

                if (configuration.Selectors.Any(selector => selector.Predicate(classType)))
                {
                    return true;
                }
            }

            return false;
        }

        public static string Serialize(object obj)
        {
            var options = new JsonSerializerSettings
            {
                ContractResolver = new AuditingContractResolver()
            };

            return JsonConvert.SerializeObject(obj, options);
        }
    }
}