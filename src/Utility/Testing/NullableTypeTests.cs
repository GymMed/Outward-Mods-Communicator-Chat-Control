#if DEBUG
using System;
using System.Collections.Generic;

namespace OutwardModsCommunicatorChatControl.Utility.Testing
{
    public static class NullableTypeTests
    {
        public static List<ITestCase> GetTestCases()
        {
            return new List<ITestCase>
            {
                // Nullable Int
                new NullableTestCase<int>("NullableInt32", "ValidPositive", "42", true, 42),
                new NullableTestCase<int>("NullableInt32", "ValidNegative", "-123", true, -123),
                new NullableTestCase<int>("NullableInt32", "ValidZero", "0", true, 0),
                new NullableTestCase<int>("NullableInt32", "InvalidLetters", "abc", false, null),
                new NullableTestCase<int>("NullableInt32", "InvalidDecimal", "12.5", false, null),
                new NullableTestCase<int>("NullableInt32", "InvalidMixed", "123abc", false, null),
                new NullableTestCase<int>("NullableInt32", "InvalidWhitespace", "   ", false, null),

                // Nullable Float
                new NullableTestCase<float>("NullableFloat", "ValidPositive", "3.14", true, 3.14f),
                new NullableTestCase<float>("NullableFloat", "ValidNegative", "-0.5", true, -0.5f),
                new NullableTestCase<float>("NullableFloat", "ValidInteger", "42", true, 42f),
                new NullableTestCase<float>("NullableFloat", "InvalidLetters", "abc", false, null),
                new NullableTestCase<float>("NullableFloat", "InvalidMixed", "3.14abc", false, null),
                new NullableTestCase<float>("NullableFloat", "InvalidWhitespace", "   ", false, null),

                // Nullable Double
                new NullableTestCase<double>("NullableDouble", "ValidPi", "3.14159", true, 3.14159d),
                new NullableTestCase<double>("NullableDouble", "ValidNegative", "-2.718", true, -2.718d),
                new NullableTestCase<double>("NullableDouble", "InvalidLetters", "abc", false, null),
                new NullableTestCase<double>("NullableDouble", "InvalidWhitespace", "   ", false, null),

                // Nullable Long
                new NullableTestCase<long>("NullableInt64", "ValidPositive", "9223372036854775807", true, 9223372036854775807L),
                new NullableTestCase<long>("NullableInt64", "ValidNegative", "-9223372036854775808", true, -9223372036854775808L),
                new NullableTestCase<long>("NullableInt64", "ValidZero", "0", true, 0L),
                new NullableTestCase<long>("NullableInt64", "InvalidLetters", "abc", false, null),
                new NullableTestCase<long>("NullableInt64", "InvalidWhitespace", "   ", false, null),

                // Nullable Decimal
                new NullableTestCase<decimal>("NullableDecimal", "ValidCurrency", "123.45", true, 123.45m),
                new NullableTestCase<decimal>("NullableDecimal", "ValidNegative", "-99.99", true, -99.99m),
                new NullableTestCase<decimal>("NullableDecimal", "InvalidLetters", "abc", false, null),
                new NullableTestCase<decimal>("NullableDecimal", "InvalidWhitespace", "   ", false, null),

                // Nullable Bool - true values
                new NullableTestCase<bool>("NullableBoolean", "ValidTrue_True", "true", true, true),
                new NullableTestCase<bool>("NullableBoolean", "ValidTrue_ONE", "1", true, true),
                new NullableTestCase<bool>("NullableBoolean", "ValidTrue_Yes", "yes", true, true),
                new NullableTestCase<bool>("NullableBoolean", "ValidTrue_On", "on", true, true),

                // Nullable Bool - false values
                new NullableTestCase<bool>("NullableBoolean", "ValidFalse_False", "false", true, false),
                new NullableTestCase<bool>("NullableBoolean", "ValidFalse_Zero", "0", true, false),
                new NullableTestCase<bool>("NullableBoolean", "ValidFalse_No", "no", true, false),
                new NullableTestCase<bool>("NullableBoolean", "ValidFalse_Off", "off", true, false),

                // Nullable Bool - invalid values
                new NullableTestCase<bool>("NullableBoolean", "InvalidMaybe", "maybe", false, null),
                new NullableTestCase<bool>("NullableBoolean", "InvalidEmpty", "", false, null),
                new NullableTestCase<bool>("NullableBoolean", "InvalidTwo", "2", false, null),

                // Nullable Char
                new NullableTestCase<char>("NullableChar", "ValidLetter", "a", true, 'a'),
                new NullableTestCase<char>("NullableChar", "ValidDigit", "5", true, '5'),
                new NullableTestCase<char>("NullableChar", "InvalidMultipleChars", "ab", false, null),
                new NullableTestCase<char>("NullableChar", "InvalidEmpty", "", false, null),
                new NullableTestCase<char>("NullableChar", "InvalidWhitespace", "   ", false, null),
            };
        }
    }

    internal class NullableTestCase<T> : ITestCase where T : struct
    {
        public string Category => "Nullable";
        public string TestName { get; }
        public Type TargetType => typeof(Nullable<T>);
        public string InputValue { get; }
        public bool ExpectSuccess { get; }
        public object ExpectedValue { get; }

        public NullableTestCase(string typeName, string testName, string input, bool expectSuccess, object expected)
        {
            TestName = $"{typeName}_{testName}";
            InputValue = input;
            ExpectSuccess = expectSuccess;
            ExpectedValue = expected;
        }
    }
}
#endif
