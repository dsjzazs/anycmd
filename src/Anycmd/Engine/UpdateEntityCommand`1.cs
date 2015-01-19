﻿
namespace Anycmd.Engine
{
    using Commands;
    using System;

    /// <summary>
    /// 表示表达更新给定的实体的命令的.NET类型。
    /// <remarks>.NET类型命名规则：Update'EntityType'Command。如UpdateUserCommand、UpdateRoleCommand、UpdateOrganizationCommand。</remarks>
    /// </summary>
    public abstract class UpdateEntityCommand<TEntityUpdateInput> : Command where TEntityUpdateInput : class, IEntityUpdateInput
    {
        protected UpdateEntityCommand(IUserSession userSession, TEntityUpdateInput output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            this.UserSession = userSession;
            this.Output = output;
        }

        public IUserSession UserSession { get; private set; }

        public TEntityUpdateInput Output { get; private set; }
    }
}
