// <copyright file="SpreadsheetTests.cs" company="UofU-CS3500">
//   Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

/// <summary>
/// Author:    Nandhini Ramanathan
/// Partner:   None
/// Date:      September 27, 2024
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Nandhini Ramanathan - This work may not
///            be copied for use in Academic Coursework.
///
//// I, Nandhini Ramanathan, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All
/// references used in the completion of the assignments are cited
/// in my README file.
///
//// File Contents
///    This file contains MS unit tests for the spreadsheet class.
/// </summary>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using CS3500.Spreadsheet;
using CS3500.Formula;
using System.Collections.Generic;

namespace SpreadsheetTests
{
    /// <summary>
    ///   <para>
    ///     The following class is a tester class for the Spreadsheet class.
    ///   </para>
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        // --- Tests for bla method ---

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
            spreadsheet.SetCellContents("A3", "");

            Assert.AreEqual(string.Empty, spreadsheet.GetCellContents("A3"));
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
            spreadsheet.SetCellContents("B1", new Formula("A1")); // This should throw CircularException
        }

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
        ///     Tests getting cell contents with an invalid cell name and expects an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents_InvalidCellName_ThrowsException()
        {
            var spreadsheet = new Spreadsheet();
            spreadsheet.GetCellContents(""); // Empty name should throw InvalidNameException
        }

        /// <summary>
        ///     Tests setting cell contents with an invalid cell name and expects an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents_InvalidCellName_ThrowsException()
        {
            var spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("1A", "Invalid"); // Invalid name should throw exception
        }

        /// <summary>
        ///     Tests getting contents of a non-existent cell and expects an empty string.
        /// </summary>
        [TestMethod]
        public void GetCellContents_NonExistentCell_ReturnsEmptyString()
        {
            var spreadsheet = new Spreadsheet();

            Assert.AreEqual(string.Empty, spreadsheet.GetCellContents("C1")); // Should return empty string
        }

        /// <summary>
        ///     Tests setting a cell to a formula that references itself and checks for a circular dependency.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContents_SelfReferencingFormula_ThrowsException()
        {
            var spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("A1", new Formula("A1 + 1")); // This should throw CircularException
        }
    }
}