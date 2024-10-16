// <copyright file="SpreadsheetTests.cs" company="UofU-CS3500">
//   Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

namespace SpreadsheetTests;

using CS3500.Formula;
using CS3500.Spreadsheet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using System.Runtime.CompilerServices;

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
    // --- Tests for SetContentsOfCell method ---

    /// <summary>
    ///     Tests setting cell contents to a number and retrieving it.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_Number_GetCellContents_ReturnsNumber()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "42.0");

        Assert.AreEqual(42.0, spreadsheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Tests setting cell contents to text and retrieving it.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_Text_GetCellContents_ReturnsText()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A2", "Hello");

        Assert.AreEqual("Hello", spreadsheet.GetCellContents("A2"));
    }

    /// <summary>
    ///     Tests removing cell contents by setting it to an empty string.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_EmptyString_RemovesCell()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A3", "World");
        spreadsheet.SetContentsOfCell("A3", string.Empty);

        Assert.AreEqual(string.Empty, spreadsheet.GetCellContents("A3"));
    }

    /// <summary>
    ///     Tests setting a cell to a formula that references itself and checks for a circular dependency.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetContentsOfCell_SelfReferencingFormula_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "=A1 + 1");
    }

    /// <summary>
    ///     Tests setting cell contents to a formula and retrieving it.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_Formula_GetCellContents_ReturnsFormula()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("B1", "A1 + 5");

        Assert.AreEqual("A1 + 5", spreadsheet.GetCellContents("B1"));
    }

    /// <summary>
    ///     Tests setting cell contents that create a circular dependency and expects an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetContentsOfCell_CircularDependency_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "=B1");
        spreadsheet.SetContentsOfCell("B1", "=A1");
    }

    /// <summary>
    ///     Tests setting cell contents with an invalid cell name and expects an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetContentsOfCell_InvalidCellName_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("1A", "Invalid");
    }

    /// <summary>
    ///     Tests adding multiple cells and retrieving their contents.
    /// </summary>
    [TestMethod]
    public void SetMultipleCellContents_GetCellContents_ReturnsAllValues()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "10.0");
        spreadsheet.SetContentsOfCell("B1", "Hello");
        spreadsheet.SetContentsOfCell("C1", "=A1 + 5");

        Assert.AreEqual(10.0, spreadsheet.GetCellContents("A1"));
        Assert.AreEqual("Hello", spreadsheet.GetCellContents("B1"));
        Assert.AreEqual(new Formula("A1 + 5"), spreadsheet.GetCellContents("C1"));
    }

    /// <summary>
    ///     Tests setting a cell with a complex formula and retrieving its value.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_ComplexFormula_GetCellContents_ReturnsFormula()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "10.0");
        spreadsheet.SetContentsOfCell("B1", "=A1 * 2 + 3");

        Assert.AreEqual(new Formula("A1 * 2 + 3"), spreadsheet.GetCellContents("B1"));
    }

    /// <summary>
    ///     Tests setting a cell with a formula that references multiple cells.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_MultipleCellReferences_Formula()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "5.0");
        spreadsheet.SetContentsOfCell("B1", "10.0");
        spreadsheet.SetContentsOfCell("C1", "=A1 + B1");

        Assert.AreEqual(new Formula("A1 + B1"), spreadsheet.GetCellContents("C1"));
    }

    /// <summary>
    ///     Tests setting a cell with a formula that references a non-existent cell.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_ReferenceToNonExistentCell_Formula()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "=B1 + 1");

        Assert.AreEqual(new Formula("B1 + 1"), spreadsheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Tests setting a cell with a valid formula, then setting it to text, and checks content.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_OverwriteFormulaWithText_ReturnsText()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "B1 + 2");
        spreadsheet.SetContentsOfCell("A1", "Yuji");

        Assert.AreEqual("Yuji", spreadsheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Tests setting a very large numeric value in a cell and retrieving it.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_LargeNumber_ReturnsLargeNumber()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "2e100");

        Assert.AreEqual(2e100, spreadsheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Tests setting a negative number in a cell and retrieving it.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_NegativeNumber_ReturnsNegativeNumber()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "-123.456");

        Assert.AreEqual(-123.456, spreadsheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Tests setting a formula containing a large number of variables and retrieving it.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_FormulaWithManyVariables_GetCellContents_ReturnsFormula()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("X1", "A1 + B1 + C1 + D1 + E1 + F1 + G1 + H1");

        Assert.AreEqual("A1 + B1 + C1 + D1 + E1 + F1 + G1 + H1", spreadsheet.GetCellContents("X1"));
    }

    /// <summary>
    ///     Tests clearing a cell that contains a formula and checks if it is empty.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_CheckIfEmpty_ReturnsEmptyString()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "B1 + 5");
        spreadsheet.SetContentsOfCell("A1", string.Empty);

        Assert.AreEqual(string.Empty, spreadsheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Tests that a cell whose contents have not been set returns and empty string when GetCellContents is called.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_CellContentsNeverSet_ReturnsEmptyString()
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
        spreadsheet.SetContentsOfCell("A1", "10.0");
        spreadsheet.SetContentsOfCell("B1", "Hello");
        spreadsheet.SetContentsOfCell("C1", "A1 + 5");

        // Clear all cells
        spreadsheet.SetContentsOfCell("A1", string.Empty);
        spreadsheet.SetContentsOfCell("B1", string.Empty);
        spreadsheet.SetContentsOfCell("C1", string.Empty);

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
    public void SetContentsOfCell_MultipleCellReferences_CircularDependencyThrows()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "=B1 + C1");
        spreadsheet.SetContentsOfCell("B1", "=D1");
        spreadsheet.SetContentsOfCell("C1", "=A1");
    }

    /// <summary>
    ///     Tests clearing a non-existent cell.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_NonExistentCell_Clear_NoException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("C1", string.Empty);

        Assert.AreEqual(string.Empty, spreadsheet.GetCellContents("C1"));
    }

    /// <summary>
    ///     Tests setting a cell with a formula referencing itself indirectly through another cell throws circular exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetContentsOfCell_IndirectSelfReference_CircularException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "=B1 + 1");
        spreadsheet.SetContentsOfCell("B1", "=A1 + 1");

        Assert.IsNotNull(spreadsheet.GetCellContents("A1"));
        Assert.IsNotNull(spreadsheet.GetCellContents("B1"));
    }

    /// <summary>
    ///     Tests setting a formula that indirectly references itself, verifying circular dependency detection.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetContentsOfCell_IndirectCircularDependency_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "=B1 + 1");
        spreadsheet.SetContentsOfCell("B1", "=C1 + 1");
        spreadsheet.SetContentsOfCell("C1", "=A1 + 1");
    }

    /// <summary>
    ///     Tests setting a formula that directly references itself, verifying circular dependency detection.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetContentsOfCell_DirectSelfReference_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "=A1 + 1");
    }

    // --- Tests for GetNamesOfAllNonemptyCells method ---

    /// <summary>
    ///     Tests getting names of all non-empty cells in the spreadsheet.
    /// </summary>
    [TestMethod]
    public void GetNamesOfAllNonemptyCells_ReturnsNonEmptyCells()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "10.0");
        spreadsheet.SetContentsOfCell("A2", "Text");
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
        spreadsheet.SetContentsOfCell("A1", "10.0");
        spreadsheet.SetContentsOfCell("A2", "Hello");
        spreadsheet.SetContentsOfCell("A1", string.Empty); // Clear A1
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
        spreadsheet.SetContentsOfCell("A1", "10.0");
        spreadsheet.SetContentsOfCell("A2", "Text");
        spreadsheet.SetContentsOfCell("A1", string.Empty);
        spreadsheet.SetContentsOfCell("A2", string.Empty);

        var nonEmptyCells = spreadsheet.GetNamesOfAllNonemptyCells();
        Assert.AreEqual(0, nonEmptyCells.Count);
    }

    // --- Tests for Spreadsheet Constructors method ---

    /// <summary>
    /// Tests that the zero-argument constructor initializes the spreadsheet with the name "default".
    /// </summary>
    [TestMethod]
    public void Spreadsheet_ZeroArgumentConstructor_NameIsDefault()
    {
        var spreadsheet = new Spreadsheet();
        Assert.AreEqual("default", spreadsheet.Name);
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

    // --- Tests for SetContentsOfCell method and GetCellContents method ---

    /// <summary>
    ///     Tests retrieving a cell's value after a formula is updated.
    /// </summary>
    [TestMethod]
    public void GetCellContents_GetValue_ReturnsUpdatedResult()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "2.0");
        spreadsheet.SetContentsOfCell("B1", "=A1 * 2");
        spreadsheet.SetContentsOfCell("A1", "5.0");

        Assert.AreEqual(new Formula("A1 * 2"), spreadsheet.GetCellContents("B1"));
    }

    /// <summary>
    ///     Tests a mix of different cell contents.
    /// </summary>
    [TestMethod]
    public void MixedCellContents_GetCellContents_ReturnsAllValues()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "10.0");
        spreadsheet.SetContentsOfCell("B1", "Text");
        spreadsheet.SetContentsOfCell("C1", "=A1 + 5");
        spreadsheet.SetContentsOfCell("D1", string.Empty);

        Assert.AreEqual(10.0, spreadsheet.GetCellContents("A1"));
        Assert.AreEqual("Text", spreadsheet.GetCellContents("B1"));
        Assert.AreEqual(new Formula("A1 + 5"), spreadsheet.GetCellContents("C1"));
        Assert.AreEqual(string.Empty, spreadsheet.GetCellContents("D1"));
    }

    // --- Tests for GetCellValue method With SetCellContents Method ---

    /// <summary>
    ///     Tests getting the value of a cell containing a number.
    /// </summary>
    [TestMethod]
    public void GetCellValue_NumberCell_ReturnsNumberValue()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "42.0");
        var value = spreadsheet.GetCellValue("A1");

        Assert.AreEqual(42.0, value);
    }

    /// <summary>
    ///     Tests getting the value of a cell containing text.
    /// </summary>
    [TestMethod]
    public void GetCellValue_TextCell_ReturnsTextValue()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A2", "Hello");
        var value = spreadsheet.GetCellValue("A2");

        Assert.AreEqual("Hello", value);
    }

    /// <summary>
    ///     Tests getting the value of a cell containing a formula.
    /// </summary>
    [TestMethod]
    public void GetCellValue_FormulaCell_ReturnsCalculatedValue()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "5.0");
        spreadsheet.SetContentsOfCell("B1", "15.0");
        var value = spreadsheet.GetCellValue("B1");

        Assert.AreEqual(15.0, value);
    }

    /// <summary>
    ///     Tests getting the value of a cell with an invalid cell name throws an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellValue_InvalidCellName_Exception()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("1A2", "B1 + 1");
    }

    /// <summary>
    ///     Tests getting the value of a cell with a blank space as cell name throws exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellValue_SpaceCellName_Exception()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell(" ", "B1 + 1");
    }

    /// <summary>
    ///     Tests getting the value of a cell with a circular dependency and expects an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellValue_CellNotInDictionary_Exception()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.GetCellValue("k345");
    }

    /// <summary>
    ///     Tests getting the value of a cell after clearing its contents.
    /// </summary>
    [TestMethod]
    public void GetCellValue_AfterClearingCell_ReturnsDefaultValue()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "100.0");
        spreadsheet.SetContentsOfCell("A1", "0"); // Clear contents
        var value = spreadsheet.GetCellValue("A1");

        Assert.AreEqual(0.0, value); // Should return default value
    }

    /// <summary>
    ///     Tests getting the value of a cell with an invalid name and expects an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellValue_InvalidCellName_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.GetCellValue("InvalidCell");
    }

    /// <summary>
    ///     Tests getting the value of a complex formula with multiple dependencies.
    /// </summary>
    [TestMethod]
    public void GetCellValue_ComplexFormula_ReturnsCalculatedValue()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "10.0");
        spreadsheet.SetContentsOfCell("B1", "20.0");
        spreadsheet.SetContentsOfCell("C1", "35.0");
        var value = spreadsheet.GetCellValue("C1");

        Assert.AreEqual(35.0, value); // C1 should calculate the sum of A1 and B1
    }

    /// <summary>
    ///     Tests getting the value of a cell with a circular dependency and expects an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void GetCellValue_CircularDependency_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "=B1 + 1");
        spreadsheet.SetContentsOfCell("B1", "=A1 + 1"); // Circular dependency
        spreadsheet.GetCellValue("A1");
    }

    /// <summary>
    ///     Tests that it throws a formula format exception when formula is not formatted correctly.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void GetCellValue_InvalidFormulaFormat_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "=1B1 @ 1");
        spreadsheet.SetContentsOfCell("B1", "=A1 + 1");
        spreadsheet.GetCellValue("A1");
    }

    // --- Tests for Indexer [] Method ---

    /// <summary>
    ///     Tests that it throws an InvalidNameException when an invalid cell name is provided.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void Indexer_InvalidCellName_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();

        // Try to access an invalid cell name
        var value = spreadsheet["1A"];
    }

    /// <summary>
    ///     Tests that it returns the correct value for a valid cell name.
    /// </summary>
    [TestMethod]
    public void Indexer_ValidCellName_ReturnsCorrectValue()
    {
        var spreadsheet = new Spreadsheet();

        // Set the value of a cell
        spreadsheet.SetContentsOfCell("A1", "5");

        // Access the value using the indexer
        var value = spreadsheet["A1"];

        // Assert that the value is correct
        Assert.AreEqual(5.0, value);
    }

    /// <summary>
    ///     Tests that it throws an InvalidNameException when the cell name is null or empty.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void Indexer_NullOrEmptyCellName_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();

        // Try to access a cell with null or empty name
        var value = spreadsheet[string.Empty];
    }

    // --- Tests Save Method ---

    /// <summary>
    ///     Tests saving a spreadsheet to a file and loading it back.
    /// </summary>
    [TestMethod]
    public void Save_Spreadsheet_ReturnsIdenticalSpreadsheet()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "10.0");
        spreadsheet.SetContentsOfCell("B1", "Hello");

        spreadsheet.Save("save.txt");

        Assert.AreEqual(10.0, spreadsheet.GetCellContents("A1"));
        Assert.AreEqual("Hello", spreadsheet.GetCellContents("B1"));
    }

    /// <summary>
    ///     Tests saving and loading an empty spreadsheet.
    /// </summary>
    [TestMethod]
    public void Save_EmptySpreadsheet_ReturnsEmptySpreadsheet()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.Save("save.txt");
        var loadedSpreadsheet = new Spreadsheet("save.txt");

        Assert.AreEqual(string.Empty, loadedSpreadsheet.GetCellContents("A1"));
        Assert.AreEqual(string.Empty, loadedSpreadsheet.GetCellContents("B1"));
    }

    /// <summary>
    /// Tests saving a spreadsheet with a formula in a cell to ensure the
    /// formula is serialized correctly with the '=' prefix.
    /// </summary>
    [TestMethod]
    public void Save_FormulaCell_SerializesCorrectly()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "5.0");
        spreadsheet.SetContentsOfCell("B1", "=A1 + 2");

        string filename = "formula_save.txt";
        spreadsheet.Save(filename);

        var loadedSpreadsheet = new Spreadsheet(filename);
        loadedSpreadsheet.Load(filename);

        Assert.AreEqual("=A1+2", loadedSpreadsheet.GetCellContents("B1"));
    }

    /// <summary>
    /// Tests saving a spreadsheet with an unsupported content type to ensure
    /// it defaults to an empty string in the JSON.
    /// </summary>
    [TestMethod]
    public void Save_UnsupportedContent_DefaultsToEmptyString()
    {
        var spreadsheet = new Spreadsheet();

        spreadsheet.SetContentsOfCell("A1", string.Empty);
        spreadsheet.Save("testSave.txt");

        var loadedSpreadsheet = new Spreadsheet("testSave.txt");

        Assert.AreEqual(string.Empty, loadedSpreadsheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Tests saving a spreadsheet with multiple cells and loading it back.
    /// </summary>
    [TestMethod]
    public void Save_MultipleCells_ReturnsIdenticalValues()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "5.0");
        spreadsheet.SetContentsOfCell("B1", "Test");
        spreadsheet.SetContentsOfCell("C1", "A1 + 2");

        spreadsheet.Save("save.txt");

        Assert.AreEqual(5.0, spreadsheet.GetCellContents("A1"));
        Assert.AreEqual("Test", spreadsheet.GetCellContents("B1"));
        Assert.AreEqual("A1 + 2", spreadsheet.GetCellContents("C1"));
    }

    /// <summary>
    /// Tests that an exception is thrown when saving to an invalid file path.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Save_InvalidFilePath_ThrowsSpreadsheetReadWriteException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.SetContentsOfCell("A1", "Test");

        spreadsheet.Save(@"C:\invalid_directory\save.txt");
    }

    // --- Tests Load Method ---

    /// <summary>
    ///     Tests loading a spreadsheet from a non-existent file and expects an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Load_NonExistentFile_ThrowsException()
    {
        var spreadsheet = new Spreadsheet();
        spreadsheet.Load("nonExistentFile.txt");
    }

    /// <summary>
    ///     Tests loading a spreadsheet from a JSON file.
    /// </summary>
    [TestMethod]
    public void Load_ValidJsonFile_ReturnsIdenticalValues()
    {
        // Arrange: Create a JSON representation of the spreadsheet.
        string expectedOutput = @"
    {
        ""Cells"": {
            ""A1"": { ""StringForm"": ""5.0"" },
            ""B1"": { ""StringForm"": ""Test"" },
            ""C1"": { ""StringForm"": ""A1 + 2"" }
        }
    }";

        File.WriteAllText("known_values.txt", expectedOutput);

        var spreadsheet = new Spreadsheet();
        spreadsheet.Load("known_values.txt");

        Assert.AreEqual("5.0", spreadsheet.GetCellContents("A1"));
        Assert.AreEqual("Test", spreadsheet.GetCellContents("B1"));
        Assert.AreEqual("A1 + 2", spreadsheet.GetCellContents("C1"));
    }

    /// <summary>
    ///     Tests loading a spreadsheet from an invalid format JSON file.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Load_InvalidFormatJsonFile_ThrowsException()
    {
        string expectedOutput = @"
    {
        ""Bla"": {
            ""A1"": { ""StringForm"": ""5.0"" },
            ""B1"": { ""StringForm"": ""Test"" },
            ""C1"": { ""StringForm"": ""A1 + 2"" }
        }
    }";

        File.WriteAllText("known_values.txt", expectedOutput);

        var spreadsheet = new Spreadsheet();
        spreadsheet.Load("known_values.txt");
    }

    /// <summary>
    ///     Tests loading a spreadsheet from a null JSON file.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Load_NullJsonFile_ThrowsException()
    {
        File.WriteAllText("known_values.txt", null);

        var spreadsheet = new Spreadsheet();
        spreadsheet.Load("known_values.txt");
    }

    /// <summary>
    ///     Tests loading a spreadsheet from an empty JSON file.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Load_EmptyJsonFile_ThrowsException()
    {
        string expectedOutput = string.Empty;

        File.WriteAllText("known_values.txt", expectedOutput);

        var spreadsheet = new Spreadsheet();
        spreadsheet.Load("known_values.txt");
    }

    /// <summary>
    ///     Tests loading a spreadsheet from a a file that does not start with "Cell" throws exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Load_DoesNotStartWithCell_ThrowsException()
    {
        string expectedOutput = @"
    {
        ""Bla"": {
            ""A1"": { ""StringForm"": ""5.0""},
        }
    }";

        File.WriteAllText("known_values.txt", expectedOutput);

        var spreadsheet = new Spreadsheet();
        spreadsheet.Load("known_values.txt");
    }

    // --- Stress Tests ---

    /// <summary>
    /// Stress test that creates a long chain of dependencies in the spreadsheet.
    /// Each cell A1 to A10000 depends on the previous cell.
    /// This tests the performance and correctness of handling many dependencies.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    public void StressTest_SetCellContents_LongDependencyChain()
    {
        Spreadsheet spreadsheet = new Spreadsheet();

        spreadsheet.SetContentsOfCell("A1", "0");

        // Create a long chain of dependencies from A1 to A10000
        for (int i = 2; i <= 10000; i++)
        {
            string currentCell = "A" + i;
            string previousCell = "A" + (i - 1);

            spreadsheet.SetContentsOfCell(currentCell, "=" + previousCell);
        }

        spreadsheet.SetContentsOfCell("A10000", "5");
        Assert.AreEqual(5.0, spreadsheet.GetCellContents("A10000"));
    }

    /// <summary>
    /// Tests the Save and Load functionalities of the Spreadsheet class
    /// by creating a large spreadsheet with 10,000 cells, saving it to a file,
    /// and then loading it back to verify that all values are retained correctly.
    /// </summary>
    [TestMethod]
    [Timeout(10000)]
    public void StressTest_SaveAndLoad_LargeSpreadsheet()
    {
        // Create a new instance of the Spreadsheet
        Spreadsheet spreadsheet = new Spreadsheet();

        // Fill the spreadsheet with a large number of cells
        for (int i = 1; i <= 10000; i++)
        {
            spreadsheet.SetContentsOfCell($"A{i}", (i * 1.0).ToString()); // Set A1 to A10000 with values 1.0 to 10000.0
        }

        // Save the spreadsheet to a file
        string filename = "large_spreadsheet.txt";
        spreadsheet.Save(filename);

        // Create a new spreadsheet and load from the saved file
        Spreadsheet loadedSpreadsheet = new Spreadsheet(filename);
        loadedSpreadsheet.Load(filename);

        // Verify that the values are correct in the loaded spreadsheet
        for (int i = 1; i <= 10000; i++)
        {
            Assert.AreEqual((i * 1.0).ToString(), loadedSpreadsheet.GetCellContents($"A{i}"));
        }
    }

    /// <summary>
    /// Stress test that updates a large number of cells in the spreadsheet.
    /// This test sets a value to each cell from A1 to A10000, then updates them,
    /// ensuring that the update process is efficient and correct.
    /// </summary>
    [TestMethod]
    [Timeout(10000)]
    public void StressTest_UpdateLargeNumberOfCells_ExpectedValue()
    {
        Spreadsheet spreadsheet = new Spreadsheet();

        // Initialize all cells from A1 to A10000 with a simple value
        for (int i = 1; i <= 10000; i++)
        {
            spreadsheet.SetContentsOfCell($"A{i}", (i * 1.0).ToString()); // Set A1 to A10000 with values 1.0 to 10000.0
        }

        // Now update all cells to a new value
        for (int i = 1; i <= 10000; i++)
        {
            spreadsheet.SetContentsOfCell($"A{i}", (i * 2.0).ToString()); // Set A1 to A10000 with values 2.0 to 20000.0
        }

        // Verify that the values have been updated correctly
        for (int i = 1; i <= 10000; i++)
        {
            Assert.AreEqual(i * 2.0, spreadsheet.GetCellContents($"A{i}"));
        }
    }
}