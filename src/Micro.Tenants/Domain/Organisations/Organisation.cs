﻿using Micro.Tenants.Domain.Memberships;
using Micro.Tenants.Domain.Projects;

namespace Micro.Tenants.Domain.Organisations;

public class Organisation : BaseEntity
{
    private Organisation(OrganisationId id, OrganisationName name)
    {
        Id = id;
        Name = name;
    }

    public static Organisation Create(OrganisationId id, OrganisationName name)
    {
        return new Organisation(id, name);
    }

    public OrganisationId Id { get; private init; }
    public OrganisationName Name { get; private set; }

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public void ChangeName(OrganisationName name)
    {
        Name = name;
    }
}