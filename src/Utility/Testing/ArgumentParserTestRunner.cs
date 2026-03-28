#if DEBUG
using OutwardModsCommunicatorChatControl.Utility.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OutwardModsCommunicatorChatControl.Utility.Testing
{
    public static class ArgumentParserTestRunner
    {
        public static void RunAllTests()
        {
            var tests = new List<ITestCase>();
            
            tests.AddRange(ScalarParserTests.GetTestCases());
            tests.AddRange(NullableTypeTests.GetTestCases());
            tests.AddRange(EnumTests.GetTestCases());
            tests.AddRange(CollectionTests.GetTestCases());

            int totalPassed = 0;
            int totalFailed = 0;
            var failures = new List<ITestResult>();

            OMCCC.LogMessage("========================================");
            OMCCC.LogMessage("=== EventArgumentParser Test Suite ===");
            OMCCC.LogMessage($"=== Total Tests: {tests.Count} ===");
            OMCCC.LogMessage("========================================");

            var grouped = tests.GroupBy(t => t.Category);

            foreach (var group in grouped)
            {
                OMCCC.LogMessage($"--- {group.Key} Tests ({group.Count()}) ---");
                int categoryPassed = 0;
                int categoryFailed = 0;

                foreach (var test in group)
                {
                    var result = ExecuteTest(test);
                    
                    if (result.Passed)
                    {
                        totalPassed++;
                        categoryPassed++;
                        OMCCC.LogMessage($"  [PASS] {test.TestName}");
                    }
                    else
                    {
                        totalFailed++;
                        categoryFailed++;
                        failures.Add(result);
                        OMCCC.LogMessage($"  [FAIL] {test.TestName}");
                        OMCCC.LogMessage($"         {result.FailureMessage}");
                    }
                }

                OMCCC.LogMessage($"  Category Summary: {categoryPassed} passed, {categoryFailed} failed");
                OMCCC.LogMessage("");
            }

            OMCCC.LogMessage("========================================");
            OMCCC.LogMessage($"=== FINAL RESULTS: {totalPassed} passed, {totalFailed} failed ===");
            
            if (totalFailed > 0)
            {
                OMCCC.LogMessage("=== FAILURES DETAIL ===");
                foreach (var failure in failures)
                {
                    OMCCC.LogMessage($"  [{failure.TestCase.Category}] {failure.TestCase.TestName}");
                    OMCCC.LogMessage($"    Input: \"{failure.TestCase.InputValue}\"");
                    OMCCC.LogMessage($"    Target Type: {failure.TestCase.TargetType.Name}");
                    OMCCC.LogMessage($"    Expected: {(failure.TestCase.ExpectSuccess ? "Success" : "Failure")}");
                    OMCCC.LogMessage($"    Reason: {failure.FailureMessage}");
                }
            }
            OMCCC.LogMessage("========================================");
        }

        private static ITestResult ExecuteTest(ITestCase test)
        {
            try
            {
                var (success, value, error) = EventArgumentParser.TryParseWithDetails(test.TargetType, test.InputValue);

                if (test.ExpectSuccess)
                {
                    if (success)
                    {
                        if (test.Category == "Collection")
                        {
                            int expectedCount = (int)test.ExpectedValue;
                            int actualCount = GetCollectionCount(value);
                            
                            if (expectedCount != actualCount)
                            {
                                return new TestResult(test, false, $"Element count mismatch. Expected: {expectedCount}, Got: {actualCount}");
                            }
                        }
                        else if (test.ExpectedValue != null)
                        {
                            if (!test.ExpectedValue.Equals(value))
                            {
                                return new TestResult(test, false, $"Value mismatch. Expected: {test.ExpectedValue}, Got: {value}");
                            }
                        }
                        return new TestResult(test, true, null);
                    }
                    return new TestResult(test, false, error ?? "Expected success but parsing failed");
                }
                else
                {
                    if (!success)
                    {
                        return new TestResult(test, true, null);
                    }
                    return new TestResult(test, false, $"Expected failure but parsing succeeded with value: {value}");
                }
            }
            catch (Exception ex)
            {
                return new TestResult(test, false, $"Exception: {ex.Message}");
            }
        }

        private static int GetCollectionCount(object collection)
        {
            if (collection == null)
                return 0;

            if (collection is Array array)
                return array.Length;

            if (collection is ICollection col)
                return col.Count;

            if (collection is IEnumerable enumerable)
            {
                int count = 0;
                foreach (var item in enumerable)
                    count++;
                return count;
            }

            return 0;
        }
    }

    internal class TestResult : ITestResult
    {
        public bool Passed { get; }
        public string FailureMessage { get; }
        public ITestCase TestCase { get; }

        public TestResult(ITestCase testCase, bool passed, string failureMessage)
        {
            TestCase = testCase;
            Passed = passed;
            FailureMessage = failureMessage ?? string.Empty;
        }
    }
}
#endif
