﻿
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class NodeOntologyCareRemovedEvent : DomainEvent
    {
        public NodeOntologyCareRemovedEvent(IUserSession userSession, NodeOntologyCareBase source) : base(userSession, source) { }
    }
}
