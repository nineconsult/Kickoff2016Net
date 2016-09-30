using System;

namespace Abp.AutoMapper
{
    /// <summary>
    /// Automap direction
    /// </summary>
    [Flags]
    public enum AutoMapDirection
    {
        /// <summary>
        /// Direction From
        /// </summary>
        From  =1 ,
        /// <summary>
        /// Direction To
        /// </summary>
        To = 2
    }
}