// <copyright file="Spreadsheet.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// Written by Joe Zachary for CS 3500, September 2013
// Update by Professor Kopta and de St. Germain
//     - Updated return types
//     - Updated documentation

namespace CS3500.Spreadsheet;

using CS3500.Formula;
using System.Text.Json;
using System.Text.RegularExpressions;

/// <summary>
/// Author:    Nandhini Ramanathan
/// Partner:   None
/// Date:      October 18,2024
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Nandhini Ramanathan - This work may not
///            be copied for use in Academic Coursework.
///
/// I, Nandhini Ramanathan, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source. All
/// references used in the completion of the assignments are cited
/// in my README file.
///
/// File Contents:
/// The Spreadsheet class implements a spreadsheet, storing cell contents
/// and managing dependencies between cells. It supports setting cells to values,
/// text, or formulas, and ensures there are no circular dependencies. This file also
/// includes the CircularException and InvalidNameException classes, which handle
/// invalid name inputs and circular dependency errors respectively.
/// </summary>

/// <summary>
///   <para>
///     Thrown to indicate that a change to a cell will cause a circular dependency.
///   </para>
/// </summary>
public class CircularException : Exception
{
}

/// <summary>
///   <para>
///     Thrown to indicate that a name parameter was invalid.
///   </para>
/// </summary>
public class InvalidNameException : Exception
{
}

/// <summary>
///   <para>
///     A Spreadsheet object represents the state of a simple spreadsheet.  A
///     spreadsheet represents an infinite number of named cells.
///   </para>
///   <para>
///     Valid Cell Names: A string is a valid cell name if and only if it is one or
///     more letters followed by one or more numbers, e.g., A5, BC27. Cell names are
///     case insensitive, so "x1" and "X1" are the same cell name. Each cell has a
///     contents and a value.
///   </para>
///   <para>
///     Spreadsheets are never allowed to contain a combination of Formulas that establish
///     a circular dependency.  A circular dependency exists when a cell depends on itself.
///   </para>
/// </summary>
public class Spreadsheet
{
    /// <summary>
    /// A dictionary to hold cell contents by its names.
    /// </summary>
    private readonly Dictionary<string, Cell> cellContents;

    /// <summary>
    /// An instance of the DependencyGraph to manage cell dependencies.
    /// </summary>
    private readonly DependencyGraph dependencyGraph;

    /// <summary>
    /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
    /// It initializes the cell contents dictionary, the dependency graph, and creates an empty spreadsheet with the name "default".
    /// </summary>
    public Spreadsheet()
    {
        Name = "default";
        cellContents = new Dictionary<string, Cell>();
        dependencyGraph = new DependencyGraph();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
    /// It initializes the cell contents dictionary, the dependency graph, and creates an empty spreadsheet with the name given.
    /// </summary>
    /// <param name="filename">The name of the spreadsheet.</param>
    public Spreadsheet(string filename)
    {
        cellContents = new Dictionary<string, Cell>();
        dependencyGraph = new DependencyGraph();
        Name = filename;
    }

    /// <summary>
    /// Gets the name of a Spreadsheet, either default or the given name as a parameter.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets a value indicating whether spreadsheet has been changed.
    /// </summary>
    public bool Changed { get; set; } = false;

    /// <summary>
    ///   <para>
    ///     Shortcut syntax to for getting the value of the cell
    ///     using the [] operator.
    ///   </para>
    ///   <para>
    ///     See: <see cref="GetCellValue(string)"/>.
    ///   </para>
    /// </summary>
    /// <param name="cellName"> Any valid cell name. </param>
    /// <exception cref="InvalidNameException">
    ///     If the name parameter is invalid, throw an InvalidNameException.
    /// </exception>
    public object this[string cellName]
    {
        get
        {
            // Validate the cell name
            if (string.IsNullOrWhiteSpace(cellName) || !IsValidCellName(cellName))
            {
                throw new InvalidNameException();
            }

            // Retrieve the cell value using the existing method
            return GetCellValue(cellName);
        }
    }

    /// <summary>
    ///   <para>
    ///     Writes the contents of this spreadsheet to the named file using a JSON format.
    ///     If the file already exists, overwrite it. After saving the file, the spreadsheet is
    ///     no longer "changed".
    ///   </para>
    /// </summary>
    /// <param name="filename"> The name (with path) of the file to save to.</param>
    /// <exception cref="SpreadsheetReadWriteException">
    ///   If there are any problems opening, writing, or closing the file,
    ///   the method should throw a SpreadsheetReadWriteException with an
    ///   explanatory message.
    /// </exception>
    public void Save(string filename)
    {
        try
        {
            // Prepare a dictionary to store cell data for JSON serialization.
            var data = new Dictionary<string, Dictionary<string, string>>();

            // Iterate over each cell and convert its contents to string format.
            foreach (var entry in cellContents)
            {
                string stringForm;
                if (entry.Value.Contents is double d)
                {
                    stringForm = d.ToString();
                }
                else if (entry.Value.Contents is Formula formula)
                {
                    stringForm = "=" + formula.ToString();
                }
                else
                {
                    stringForm = entry.ToString();
                }

                // Store the cell's string representation in the dictionary.
                data[entry.Key] = new Dictionary<string, string> { { "StringForm", stringForm } };
            }

            // Serialize the data to a JSON string
            var jsonString = JsonSerializer.Serialize(new { Cells = data }, new JsonSerializerOptions { WriteIndented = true });

            // Write the JSON string to the specified file.
            File.WriteAllText(filename, jsonString);

            // Mark the spreadsheet as unchanged after saving.
            Changed = false;
        }
        catch (Exception)
        {
            throw new SpreadsheetReadWriteException("Error saving spreadsheet :(");
        }
    }

    /// <summary>
    ///   <para>
    ///     Read the data (JSON) from the file and instantiate the current
    ///     spreadsheet.  See <see cref="Save(string)"/> for expected format.
    ///   </para>
    ///   <para>
    ///     Loading a spreadsheet should set changed to false.  External
    ///     programs should alert the user before loading over a changed sheet.
    ///   </para>
    /// </summary>
    /// <param name="filename"> The saved file name including the path. </param>
    /// <exception cref="SpreadsheetReadWriteException"> When the file cannot be opened or the json is bad.</exception>
    public void Load(string filename)
    {
        var newCellContents = new Dictionary<string, Cell>();

        try
        {
            // Read the JSON content from file and deserialize.
            string jsonString = File.ReadAllText(filename);
            var loadedData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);

            if (loadedData == null || !loadedData.ContainsKey("Cells"))
            {
                throw new SpreadsheetReadWriteException("Invalid file format.");
            }

            foreach (var cell in loadedData["Cells"].EnumerateObject())
            {
                string cellName = cell.Name;

                if (cell.Value.TryGetProperty("StringForm", out JsonElement stringFormElement))
                {
                    string cellValue = stringFormElement.GetString() ?? string.Empty;

                    // Check if the value starts with '=' to identify a formula.
                    if (cellValue.StartsWith("="))
                    {
                        // Parse the formula (excluding the '=' prefix).
                        string formulaString = cellValue.Substring(1);
                        var formula = new Formula(formulaString);
                        newCellContents[cellName] = new Cell(cellName, formula);
                    }
                    else if (double.TryParse(cellValue, out double number))
                    {
                        newCellContents[cellName] = new Cell(cellName, number);
                    }
                    else
                    {
                        newCellContents[cellName] = new Cell(cellName, cellValue);
                    }
                }
            }

            // Clear the current contents and load the new ones.
            cellContents.Clear();
            foreach (var kvp in newCellContents)
            {
                cellContents[kvp.Key] = kvp.Value;
            }

            Changed = false;
        }
        catch (Exception)
        {
            throw new SpreadsheetReadWriteException("Error loading the spreadsheet: ");
        }
    }

    /// <summary>
    ///   <para>
    ///     Return the value of the named cell.
    ///   </para>
    /// </summary>
    /// <param name="cellName"> The cell in question. </param>
    /// <returns>
    ///   Returns the value (as opposed to the contents) of the named cell.  The return
    ///   value's type should be either a string, a double, or a CS3500.Formula.FormulaError.
    ///   If the cell contents are a formula, the value should have already been computed
    ///   at this point.
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///   If the provided name is invalid, throws an InvalidNameException.
    /// </exception>
    public object GetCellValue(string cellName)
    {
        if (!IsValidCellName(cellName))
        {
            throw new InvalidNameException();
        }

        // Check if the cell exists in the dictionary
        if (!cellContents.TryGetValue(cellName, out var cell))
        {
            return string.Empty; // Return empty if the cell doesn't exist
        }

        // Check if the content is a Formula
        if (cell.Contents is Formula formula)
        {
            return formula.Evaluate(Lookup);
        }

        // If it's not a formula, just return the content directly
        return cell.Contents;
    }

    /// <summary>
    ///   <para>
    ///       Sets the contents of the named cell to the appropriate object
    ///       based on the string in <paramref name="content"/>.
    ///   </para>
    ///   <para>
    ///       First, if the <paramref name="content"/> parses as a double, the contents of the named
    ///       cell becomes that double.
    ///   </para>
    ///   <para>
    ///       Otherwise, if the <paramref name="content"/> begins with the character '=', an attempt is made
    ///       to parse the remainder of content into a Formula.
    ///   </para>
    ///   <para>
    ///       There are then three possible outcomes when a formula is detected:
    ///   </para>
    ///
    ///   <list type="number">
    ///     <item>
    ///       If the remainder of content cannot be parsed into a Formula, a
    ///       FormulaFormatException is thrown.
    ///     </item>
    ///     <item>
    ///       If changing the contents of the named cell to be f
    ///       would cause a circular dependency, a CircularException is thrown,
    ///       and no change is made to the spreadsheet.
    ///     </item>
    ///     <item>
    ///       Otherwise, the contents of the named cell becomes f.
    ///     </item>
    ///   </list>
    ///   <para>
    ///     Finally, if the content is a string that is not a double and does not
    ///     begin with an "=" (equal sign), save the content as a string.
    ///   </para>
    ///   <para>
    ///     On successfully changing the contents of a cell, the spreadsheet will be <see cref="Changed"/>.
    ///   </para>
    /// </summary>
    /// <param name="name"> The cell name that is being changed.</param>
    /// <param name="content"> The new content of the cell.</param>
    /// <returns>
    ///   <para>
    ///     This method returns a list consisting of the passed in cell name,
    ///     followed by the names of all other cells whose value depends, directly
    ///     or indirectly, on the named cell. The order of the list MUST BE any
    ///     order such that if cells are re-evaluated in that order, their dependencies
    ///     are satisfied by the time they are evaluated.
    ///   </para>
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///   If the name parameter is invalid, throw an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///   If changing the contents of the named cell to be the formula would
    ///   cause a circular dependency, throw a CircularException. No change made to the Spreadsheet.
    /// </exception>
    public IList<string> SetContentsOfCell(string name, string content)
    {
        // Validate the name.
        if (!IsValidCellName(name))
        {
            throw new InvalidNameException();
        }

        // Try parsing the string as a double.
        IList<string> result;
        if (double.TryParse(content, out double numberContent))
        {
            // If it's a valid double, call the SetCellContents method for double.
            result = SetCellContents(name, numberContent);
        }
        else if (content.StartsWith("=")) // Try interpreting the string as a formula.
        {
        Formula formulaContent = new Formula(content.Substring(1));  // Remove the '=' when parsing as a formula.
        result = SetCellContents(name, formulaContent);
        }
        else // Otherwise, treat it as plain text.
        {
            result = SetCellContents(name, content);
        }

        // If the contents were successfully set, mark the spreadsheet as changed.
        Changed = true;

        return result;
    }

    /// <summary>
    ///   Provides a copy of the names of all of the cells in the spreadsheet
    ///   that contain information (i.e., not empty cells).
    /// </summary>
    /// <returns>
    ///   A set of the names of all the non-empty cells in the spreadsheet.
    /// </returns>
    public ISet<string> GetNamesOfAllNonemptyCells()
    {
        HashSet<string> nonEmptyCells = new HashSet<string>();

        foreach (var entry in cellContents)
        {
            if (!string.IsNullOrEmpty(entry.Value.Contents.ToString()))
            {
                nonEmptyCells.Add(entry.Key);
            }
        }

        return nonEmptyCells;
    }

    /// <summary>
    ///   Returns the contents (as opposed to the value) of the named cell.
    /// </summary>
    /// <exception cref="InvalidNameException">
    ///   Thrown if the name is invalid.
    /// </exception>
    /// <param name="name">The name of the spreadsheet cell to query. </param>
    /// <returns>
    ///   The contents as either a string, a double, or a Formula.
    ///   See the class header summary.
    /// </returns>
    public object GetCellContents(string name)
    {
        if (!IsValidCellName(name))
        {
            throw new InvalidNameException();
        }

        if (cellContents.ContainsKey(name))
        {
            return cellContents[name].Contents;
        }
        else
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Private lookup delegate to be passed into the Evaluate Method to evaluate formulas.
    /// </summary>
    /// <param name="name">the name of the cell to be looked up.</param>
    /// <returns>Returns the value of the cell if it exists.</returns>
    /// <exception cref="InvalidNameException">Exception if the value of the cell doe not exist.</exception>
    private double Lookup(string name)
    {
        if (cellContents.TryGetValue(name, out var cell) && cell.Contents is double number)
        {
            return number;
        }

        throw new InvalidNameException();
    }

    /// <summary>
    ///  Set the contents of the named cell to the given number.
    /// </summary>
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="number"> The new content of the cell. </param>
    /// <returns>
    ///   <para>
    ///     This method returns an ordered list consisting of the passed in name
    ///     followed by the names of all other cells whose value depends, directly
    ///     or indirectly, on the named cell. The order must correspond to a valid
    ///     dependency ordering for recomputing all of the cells.
    ///   </para>
    /// </returns>
    private IList<string> SetCellContents(string name, double number)
    {
        return SetCellContentsHelper(name, number);
    }

    /// <summary>
    ///   The contents of the named cell becomes the given text.
    /// </summary>
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="text"> The new content of the cell. </param>
    /// <returns>
    ///   The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, string text)
    {
        return SetCellContentsHelper(name, text);
    }

    /// <summary>
    ///   Set the contents of the named cell to the given formula.
    /// </summary>
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///   <para>
    ///     If changing the contents of the named cell to be the formula would
    ///     cause a circular dependency, throw a CircularException.
    ///   </para>
    ///   <para>
    ///     No change is made to the spreadsheet.
    ///   </para>
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="formula"> The new content of the cell. </param>
    /// <returns>
    ///   The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, Formula formula)
    {
        return SetCellContentsHelper(name, formula);
    }

    /// <summary>
    ///   Returns an enumeration, without duplicates, of the names of all cells whose
    ///   values depend directly on the value of the named cell.
    /// </summary>
    /// <param name="name"> This <b>MUST</b> be a valid name.  </param>
    /// <returns>
    ///   <para>
    ///     Returns an enumeration, without duplicates, of the names of all cells
    ///     that contain formulas containing name.
    ///   </para>
    /// </returns>
    private IEnumerable<string> GetDirectDependents(string name)
    {
        return dependencyGraph.GetDependents(name);
    }

    /// <summary>
    /// Checks if the given cell name is valid. It must be one or more letters
    ///   followed by one or more numbers. It cannot be empty or null.
    /// </summary>
    /// <param name="name">The cell name to check.</param>
    /// <returns>True if valid, otherwise false.</returns>
    private bool IsValidCellName(string name)
    {
        // Check if the name is not null or empty.
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        // Define the regex pattern for a valid cell name (from the Formula class)
        string cellNamePattern = @"^[a-zA-Z]+[0-9]+$";

        // Check if the name matches the regex pattern.
        return Regex.IsMatch(name, cellNamePattern);
    }

    /// <summary>
    /// Sets the contents of a specified cell and updates the dependencies.
    /// If a circular dependency is detected, the operation is stopped and reverted.
    /// </summary>
    /// <exception cref="InvalidNameException">
    /// Thrown if the cell name is invalid.
    /// </exception>
    /// <exception cref="CircularException">
    /// Thrown if setting the content causes a circular dependency.
    /// </exception>
    /// <param name="name">The name of the cell to set the content for.</param>
    /// <param name="content">The content to set (string, double, or formula).</param>
    /// <returns>A list of cells that need to be recalculated after the update.</returns>
    private IList<string> SetCellContentsHelper(string name, object content)
    {
        object originalContent;
        if (cellContents.ContainsKey(name))
        {
            originalContent = cellContents[name].Contents;
        }
        else
        {
            originalContent = string.Empty;
        }

        // Backup the original dependents
        HashSet<string> originalDependents = new HashSet<string>(dependencyGraph.GetDependees(name));

        try
        {
            // If it's a formula, validate for circular dependencies
            if (content is Formula formula)
            {
                // Temporarily set the formula's variables as dependees for the cell
                dependencyGraph.ReplaceDependees(name, formula.GetVariables());
            }
            else
            {
                // If it's not a formula, remove any existing dependees for the cell
                dependencyGraph.ReplaceDependees(name, new HashSet<string>());
            }

            // Set the new content in the cell
            cellContents[name] = new Cell(name, content);

            // Return a list of cells that need to be recalculated
            return GetCellsToRecalculate(name).ToList();
        }
        catch (CircularException)
        {
            // Revert content and dependencies on circular dependency
            if (cellContents.ContainsKey(name))
            {
                cellContents[name].Contents = originalContent;
            }

            // Restore the original dependents
            dependencyGraph.ReplaceDependees(name, originalDependents);

            throw;
        }
    }

    /// <summary>
    ///   <para>
    ///     Returns an enumeration of the names of all cells whose values must
    ///     be recalculated, assuming that the contents of the cell referred
    ///     to by name has changed.  The cell names are enumerated in an order
    ///     in which the calculations should be done.
    ///   </para>
    ///   <exception cref="CircularException">
    ///     If the cell referred to by name is involved in a circular dependency,
    ///     throws a CircularException.
    ///   </exception>
    /// </summary>
    /// <param name="name"> The name of the cell. Requires that name be a valid cell name.</param>
    /// <returns>
    ///    Returns an enumeration of the names of all cells whose values must
    ///    be recalculated.
    /// </returns>
    private IEnumerable<string> GetCellsToRecalculate(string name)
    {
        // Initialize a linked list to keep track of cells that need to be recalculated.
        LinkedList<string> changed = new();

        // Initialize a hash set to track visited cells and prevent infinite loops.
        HashSet<string> visited = [];

        // Start the depth-first search to visit all dependent cells.
        Visit(name, name, visited, changed);

        // Return the list of cells that need to be recalculated, in the correct order.
        return changed;
    }

    /// <summary>
    /// This method performs a depth-first search to find all cells that depend
    /// on the given cell. It checks for circular dependencies and adds the
    /// names of dependent cells to the changed list in the order they should
    /// be recalculated. It is a helper method used for the GetCellsToRecalculate method.
    /// </summary>
    /// <param name="start">The name of the original cell being checked for dependencies.</param>
    /// <param name="name">The name of the current cell being visited in the search.</param>
    /// <param name="visited">A set of visited cells to track which cells have already been processed.</param>
    /// <param name="changed">>A linked list to maintain the order of cells that need to be recalculated.</param>
    /// <exception cref="CircularException">Thrown when a circular dependency is detected.</exception>
    private void Visit(string start, string name, ISet<string> visited, LinkedList<string> changed)
    {
        // Mark the current cell as visited.
        visited.Add(name);

        // Iterate over all direct dependents of the current cell.
        foreach (string dependent in GetDirectDependents(name))
        {
            // Check for circular dependency by seeing if the dependent is the starting cell.
            if (dependent.Equals(start))
            {
                throw new CircularException();
            }

            // If the dependent cell hasn't been visited yet, recursively visit it.
            else if (!visited.Contains(dependent))
            {
                Visit(start, dependent, visited, changed);
            }
        }

        // Add the current cell to the front of the changed list, indicating it needs to be recalculated.
        changed.AddFirst(name);
    }
}

/// <summary>
/// Represents an individual cell in the spreadsheet.
/// </summary>
internal class Cell
{
    private readonly string name;

    /// <summary>
    /// Initializes a new instance of the <see cref="Cell"/> class.
    /// </summary>
    /// <param name="label">The label/name of the cell.</param>
    /// <param name="contents">The contents of the cell.</param>
    public Cell(string label, object contents)
    {
        name = label;
        Contents = contents;
    }

    /// <summary>
    /// Gets or sets the contents of the cell.
    /// </summary>
    public object Contents { get; set; }
}

/// <summary>
/// <para>
///   Thrown to indicate that a read or write attempt has failed with
///   an expected error message informing the user of what went wrong.
/// </para>
/// </summary>
public class SpreadsheetReadWriteException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpreadsheetReadWriteException"/> class.
    /// Creates the exception with a message defining what went wrong.
    /// </summary>
    /// <param name="msg"> An informative message to the user.</param>
    public SpreadsheetReadWriteException(string msg)
    : base(msg)
    {
    }
}