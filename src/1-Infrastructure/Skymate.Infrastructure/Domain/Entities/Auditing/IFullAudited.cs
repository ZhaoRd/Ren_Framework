// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFullAudited.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义全部审计接口类型.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Domain.Entities.Auditing
{
    /// <summary>
    /// 全部审计接口.
    /// </summary>
    public interface IFullAudited : IAudited, IDeletionAudited
    {
    }
}
