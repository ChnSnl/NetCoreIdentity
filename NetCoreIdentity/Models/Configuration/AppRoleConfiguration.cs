using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCoreIdentity.Models.Entities;

namespace NetCoreIdentity.Models.Configuration
{
    public class AppRoleConfiguration : BaseConfiguration<AppRole>
    {
        public override void Configure(EntityTypeBuilder<AppRole> builder)
        {
            base.Configure(builder);
            builder.Ignore(x => x.ID);
            builder.HasMany(x => x.UserRoles).WithOne(x => x.Role).HasForeignKey(x => x.RoleId).IsRequired();
        }
    }
}
