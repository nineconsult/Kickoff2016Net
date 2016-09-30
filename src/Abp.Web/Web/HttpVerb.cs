using System;

namespace Abp.Web
{
    /// <summary>
    /// Represents an HTTP verb.
    /// </summary>
    [Flags]
    public enum HttpVerb
    {
        /// <summary>
        /// GET
        /// </summary>
        Get = 1,

        /// <summary>
        /// POST
        /// </summary>
        Post = 2,

        /// <summary>
        /// PUT
        /// </summary>
        Put = 4,

        /// <summary>
        /// DELETE
        /// </summary>
        Delete = 8,

        /// <summary>
        /// OPTIONS
        /// </summary>
        Options = 16,

        /// <summary>
        /// TRACE
        /// </summary>
        Trace = 32,

        /// <summary>
        /// HEAD
        /// </summary>
        Head = 64,
    }
}