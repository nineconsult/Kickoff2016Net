using System.Collections.Generic;
using Abp.Dependency;
using Abp.Reflection;
using Castle.DynamicProxy;

namespace Abp.Authorization.Interceptors
{
    /// <summary>
    ///     This class is used to intercept methods to make authorization if the method defined
    ///     <see cref="AbpAuthorizeAttribute" />.
    /// </summary>
    public class AuthorizationInterceptor : IInterceptor
    {
        private readonly IIocResolver _iocResolver;

        public AuthorizationInterceptor(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public void Intercept(IInvocation invocation)
        {
            var authorizeAttributes =
                ReflectionHelper.GetAttributesOfMemberAndDeclaringType<AbpAuthorizeAttribute>(
                    invocation.MethodInvocationTarget
                    );

            if (authorizeAttributes.Count <= 0)
            {
                invocation.Proceed();
                return;
            }

            InterceptSync(invocation, authorizeAttributes);
        }


        private void InterceptSync(IInvocation invocation, IEnumerable<AbpAuthorizeAttribute> authorizeAttributes)
        {
            Authorize(authorizeAttributes);
            invocation.Proceed();
        }

        private void Authorize(IEnumerable<AbpAuthorizeAttribute> authorizeAttributes)
        {
            using (var authorizationAttributeHelper = _iocResolver.ResolveAsDisposable<IAuthorizeAttributeHelper>())
            {
                authorizationAttributeHelper.Object.Authorize(authorizeAttributes);
            }
        }
    }
}