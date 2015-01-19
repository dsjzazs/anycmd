﻿
namespace Anycmd.Ac.ViewModels.Infra.ButtonViewModels
{
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Infra;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class ButtonUpdateInput : IButtonUpdateIo
    {
        public ButtonUpdateInput()
        {
            OntologyCode = "Button";
            Verb = "Update";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string CategoryCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(1)]
        public int IsEnabled { get; set; }

        public UpdateButtonCommand ToCommand(IUserSession userSession)
        {
            return new UpdateButtonCommand(userSession, this);
        }
    }
}
