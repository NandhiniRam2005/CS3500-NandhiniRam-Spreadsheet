// <copyright file="Spreadsheet.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// Written by Joe Zachary for CS 3500, September 2013
// Update by Professor Kopta and de St. Germain
//     - Updated return types
//     - Updated documentation

namespace CS3500.Spreadsheet;

using CS3500.Formula;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

/// <summary>
/// Author:    Nandhini Ramanathan
/// Partner:   None
/// Date:      September 27,2024
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Nandhini Ramanathan - This work may not
///            be copied for use in Academic Coursework.
///
/// I, Nandhini Ramanathan, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All
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
    // A dictionary to hold cell contents by their names.
    private readonly Dictionary<string, object> cellContents;

    // An instance of the DependencyGraph to manage cell dependencies.
    private readonly DependencyGraph dependencyGraph;

    /// <summary>
    /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
    /// It initializes the cell contents dictionary and the dependency graph.
    /// </summary>
    public Spreadsheet()
    {
        cellContents = new Dictionary<string, object>();
        dependencyGraph = new DependencyGraph();
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
            if (!string.IsNullOrEmpty(entry.Value.ToString()))
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
            return cellContents[name];
        }
        else
        {
            return string.Empty;
        }
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
    public IList<string> SetCellContents(string name, double number)
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
    public IList<string> SetCellContents(string name, string text)
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
    public IList<string> SetCellContents(string name, Formula formula)
    {
        // Track visited cells to avoid infinite loops
        var visited = new HashSet<string>();

        if (HasCircularDependencyHelper(name, formula, visited))
        {
            throw new CircularException();
        }

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
        string cellNamePattern = @"[a-zA-Z]+\d+";

        // Check if the name matches the regex pattern.
        return Regex.IsMatch(name, cellNamePattern);
    }

    /// <summary>
    /// Checks if setting the contents of the specified cell to the given formula
    /// would create a circular dependency.
    /// </summary>
    /// <param name="name">The name of the cell to check.</param>
    /// <param name="formula">The formula to set.</param>
    /// <param name="visited">A set to keep track of visited cells.</param>
    /// <returns>True if a circular dependency would occur, otherwise false.</returns>
    private bool HasCircularDependencyHelper(string name, Formula formula, HashSet<string> visited)
    {
        // Loop through variables in the formula
        foreach (var variable in formula.GetVariables())
        {
            // If the variable is already visited, we have a circular dependency
            if (visited.Contains(variable))
            {
                return true;
            }

            // Add the current variable to the visited set
            visited.Add(variable);

            // Recursively check dependents for circular dependencies
            foreach (var dependent in GetDirectDependents(variable))
            {
                if (dependent == name)
                {
                    return true;
                }

                if (HasCircularDependencyHelper(dependent, formula, visited))
                {
                    return true;
                }
            }

            // Remove variable after checking
            visited.Remove(variable);
        }

        return false;
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
        // Check if the cell name is valid
        if (!IsValidCellName(name))
        {
            throw new InvalidNameException();
        }

        // Store the original contents of the cell in case we need to revert to it
        object originalContent;

        // Check if the cell already has content and store it, or set it to an empty string
        if (cellContents.ContainsKey(name))
        {
            originalContent = cellContents[name];
        }
        else
        {
            originalContent = string.Empty;
        }

        // Clear existing dependencies on the cell to avoid issues when updating content
        if (cellContents.ContainsKey(name))
        {
            dependencyGraph.ReplaceDependents(name, new HashSet<string>());
        }

        try
        {
            // Update the cell content with the new value
            cellContents[name] = content;

            if (content is Formula formula)
            {
                foreach (var variable in formula.GetVariables())
                {
                    dependencyGraph.AddDependency(name, variable);
                }
            }

            // Return a list of cells that need to be recalculated as a result of this change
            return GetCellsToRecalculate(name).ToList();
        }
        catch (CircularException)
        {
            // If a CircularException is thrown, revert the cell contents to the original value
            cellContents[name] = originalContent;

            // Rethrow the exception to indicate that the operation failed due to a circular dependency
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

        Visit(name, name, visited, changed);
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

        // Add the current cell to the front of the changed list, indicating it needsto be recalculated.
        changed.AddFirst(name);
    }
}