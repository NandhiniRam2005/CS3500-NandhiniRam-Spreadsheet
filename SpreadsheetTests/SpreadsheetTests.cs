// <copyright file="SpreadsheetTests.cs" company="UofU-CS3500">
//   Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

namespace SpreadsheetTests;

using System.Collections.Generic;
using CS3500.Formula;
using CS3500.Spreadsheet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Author:    Nandhini Ramanathan
/// Partner:   None
/// Date:      September 27, 2024
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
///    This file contains MS unit tests for the spreadsheet class.
/// </summary>
[TestClass]
public class SpreadsheetTests
{
    // --- Tests for SetCellContents method ---

    /// <summary>
    ///     Tests setting cell contents to a number and retrieving it.
    /// </summary>
    [TestMethod]
    public void SetCellContents_Number_GetCellContents_ReturnsNumber()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", 42.0);

        Assert.AreEqual(42.0, spreadsheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Tests setting cell contents to text and retrieving it.
    /// </summary>
    [TestMethod]
    public void SetCellContents_Text_GetCellContents_ReturnsText()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A2", "Hello");

        Assert.AreEqual("Hello", spreadsheet.GetCellContents("A2"));
    }

    /// <summary>
    ///     Tests removing cell contents by setting it to an empty string.
    /// </summary>
    [TestMethod]
    public void SetCellContents_EmptyString_RemovesCell()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A3", "World");
        spreadsheet.SetCellContents("A3", string.Empty);

        Assert.AreEqual(string.Empty, spreadsheet.GetCellContents("A3"));
    }

    /// <summary>
    ///     Tests setting a cell to a formula that references itself and checks for a circular dependency.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContents_SelfReferencingFormula_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", new Formula("A1 + 1"));
    }

    /// <summary>
    ///     Tests setting cell contents to a formula and retrieving it.
    /// </summary>
    [TestMethod]
    public void SetCellContents_Formula_GetCellContents_ReturnsFormula()
    {
        var spreadsheet = new Spreadsheet();
        var formula = new Formula("A1 + 5");
        spreadsheet.SetCellContents("B1", formula);

        Assert.AreEqual(formula, spreadsheet.GetCellContents("B1"));
    }

    /// <summary>
    ///     Tests setting cell contents that create a circular dependency and expects an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContents_CircularDependency_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", new Formula("B1"));
        spreadsheet.SetCellContents("B1", new Formula("A1"));
    }

    /// <summary>
    ///     Tests setting cell contents with an invalid cell name and expects an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContents_InvalidCellName_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("1A", "Invalid");
    }

    /// <summary>
    ///     Tests adding multiple cells and retrieving their contents.
    /// </summary>
    [TestMethod]
    public void SetMultipleCellContents_GetCellContents_ReturnsAllValues()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", 10.0);
        spreadsheet.SetCellContents("B1", "Hello");
        spreadsheet.SetCellContents("C1", new Formula("A1 + 5"));

        Assert.AreEqual(10.0, spreadsheet.GetCellContents("A1"));
        Assert.AreEqual("Hello", spreadsheet.GetCellContents("B1"));
        Assert.AreEqual(new Formula("A1 + 5"), spreadsheet.GetCellContents("C1"));
    }

    /// <summary>
    ///     Tests setting a cell with a complex formula and retrieving its value.
    /// </summary>
    [TestMethod]
    public void SetCellContents_ComplexFormula_GetCellContents_ReturnsFormula()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", 10.0);
        spreadsheet.SetCellContents("B1", new Formula("A1 * 2 + 3"));

        Assert.AreEqual(new Formula("A1 * 2 + 3"), spreadsheet.GetCellContents("B1"));
    }

    /// <summary>
    ///     Tests setting a cell with a formula that references multiple cells.
    /// </summary>
    [TestMethod]
    public void SetCellContents_MultipleCellReferences_Formula()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", 5.0);
        spreadsheet.SetCellContents("B1", 10.0);
        spreadsheet.SetCellContents("C1", new Formula("A1 + B1"));

        Assert.AreEqual(new Formula("A1 + B1"), spreadsheet.GetCellContents("C1"));
    }

    /// <summary>
    ///     Tests setting a cell with a formula that references a non-existent cell.
    /// </summary>
    [TestMethod]
    public void SetCellContents_ReferenceToNonExistentCell_Formula()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", new Formula("B1 + 1"));

        Assert.AreEqual(new Formula("B1 + 1"), spreadsheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Tests setting a cell with a valid formula, then setting it to text, and checks content.
    /// </summary>
    [TestMethod]
    public void SetCellContents_OverwriteFormulaWithText_ReturnsText()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", new Formula("B1 + 2"));
        spreadsheet.SetCellContents("A1", "Yuji");

        Assert.AreEqual("Yuji", spreadsheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Tests setting a very large numeric value in a cell and retrieving it.
    /// </summary>
    [TestMethod]
    public void SetCellContents_LargeNumber_ReturnsLargeNumber()
    {
        var spreadsheet = new Spreadsheet();
        double largeNumber = 2e100;
        spreadsheet.SetCellContents("A1", largeNumber);

        Assert.AreEqual(largeNumber, spreadsheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Tests setting a negative number in a cell and retrieving it.
    /// </summary>
    [TestMethod]
    public void SetCellContents_NegativeNumber_ReturnsNegativeNumber()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", -123.456);

        Assert.AreEqual(-123.456, spreadsheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Tests setting a formula containing a large number of variables and retrieving it.
    /// </summary>
    [TestMethod]
    public void SetCellContents_FormulaWithManyVariables_GetCellContents_ReturnsFormula()
    {
        var spreadsheet = new Spreadsheet();
        var formula = new Formula("A1 + B1 + C1 + D1 + E1 + F1 + G1 + H1");
        spreadsheet.SetCellContents("X1", formula);

        Assert.AreEqual(formula, spreadsheet.GetCellContents("X1"));
    }

    /// <summary>
    ///     Tests clearing a cell that contains a formula and checks if it is empty.
    /// </summary>
    [TestMethod]
    public void SetCellContents_CheckIfEmpty_ReturnsEmptyString()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", new Formula("B1 + 5"));
        spreadsheet.SetCellContents("A1", string.Empty);

        Assert.AreEqual(string.Empty, spreadsheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Tests that a cell whose contents have not been set returns and empty string when GetCellContents is called.
    /// </summary>
    [TestMethod]
    public void SetCellContents_CellContentsNeverSet_ReturnsEmptyString()
    {
        var spreadsheet = new Spreadsheet();

        Assert.AreEqual(string.Empty, spreadsheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Tests setting multiple cells and clearing them, ensuring all are empty.
    /// </summary>
    [TestMethod]
    public void SeCellContents_ClearMultipleCells_AllEmpty()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", 10.0);
        spreadsheet.SetCellContents("B1", "Hello");
        spreadsheet.SetCellContents("C1", new Formula("A1 + 5"));

        // Clear all cells
        spreadsheet.SetCellContents("A1", string.Empty);
        spreadsheet.SetCellContents("B1", string.Empty);
        spreadsheet.SetCellContents("C1", string.Empty);

        Assert.AreEqual(string.Empty, spreadsheet.GetCellContents("A1"));
        Assert.AreEqual(string.Empty, spreadsheet.GetCellContents("B1"));
        Assert.AreEqual(string.Empty, spreadsheet.GetCellContents("C1"));
    }

    /// <summary>
    ///     Tests setting multiple formulas where the circular dependency is detected after multiple entries where one
    ///     cell causes a circularDependencyException but the other cell does not.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContents_MultipleCellReferences_CircularDependencyThrows()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", new Formula("B1 + C1"));
        spreadsheet.SetCellContents("B1", new Formula("D1"));
        spreadsheet.SetCellContents("C1", new Formula("A1"));
    }

    /// <summary>
    ///     Tests clearing a non-existent cell.
    /// </summary>
    [TestMethod]
    public void SetCellContents_NonExistentCell_Clear_NoException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("C1", string.Empty);

        Assert.AreEqual(string.Empty, spreadsheet.GetCellContents("C1"));
    }

    /// <summary>
    ///     Tests setting a cell with a formula referencing itself indirectly through another cell throws circular exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContents_IndirectSelfReference_CircularException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", new Formula("B1 + 1"));
        spreadsheet.SetCellContents("B1", new Formula("A1 + 1"));

        Assert.IsNotNull(spreadsheet.GetCellContents("A1"));
        Assert.IsNotNull(spreadsheet.GetCellContents("B1"));
    }

    /// <summary>
    ///     Tests setting a formula that indirectly references itself, verifying circular dependency detection.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContents_IndirectCircularDependency_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", new Formula("B1 + 1"));
        spreadsheet.SetCellContents("B1", new Formula("C1 + 1"));
        spreadsheet.SetCellContents("C1", new Formula("A1 + 1"));
    }

    /// <summary>
    ///     Tests setting a formula that directly references itself, verifying circular dependency detection.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContents_DirectSelfReference_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", new Formula("A1 + 1"));
    }

    // --- Tests for GetNamesOfAllNonemptyCells method ---

    /// <summary>
    ///     Tests getting names of all non-empty cells in the spreadsheet.
    /// </summary>
    [TestMethod]
    public void GetNamesOfAllNonemptyCells_ReturnsNonEmptyCells()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", 10.0);
        spreadsheet.SetCellContents("A2", "Text");
        var nonEmptyCells = spreadsheet.GetNamesOfAllNonemptyCells();

        Assert.IsTrue(nonEmptyCells.Contains("A1"));
        Assert.IsTrue(nonEmptyCells.Contains("A2"));
        Assert.AreEqual(2, nonEmptyCells.Count);
    }

    /// <summary>
    ///     Tests getting names of all non-empty cells after some have been cleared.
    /// </summary>
    [TestMethod]
    public void GetNamesOfAllNonemptyCells_AfterClearing_ReturnsUpdatedList()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", 10.0);
        spreadsheet.SetCellContents("A2", "Hello");
        spreadsheet.SetCellContents("A1", string.Empty); // Clear A1
        var nonEmptyCells = spreadsheet.GetNamesOfAllNonemptyCells();

        Assert.IsFalse(nonEmptyCells.Contains("A1"));
        Assert.IsTrue(nonEmptyCells.Contains("A2"));
        Assert.AreEqual(1, nonEmptyCells.Count);
    }

    /// <summary>
    ///     Tests that clearing all cells results in no non-empty cells.
    /// </summary>
    [TestMethod]
    public void GetNamesOfAllNonemptyCells_GetNamesOfAllNonemptyCells_ReturnsEmptyList()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", 10.0);
        spreadsheet.SetCellContents("A2", "Text");
        spreadsheet.SetCellContents("A1", string.Empty);
        spreadsheet.SetCellContents("A2", string.Empty);

        var nonEmptyCells = spreadsheet.GetNamesOfAllNonemptyCells();
        Assert.AreEqual(0, nonEmptyCells.Count);
    }

    // --- Tests for GetCellContents method ---

    /// <summary>
    ///     Tests getting cell contents with an invalid cell name and expects an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellContents_InvalidCellName_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.GetCellContents(string.Empty);
    }

    /// <summary>
    ///     Tests getting contents of a non-existent cell and expects an empty string.
    /// </summary>
    [TestMethod]
    public void GetCellContents_NonExistentCell_ReturnsEmptyString()
    {
        var spreadsheet = new Spreadsheet();

        Assert.AreEqual(string.Empty, spreadsheet.GetCellContents("C1"));
    }

    /// <summary>
    ///     Tests retrieving a cell's value after a formula is updated.
    /// </summary>
    [TestMethod]
    public void GetCellContents_GetValue_ReturnsUpdatedResult()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", 2.0);
        spreadsheet.SetCellContents("B1", new Formula("A1 * 2"));
        spreadsheet.SetCellContents("A1", 5.0);

        Assert.AreEqual(new Formula("A1 * 2"), spreadsheet.GetCellContents("B1"));
    }

    /// <summary>
    ///     Tests a mix of different cell contents.
    /// </summary>
    [TestMethod]
    public void MixedCellContents_GetCellContents_ReturnsAllValues()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetCellContents("A1", 10.0);
        spreadsheet.SetCellContents("B1", "Text");
        spreadsheet.SetCellContents("C1", new Formula("A1 + 5"));
        spreadsheet.SetCellContents("D1", string.Empty);

        Assert.AreEqual(10.0, spreadsheet.GetCellContents("A1"));
        Assert.AreEqual("Text", spreadsheet.GetCellContents("B1"));
        Assert.AreEqual(new Formula("A1 + 5"), spreadsheet.GetCellContents("C1"));
        Assert.AreEqual(string.Empty, spreadsheet.GetCellContents("D1"));
    }
}