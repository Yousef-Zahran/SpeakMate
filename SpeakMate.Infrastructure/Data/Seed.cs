using SpeakMate.Core.Entities;
using SpeakMate.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SpeakMate.Infrastructure.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager ) {

            if (await userManager.Users.AnyAsync()) return;

            var memberData = await File.ReadAllTextAsync("D:\\source\\repos\\SpeakMate\\SpeakMate.Infrastructure\\Data\\UserSeedData.json");
            var members= JsonSerializer.Deserialize<List<UserSeedDto>>(memberData);

            if (members == null){ 
                Console.WriteLine("No members in seed data!");
                return;
            }

            foreach (var member in members) {

                var user = new AppUser
                {
                    UserName = member.DisplayName,
                    Id = member.Id,
                    DisplayName = member.DisplayName,
                    Email = member.Email,
                    ImageUrl = member.ImageUrl,
                    Member=new Member { 
                    Id = member.Id,
                    DisplayName = member.DisplayName,
                    City = member.City,
                    Country = member.Country,
                    Gender  = member.Gender,
                    DateOfBirth = member.DateOfBirth,
                    Discription = member.Description,
                    ImageUrl=member.ImageUrl,
                    LastActive  = member.LastActive,
                    Created = member.Created,   
                    NativeLanguage=member.NativeLanguage,
                    ForeignLanguageLearning= member.ForeignLanguageLearning
                    }
                };

                user.Member.Photos.Add(new Photo {
                    Url = member.ImageUrl!,
                    MemberId = member.Id,
                });

             var result= await userManager.CreateAsync(user,"P@ssw0rd");

                if (!result.Succeeded)
                {
                    Console.WriteLine(result.Errors.First().Description);
                }
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                DisplayName="Admin",
                UserName="admin",
                Email= "Admin1@test.com"
            };
            await userManager.CreateAsync(admin, "P@$$w0rd");
            await userManager.AddToRolesAsync(admin,["Admin", "Moderator"]);
        }
    }
}
