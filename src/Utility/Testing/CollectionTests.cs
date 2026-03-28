#if DEBUG
using System;
using System.Collections.Generic;

namespace OutwardModsCommunicatorChatControl.Utility.Testing
{
    public static class CollectionTests
    {
        public static List<ITestCase> GetTestCases()
        {
            return new List<ITestCase>
            {
                // Int Arrays
                new CollectionTestCase("Int32_Array", "ValidMultipleValues", typeof(int[]), "1 2 3", true, 3),
                new CollectionTestCase("Int32_Array", "ValidSingleValue", typeof(int[]), "42", true, 1),
                new CollectionTestCase("Int32_Array", "ValidTwoValues", typeof(int[]), "100 200", true, 2),
                new CollectionTestCase("Int32_Array", "ValidWithNegative", typeof(int[]), "-1 0 1", true, 3),
                new CollectionTestCase("Int32_Array", "ValidEmptyInput", typeof(int[]), "", false, 0),
                new CollectionTestCase("Int32_Array", "InvalidMixedTypes", typeof(int[]), "1 abc 3", false, 0),
                new CollectionTestCase("Int32_Array", "InvalidPartialFailure", typeof(int[]), "1 2 abc 4", false, 0),

                // String Arrays
                new CollectionTestCase("String_Array", "ValidMultipleValues", typeof(string[]), "hello world foo", true, 3),
                new CollectionTestCase("String_Array", "ValidSingleValue", typeof(string[]), "hello", true, 1),
                new CollectionTestCase("String_Array", "ValidEmptyValues", typeof(string[]), "a b c", true, 3),
                new CollectionTestCase("String_Array", "ValidEmptyInput", typeof(string[]), "", false, 0),

                // Float Arrays
                new CollectionTestCase("Float_Array", "ValidMultipleDecimals", typeof(float[]), "1.1 2.2 3.3", true, 3),
                new CollectionTestCase("Float_Array", "ValidIntegersAsFloats", typeof(float[]), "1 2 3", true, 3),
                new CollectionTestCase("Float_Array", "ValidMixed", typeof(float[]), "1.5 2 -3.5", true, 3),
                new CollectionTestCase("Float_Array", "InvalidMixedTypes", typeof(float[]), "1.1 abc", false, 0),

                // Double Arrays
                new CollectionTestCase("Double_Array", "ValidMultipleDecimals", typeof(double[]), "1.1 2.2 3.3", true, 3),
                new CollectionTestCase("Double_Array", "InvalidMixedTypes", typeof(double[]), "1.1 abc", false, 0),

                // Long Arrays
                new CollectionTestCase("Int64_Array", "ValidMultipleValues", typeof(long[]), "1000000000 2000000000 3000000000", true, 3),
                new CollectionTestCase("Int64_Array", "InvalidMixedTypes", typeof(long[]), "1000 abc", false, 0),

                // Bool Arrays
                new CollectionTestCase("Boolean_Array", "ValidMultipleTrue", typeof(bool[]), "true true false", true, 3),
                new CollectionTestCase("Boolean_Array", "ValidNumericBooleans", typeof(bool[]), "1 0 yes no", true, 4),
                new CollectionTestCase("Boolean_Array", "InvalidMixedTypes", typeof(bool[]), "true maybe false", false, 0),

                // Char Arrays
                new CollectionTestCase("Char_Array", "ValidMultipleChars", typeof(char[]), "a b c", true, 3),
                new CollectionTestCase("Char_Array", "ValidSingleChar", typeof(char[]), "x", true, 1),
                new CollectionTestCase("Char_Array", "InvalidMultiCharElements", typeof(char[]), "ab cd", false, 0),

                // List<int>
                new CollectionTestCase("List_Int32", "ValidMultipleValues", typeof(List<int>), "10 20 30", true, 3),
                new CollectionTestCase("List_Int32", "ValidSingleValue", typeof(List<int>), "42", true, 1),
                new CollectionTestCase("List_Int32", "ValidEmptyInput", typeof(List<int>), "", false, 0),
                new CollectionTestCase("List_Int32", "InvalidMixedTypes", typeof(List<int>), "10 abc", false, 0),

                // List<string>
                new CollectionTestCase("List_String", "ValidMultipleValues", typeof(List<string>), "a b c", true, 3),
                new CollectionTestCase("List_String", "ValidSingleValue", typeof(List<string>), "hello", true, 1),
                new CollectionTestCase("List_String", "ValidEmptyValues", typeof(List<string>), "x y z", true, 3),

                // List<float>
                new CollectionTestCase("List_Float", "ValidMultipleDecimals", typeof(List<float>), "1.1 2.2 3.3", true, 3),
                new CollectionTestCase("List_Float", "InvalidMixedTypes", typeof(List<float>), "1.1 abc", false, 0),

                // IEnumerable<int>
                new CollectionTestCase("IEnumerable_Int32", "ValidMultipleValues", typeof(IEnumerable<int>), "100 200", true, 2),
                new CollectionTestCase("IEnumerable_Int32", "ValidSingleValue", typeof(IEnumerable<int>), "42", true, 1),
                new CollectionTestCase("IEnumerable_Int32", "InvalidMixedTypes", typeof(IEnumerable<int>), "100 abc", false, 0),

                // IEnumerable<string>
                new CollectionTestCase("IEnumerable_String", "ValidMultipleValues", typeof(IEnumerable<string>), "hello world", true, 2),
                new CollectionTestCase("IEnumerable_String", "ValidSingleValue", typeof(IEnumerable<string>), "test", true, 1),

                // ICollection<int>
                new CollectionTestCase("ICollection_Int32", "ValidMultipleValues", typeof(ICollection<int>), "5 10 15", true, 3),
                new CollectionTestCase("ICollection_Int32", "ValidSingleValue", typeof(ICollection<int>), "42", true, 1),
                new CollectionTestCase("ICollection_Int32", "InvalidMixedTypes", typeof(ICollection<int>), "5 abc", false, 0),

                // HashSet<int>
                new CollectionTestCase("HashSet_Int32", "ValidMultipleValues", typeof(HashSet<int>), "1 2 3", true, 3),
                new CollectionTestCase("HashSet_Int32", "ValidSingleValue", typeof(HashSet<int>), "42", true, 1),
                new CollectionTestCase("HashSet_Int32", "ValidDuplicatesRemoved", typeof(HashSet<int>), "1 1 2 2 3", true, 3),
                new CollectionTestCase("HashSet_Int32", "InvalidMixedTypes", typeof(HashSet<int>), "1 abc", false, 0),

                // HashSet<string>
                new CollectionTestCase("HashSet_String", "ValidMultipleValues", typeof(HashSet<string>), "a b c", true, 3),
                new CollectionTestCase("HashSet_String", "ValidDuplicatesRemoved", typeof(HashSet<string>), "a a b b", true, 2),

                // Character.Factions Arrays
                new CollectionTestCase("Character_Factions_Array", "ValidMultipleValues", typeof(Character.Factions[]), "Player Bandits Deer", true, 3),
                new CollectionTestCase("Character_Factions_Array", "ValidSingleValue", typeof(Character.Factions[]), "Player", true, 1),
                new CollectionTestCase("Character_Factions_Array", "ValidMixedCase", typeof(Character.Factions[]), "player MERCHANTS deer", true, 3),
                new CollectionTestCase("Character_Factions_Array", "InvalidMixedTypes", typeof(Character.Factions[]), "Player InvalidEnum", false, 0),
                new CollectionTestCase("Character_Factions_Array", "ValidTrailingSpace", typeof(Character.Factions[]), "Player ", true, 1),

                // List<Character.Factions>
                new CollectionTestCase("List_Character_Factions", "ValidMultipleValues", typeof(List<Character.Factions>), "Merchants Mercs", true, 2),
                new CollectionTestCase("List_Character_Factions", "ValidSingleValue", typeof(List<Character.Factions>), "Bandits", true, 1),
                new CollectionTestCase("List_Character_Factions", "ValidMixedCase", typeof(List<Character.Factions>), "player GOLDEN", true, 2),
                new CollectionTestCase("List_Character_Factions", "InvalidMixedTypes", typeof(List<Character.Factions>), "Player InvalidFaction", false, 0),

                // List<string> with whitespace handling
                new CollectionTestCase("List_String_Whitespace", "ValidLeadingWhitespace", typeof(List<string>), "  a b c", true, 3),
                new CollectionTestCase("List_String_Whitespace", "ValidTrailingWhitespace", typeof(List<string>), "a b c  ", true, 3),
                new CollectionTestCase("List_String_Whitespace", "ValidMultipleSpaces", typeof(List<string>), "a  b   c", true, 3),

                // Int arrays with whitespace handling
                new CollectionTestCase("Int32_Array_Whitespace", "ValidLeadingWhitespace", typeof(int[]), "  1 2 3", true, 3),
                new CollectionTestCase("Int32_Array_Whitespace", "ValidTrailingWhitespace", typeof(int[]), "1 2 3  ", true, 3),
                new CollectionTestCase("Int32_Array_Whitespace", "ValidMultipleSpaces", typeof(int[]), "1  2   3", true, 3),
            };
        }
    }

    internal class CollectionTestCase : ITestCase
    {
        public string Category => "Collection";
        public string TestName { get; }
        public Type TargetType { get; }
        public string InputValue { get; }
        public bool ExpectSuccess { get; }
        public object ExpectedValue { get; }

        public CollectionTestCase(string typeName, string testName, Type collectionType, string input, bool expectSuccess, int expectedCount)
        {
            TestName = $"{typeName}_{testName}";
            TargetType = collectionType;
            InputValue = input;
            ExpectSuccess = expectSuccess;
            ExpectedValue = expectedCount;
        }
    }
}
#endif
