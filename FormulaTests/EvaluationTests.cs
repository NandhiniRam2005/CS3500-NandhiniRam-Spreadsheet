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