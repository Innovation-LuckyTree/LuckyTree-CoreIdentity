using CoreIdentity.Domain.Entity;
using static CoreIdentity.Application.Common.Extensions.CryptographyExtensions;

namespace CoreIdentity.Persistence
{
    public static class DbSeeder
    {
        static Guid AdminUid = Guid.Parse("0dc091c5-bf9e-4ba5-a43f-67b84591afc9");
        static Guid AccountingUid = Guid.Parse("259bec6c-1243-4d4e-8351-1dc02ecbe0da");
        static Guid PresidentUid = Guid.Parse("011694bb-0d5a-4e6d-aca7-e55f67c867b6");
        static Guid LeaderUid = Guid.Parse("24c8f9f3-93a4-4e75-a9f7-4e38d922dd80");
        static Guid GroupLeaderUid = Guid.Parse("bdf8e304-1710-4f54-b8ca-7f564759ce14");
        static Guid GroupMemberUid = Guid.Parse("fdb03bef-f062-4160-b4d5-2c9206cc6ede");
        static Guid MemberUid = Guid.Parse("a6546634-971d-49d6-993e-d577e531d155");

        public static async Task SeedAsync(CoreIdentityDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            // check if role already exist then create new
            if (!context.Roles.Where(m => m.RoleName.ToLower() == "president").Any())
            {
                context.Roles.AddRange(
                    new Roles { Id = 5, RoleName = "Admin", CreatedOn = DateTimeOffset.UtcNow },
                    new Roles { Id = 6, RoleName = "Accounting", CreatedOn = DateTimeOffset.UtcNow },
                    new Roles { Id = 7, RoleName = "President", CreatedOn = DateTimeOffset.UtcNow },
                    new Roles { Id = 8, RoleName = "Leader", CreatedOn = DateTimeOffset.UtcNow },
                    new Roles { Id = 9, RoleName = "Group Leader", CreatedOn = DateTimeOffset.UtcNow },
                    new Roles { Id = 10, RoleName = "Group Member", CreatedOn = DateTimeOffset.UtcNow },
                    new Roles { Id = 11, RoleName = "Member", CreatedOn = DateTimeOffset.UtcNow }
                );
                await context.SaveChangesAsync();
            }

            // check if user already exist
            if (!context.Users.Where(m => m.MobileNumber == "09090901111").Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Id = AdminUid,
                        UserName = "09090901111",
                        Email = "admin@gmail.com",
                        MobileNumber = "09090901111",
                        Password = CreatePassword("test@123").Password,
                        PasswordSalt = CreatePassword("test@123").Salt
                    },
                    new User
                    {
                        Id = AccountingUid,
                        UserName = "09090901112",
                        Email = "acounting@gmail.com",
                        MobileNumber = "09090901112",
                        Password = CreatePassword("test@123").Password,
                        PasswordSalt = CreatePassword("test@123").Salt
                    },
                    new User
                    {
                        Id = PresidentUid,
                        UserName = "09090901113",
                        Email = "president@gmail.com",
                        MobileNumber = "09090901113",
                        Password = CreatePassword("test@123").Password,
                        PasswordSalt = CreatePassword("test@123").Salt
                    },
                    new User
                    {
                        Id = LeaderUid,
                        UserName = "09090901114",
                        Email = "leader@gmail.com",
                        MobileNumber = "09090901114",
                        Password = CreatePassword("test@123").Password,
                        PasswordSalt = CreatePassword("test@123").Salt
                    },
                    new User
                    {
                        Id = GroupLeaderUid,
                        UserName = "09090901115",
                        Email = "groupleader@gmail.com",
                        MobileNumber = "09090901115",
                        Password = CreatePassword("test@123").Password,
                        PasswordSalt = CreatePassword("test@123").Salt
                    },
                    new User
                    {
                        Id = GroupMemberUid,
                        UserName = "09090901116",
                        Email = "groupmember@gmail.com",
                        MobileNumber = "09090901116",
                        Password = CreatePassword("test@123").Password,
                        PasswordSalt = CreatePassword("test@123").Salt
                    },
                    new User
                    {
                        Id = MemberUid,
                        UserName = "09090901117",
                        Email = "member@gmail.com",
                        MobileNumber = "09090901117",
                        Password = CreatePassword("test@123").Password,
                        PasswordSalt = CreatePassword("test@123").Salt
                    }
                );

                await context.SaveChangesAsync();
            }

            // check if user role already exist
            if (!context.UserRoles.Where(m => m.RoleId == 5).Any())
            {
                context.UserRoles.AddRange(
                    new UserRoles { UserId = AdminUid, RoleId = 5 },
                    new UserRoles { UserId = AccountingUid, RoleId = 6 },
                    new UserRoles { UserId = PresidentUid, RoleId = 7 },
                    new UserRoles { UserId = LeaderUid, RoleId = 8 },
                    new UserRoles { UserId = GroupLeaderUid, RoleId = 9 },
                    new UserRoles { UserId = GroupMemberUid, RoleId = 10 },
                    new UserRoles { UserId = MemberUid, RoleId = 11 }
                );

                await context.SaveChangesAsync();
            }

            if (!context.TenantUsers.Where(m => m.UserId == PresidentUid).Any())
            {
                context.TenantUsers.AddRange(
                    new TenantUser { UserId = AdminUid, TenantId = Guid.Parse("53df1aac-f936-4cd3-a138-652d51221394") },
                    new TenantUser { UserId = AccountingUid, TenantId = Guid.Parse("53df1aac-f936-4cd3-a138-652d51221394") },
                    new TenantUser { UserId = PresidentUid, TenantId = Guid.Parse("53df1aac-f936-4cd3-a138-652d51221394") },
                    new TenantUser { UserId = LeaderUid, TenantId = Guid.Parse("53df1aac-f936-4cd3-a138-652d51221394") },
                    new TenantUser { UserId = GroupLeaderUid, TenantId = Guid.Parse("53df1aac-f936-4cd3-a138-652d51221394") },
                    new TenantUser { UserId = GroupMemberUid, TenantId = Guid.Parse("53df1aac-f936-4cd3-a138-652d51221394") },
                    new TenantUser { UserId = MemberUid, TenantId = Guid.Parse("53df1aac-f936-4cd3-a138-652d51221394") }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
