using OutwardModsCommunicatorChatControl.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OutwardModsCommunicatorChatControl.Tests
{
    /// <summary>
    /// Unit tests for EventArgumentParser covering scalars, collections, enums, and error cases.
    /// </summary>
    public static class EventArgumentParserTests
    {
        public static void RunAllTests()
        {
            try
            {
                OMCCC.LogMessage("[TEST] Starting EventArgumentParser tests...");

                // Scalar type tests
                TestScalarInt();
                TestScalarFloat();
                TestScalarBool();
                TestScalarString();
                TestScalarChar();
                TestScalarEnum();

                // Array tests
                TestArrayInt();
                TestArrayString();
                TestArrayEnum();

                // Collection tests
                TestListInt();
                TestHashSetInt();
                TestIEnumerableInt();

                // Error cases
                TestScalarIntError();
                TestArrayIntError();
                TestEnumError();

                OMCCC.LogMessage("[TEST] All EventArgumentParser tests completed successfully!");
            }
            catch (Exception ex)
            {
                OMCCC.LogMessage($"[TEST ERROR] Test suite failed: {ex.Message}\n{ex.StackTrace}");
            }
        }

        // ============================================
        // SCALAR TYPE TESTS
        // ============================================

        private static void TestScalarInt()
        {
            var (success, value, error) = EventArgumentParser.TryParseWithDetails(typeof(int), "42");
            Assert(success, "int parsing should succeed");
            Assert(value is int && (int)value == 42, "int value should be 42");
            OMCCC.LogMessage("[PASS] Scalar int test");
        }

        private static void TestScalarFloat()
        {
            var (success, value, error) = EventArgumentParser.TryParseWithDetails(typeof(float), "3.14");
            Assert(success, "float parsing should succeed");
            Assert(value is float && Math.Abs((float)value - 3.14f) < 0.01f, "float value should be ~3.14");
            OMCCC.LogMessage("[PASS] Scalar float test");
        }

        private static void TestScalarBool()
        {
            var (success1, value1, _) = EventArgumentParser.TryParseWithDetails(typeof(bool), "true");
            Assert(success1 && (bool)value1 == true, "bool 'true' should parse");

            var (success2, value2, _) = EventArgumentParser.TryParseWithDetails(typeof(bool), "yes");
            Assert(success2 && (bool)value2 == true, "bool 'yes' should parse");

            var (success3, value3, _) = EventArgumentParser.TryParseWithDetails(typeof(bool), "false");
            Assert(success3 && (bool)value3 == false, "bool 'false' should parse");

            var (success4, value4, _) = EventArgumentParser.TryParseWithDetails(typeof(bool), "0");
            Assert(success4 && (bool)value4 == false, "bool '0' should parse as false");

            OMCCC.LogMessage("[PASS] Scalar bool test");
        }

        private static void TestScalarString()
        {
            var (success, value, error) = EventArgumentParser.TryParseWithDetails(typeof(string), "hello");
            Assert(success && (string)value == "hello", "string parsing should succeed");
            OMCCC.LogMessage("[PASS] Scalar string test");
        }

        private static void TestScalarChar()
        {
            var (success, value, error) = EventArgumentParser.TryParseWithDetails(typeof(char), "A");
            Assert(success && (char)value == 'A', "char parsing should succeed");
            OMCCC.LogMessage("[PASS] Scalar char test");
        }

        private static void TestScalarEnum()
        {
            var (success, value, error) = EventArgumentParser.TryParseWithDetails(typeof(DayOfWeek), "Monday");
            Assert(success && (DayOfWeek)value == DayOfWeek.Monday, "enum parsing should succeed");

            var (success2, value2, error2) = EventArgumentParser.TryParseWithDetails(typeof(DayOfWeek), "monday");
            Assert(success2 && (DayOfWeek)value2 == DayOfWeek.Monday, "enum parsing should be case-insensitive");

            OMCCC.LogMessage("[PASS] Scalar enum test");
        }

        // ============================================
        // ARRAY TESTS
        // ============================================

        private static void TestArrayInt()
        {
            var (success, value, error) = EventArgumentParser.TryParseWithDetails(typeof(int[]), "1 2 3");
            Assert(success, "int array parsing should succeed");
            Assert(value is int[] arr && arr.Length == 3 && arr[0] == 1 && arr[1] == 2 && arr[2] == 3, "int array should be [1,2,3]");
            OMCCC.LogMessage("[PASS] Array int test");
        }

        private static void TestArrayString()
        {
            var (success, value, error) = EventArgumentParser.TryParseWithDetails(typeof(string[]), "hello world test");
            Assert(success, "string array parsing should succeed");
            Assert(value is string[] arr && arr.Length == 3 && arr[0] == "hello" && arr[1] == "world" && arr[2] == "test", "string array should be correct");
            OMCCC.LogMessage("[PASS] Array string test");
        }

        private static void TestArrayEnum()
        {
            var (success, value, error) = EventArgumentParser.TryParseWithDetails(typeof(DayOfWeek[]), "Monday Wednesday Friday");
            Assert(success, "enum array parsing should succeed");
            Assert(value is DayOfWeek[] arr && arr.Length == 3 && arr[0] == DayOfWeek.Monday && arr[1] == DayOfWeek.Wednesday, "enum array should be correct");
            OMCCC.LogMessage("[PASS] Array enum test");
        }

        // ============================================
        // COLLECTION TESTS
        // ============================================

        private static void TestListInt()
        {
            var (success, value, error) = EventArgumentParser.TryParseWithDetails(typeof(List<int>), "10 20 30");
            Assert(success, "List<int> parsing should succeed");
            Assert(value is List<int> list && list.Count == 3 && list[0] == 10 && list[1] == 20, "List<int> should contain correct values");
            OMCCC.LogMessage("[PASS] Collection List<int> test");
        }

        private static void TestHashSetInt()
        {
            var (success, value, error) = EventArgumentParser.TryParseWithDetails(typeof(HashSet<int>), "5 10 15");
            Assert(success, "HashSet<int> parsing should succeed");
            Assert(value is HashSet<int> set && set.Count == 3 && set.Contains(5) && set.Contains(10), "HashSet<int> should contain correct values");
            OMCCC.LogMessage("[PASS] Collection HashSet<int> test");
        }

        private static void TestIEnumerableInt()
        {
            var (success, value, error) = EventArgumentParser.TryParseWithDetails(typeof(IEnumerable<int>), "7 8 9");
            Assert(success, "IEnumerable<int> parsing should succeed");
            Assert(value != null, "IEnumerable<int> should not be null");
            var list = ((IEnumerable<int>)value).ToList();
            Assert(list.Count == 3 && list[0] == 7 && list[1] == 8, "IEnumerable<int> should contain correct values");
            OMCCC.LogMessage("[PASS] Collection IEnumerable<int> test");
        }

        // ============================================
        // ERROR CASES
        // ============================================

        private static void TestScalarIntError()
        {
            var (success, value, error) = EventArgumentParser.TryParseWithDetails(typeof(int), "abc");
            Assert(!success, "int parsing of 'abc' should fail");
            Assert(error != null && !string.IsNullOrEmpty(error), "error message should be provided");
            OMCCC.LogMessage($"[PASS] Scalar int error test (error: {error})");
        }

        private static void TestArrayIntError()
        {
            var (success, value, error) = EventArgumentParser.TryParseWithDetails(typeof(int[]), "1 2 abc 4");
            Assert(!success, "int array parsing with invalid element should fail");
            Assert(error != null && error.Contains("position"), "error should indicate position");
            OMCCC.LogMessage($"[PASS] Array int error test (error: {error})");
        }

        private static void TestEnumError()
        {
            var (success, value, error) = EventArgumentParser.TryParseWithDetails(typeof(DayOfWeek), "InvalidDay");
            Assert(!success, "enum parsing of invalid value should fail");
            Assert(error != null && error.Contains("Expected one of"), "error should list valid options");
            OMCCC.LogMessage($"[PASS] Enum error test (error: {error})");
        }

        // ============================================
        // TEST UTILITIES
        // ============================================

        private static void Assert(bool condition, string message)
        {
            if (!condition)
                throw new Exception($"Assertion failed: {message}");
        }
    }
}
