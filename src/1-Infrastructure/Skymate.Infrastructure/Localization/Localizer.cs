// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Localizer.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Localization
{
    /// <summary>
    /// The localizer.
    /// </summary>
    /// <param name="key">
    /// The key.
    /// </param>
    /// <param name="args">
    /// The args.
    /// </param>
    public delegate LocalizedString Localizer(string key, params object[] args);
}