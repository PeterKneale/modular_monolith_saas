using Micro.Common.Exceptions;
using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Application;

[ExcludeFromCodeCoverage]
public class OrganisationNameInUseException(OrganisationName name) : PlatformException($"Organisation name in use {name}");