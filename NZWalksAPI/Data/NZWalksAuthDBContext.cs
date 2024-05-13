using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalksAPI.Data
{
    public class NZWalksAuthDBContext : IdentityDbContext
    {
        public NZWalksAuthDBContext(DbContextOptions<NZWalksAuthDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "c63ff6d0-4218-4919-98df-ba54d726fd20";
            var writerRoleId = "d8adddba-969f-400d-bab3-4ab0f6453356";
            var roles = new List<IdentityRole>{
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp=readerRoleId,
                    Name = "Reader",
                    NormalizedName= "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id=writerRoleId,
                    Name="Writer",
                    ConcurrencyStamp=writerRoleId,
                    NormalizedName="Writer".ToUpper()
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
