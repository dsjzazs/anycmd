﻿
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddPropertyCommand : AddEntityCommand<IPropertyCreateIo>, IAnycmdCommand
    {
        public AddPropertyCommand(IUserSession userSession, IPropertyCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
