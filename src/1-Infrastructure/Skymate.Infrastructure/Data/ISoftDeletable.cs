// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISoftDeletable.cs" company="Skymate">
//   copyright ©  zhaord. All Right
// </copyright>
// <summary>
//   软删除
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Data
{
    /// <summary>
    /// The SoftDeletable interface.
    /// 软删除接口
    /// </summary>
    public interface ISoftDeletable
    {
        /// <summary>
        /// Gets a value indicating whether deleted.
        /// 获取是否为软删除
        /// </summary>
        bool Deleted { get; }
    }
}