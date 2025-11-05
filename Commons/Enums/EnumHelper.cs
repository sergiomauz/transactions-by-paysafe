using System.ComponentModel;
using System.Reflection;


namespace Commons.Enums
{
    public static class EnumHelper
    {
        public static TEnum? FromDescription<TEnum>(string description) where TEnum : struct, Enum
        {
            foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attr = field.GetCustomAttribute<DescriptionAttribute>();
                if (attr?.Description == description)
                {
                    return Enum.Parse<TEnum>(field.Name);
                }
            }
            return null;
        }

        public static bool IsValidDescription<TEnum>(string value) where TEnum : Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return typeof(TEnum)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => f.GetCustomAttribute<DescriptionAttribute>()?.Description)
                .Any(desc => string.Equals(desc, value, StringComparison.OrdinalIgnoreCase));
        }
    }
}
