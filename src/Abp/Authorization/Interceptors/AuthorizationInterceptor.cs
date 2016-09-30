﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Reflection;
using Abp.Threading;
using Castle.DynamicProxy;

namespace Abp.Authorization.Interceptors
{
    /// <summary>
    /// This class is used to intercept methods to make authorization if the method defined <see cref="AbpAuthorizeAttribute"/>.
    /// </summary>
    public class AuthorizationInterceptor : IInterceptor
    {
        private readonly IIocResolver _iocResolver;

        /// <summary>
        /// Constructor taking resolver
        /// </summary>
        /// <param name="iocResolver"></param>
        public AuthorizationInterceptor(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }
        /// <summary>
        /// Intercept invocation
        /// </summary>
        /// <param name="invocation"></param>
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

        private void InterceptAsync(IInvocation invocation, IEnumerable<AbpAuthorizeAttribute> authorizeAttributes)
        {
            if (invocation.Method.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = InternalAsyncHelper
                    .AwaitTaskWithPreActionAndPostActionAndFinally(
                        () =>
                        {
                            invocation.Proceed();
                            return (Task)invocation.ReturnValue;
                        },
                        preAction: () => AuthorizeAsync(authorizeAttributes)
                    );
            }
            else //Task<TResult>
            {
                invocation.ReturnValue = InternalAsyncHelper
                    .CallAwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult(
                        invocation.Method.ReturnType.GenericTypeArguments[0],
                        () =>
                        {
                            invocation.Proceed();
                            return invocation.ReturnValue;
                        },
                        preAction: async () => await AuthorizeAsync(authorizeAttributes)
                    );
            }
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

        private async Task AuthorizeAsync(IEnumerable<AbpAuthorizeAttribute> authorizeAttributes)
        {
            using (var authorizationAttributeHelper = _iocResolver.ResolveAsDisposable<IAuthorizeAttributeHelper>())
            {
                await authorizationAttributeHelper.Object.AuthorizeAsync(authorizeAttributes);
            }
        }
    }
}
