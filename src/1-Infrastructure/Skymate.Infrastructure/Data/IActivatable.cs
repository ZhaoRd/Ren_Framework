// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IActivatable.cs" company="Skymate">
//   copyright ©  zhaord. All Right
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Data
{
    /// <summary>
    /// The Activatable interface.
    /// </summary>
    public interface IActivatable
    {
        /// <summary>
        /// Gets a value indicating whether is active.
        /// </summary>
        bool IsActive { get; }
    }
}