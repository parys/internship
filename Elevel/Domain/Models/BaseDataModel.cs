using System;

namespace Elevel.Domain.Models
{
    public abstract class BaseDataModel
    {
        public Guid Id { get; set; }

        /* LevelEnum - перечисление всех уровней от A2 по C1
         * Для порядка A2 = 1, ..., C1 = 4 - последовательные уровни знания английского
        */
        public enum LevelEnum
        {
            A2 = 1,
            B1,
            B2,
            C1
        };
    }
}