// using System;
// using Microsoft.AspNetCore.Identity;

// namespace Inspiration_International.Identity
// {
//     public static class MyIdentityDataInitializer
//     {
//         public static void SeedData
//     (UserManager<ApplicationUser> userManager,
//     RoleManager<ApplicationRole> roleManager)
//         {
//             SeedRoles(roleManager);
//             SeedUsers(userManager);
//         }

//         public static void SeedUsers
//     (UserManager<ApplicationUser> userManager)
//         {
//             if (userManager.FindByEmailAsync("kent2cky").Result == null)
//             {
//                 ApplicationUser user = new ApplicationUser();
//                 user.UserName = "kent2cky";
//                 user.Email = "kent2ckymaduka@localhost";
//                 user.FullName = "Kennis Maduka";

//                 IdentityResult result = userManager.CreateAsync(
//                     user, "123456"
//                 ).Result;

//                 if (result.Succeeded)
//                 {
//                     userManager.AddToRoleAsync(user, "NormalUser").Wait();
//                 }
//             }

//             if (userManager.FindByEmailAsync("Oris").Result == null)
//             {
//                 ApplicationUser user = new ApplicationUser();
//                 user.UserName = "Oris";
//                 user.Email = "oris@localhost";
//                 user.FullName = "Oris Adeoye";

//                 IdentityResult result = userManager.CreateAsync
//                 (
//                     user, "654321"
//                 ).Result;

//                 if (result.Succeeded)
//                 {
//                     userManager.AddToRoleAsync(user, "Administrator").Wait();
//                 }
//             }
//         }

//         public static void SeedRoles
//     (RoleManager<ApplicationRole> roleManager)
//         {
//             if (!roleManager.RoleExistsAsync("NormalUser").Result)
//             {
//                 ApplicationRole role = new ApplicationRole();
//                 role.Name = "NormalUser";
//                 role.Description = "Perform normal operations.";
//                 IdentityResult roleResult = roleManager.CreateAsync(role).Result;
//             }

//             if (!roleManager.RoleExistsAsync("Administrator").Result)
//             {
//                 ApplicationRole role = new ApplicationRole();
//                 role.Name = "Administrator";
//                 role.Description = "Perform all the operations.";
//                 IdentityResult roleResult = roleManager.CreateAsync(role).Result;
//             }
//         }
//     }
// }