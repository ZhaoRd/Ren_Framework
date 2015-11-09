// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAudited.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved. 
// </copyright>
// <summary>
//   定义审计接口类型   .
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Domain.Entities.Auditing
{
    /// <summary>
    /// 审计接口.
    /// </summary>
    public interface IAudited : ICreationAudited, IModificationAudited
    {
    }
}
