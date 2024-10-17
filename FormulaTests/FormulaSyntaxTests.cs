// <copyright file="FormulaSyntaxTests.cs" company="UofU-CS3500">
//   Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

namespace CS3500.Formula;

using CS3500.Formula;

/// <summary>
/// Author:    Nandhini Ramanathan
/// Partner:   None
/// Date:      September 6,2024
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Nandhini Ramanathan - This work may not
///            be copied for use in Academic Coursework.
///
/// I, Nandhini Ramanathan, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All
/// references used in the completion of the assignments are cited
/// in my README file.
///
/// File Contents
///    This file contains MS unit tests for the formula class. It tests the three main public methods:
///    Formula Constructor, GetVariables, and ToString, along with implicitly testing the private helper methods.
/// </summary>

/// <summary>
///   <para>
///     The following class is a tester class for the Formula class.
///   </para>
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
    /// </summary>
    [TestMethod]
    [ExpectedException( typeof( FormulaFormatException ) )]
    public void FormulaConstructor_TestNoTokens_Invalid( )
    {
        _ = new Formula( string.Empty );
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
    ///     This test makes sure a single positive scientific notation number token using a small e is still valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestPositiveScientificNotationSmalle_Valid()
    {
        _ = new Formula("2e6");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a single positive scientific notation number token using a capital E is still valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestPositiveScientificNotationCapitalE_Valid()
    {
        _ = new Formula("8E6");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a single positive scientific notation number token using a decimal in front of the E is still valid.
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
    ///     This test makes sure a single positive scientific notation number token using a decimal in front of the e is still valid.
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
    ///     This test makes sure an operator token by itself is invalid. This also tests that having an operator as the first token is invalid.
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
    public void FormulaConstructor_TestVariableFollowedByAScientificNotationNumber_Invalid()
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
    ///     This test makes sure that a decimal beginning with a decimal point is still valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_DecimalPointFollowedByNumber_Valid()
    {
        _ = new Formula(".9");
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure that a number followed by a decimal point is still valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_NumberFollowedByDecimalPoint_Valid()
    {
        _ = new Formula("7.");
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
    ///     This test makes sure a single parenthesis token by itself is invalid.
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
    public void FormulaConstructor_TestBalancedParenthesisInWrongOrder_Invalid()
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
    ///     This test makes sure that having a closing parenthesis as the first token is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestFirstTokenClosingParenthesis_Invalid()
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

    /// <summary>
    ///    <para>
    ///     This test makes sure an operator as the last token is invalid.
    ///    </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestLastTokenOperator_Invalid()
    {
        _ = new Formula("0 +");
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
    ///     This test makes sure a that a formula containing no variables returns an empty set.
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

    /// <summary>
    ///   <para>
    ///     This test makes sure a that a formula containing a single variable gets normalized and returned in the toString method.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_SingleVariableToken_AreEqual()
    {
        Formula formula = new Formula("x568");
        Assert.AreEqual("X568", formula.ToString());
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a that a formula containing variables with operation signs returns the expected string.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_SimpleFormulaWithVariables_AreEqual()
    {
        Formula formula = new Formula("x234+y78");
        Assert.AreEqual("X234+Y78", formula.ToString());
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a that a formula containing integers returns the expected string.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_NumbersInFormula_AreEqual()
    {
        Formula formula = new Formula("6/8+0");
        Assert.AreEqual("6/8+0", formula.ToString());
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a that a formula containing parenthesis still returns the expected string.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_ParenthesisAroundTokens_AreEqual()
    {
        Formula formula = new Formula("(P56789)+((8)+(y6))");
        Assert.AreEqual("(P56789)+((8)+(Y6))", formula.ToString());
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a that a scientific notation value returns the expected string.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_ScientificNotationNumber_AreEqual()
    {
        Formula formula = new Formula("10e-67");
        Assert.AreEqual("1E-66", formula.ToString());
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a that a decimal with many leading zeroes is changed to standard form by double.tryParse.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_DecimalWithManyZeroes_AreEqual()
    {
        Formula formula = new Formula("1.00000000");
        Assert.AreEqual("1", formula.ToString());
    }

    /// <summary>
    ///   <para>
    ///     This test makes sure a that a formula with many spaces still returns a valid string without the spaces.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_SpacesInValidFormula_AreEqual()
    {
        Formula formula = new Formula(" 6.7   + 8");
        Assert.AreEqual("6.7+8", formula.ToString());
    }
}