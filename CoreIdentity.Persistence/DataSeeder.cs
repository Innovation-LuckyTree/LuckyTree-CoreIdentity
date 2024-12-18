using CoreIdentity.Domain.Entity;
using static CoreIdentity.Application.Common.Extensions.CryptographyExtensions;

namespace CoreIdentity.Persistence;

public static class DataSeeder
{
    static Guid AdminUserId = Guid.NewGuid();
    static Guid BdmUserId = Guid.NewGuid();
    static Guid BdoUserId = Guid.NewGuid();

    public static IEnumerable<UserRoles> GetUserRoles()
    {
        return [
            new UserRoles
            {
                UserId = AdminUserId,
                RoleId = 1
            },
             new UserRoles
             {
                 UserId = BdmUserId,
                 RoleId = 2
             },
            new UserRoles
            {
                UserId = BdoUserId,
                RoleId = 3
            }
        ];
    }
    
    public static IEnumerable<User> GetUserList()
    {

        return [
            new User
            {
                Id = AdminUserId,
                UserName = "juanTmadAdmin",
                Email = "juanTmadAdmin@gmail.com",
                MobileNumber = "09090909099",
                Password = CreatePassword("test@123").Password,
                PasswordSalt = CreatePassword("test@123").Salt
            },
            new User
            {
                Id = BdmUserId,
                UserName = "juanTmadBdm",
                Email = "juanTmadBdm@gmail.com",
                MobileNumber = "09090909199",
                Password = CreatePassword("test@123").Password,
                PasswordSalt = CreatePassword("test@123").Salt
            },
            new User
            {
                Id = BdoUserId,
                UserName = "juanTmadBdo",
                Email = "juanTmadBdo@gmail.com",
                MobileNumber = "09090909299",
                Password = CreatePassword("123456789").Password,
                PasswordSalt = CreatePassword("123456789").Salt
            }
        ];
    }
}