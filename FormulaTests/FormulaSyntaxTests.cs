// <copyright file="FormulaSyntaxTests.cs" company="UofU-CS3500">
//   Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors> Nandhini Ramanathan </authors>
// <date> August 25,2023 </date>

namespace CS3500.Formula;

using CS3500.Formula; // Change this using statement to use different formula implementations.

/// <summary>
///   <para>
///     The following class shows the basics of how to use the MSTest framework,
///     including:
///   </para>
///   <list type="number">
///     <item> How to catch exceptions. </item>
///     <item> How a test of valid code should look. </item>
///   </list>
/// </summary>
[TestClass]
public class FormulaSyntaxTests
{
    // --- Tests for One Token Rule ---

    /// <summary>
    ///   <para>
    ///     This test makes sure the right kind of exception is thrown
    ///     when trying to create a formula with no tokens.
    ///   </para>
    ///   <remarks>
    ///     <list type="bullet">
    ///       <item>
    ///         We use the _ (discard) notation because the formula object
    ///         is not used after that point in the method.  Note: you can also
    ///         use _ when a method must match an interface but does not use
    ///         some of the required arguments to that method.
    ///       </item>
    ///       <item>
    ///         string.Empty is often considered best practice (rather than using "") because it
    ///         is explicit in intent (e.g., perhaps the coder forgot to but something in "").
    ///       </item>
    ///       <item>
    ///         The name of a test method should follow the MS standard:
    ///         https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
    ///       </item>
    ///       <item>
    ///         All methods should be documented, but perhaps not to the same extent
    ///         as this one.  The remarks here are for your educational
    ///         purposes (i.e., a developer would assume another developer would know these
    ///         items) and would be superfluous in your code.
    ///       </item>
    ///       <item>
    ///         Notice the use of the attribute tag [ExpectedException] which tells the test
    ///         that the code should throw an exception, and if it doesn't an error has occurred;
    ///         i.e., the correct implementation of the constructor should result
    ///         in this exception being thrown based on the given poorly formed formula.
    ///       </item>
    ///     </list>
    ///   </remarks>
    ///   <example>
    ///     <code>
    ///        // here is how we call the formula constructor with a string representing the formula
    ///        _ = new Formula( "5+5" );
    ///     </code>
    ///   </example>
    /// </summary>
    [TestMethod]
    [ExpectedException( typeof( FormulaFormatException ) )]
    public void FormulaConstructor_TestNoTokens_Invalid( )
    {
        _ = new Formula( string.Empty );  // note: it is arguable that you should replace "" with string.Empty for readability and clarity of intent (e.g., not a cut and paste error or a "I forgot to put something there" error).
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure just a space is an invalid token.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestSpace_Invalid()
    {
        _ = new Formula(" ");
    }

    // --- Tests for Valid Token Rule ---

    /// <summary>
    ///   <para>
    ///     This test makes sure an operator followed by another operator in an equation has invalid syntax.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOperatorFollowedByOperator_Invalid()
    {
        _ = new Formula("5++5");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a single number token is still valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNumberToken_Valid()
    {
        _ = new Formula("69");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a single float value token is still valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFloatToken_Valid()
    {
        _ = new Formula("3.14");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a single postivie scientific notation number token using a small e is still valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestPositiveScientificNotationSmalle_Valid()
    {
        _ = new Formula("2e6");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a single postivie scientific notation number token using a capital E is still valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestPositiveScientificNotationCapitalE_Valid()
    {
        _ = new Formula("8E6");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a single postivie scientific notation number token using a decimal in front of the E is still valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestPositiveScientificNotationWithDecimalCapitalE_Valid()
    {
        _ = new Formula("2.1E6");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a single negative scientific notation number token using a decimal in front of the e is still valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNegativeScientificNotationWithDecimalSmalle_Valid()
    {
        _ = new Formula("38.9e-6");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a single negative scientific notation number token using a decimal in front of the E is still valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNegativeScientificNotationWithDecimalCapitalE_Valid()
    {
        _ = new Formula("2.1E-6");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a single postivie scientific notation number token using a decimal in front of the e is still valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestPositiveScientificNotationWithDecimalSmalle_Valid()
    {
        _ = new Formula("38.9e6");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a single negative scientific notation number token using a number in front of the E is still valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNegativeScientificNotation_Valid()
    {
        _ = new Formula("89E-9");
    }


    /// <summary>
    ///   <para>
    ///     This test makes sure a single negative scientific notation number token using a small e is still valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNegativeScientificNotationSmalle_Valid()
    {
        _ = new Formula("4e-8");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure an operator token by itself is invalid. This also tests that having an operator as the first token is invalid
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOperatorToken_Invalid()
    {
        _ = new Formula("*");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a letter with a digit following it is a valid variable token.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLetterVariableToken_Valid()
    {
        _ = new Formula("f1");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure an uppercase letter with a digit following it is a valid variable token.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestUppercaseLetterVariableToken_Valid()
    {
        _ = new Formula("H8");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure multiple letters with multiple digits following it is a valid variable token.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestMultipleLettersAndMultipleNumbersVariableToken_Valid()
    {
        _ = new Formula("AAab167");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure just a letter is an invalid variable token.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOnlyLetterVariableToken_Invalid()
    {
        _ = new Formula("C");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure just a letter followed by a scientific notation number is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestVariableFollowedByScientificaNotationNumber_Invalid()
    {
        _ = new Formula("C4e9");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure just a letter followed by a decimal number is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestVariableFollowedByDecimalNumber_Invalid()
    {
        _ = new Formula("C0.09");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a number followed by a letter is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestNumberFollowedByLetterVariableToken_Invalid()
    {
        _ = new Formula("12c");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure the $ is an invalid token.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestDollarSign_Invalid()
    {
        _ = new Formula("$");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure the @ is an invalid token.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestAtSign_Invalid()
    {
        _ = new Formula("@");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure that a float formatted incorrectly is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestMultipleDecimalNumber_Invalid()
    {
        _ = new Formula("5.9.0.6");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure that a formula with a variable is valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestVariableInFormula_Valid()
    {
        _ = new Formula("j890-890");
    }

    // --- Tests for Closing Parenthesis Rule

    /// <summary>
    ///   <para>
    ///     This test makes sure that an extra closing parenthesis in a formula is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraClosingParenthesis_InValid()
    {
        _ = new Formula("(11))");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure that an extra open parenthesis in a formula is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraOpenParenthesis_InValid()
    {
        _ = new Formula("((5)");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure that a closing parenthesis comes before an opening parenthesis in a formula is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosingBeforeOpenParenthesis_InValid()
    {
        _ = new Formula(")(9)");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure that many sets of parenthesis formatted correctly in a formula is valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestManyParenthesis_Valid()
    {
        _ = new Formula("(4)+(5)-(3)/(1)");
    }

    // --- Tests for Balanced Parentheses Rule

    /// <summary>
    ///   <para>
    ///     This test makes sure that many nested sets of parenthesis formatted correctly in a formula is valid. This also tests that having an open parenthesis token after an opening parenthesis is valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNestedParenthesis_Valid()
    {
        _ = new Formula("((((890))))");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a single prenthesis token by itself is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOneParenthesis_Invalid()
    {
        _ = new Formula("(");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a pair of parenthesis without a token inside is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOneParenthesisPair_Invalid()
    {
        _ = new Formula("()");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure that an unbalanced amount of open parenthesis is invalid, where only one closing parenthesis is present.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestUnbalancedOpenParenthesis_Valid()
    {
        _ = new Formula("890)");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure that a balanced number of closed and open parenthesis placed in the wrong order is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestBalencedParenthesisInWrongOrder_Invalid()
    {
        _ = new Formula("8)(7)(6");
    }

    // --- Tests for First Token Rule

    /// <summary>
    ///   <para>
    ///     Make sure a simple well formed formula is accepted by the constructor (the constructor
    ///     should not throw an exception).
    ///   </para>
    ///   <remarks>
    ///     This is an example of a test that is not expected to throw an exception, i.e., it succeeds.
    ///     In other words, the formula "1+1" is a valid formula which should not cause any errors.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenNumber_Valid( )
    {
        _ = new Formula( "1+1" );
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure that having a closing parenthesis as the fisrt token is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestFisrtTokenClosingParenthesis_Invalid()
    {
        _ = new Formula(")(987)");
    }

    // --- Tests for  Last Token Rule ---

    /// <summary>
    ///   <para>
    ///     This test makes sure that having a opening parenthesis as the last token is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestLastTokenOpenParenthesis_Invalid()
    {
        _ = new Formula("(987)(");
    }

    /// <summary>
    ///   <para>
    ///    This test makes sure an closing parenthesis as the last token is valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLastTokenClosingParentheses_Valid()
    {
        _ = new Formula("67+(90)");
    }

    // --- Tests for Parentheses/Operator Following Rule ---

    /// <summary>
    ///   <para>
    ///     Make sure a simple well formed formula with parathesis around it is valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestForParentheses_Valid()
    {
        _ = new Formula("(1+1)");
    }

    /// <summary>
    ///   <para>
    ///     Test that makes sure a token that follows an open parenthesis is an operator is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOpenParenthesisFollowedByOperator_Invalid()
    {
        _ = new Formula("(+90)");
    }

    // --- Tests for Extra Following Rule ---

    /// <summary>
    ///   <para>
    ///     Test that makes sure a token that follows an closing parenthesis is a variable is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosingParenthesisFollowedByVariable_Invalid()
    {
        _ = new Formula("(90)u56");
    }

    /// <summary>
    ///   <para>
    ///     Test that makes sure a formula with multiple spaces following each token is still valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestSpacing_Valid()
    {
        _ = new Formula("( 56     +   78)");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a number followed by a letter then followed by a number is invalid. No operator after a number.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestNumberFollowedByLetterThenNumberVariableToken_Invalid()
    {
        _ = new Formula("1a4");
    }

    // --- Tests for getVariables Method ---

    /// <summary>
    ///   <para>
    ///     This test makes sure a that a formula containing a single variable gets normalized and returned in the getVariables method.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetVariables_SingleVariableToken_IsTrueContains()
    {
        Formula formula = new Formula("x1");
        HashSet<string> variables = new HashSet<string>(formula.GetVariables());
        Assert.IsTrue(variables.Contains("X1"));
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a that a formula containing variables with operation signs returns the expected variables only.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetVariables_VariablesWithOperatorInbetween_SetEqualsTrue()
    {
        Formula formula = new Formula("x234 + y78");
        HashSet<string> variables = new HashSet<string>(formula.GetVariables());
        Assert.IsTrue(variables.SetEquals(new[] { "X234", "Y78" }));
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a that a formula containing no variables returns an empty set
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetVariables_NoVariableToken_EmptySet()
    {
        Formula formula = new Formula("6/8+0");
        HashSet<string> variables = new HashSet<string>(formula.GetVariables());
        Assert.AreEqual(0, variables.Count);
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a that a formula containing parenthesis around variables still returns the expected variable set.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetVariables_ParenthesisAroundVariables_SetEqualsTrue()
    {
        Formula formula = new Formula("(P56789)+((8)+(y6))");
        HashSet<string> variables = new HashSet<string>(formula.GetVariables());
        Assert.IsTrue(variables.SetEquals(new[] { "Y6", "P56789" }));
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a that a scientific notation value is not a variable.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetVariables_ScientificNotationNumber_EmptySet()
    {
        Formula formula = new Formula("10e-67");
        HashSet<string> variables = new HashSet<string>(formula.GetVariables());
        Assert.AreEqual(0, variables.Count);
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a that a duplicate variable is only counted once.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetVariables_DuplicateVariables_OnlyAppearOnceInSet()
    {
        Formula formula = new Formula("e45 * e45 + y78");
        HashSet<string> variables = new HashSet<string>(formula.GetVariables());
        Assert.IsTrue(variables.SetEquals(new[] { "E45", "Y78" }));
    }

    // --- Tests for toString Method ---




}