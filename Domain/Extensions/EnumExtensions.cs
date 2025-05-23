using System.ComponentModel;
using System.Reflection;

namespace Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription<T>(this T enumValue) where T : Enum
        {
            FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

            return attribute != null ? attribute.Description : enumValue.ToString();
        }
    }
}
