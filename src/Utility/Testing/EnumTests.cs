#if DEBUG
using System;
using System.Collections.Generic;

namespace OutwardModsCommunicatorChatControl.Utility.Testing
{
    public static class EnumTests
    {
        public static List<ITestCase> GetTestCases()
        {
            return new List<ITestCase>
            {
                // Character.Factions enum - valid values
                new EnumTestCase("Character_Factions", "ValidPlayer_Lowercase", Character.Factions.Player, "player", true),
                new EnumTestCase("Character_Factions", "ValidPlayer_Uppercase", Character.Factions.Player, "PLAYER", true),
                new EnumTestCase("Character_Factions", "ValidPlayer_MixedCase", Character.Factions.Player, "Player", true),
                new EnumTestCase("Character_Factions", "ValidBandits", Character.Factions.Bandits, "Bandits", true),
                new EnumTestCase("Character_Factions", "ValidBandits_Lowercase", Character.Factions.Bandits, "bandits", true),
                new EnumTestCase("Character_Factions", "ValidDeer", Character.Factions.Deer, "Deer", true),
                new EnumTestCase("Character_Factions", "ValidDeer_Lowercase", Character.Factions.Deer, "deer", true),
                new EnumTestCase("Character_Factions", "ValidMerchants", Character.Factions.Merchants, "Merchants", true),
                new EnumTestCase("Character_Factions", "ValidMercs", Character.Factions.Mercs, "Mercs", true),
                new EnumTestCase("Character_Factions", "ValidTuanosaurs", Character.Factions.Tuanosaurs, "Tuanosaurs", true),
                new EnumTestCase("Character_Factions", "ValidHounds", Character.Factions.Hounds, "Hounds", true),
                new EnumTestCase("Character_Factions", "ValidGolden", Character.Factions.Golden, "Golden", true),
                new EnumTestCase("Character_Factions", "ValidCorruptionSpirit", Character.Factions.CorruptionSpirit, "CorruptionSpirit", true),
                new EnumTestCase("Character_Factions", "ValidNONE", Character.Factions.NONE, "NONE", true),
                new EnumTestCase("Character_Factions", "ValidNONE_Lowercase", Character.Factions.NONE, "none", true),

                // Character.Factions - invalid values
                new EnumTestCase("Character_Factions", "InvalidNonExistent", Character.Factions.NONE, "InvalidFaction", false),
                new EnumTestCase("Character_Factions", "InvalidEmpty", Character.Factions.NONE, "", false),
                new EnumTestCase("Character_Factions", "InvalidPartialName", Character.Factions.NONE, "Band", false),

                // Nullable Character.Factions - valid values
                new NullableEnumTestCase("Nullable_Character_Factions", "ValidBandits", Character.Factions.Bandits, "Bandits", true),
                new NullableEnumTestCase("Nullable_Character_Factions", "ValidDeer", Character.Factions.Deer, "Deer", true),
                new NullableEnumTestCase("Nullable_Character_Factions", "ValidPlayer_Lowercase", Character.Factions.Player, "player", true),

                // Nullable Character.Factions - invalid values
                new NullableEnumTestCase("Nullable_Character_Factions", "InvalidNonExistent", null, "InvalidEnum", false),
                new NullableEnumTestCase("Nullable_Character_Factions", "InvalidEmpty", null, "", false),
            };
        }
    }

    internal class EnumTestCase : ITestCase
    {
        public string Category => "Enum";
        public string TestName { get; }
        public Type TargetType { get; }
        public string InputValue { get; }
        public bool ExpectSuccess { get; }
        public object ExpectedValue { get; }

        public EnumTestCase(string typeName, string testName, object expectedEnum, string input, bool expectSuccess)
        {
            TestName = $"{typeName}_{testName}";
            TargetType = expectedEnum?.GetType() ?? typeof(Character.Factions);
            InputValue = input;
            ExpectSuccess = expectSuccess;
            ExpectedValue = expectedEnum;
        }
    }

    internal class NullableEnumTestCase : ITestCase
    {
        public string Category => "NullableEnum";
        public string TestName { get; }
        public Type TargetType { get; }
        public string InputValue { get; }
        public bool ExpectSuccess { get; }
        public object ExpectedValue { get; }

        public NullableEnumTestCase(string typeName, string testName, object expectedEnum, string input, bool expectSuccess)
        {
            TestName = $"{typeName}_{testName}";
            TargetType = typeof(Nullable<>).MakeGenericType(typeof(Character.Factions));
            InputValue = input;
            ExpectSuccess = expectSuccess;
            ExpectedValue = expectedEnum;
        }
    }
}
#endif
