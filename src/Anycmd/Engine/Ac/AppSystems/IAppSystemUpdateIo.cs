﻿
namespace Anycmd.Engine.Ac.AppSystems
{
    using Engine.InOuts;
    using System;

    /// <summary>
    /// 表示该接口的实现类是更新应用系统时的输入或输出参数类型。
    /// </summary>
    public interface IAppSystemUpdateIo : IEntityUpdateInput, IManagedPropertyValues
    {
        string Code { get; }
        string Description { get; }
        string Icon { get; }
        string ImageUrl { get; }
        int IsEnabled { get; }
        string Name { get; }
        Guid PrincipalId { get; }
        int SortCode { get; }
        string SsoAuthAddress { get; }
    }
}
