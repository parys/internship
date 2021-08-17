using System;

namespace Elevel.Test.Infrastructure.Extensions
{
    public class Generator
    {
        public static string Generate(string prefix, int? length = null)
        {
            var str = $"{prefix}_{Guid.NewGuid()}";
            return length.HasValue && str.Length > length ? str.Substring(0, length.Value) : str;
        }

        public static int Generate(int min, int max) => new Random().Next(min, max);
    }
}
