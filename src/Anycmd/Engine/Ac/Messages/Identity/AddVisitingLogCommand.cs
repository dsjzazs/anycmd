﻿
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Commands;
    using System;

    public class AddVisitingLogCommand : Command, IAnycmdCommand
    {
        public AddVisitingLogCommand(IUserSession userSession)
        {
            this.UserSession = userSession;
        }

        public IUserSession UserSession { get; private set; }

        /// <summary>
        /// 账户表示
        /// </summary>
        public Guid? AccountId { get; set; }
        /// <summary>
        /// Gets or sets 登录名.
        /// </summary>
        /// <value>登录名.</value>
        public string LoginName { get; set; }
        /// <summary>
        /// Gets or sets the visit on.
        /// </summary>
        /// <value>The visit on.</value>
        public DateTime VisitOn { get; set; }
        /// <summary>
        /// Gets or sets the visited on.
        /// </summary>
        /// <value>The visited on.</value>
        public DateTime? VisitedOn { get; set; }
        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        public string IpAddress { get; set; }
        /// <summary>
        /// Gets or sets the state code.
        /// </summary>
        /// <value>The state code.</value>
        public int StateCode { get; set; }
        /// <summary>
        /// Gets or sets the reason phrase.
        /// </summary>
        /// <value>The reason phrase.</value>
        public string ReasonPhrase { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
    }
}
