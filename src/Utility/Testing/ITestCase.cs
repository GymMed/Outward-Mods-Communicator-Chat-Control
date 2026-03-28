using System;

namespace OutwardModsCommunicatorChatControl.Utility.Testing
{
    public interface ITestCase
    {
        string Category { get; }
        string TestName { get; }
        Type TargetType { get; }
        string InputValue { get; }
        bool ExpectSuccess { get; }
        object ExpectedValue { get; }
    }

    public interface ITestResult
    {
        bool Passed { get; }
        string FailureMessage { get; }
        ITestCase TestCase { get; }
    }
}
