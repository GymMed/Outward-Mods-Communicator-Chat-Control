using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OutwardModsCommunicatorChatControl.Utility.Helpers
{
    public static class EventArgumentParser
    {
        private static readonly HashSet<Type> WhitelistedTypes = new()
        {
            typeof(string),
            typeof(int),
            typeof(float),
            typeof(bool),
            typeof(double),
            typeof(long),
            typeof(decimal),
            typeof(char)
        };

        public static bool CanParse(Type type)
        {
            if (type == null)
                return false;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                return CanParse(underlyingType);
            }

            if (WhitelistedTypes.Contains(type))
                return true;

            if (type.IsEnum)
                return true;

            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                return elementType != null && WhitelistedTypes.Contains(elementType);
            }

            if (type.IsGenericType)
            {
                var genericDef = type.GetGenericTypeDefinition();
                if (genericDef == typeof(IEnumerable<>) || genericDef == typeof(List<>))
                {
                    var args = type.GetGenericArguments();
                    if (args.Length == 1 && WhitelistedTypes.Contains(args[0]))
                        return true;
                }
            }

            return false;
        }

        public static bool IsEventPublishable(OutwardModsCommunicator.EventBus.EventDefinition eventDef, out string unsupportedParam)
        {
            unsupportedParam = null;

            if (eventDef?.Schema?.Fields == null)
                return true;

            foreach (var field in eventDef.Schema.Fields)
            {
                if (!CanParse(field.Value))
                {
                    unsupportedParam = field.Key;
                    return false;
                }
            }

            return true;
        }

        public static bool TryParse(Type targetType, string value, out object result)
        {
            result = null;

            if (value == null)
                return false;

            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = Nullable.GetUnderlyingType(targetType);
                if (TryParse(underlyingType, value, out var parsed))
                {
                    result = parsed;
                    return true;
                }
                return false;
            }

            if (targetType == typeof(string))
            {
                result = value;
                return true;
            }

            if (targetType == typeof(int))
            {
                if (int.TryParse(value, out int parsed))
                {
                    result = parsed;
                    return true;
                }
                return false;
            }

            if (targetType == typeof(float))
            {
                if (float.TryParse(value, out float parsed))
                {
                    result = parsed;
                    return true;
                }
                return false;
            }

            if (targetType == typeof(double))
            {
                if (double.TryParse(value, out double parsed))
                {
                    result = parsed;
                    return true;
                }
                return false;
            }

            if (targetType == typeof(long))
            {
                if (long.TryParse(value, out long parsed))
                {
                    result = parsed;
                    return true;
                }
                return false;
            }

            if (targetType == typeof(decimal))
            {
                if (decimal.TryParse(value, out decimal parsed))
                {
                    result = parsed;
                    return true;
                }
                return false;
            }

            if (targetType == typeof(bool))
            {
                var lower = value.ToLowerInvariant();
                switch (lower)
                {
                    case "true":
                    case "1":
                    case "yes":
                    case "on":
                        {
                            result = true;
                            return true;
                        }
                    case "false":
                    case "0":
                    case "no":
                    case "off":
                        {
                            result = false;
                            return true;
                        }
                    default:
                        {
                            return false;
                        }
                }
            }

            if (targetType == typeof(char))
            {
                if (char.TryParse(value, out char parsed))
                {
                    result = parsed;
                    return true;
                }
                return false;
            }

            if (targetType.IsEnum)
            {
                try
                {
                    result = Enum.Parse(targetType, value, ignoreCase: true);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            if (targetType.IsArray)
            {
                var elementType = targetType.GetElementType();
                if (elementType == null)
                    return false;

                var parts = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var array = Array.CreateInstance(elementType, parts.Length);

                for (int i = 0; i < parts.Length; i++)
                {
                    if (!TryParse(elementType, parts[i], out var item))
                        return false;
                    array.SetValue(item, i);
                }

                result = array;
                return true;
            }

            if (targetType.IsGenericType)
            {
                var args = targetType.GetGenericArguments();
                if (args.Length == 1 && WhitelistedTypes.Contains(args[0]))
                {
                    var elementType = args[0];
                    var parts = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var listType = typeof(List<>).MakeGenericType(elementType);
                    var list = (IList)Activator.CreateInstance(listType);

                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (!TryParse(elementType, parts[i], out var item))
                            return false;
                        list.Add(item);
                    }

                    result = list;
                    return true;
                }
            }

            return false;
        }
    }
}
