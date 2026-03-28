#if DEBUG
using System;
using System.Collections.Generic;

namespace OutwardModsCommunicatorChatControl.Utility.Testing
{
    public static class ScalarParserTests
    {
        public static List<ITestCase> GetTestCases()
        {
            return new List<ITestCase>
            {
                // String tests
                new ScalarTestCase<string>("String", "ValidNormalString", "hello world", true, "hello world"),
                new ScalarTestCase<string>("String", "ValidEmptyString", "", false, ""),
                new ScalarTestCase<string>("String", "ValidWithNumbers", "test123", true, "test123"),
                new ScalarTestCase<string>("String", "ValidSpecialChars", "!@#$%^&*()", true, "!@#$%^&*()"),

                // Int tests
                new ScalarTestCase<int>("Int32", "ValidPositiveNumber", "42", true, 42),
                new ScalarTestCase<int>("Int32", "ValidNegativeNumber", "-123", true, -123),
                new ScalarTestCase<int>("Int32", "ValidZero", "0", true, 0),
                new ScalarTestCase<int>("Int32", "ValidMaxValue", "2147483647", true, 2147483647),
                new ScalarTestCase<int>("Int32", "InvalidLetters", "abc", false, 0),
                new ScalarTestCase<int>("Int32", "InvalidDecimal", "12.5", false, 0),
                new ScalarTestCase<int>("Int32", "InvalidMixed", "123abc", false, 0),
                new ScalarTestCase<int>("Int32", "InvalidWhitespace", "   ", false, 0),

                // Float tests
                new ScalarTestCase<float>("Float", "ValidPositiveDecimal", "3.14", true, 3.14f),
                new ScalarTestCase<float>("Float", "ValidNegativeDecimal", "-0.5", true, -0.5f),
                new ScalarTestCase<float>("Float", "ValidIntegerAsFloat", "42", true, 42f),
                new ScalarTestCase<float>("Float", "ValidNegativeIntegerAsFloat", "-100", true, -100f),
                new ScalarTestCase<float>("Float", "ValidScientificNotation", "1e-3", true, 0.001f),
                new ScalarTestCase<float>("Float", "InvalidLetters", "abc", false, 0f),
                new ScalarTestCase<float>("Float", "InvalidMixed", "3.14abc", false, 0f),

                // Double tests
                new ScalarTestCase<double>("Double", "ValidPi", "3.14159", true, 3.14159d),
                new ScalarTestCase<double>("Double", "ValidNegative", "-2.718", true, -2.718d),
                new ScalarTestCase<double>("Double", "ValidLargeNumber", "1.7976931348623157E+308", true, 1.7976931348623157E+308d),
                new ScalarTestCase<double>("Double", "InvalidLetters", "abc", false, 0d),

                // Long tests
                new ScalarTestCase<long>("Int64", "ValidPositive", "9223372036854775807", true, 9223372036854775807L),
                new ScalarTestCase<long>("Int64", "ValidNegative", "-9223372036854775808", true, -9223372036854775808L),
                new ScalarTestCase<long>("Int64", "ValidZero", "0", true, 0L),
                new ScalarTestCase<long>("Int64", "InvalidLetters", "abc", false, 0L),

                // Decimal tests
                new ScalarTestCase<decimal>("Decimal", "ValidCurrency", "123.45", true, 123.45m),
                new ScalarTestCase<decimal>("Decimal", "ValidNegativeCurrency", "-99.99", true, -99.99m),
                new ScalarTestCase<decimal>("Decimal", "ValidLargeNumber", "79228162514264337593543950335", true, 79228162514264337593543950335m),
                new ScalarTestCase<decimal>("Decimal", "InvalidLetters", "abc", false, 0m),

                // Bool tests - true values
                new ScalarTestCase<bool>("Boolean", "TrueValue_True", "true", true, true),
                new ScalarTestCase<bool>("Boolean", "TrueValue_True_Uppercase", "TRUE", true, true),
                new ScalarTestCase<bool>("Boolean", "TrueValue_One", "1", true, true),
                new ScalarTestCase<bool>("Boolean", "TrueValue_Yes", "yes", true, true),
                new ScalarTestCase<bool>("Boolean", "TrueValue_Yes_Uppercase", "YES", true, true),
                new ScalarTestCase<bool>("Boolean", "TrueValue_On", "on", true, true),

                // Bool tests - false values
                new ScalarTestCase<bool>("Boolean", "FalseValue_False", "false", true, false),
                new ScalarTestCase<bool>("Boolean", "FalseValue_Zero", "0", true, false),
                new ScalarTestCase<bool>("Boolean", "FalseValue_No", "no", true, false),
                new ScalarTestCase<bool>("Boolean", "FalseValue_Off", "off", true, false),

                // Bool tests - invalid values
                new ScalarTestCase<bool>("Boolean", "InvalidValue_Maybe", "maybe", false, false),
                new ScalarTestCase<bool>("Boolean", "InvalidValue_Empty", "", false, false),
                new ScalarTestCase<bool>("Boolean", "InvalidValue_Two", "2", false, false),

                // Char tests
                new ScalarTestCase<char>("Char", "ValidLetter", "a", true, 'a'),
                new ScalarTestCase<char>("Char", "ValidLetter_Z", "Z", true, 'Z'),
                new ScalarTestCase<char>("Char", "ValidDigit", "5", true, '5'),
                new ScalarTestCase<char>("Char", "ValidSpecialChar", "!", true, '!'),
                new ScalarTestCase<char>("Char", "ValidSpace", " ", false, '\0'),
                new ScalarTestCase<char>("Char", "InvalidMultipleChars", "ab", false, '\0'),
                new ScalarTestCase<char>("Char", "InvalidEmpty", "", false, '\0'),
                new ScalarTestCase<char>("Char", "InvalidWord", "hello", false, '\0'),
            };
        }
    }

    internal class ScalarTestCase<T> : ITestCase
    {
        public string Category => "Scalar";
        public string TestName { get; }
        public Type TargetType => typeof(T);
        public string InputValue { get; }
        public bool ExpectSuccess { get; }
        public object ExpectedValue { get; }

        public ScalarTestCase(string typeName, string testName, string input, bool expectSuccess, T expected)
        {
            TestName = $"{typeName}_{testName}";
            InputValue = input;
            ExpectSuccess = expectSuccess;
            ExpectedValue = expected;
        }
    }
}
#endif
