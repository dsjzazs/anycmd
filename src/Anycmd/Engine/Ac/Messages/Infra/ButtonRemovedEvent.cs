﻿
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class ButtonRemovedEvent : DomainEvent
    {
        public ButtonRemovedEvent(IUserSession userSession, ButtonBase source)
            : base(userSession, source)
        {
        }
    }
}