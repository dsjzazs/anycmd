﻿
namespace Anycmd.Engine.Ac.Groups
{

    public class AddGroupCommand : AddEntityCommand<IGroupCreateIo>, IAnycmdCommand
    {
        public AddGroupCommand(IAcSession acSession, IGroupCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
