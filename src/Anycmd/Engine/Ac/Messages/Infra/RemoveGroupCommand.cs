﻿
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveGroupCommand : RemoveEntityCommand
    {
        public RemoveGroupCommand(IUserSession userSession, Guid groupId)
            : base(userSession, groupId)
        {

        }
    }
}
