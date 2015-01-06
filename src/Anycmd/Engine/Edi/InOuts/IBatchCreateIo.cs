﻿using System;

namespace Anycmd.Engine.Edi.InOuts
{

    public interface IBatchCreateIo : IEntityCreateInput
    {
        string Description { get; }
        bool? IncludeDescendants { get; }
        Guid NodeId { get; }
        Guid OntologyId { get; }
        string OrganizationCode { get; }
        string Title { get; }
        string Type { get; }
    }
}