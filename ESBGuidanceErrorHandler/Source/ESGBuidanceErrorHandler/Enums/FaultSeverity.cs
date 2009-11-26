#region Directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace ESBGuidanceErrorHandler.Enums
{
    /// <summary>
    /// Severity of faults in ESBG
    /// </summary>
    public enum FaultSeverity
    {
        /// <summary>
        /// Information
        /// </summary>
        Information = 0,

        /// <summary>
        /// Warning
        /// </summary>
        Warning = 1,

        /// <summary>
        /// Error
        /// </summary>
        Error = 2,

        /// <summary>
        /// Severe
        /// </summary>
        Severe = 3,

        /// <summary>
        /// Critical
        /// </summary>
        Critical = 4
    }
}
