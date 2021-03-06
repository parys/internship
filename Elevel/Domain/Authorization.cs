using System;
using System.Collections.Generic;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;

namespace Elevel.Domain
{
    public class Authorization
    {
        public const string DefaultPassword = "Pa$$w0rd.";

        public static Dictionary<UserRole, List<User>> DefaultUsers = new Dictionary<UserRole, List<User>>()
        {
            {
                UserRole.Administrator,
                new List<User>()
                {
                    new User()
                    {
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "Admin",
                        LastName = "Adminov",
                        UserName = "Admin_Adminov",
                        Email = "ElevelAdministrator@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = @"ava1.jpg"
                    },
                    new User()
                    {
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "Admin1",
                        LastName = "Adminov1",
                        UserName = "Admin_Adminov1",
                        Email = "ElevelAdministrator1@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                       Avatar = @"ava2.jpg"
                    },
                    new User()
                    {
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "Admin2",
                        LastName = "Adminov2",
                        UserName = "Admin_Adminov2",
                        Email = "ElevelAdministrator2@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = @"ava3.jpg"
                    }
                }
            },
            {
                UserRole.User,
                new List<User>()
                {
                    new User()
                    {
                        UserName = "User_Userov",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "User",
                        LastName = "Userov",
                        Email = "ElevelUser@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = @"ava4.jpg"
                    },
                    new User()
                    {
                        UserName = "User_Userov1",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "User1",
                        LastName = "Userov1",
                        Email = "ElevelUser1@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = @"ava5.jpg"
                    },
                    new User()
                    {
                        UserName = "User_Userov2",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "User2",
                        LastName = "Userov2",
                        Email = "ElevelUser2@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = @"ava6.jpg"
                    },
                    new User()
                    {
                        UserName = "User_Userov3",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "User3",
                        LastName = "Userov3",
                        Email = "ElevelUser3@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = @"ava7.jpg"
                    },
                    new User()
                    {
                        UserName = "User_Userov4",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "User4",
                        LastName = "Userov4",
                        Email = "ElevelUser4@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = @"ava8.jpg"
                    },
                    new User()
                    {
                        UserName = "User_Userov5",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "User5",
                        LastName = "Userov5",
                        Email = "ElevelUser5@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = @"ava9.jpg"
                    },
                }
            },
            {
                UserRole.Coach,
                new List<User>()
                {
                    new User()
                    {
                        UserName = "Coach_Coachev",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "Coach",
                        LastName = "Coachev",
                        Email = "ElevelCoach@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = @"ava10.jpg"
                    },
                    new User()
                    {
                        UserName = "Coach_Coachev1",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "Coach1",
                        LastName = "Coachev1",
                        Email = "ElevelCoach1@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = @"ava11.jpg"
                    }
                }
            },
            {
                UserRole.HumanResourceManager,
                new List<User>()
                {
                    new User()
                    {
                        UserName = "Manager_Managerov",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "Manager",
                        LastName = "Managerov",
                        Email = "ElevelHumanResourceManager@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = @"ava12.jpg"
                    },
                    new User()
                    {
                        UserName = "Manager_Managerov1",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "Manager1",
                        LastName = "Managerov1",
                        Email = "ElevelHumanResourceManager1@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = @"ava13.jpg"
                    },
                    new User()
                    {
                        UserName = "Manager_Managerov2",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        FirstName = "Manager2",
                        LastName = "Managerov2",
                        Email = "ElevelHumanResourceManager2@gmail.com",
                        CreationDate = DateTimeOffset.Now,
                        Avatar = @"ava14.jpg"
                    }
                }
            }
        };
    }
}