﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tradify.Identity.Domain.Entities;

namespace Tradify.Identity.Persistence.EntityTypeConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasOne(u => u.UserData)
                .WithOne(ud => ud.User)
                .HasForeignKey<UserData>(ud => ud.Id);
        }
    }
}
