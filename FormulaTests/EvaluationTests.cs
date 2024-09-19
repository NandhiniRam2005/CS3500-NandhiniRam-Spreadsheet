// <copyright file="EvaluationTests.cs" company="UofU-CS3500">
//   Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

/// <summary>
/// Author:    Nandhini Ramanathan
/// Partner:   None
/// Date:      September 20,2024
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
///    This file contains MS unit tests for the formula class evaluation methods and equality between Formulas.
/// </summary>

namespace CS3500.Formula;

using CS3500.Formula;

/// <summary>
///   <para>
///     The following class is a tester class for the Formula class,
///     focusing on the methods that evaluate the Formula and check for equality.
///   </para>
/// </summary>
[TestClass]
public class EvaluationTests
{
    // --- Tests for Evaluate method ---

    /// <summary>
    ///     Test addition between variables.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_SimpleAdditionWithVariables_ExpectedResult()
    {
        var formula = new Formula("A1 + B1");
        var result = formula.Evaluate(MyVariables);
        Assert.AreEqual(5.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test subtraction between variables.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_SimpleSubtractionWithVariables_ExpectedResult()
    {
        var formula = new Formula("A1 - B1");
        var result = formula.Evaluate(MyVariables);
        Assert.AreEqual(-1.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test multiplication between variables.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_SimpleMultiplicationWithVariables_ExpectedResult()
    {
        var formula = new Formula("A1 * B1");
        var result = formula.Evaluate(MyVariables);
        Assert.AreEqual(6.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test division between variables.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_SimpleDivisionWithVariables_ExpectedResult()
    {
        var formula = new Formula("B1 / A1");
        var result = formula.Evaluate(MyVariables);
        Assert.AreEqual(1.5, result);
    }

    /// <summary>
    ///   <para>
    ///     Test addition between a variable and a number.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_AdditionWithVariableAndConstant_ExpectedResult()
    {
        var formula = new Formula("A1 + 3");
        var result = formula.Evaluate(MyVariables);
        Assert.AreEqual(5.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test subtraction between a variable and a number.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_SubtractionWithVariableAndConstant_ExpectedResult()
    {
        var formula = new Formula("B1 - 1");
        var result = formula.Evaluate(MyVariables);
        Assert.AreEqual(2.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test multiplication between a variable and a number.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_MultiplicationWithVariableAndConstant_ExpectedResult()
    {
        var formula = new Formula("A1 * 4");
        var result = formula.Evaluate(MyVariables);
        Assert.AreEqual(8.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test division between a variable and a number.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_DivisionWithVariableAndConstant_ExpectedResult()
    {
        var formula = new Formula("B1 / 3");
        var result = formula.Evaluate(MyVariables);
        Assert.AreEqual(1.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test formula with unknown variable, should throw FormulaError.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_WithUnknownVariable_FormulaError()
    {
        var formula = new Formula("C1 + 1");
        var result = formula.Evaluate(MyVariables);
        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    /// <summary>
    ///   <para>
    ///     Test formula with a combination of variables and numbers.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_ComplexExpressionWithVariablesAndConstants_ExpectedResult()
    {
        var formula = new Formula("2 / A1 + B1 * A1 + 8");
        var result = formula.Evaluate(MyVariables);
        Assert.AreEqual(15.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test complex formula with parentheses.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_ExpressionWithParentheses_ExpectedResult()
    {
        var formula = new Formula("A1 + B1 / (A1 + 1)");
        var result = formula.Evaluate(MyVariables);
        Assert.AreEqual(3.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test formula with division by zero, should throw FormulaError.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_DivisionByZero_FormulaError()
    {
        var formula = new Formula("A1 / 0");
        var result = formula.Evaluate(MyVariables);
        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    /// <summary>
    ///   <para>
    ///     Test addition with nested parentheses.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_NestedParentheses_Addition_AreEqual()
    {
        Formula formula = new Formula("((A1 + A1) + (A1 + A1))");
        var result = formula.Evaluate((name) => 5);
        Assert.AreEqual(20.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test subtraction with nested parentheses.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_NestedParentheses_Subtraction_AreEqual()
    {
        var formula = new Formula("((A1 - A1) - (A1 - A1))");
        var result = formula.Evaluate((name) => 5);
        Assert.AreEqual(0.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test complex nested expression with various operators.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_ComplexNestedExpression_AreEqual()
    {
        var formula = new Formula("(A1 + (A1 - (A1 * 2) / A1) * 2)");
        var result = formula.Evaluate((name) => 5);
        Assert.AreEqual(11.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test formula with single variable "A1", should evaluate to 2.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_SingleVariable()
    {
        var formula = new Formula("A1");
        var result = formula.Evaluate(MyVariables);
        Assert.AreEqual(2.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test addition between two constants.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_AdditionWithConstants_ExpectedResult()
    {
        var formula = new Formula("2 + 3");
        var result = formula.Evaluate(name => 0);
        Assert.AreEqual(5.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test subtraction between two constants.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_SubtractionWithConstants_ExpectedResult()
    {
        var formula = new Formula("5-9");
        var result = formula.Evaluate(name => 0);
        Assert.AreEqual(-4.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test multiplication between two constants.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_MultiplicationWithConstants_ExpectedResult()
    {
        var formula = new Formula("2 * 3");
        var result = formula.Evaluate(name => 0);
        Assert.AreEqual(6.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test division between two constants.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_DivisionWithConstants_ExpectedResult()
    {
        var formula = new Formula("0 / 3");
        var result = formula.Evaluate(name => 0);
        Assert.AreEqual(0.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test complex equation with parenthesis with constants.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_ComplexEquationConstantWithParentheses_ExpectedResult()
    {
        var formula = new Formula("(8-9)*7+((87-900)/6)");
        var result = formula.Evaluate(name => 0);
        Assert.AreEqual(-142.5, result);
    }

    /// <summary>
    ///   <para>
    ///     Test division by zero with nested parenthesis.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_DivisionByZeroWithParentheses_FormulaError()
    {
        var formula = new Formula("(((6 + 7)/0 - (0/0))/0)");
        var result = formula.Evaluate(name => 0);
        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    /// <summary>
    ///   <para>
    ///     Test multiplication after a closing parentheses.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_MultiplicationAfterParentheses_FormulaError()
    {
        var formula = new Formula("(8-9)*(8*6)");
        var result = formula.Evaluate(name => 0);
        Assert.AreEqual(-48.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test formula with division by zero where the variable equals 0, should throw FormulaError.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_DivisionByZeroVariable_FormulaError()
    {
        var formula = new Formula("9/A1");
        var result = formula.Evaluate(name => 0);
        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    /// <summary>
    ///   <para>
    ///     Test evaluating a formula when division by zero is triggered by subtraction.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_DivisionByZeroIsTriggered_FormulaError()
    {
        var formula = new Formula("10 / (2 - 2)");
        Lookup lookup = variable => 0;

        var result = formula.Evaluate(lookup);
        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    /// <summary>
    ///   <para>
    ///     Test evaluating a formula with nested parentheses only constants.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_ExpressionWithParentheses_AreEqual()
    {
        var formula = new Formula("5 + (3 * (2 + 1))");
        Lookup lookup = variable => 0;

        var result = formula.Evaluate(lookup);
        Assert.AreEqual(14.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test evaluating a formula when division by zero is triggered with a subtraction in a complex formula.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_ComplexExpressionWithTriggeredDivisionByZero_FormulaError()
    {
        var formula = new Formula("(2 + 3) * (5 / (2 - 2))");
        Lookup lookup = variable => 0;

        var result = formula.Evaluate(lookup);
        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    /// <summary>
    ///   <para>
    ///     Test evaluating a single number.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_Constant_AreEqual()
    {
        var formula = new Formula("90");
        var result = formula.Evaluate(name => 0);
        Assert.AreEqual(90.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test evaluating a single number with parentheses.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_ConstantWithParentheses_AreEqual()
    {
        var formula = new Formula("(90)");
        var result = formula.Evaluate(name => 0);
        Assert.AreEqual(90.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test evaluating a single variable.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_Variable_AreEqual()
    {
        var formula = new Formula("H890");
        var result = formula.Evaluate(name => 899);
        Assert.AreEqual(899.0, result);
    }

    /// <summary>
    ///   <para>
    ///     Test evaluating a single variable with parentheses.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_VariableWithParentheses_AreEqual()
    {
        var formula = new Formula("(H890)");
        var result = formula.Evaluate(name => 899);
        Assert.AreEqual(899.0, result);
    }

    // --- Tests for GetHashCode method ---

    /// <summary>
    ///   <para>
    ///     Test GetHashCode for identical formulas.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetHashCode_IdenticalFormulas_SameHashCode()
    {
        var formula1 = new Formula("A1 + B1");
        var formula2 = new Formula("A1 + B1");
        Assert.AreEqual(formula1.GetHashCode(), formula2.GetHashCode());
    }

    /// <summary>
    ///   <para>
    ///     Test GetHashCode for different formulas.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetHashCode_DifferentFormulas_DifferentHashCodes()
    {
        var formula1 = new Formula("A1 + B1");
        var formula2 = new Formula("A1 - B1");
        Assert.AreNotEqual(formula1.GetHashCode(), formula2.GetHashCode());
    }

    /// <summary>
    ///   <para>
    ///     Test GetHashCode for same formulas, both are constants.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetHashCode_SameConstant_SameHashCodes()
    {
        var formula1 = new Formula("69");
        var formula2 = new Formula("69");
        Assert.AreEqual(formula1.GetHashCode(), formula2.GetHashCode());
    }

    /// <summary>
    ///   <para>
    ///     Test GetHashCode for mathematically equivalent statements but have different hashcodes.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetHashCode_MathematicallyEquivalentButDifferentHashcode_DifferentHashCodes()
    {
        var formula1 = new Formula("(69)");
        var formula2 = new Formula("69");
        Assert.AreNotEqual(formula1.GetHashCode(), formula2.GetHashCode());
    }

    // --- Tests for Equals method ---

    /// <summary>
    ///   <para>
    ///     Test Equals method with identical formulas.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Equals_IdenticalFormulas_True()
    {
        var formula1 = new Formula("A1 + B1");
        var formula2 = new Formula("A1 + B1");
        Assert.IsTrue(formula1.Equals(formula2));
    }

    /// <summary>
    ///   <para>
    ///     Test Equals method with a null formula.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Equals_WithNull_False()
    {
        var formula1 = new Formula("A1 + B1");
        Formula? formula2 = null;
        Assert.IsFalse(formula1.Equals(formula2));
    }

    /// <summary>
    ///   <para>
    ///     Test Equals method with a formula and a different typed object but with same string content.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Equals_DifferentObject_False()
    {
        var formula1 = new Formula("A1 + B1");
        string hello = "A1 + B1";
        Assert.IsFalse(formula1.Equals(hello));
    }

    /// <summary>
    ///   <para>
    ///     Test Equals method with a formula leading spaces.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Equals_LeadingSpacesInFormula_True()
    {
        var formula1 = new Formula("     8 +    90 ");
        var formula2 = new Formula("8+90");
        Assert.IsTrue(formula1.Equals(formula2));
    }

    /// <summary>
    ///   <para>
    ///     Test Equals method with different formulas.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Equals_DifferentFormulas_False()
    {
        var formula1 = new Formula("A1 + B1");
        var formula2 = new Formula("A1 * B1");
        Assert.IsFalse(formula1.Equals(formula2));
    }

    /// <summary>
    ///   <para>
    ///     Test Equals for same formulas, both are constants.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Equals_SameConstant_True()
    {
        var formula1 = new Formula("69");
        var formula2 = new Formula("69");
        Assert.IsTrue(formula1.Equals(formula2));
    }

    /// <summary>
    ///   <para>
    ///     Test Equals for mathematically equivalent statements but are not equal.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Equals_MathematicallyEquivalentButNotEqual_False()
    {
        var formula1 = new Formula("(69)");
        var formula2 = new Formula("69");
        Assert.IsFalse(formula1.Equals(formula2));
    }

    // --- Tests for == operator ---

    /// <summary>
    ///   <para>
    ///     Test == operator with identical formulas.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void OperatorEquals_IdenticalFormulas_True()
    {
        var formula1 = new Formula("A1 + B1");
        var formula2 = new Formula("A1 + B1");
        Assert.IsTrue(formula1 == formula2);
    }

    /// <summary>
    ///   <para>
    ///     Test == operator with different formulas.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void OperatorEquals_DifferentFormulas_False()
    {
        var formula1 = new Formula("A1 + B1");
        var formula2 = new Formula("A1 - B1");
        Assert.IsFalse(formula1 == formula2);
    }

    /// <summary>
    ///   <para>
    ///     Test == for same formulas, both are constants.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void EqualsOperator_SameConstant_True()
    {
        var formula1 = new Formula("69");
        var formula2 = new Formula("69");
        Assert.IsTrue(formula1 == formula2);
    }

    /// <summary>
    ///   <para>
    ///     Test == for mathematically equivalent statements but are not equal.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void EqualsOperator_MathematicallyEquivalentButNotEqual_False()
    {
        var formula1 = new Formula("(69)");
        var formula2 = new Formula("69");
        Assert.IsFalse(formula1 == formula2);
    }

    // --- Tests for != operator ---

    /// <summary>
    ///   <para>
    ///     Test != operator with identical formulas.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void OperatorNotEquals_IdenticalFormulas_False()
    {
        var formula1 = new Formula("A1 + B1");
        var formula2 = new Formula("A1 + B1");
        Assert.IsFalse(formula1 != formula2);
    }

    /// <summary>
    ///   <para>
    ///     Test != operator with different formulas.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void OperatorNotEquals_DifferentFormulas_True()
    {
        var formula1 = new Formula("A1 + B1");
        var formula2 = new Formula("A1 - B1");
        Assert.IsTrue(formula1 != formula2);
    }

    /// <summary>
    ///   <para>
    ///     Test != for same formulas, both are constants.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void NotEqualsOperator_SameConstant_False()
    {
        var formula1 = new Formula("69");
        var formula2 = new Formula("69");
        Assert.IsFalse(formula1 != formula2);
    }

    /// <summary>
    ///   <para>
    ///     Test != for mathematically equivalent statements but are not equal.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void NotEqualsOperator_MathematicallyEquivalentButNotEqual_True()
    {
        var formula1 = new Formula("(69)");
        var formula2 = new Formula("69");
        Assert.IsTrue(formula1 != formula2);
    }

    /// <summary>
    /// This method is a Lookup Delegate function From Assignment Instructions.
    /// </summary>
    /// <param name="name">name represents the current variable.</param>
    /// <returns> A double that the variable holds.</returns>
    /// <exception cref="ArgumentException">Throws an argument exception when the variable is unknown.</exception>
    private double MyVariables(string name)
    {
        if (name == "A1")
        {
            return 2;
        }
        else if (name == "B1")
        {
            return 3;
        }
        else
        {
            throw new ArgumentException("I don't know that variable");
        }
    }
}