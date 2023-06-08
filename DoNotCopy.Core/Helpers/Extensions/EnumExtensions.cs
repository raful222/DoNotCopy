using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DoNotCopy.Core.Helpers.Extensions
{
    public static class EnumExtensions
    {
        public static TAttribute GetCustomAttribute<TAttribute>(this Enum enumerator) where TAttribute : Attribute
        {
            return enumerator.GetType().GetMember(enumerator.ToString()).FirstOrDefault()?.GetCustomAttribute<TAttribute>();
        }

        public static string Name(this Enum enumerator)
        {
            return enumerator.GetCustomAttribute<DisplayAttribute>().Name;
        }

        public static string Description(this Enum enumerator)
        {
            return enumerator.GetCustomAttribute<DescriptionAttribute>().Description;
        }
    }
}