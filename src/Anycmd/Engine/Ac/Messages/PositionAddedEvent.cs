﻿
namespace Anycmd.Engine.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using InOuts;

    public class PositionAddedEvent : EntityAddedEvent<IPositionCreateIo>
    {
        public PositionAddedEvent(GroupBase source, IPositionCreateIo output)
            : base(source, output)
        {
        }
    }
}