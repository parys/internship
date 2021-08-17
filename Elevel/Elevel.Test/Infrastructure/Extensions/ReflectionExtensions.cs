using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Elevel.Test.Infrastructure.Extensions
{
    public static class ReflectionExtensions
    {
        public static bool AreEquivalent(this MemberInfo self, MemberInfo to)
        {
            return self.DeclaringType == to.DeclaringType && self.Name == to.Name;
        }

        public static bool ArePublicInstancePropertiesEqual<T, TU>(this T self, TU to, params string[] ignore)
            where T : class where TU : class
        {
            if (self == null && to == null)
            {
                return true;
            };

            if (self == null || to == null)
            {
                return false;
            }

            var ignoreList = new List<string>(ignore);
            var selfType = typeof(T);
            var toType = typeof(TU);

            var selfTypeProperties = selfType.GetTypeProperties(ignoreList);
            var toTypeProperties = toType.GetTypeProperties(ignoreList);

            var propertiesToCompare = selfTypeProperties.Where(x => toTypeProperties.Contains(x)).ToList();

            var unequalProperties = propertiesToCompare
                .Where(pi =>
                {
                    var selfValue = selfType.GetProperty(pi).GetValue(self, null);
                    var toValue = toType.GetProperty(pi).GetValue(to, null);
                    return selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue));
                }).ToList();

            return !unequalProperties.Any();
        }

        public static IEnumerable<string> GetTypeProperties(this IReflect selfType, ICollection<string> ignoreList)
        {
            return selfType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(pi => !ignoreList.Contains(pi.Name) && pi.GetUnderlyingType().IsSimpleType() &&
                    pi.GetIndexParameters().Length == 0)
                .Select(pi => pi.Name)
                .ToList();
        }

        public static IEnumerable<object[]> GetTypePropertiesAsObjects(this IReflect selfType, params string[] ignore)
        {
            return selfType.GetTypeProperties(new List<string>(ignore))
                .Select(property => new object[] { property })
                .ToList();
        }

        public static object GetPropertyValue(this object instance, string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentNullException(nameof(property));
            }

            return instance.GetType().GetProperty(property).GetValue(instance);
        }

        public static bool IsSimpleType(this Type type)
        {
            return
                type.IsValueType ||
                type.IsPrimitive ||
                new[] { typeof(string), typeof(decimal), typeof(DateTime), typeof(DateTimeOffset), typeof(TimeSpan), typeof(Guid) }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object;
        }

        public static Type GetUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    (
                        "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
        }

    }
}
