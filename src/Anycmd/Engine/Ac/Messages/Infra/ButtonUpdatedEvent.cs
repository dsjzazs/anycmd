﻿
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class ButtonUpdatedEvent : DomainEvent
    {
        public ButtonUpdatedEvent(IUserSession userSession, ButtonBase source, IButtonUpdateIo input)
            : base(userSession, source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IButtonUpdateIo Input { get; private set; }
    }
}