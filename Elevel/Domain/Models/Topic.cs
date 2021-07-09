﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elevel.Domain.Enums;

namespace Elevel.Domain.Models
{
    public class Topic : BaseDataModel
    {
        public Level Level { get; set; }
        public string TopicName { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}