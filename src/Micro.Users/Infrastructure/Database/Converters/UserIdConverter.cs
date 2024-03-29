﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Users.Infrastructure.Database.Converters;

public class UserIdConverter() : ValueConverter<UserId, Guid>(v => v.Value, v => new UserId(v));