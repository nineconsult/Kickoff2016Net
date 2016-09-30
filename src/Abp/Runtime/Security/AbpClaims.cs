namespace Abp.Runtime.Security
{
    /// <summary>
    ///     Used to get ABP-specific claim type names.
    /// </summary>
    public static class AbpClaimTypes
    {
        /// <summary>
        ///     ImpersonatorTenantId
        /// </summary>
        public const string ImpersonatorTenantId = "http://www.aspnetboilerplate.com/identity/claims/impersonatorTenantId";

        /// <summary>
        ///     ImpersonatorUserId.
        /// </summary>
        public const string ImpersonatorUserId = "http://www.aspnetboilerplate.com/identity/claims/impersonatorUserId";

        /// <summary>
        ///     TenantId.
        /// </summary>
        public const string TenantId = "http://www.aspnetboilerplate.com/identity/claims/tenantId";
    }
}