using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OutwardModsCommunicatorChatControl.Utility.Helpers
{
    public static class EventArgumentParser
    {
        private static readonly HashSet<Type> WhitelistedScalarTypes = new()
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

        /// <summary>
        /// Checks if a type can be parsed from a string value.
        /// Supports scalars, enums, arrays, collections (HashSet, List, IEnumerable), and nullable types.
        /// </summary>
        public static bool CanParse(Type type)
        {
            if (type == null)
                return false;

            // Handle nullable types
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                return CanParse(underlyingType);
            }

            // Whitelisted scalar types
            if (WhitelistedScalarTypes.Contains(type))
                return true;

            // Enum types
            if (type.IsEnum)
                return true;

            // Check if it's a collection type using advanced parser
            if (CollectionValueParser.IsCollectionType(type))
            {
                var elementType = CollectionValueParser.GetElementType(type);
                return elementType != null && CanParseScalar(elementType);
            }

            return false;
        }

        /// <summary>
        /// Checks if a scalar type can be parsed.
        /// </summary>
        private static bool CanParseScalar(Type type)
        {
            if (type == null)
                return false;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                return CanParseScalar(underlyingType);
            }

            if (WhitelistedScalarTypes.Contains(type))
                return true;

            if (type.IsEnum)
                return true;

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

        /// <summary>
        /// Attempts to parse a string value to the target type with detailed error information.
        /// Returns a tuple of (success, value, error message).
        /// </summary>
        public static (bool success, object value, string error) TryParseWithDetails(Type targetType, string valueString)
        {
            if (targetType == null)
                return (false, null, "Target type is null");

            if (string.IsNullOrEmpty(valueString))
                return (false, null, "Value is null or empty");

            // Handle nullable types
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = Nullable.GetUnderlyingType(targetType);
                return TryParseWithDetails(underlyingType, valueString);
            }

            // Try collection parsing first (for HashSet, List, IEnumerable, etc.)
            if (CollectionValueParser.IsCollectionType(targetType))
            {
                var (result, error) = CollectionValueParser.TryParse(targetType, valueString);
                return (error == null, result, error);
            }

            // Try scalar parsing
            var (scalarResult, scalarError) = TryParseScalar(targetType, valueString);
            return (scalarError == null, scalarResult, scalarError);
        }

        /// <summary>
        /// Legacy method for backward compatibility. Attempts to parse a string value to the target type.
        /// </summary>
        public static bool TryParse(Type targetType, string value, out object result)
        {
            result = null;

            if (value == null)
                return false;

            var (success, parsedValue, _) = TryParseWithDetails(targetType, value);
            if (success)
            {
                result = parsedValue;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to parse a scalar value (non-collection).
        /// Returns (value, error) where error is null on success.
        /// </summary>
        internal static (object value, string error) TryParseScalar(Type targetType, string valueString)
        {
            if (string.IsNullOrWhiteSpace(valueString))
                return (null, "Empty value");

            valueString = valueString.Trim();

            // String
            if (targetType == typeof(string))
                return (valueString, null);

            // Integer
            if (targetType == typeof(int))
            {
                if (int.TryParse(valueString, out int parsed))
                    return (parsed, null);
                return (null, $"Cannot convert '{valueString}' to int");
            }

            // Float
            if (targetType == typeof(float))
            {
                if (float.TryParse(valueString, out float parsed))
                    return (parsed, null);
                return (null, $"Cannot convert '{valueString}' to float");
            }

            // Double
            if (targetType == typeof(double))
            {
                if (double.TryParse(valueString, out double parsed))
                    return (parsed, null);
                return (null, $"Cannot convert '{valueString}' to double");
            }

            // Long
            if (targetType == typeof(long))
            {
                if (long.TryParse(valueString, out long parsed))
                    return (parsed, null);
                return (null, $"Cannot convert '{valueString}' to long");
            }

            // Decimal
            if (targetType == typeof(decimal))
            {
                if (decimal.TryParse(valueString, out decimal parsed))
                    return (parsed, null);
                return (null, $"Cannot convert '{valueString}' to decimal");
            }

            // Boolean
            if (targetType == typeof(bool))
            {
                var lower = valueString.ToLowerInvariant();
                if (lower is "true" or "1" or "yes" or "on")
                    return (true, null);
                if (lower is "false" or "0" or "no" or "off")
                    return (false, null);
                return (null, $"Cannot convert '{valueString}' to bool. Expected: true/false, 1/0, yes/no, on/off");
            }

            // Character
            if (targetType == typeof(char))
            {
                if (char.TryParse(valueString, out char parsed))
                    return (parsed, null);
                return (null, $"Cannot convert '{valueString}' to char");
            }

            // Enum
            if (targetType.IsEnum)
            {
                try
                {
                    var enumValue = Enum.Parse(targetType, valueString, ignoreCase: true);
                    return (enumValue, null);
                }
                catch
                {
                    var enumNames = string.Join(", ", Enum.GetNames(targetType));
                    return (null, $"Invalid enum value. Expected one of: {enumNames}");
                }
            }

            return (null, $"Cannot parse type {targetType.Name}");
        }
    }
}
