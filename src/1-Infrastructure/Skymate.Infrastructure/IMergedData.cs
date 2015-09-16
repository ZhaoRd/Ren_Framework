// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMergedData.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate
{
    using System.Collections.Generic;

    /// <summary>
    /// The MergedData interface.
    /// </summary>
    public interface IMergedData
    {
        /// <summary>
        /// Gets or sets a value indicating whether merged data ignore.
        /// </summary>
        bool MergedDataIgnore { get; set; }

        /// <summary>
        /// Gets the merged data values.
        /// </summary>
        Dictionary<string, object> MergedDataValues { get; }
    }
}