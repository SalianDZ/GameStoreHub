using GameStoreHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStoreHub.Data.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasData(GenerateUsers());
        }

        private ApplicationUser[] GenerateUsers()
        {
            ICollection<ApplicationUser> users = new HashSet<ApplicationUser>();

            ApplicationUser user;

            user = new ApplicationUser()
            {
                Id = Guid.Parse("30F0662A-29C5-48DF-8B57-9C61C671E0FB"),
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "admin@gamefinity.com",
                NormalizedUserName = "ADMIN@GAMEFINITY.COM",
                Email = "admin@gamefinity.com",
                NormalizedEmail = "ADMIN@GAMEFINITY.COM",
                PasswordHash = "AQAAAAEAACcQAAAAEP6HBNREH9Mkpk1HC/mZSdZ4K2+7X5A1FgfPtxgeuBkfuSp+GRhfwkc35x+TDUfOcg==",
            };

            users.Add(user);

            return users.ToArray();
        }
    }
}
