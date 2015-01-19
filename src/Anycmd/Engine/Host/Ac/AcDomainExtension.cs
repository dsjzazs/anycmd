﻿
namespace Anycmd.Engine.Host.Ac
{
    using Engine.Ac.Messages.Identity;
    using Engine.Ac.Messages.Infra;
    using System;

    public static class AcDomainExtension
    {
        public static void RemoveAppSystem(this IAcDomain host, IUserSession userSession, Guid appSystemId)
        {
            host.Handle(new RemoveAppSystemCommand(userSession, appSystemId));
        }

        public static void RemoveAccount(this IAcDomain host, IUserSession userSession, Guid accountId)
        {
            host.Handle(new RemoveAccountCommand(userSession, accountId));
        }

        public static void RemoveButton(this IAcDomain host, IUserSession userSession, Guid buttonId)
        {
            host.Handle(new RemoveButtonCommand(userSession, buttonId));
        }

        public static void RemoveDic(this IAcDomain host, IUserSession userSession, Guid dicId)
        {
            host.Handle(new RemoveDicCommand(userSession, dicId));
        }

        public static void RemoveDicItem(this IAcDomain host, IUserSession userSession, Guid dicItemId)
        {
            host.Handle(new RemoveDicItemCommand(userSession, dicItemId));
        }

        public static void RemoveEntityType(this IAcDomain host, IUserSession userSession, Guid entityTypeId)
        {
            host.Handle(new RemoveEntityTypeCommand(userSession, entityTypeId));
        }
    }
}
