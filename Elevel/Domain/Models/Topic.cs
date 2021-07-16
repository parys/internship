﻿using System;
using System.Collections.Generic;
using Elevel.Domain.Enums;

namespace Elevel.Domain.Models
{
    public class Topic : BaseDataModel
    {
        public string TopicName { get; set; }
        public Level Level { get; set; }
        public DateTimeOffset CreationDate { get; set; }

        public bool Deleted { get; set; }

        public ICollection<Test> EssayTests { get; set; }
        public ICollection<Test> SpeakingTests { get; set; }
    }
}