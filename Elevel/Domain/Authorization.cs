﻿using System;
using System.Collections.Generic;
using Elevel.Domain.Models;

namespace Elevel.Domain
{
    public class Authorization
    {
        public enum Roles
        {
            Administrator,
            HumanResourceManager,
            Coach,
            User
        }

        public const string DefaultPassword = "Pa$$w0rd.";

        public static Dictionary<Roles, List<ApplicationUser>> DefaultUsers = new Dictionary<Roles, List<ApplicationUser>>()
        {
            {
                Roles.Administrator,
                new List<ApplicationUser>()
                {
                    new ApplicationUser()
                    {
                        FirstName = "FirstName",
                        LastName = "LastName",
                        Email = "ElevelAdministrator@gmail.com",
                        CreationDate = DateTimeOffset.Now
                    }
                }
            },
            {
                Roles.User,
                new List<ApplicationUser>()
                {
                    new ApplicationUser()
                    {
                        FirstName = "FirstName",
                        LastName = "LastName",
                        Email = "ElevelUser@gmail.com",
                        CreationDate = DateTimeOffset.Now
                    }
                }
            },
            {
                Roles.Coach,
                new List<ApplicationUser>()
                {
                    new ApplicationUser()
                    {
                        FirstName = "FirstName",
                        LastName = "LastName",
                        Email = "ElevelCoach@gmail.com",
                        CreationDate = DateTimeOffset.Now
                    }
                }
            },
            {
                Roles.HumanResourceManager,
                new List<ApplicationUser>()
                {
                    new ApplicationUser()
                    {
                        FirstName = "FirstName",
                        LastName = "LastName",
                        Email = "ElevelHumanResourceManager@gmail.com",
                        CreationDate = DateTimeOffset.Now
                    }
                }
            }
        };
    }
}