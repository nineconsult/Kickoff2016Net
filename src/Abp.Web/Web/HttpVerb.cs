using System;

namespace Abp.Web
{
    /// <summary>
    ///     Represents an HTTP verb.
    /// </summary>
    [Flags]
    public enum HttpVerb
    {
        /// <summary>
        ///     GET
        /// </summary>
        Get = 0,

        /// <summary>
        ///     POST
        /// </summary>
        Post = 1,

        /// <summary>
        ///     PUT
        /// </summary>
        Put = 2,

        /// <summary>
        ///     DELETE
        /// </summary>
        Delete = 4,

        /// <summary>
        ///     OPTIONS
        /// </summary>
        Options = 8,

        /// <summary>
        ///     TRACE
        /// </summary>
        Trace = 16,

        /// <summary>
        ///     HEAD
        /// </summary>
        Head = 32
    }
}