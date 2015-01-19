﻿
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class UpdateButtonCommand : UpdateEntityCommand<IButtonUpdateIo>, IAnycmdCommand
    {
        public UpdateButtonCommand(IUserSession userSession, IButtonUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
