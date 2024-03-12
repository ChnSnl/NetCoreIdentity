using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCoreIdentity.Models.Entities;

namespace NetCoreIdentity.Models.Configuration
{
    public class AppUserRoleConfiguration : BaseConfiguration<AppUserRole>
    {
        public override void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            base.Configure(builder);
            builder.Ignore(x => x.ID);
            builder.HasKey(x => new
            {
                x.UserId,
                x.RoleId
            });

        }
    }
}
