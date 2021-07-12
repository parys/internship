using System;
using System.Collections.Generic;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;

namespace Elevel.Domain
{
    public class Authorization
    {
        public const string DefaultPassword = "Pa$$w0rd.";

        public static Dictionary<UserRole, List<ApplicationUser>> DefaultUsers = new Dictionary<UserRole, List<ApplicationUser>>()
        {
            {
                UserRole.Administrator,
                new List<ApplicationUser>()
                {
                    new ApplicationUser()
                    {
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "Admin",
                        LastName = "Adminov",
                        UserName = "AdminAdminov",
                        Email = "ElevelAdministrator@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = "..\\Elevel.Api\\Resources\\default_avatar.jpg"
                    }
                }
            },
            {
                UserRole.User,
                new List<ApplicationUser>()
                {
                    new ApplicationUser()
                    {
                        UserName = "UserUserov",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "User",
                        LastName = "Userov",
                        Email = "ElevelUser@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = "..\\Elevel.Api\\Resources\\default_avatar.jpg"
                    }
                }
            },
            {
                UserRole.Coach,
                new List<ApplicationUser>()
                {
                    new ApplicationUser()
                    {
                        UserName = "CoachCoachev",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "Coach",
                        LastName = "Coachev",
                        Email = "ElevelCoach@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = "..\\Elevel.Api\\Resources\\default_avatar.jpg"
                    }
                }
            },
            {
                UserRole.HumanResourceManager,
                new List<ApplicationUser>()
                {
                    new ApplicationUser()
                    {
                        UserName = "ManagerManagerov",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "Manager",
                        LastName = "Managerov",
                        Email = "ElevelHumanResourceManager@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = "..\\Elevel.Api\\Resources\\default_avatar.jpg"
                    }
                }
            }
        };
    }
}