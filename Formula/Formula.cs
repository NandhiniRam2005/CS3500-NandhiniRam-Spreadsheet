// <copyright file="Formula.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

/// <summary>
/// Author:    Nandhini Ramanathan, Professor Joe, Danny, and Jim
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
///    This file defines the Formula class, which represents mathematical formulas
///    in standard infix notation. The class is responsible for validating formulas,
///    extracting variables, and converting the formula into a canonical string form
///    with different public and private helper methods.
/// </summary>

namespace CS3500.Formula;

using System.Text.RegularExpressions;

/// <summary>
///   <para>
///     This class represents formulas written in standard infix notation using standard precedence
///     rules.  The allowed symbols are non-negative numbers written using double-precision
///     floating-point syntax; variables that consist of one ore more letters followed by
///     one or more numbers; parentheses; and the four operator symbols +, -, *, and /.
///   </para>
///   <para>
///     Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
///     a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable;
///     and "x 23" consists of a variable "x" and a number "23".  Otherwise, spaces are to be removed.
///   </para>
/// </summary>
public class Formula
{
    /// <summary>
    ///   All variables are letters followed by numbers.  This pattern
    ///   represents valid variable name strings.
    /// </summary>
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";

    /// <summary>
    ///   This list represents the tokens from the given formula that are checked and validated in the constructor.
    /// </summary>
    private readonly List<string> validatedTokens;

    /// <summary>
    ///  This is a string representation of a valid canonical form of the formula.
    /// </summary>
    private readonly string canonicalFormula;

    /// <summary>
    ///   Initializes a new instance of the <see cref="Formula"/> class.
    ///   <para>
    ///     Creates a Formula from a string that consists of an infix expression written as
    ///     described in the class comment.  If the expression is syntactically incorrect,
    ///     throws a FormulaFormatException with an explanatory Message. Some syntactical errors
    ///     include invalid variable names, empty formula, mismatched parenthesis, and the
    ///     invalid following rule.
    ///   </para>
    /// </summary>
    /// <param name="formula"> The string representation of the formula to be created.</param>
    public Formula(string formula)
    {
        this.validatedTokens = new ();
        this.canonicalFormula = string.Empty;

        int parenthesisCounter = 0;
        bool isNextTokenOperand = true;
        bool hasTokenInsideParentheses = false;

        if (string.IsNullOrWhiteSpace(formula))
        {
            throw new FormulaFormatException("Formula cannot be empty or contain only whitespace.");
        }

        // Enumerates formula's tokens
        List<string> tokensInFormula = GetTokens(formula);

        foreach (string token in tokensInFormula)
        {
            if (IsVar(token) || double.TryParse(token, out double validatedNumericalValue))
            {
                HandleOperand(token, ref isNextTokenOperand, ref hasTokenInsideParentheses);
            }
            else if (token == "(" || token == ")")
            {
                HandleParenthesis(token, ref parenthesisCounter, ref isNextTokenOperand, hasTokenInsideParentheses);
            }
            else if (token == "+" || token == "-" || token == "*" || token == "/")
            {
                HandleOperator(token, ref isNextTokenOperand);
            }

            // If any other token is present that was not covered above, throw a FormulaFormatException.
            else
            {
                throw new FormulaFormatException("Invalid token present in the formula.");
            }
        }

        if (parenthesisCounter != 0)
        {
            throw new FormulaFormatException("Mismatched number of parentheses found/ parentheses are not balanced.");
        }

        // Join the strings in the validatedTokens list without any spaces and store it in the canonicalFormula string variable
        // The link to where I found this method is in my README.
        this.canonicalFormula = string.Join(string.Empty, this.validatedTokens);
    }

    /// <summary>
    ///   <para>
    ///     Returns a set of all the variables in the formula with no duplicates and variables are normalized to uppercase.
    ///   </para>
    /// </summary>
    /// <returns> the set of variables (string names) representing the variables referenced by the formula. </returns>
    public ISet<string> GetVariables()
    {
        HashSet<string> uniqueVariables = new();

        foreach (string token in this.validatedTokens)
        {
            if (IsVar(token))
            {
                uniqueVariables.Add(token.ToUpper());
            }
        }

        return uniqueVariables;
    }

    /// <summary>
    ///   <para>
    ///     Returns a string representation of a canonical form of the formula that contains no spaces.
    ///     All the variables in the string are also normalized (capital letters). This code executes in O(1) time.
    ///   </para>
    /// </summary>
    /// <returns>
    ///   A canonical version (string) of the formula. All "equal" formulas
    ///   should have the same value here.
    /// </returns>
    public override string ToString()
    {
        return this.canonicalFormula;
    }

    /// <summary>
    ///   Reports whether "token" is a variable.  It must be one or more letters
    ///   followed by one or more numbers.
    /// </summary>
    /// <param name="token"> A token that may be a variable. </param>
    /// <returns> true if the string matches the requirements, e.g., A1 or a1. </returns>
    private static bool IsVar(string token)
    {
        // notice the use of ^ and $ to denote that the entire string being matched is just the variable
        string standaloneVarPattern = $"^{VariableRegExPattern}$";
        return Regex.IsMatch(token, standaloneVarPattern);
    }

    /// <summary>
    ///   <para>
    ///     Given an expression, enumerates the tokens that compose it.
    ///   </para>
    ///   <para>
    ///     Tokens returned are:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>left paren</item>
    ///     <item>right paren</item>
    ///     <item>one of the four operator symbols</item>
    ///     <item>a string consisting of one or more letters followed by one or more numbers</item>
    ///     <item>a double literal</item>
    ///     <item>and anything that doesn't match one of the above patterns</item>
    ///   </list>
    ///   <para>
    ///     There are no empty tokens; white space is ignored (except to separate other tokens).
    ///   </para>
    /// </summary>
    /// <param name="formula"> A string representing an infix formula such as 1*B1/3.0. </param>
    /// <returns> The ordered list of tokens in the formula. </returns>
    private static List<string> GetTokens(string formula)
    {
        List<string> results = new ();

        string lpPattern = @"\(";
        string rpPattern = @"\)";
        string opPattern = @"[\+\-*/]";
        string doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
        string spacePattern = @"\s+";

        // Overall pattern
        string pattern = string.Format(
                                        "({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                        lpPattern,
                                        rpPattern,
                                        opPattern,
                                        VariableRegExPattern,
                                        doublePattern,
                                        spacePattern);

        // Enumerate matching tokens that don't consist solely of white space.
        foreach (string s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
        {
            if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
            {
                results.Add(s);
            }
        }

        return results;
    }

    /// <summary>
    /// Validates operands (variables and numbers) in the formula, whether they are expected to be placed there.
    /// </summary>
    /// <param name="token">The current token being processed.</param>
    /// <param name="isNextTokenOperand">A boolean indicating if the next token is expected to be an operand.</param>
    /// <param name="hasTokenInsideParentheses">A boolean if there is at least one valid token inside parentheses.<</param>
    /// <exception cref="FormulaFormatException">Exception thrown if an invalid operand is found or it should not be present in the formula based on the token beside it.</exception>
    private void HandleOperand(string token, ref bool isNextTokenOperand, ref bool hasTokenInsideParentheses)
    {
        if (IsVar(token))
        {
            if (isNextTokenOperand)
            {
                this.validatedTokens.Add(token.ToUpper());
                isNextTokenOperand = false;
                hasTokenInsideParentheses = true;
            }
            else
            {
                throw new FormulaFormatException("Invalid or unexpected variable here where it is not expected.");
            }
        }
        else if (double.TryParse(token, out double validatedNumericalValue))
        {
            if (isNextTokenOperand)
            {
                this.validatedTokens.Add(validatedNumericalValue.ToString());
                isNextTokenOperand = false;
                hasTokenInsideParentheses = true;
            }
            else
            {
                throw new FormulaFormatException("Invalid or unexpected number here where it is not expected.");
            }
        }
    }

    /// <summary>
    /// Validates parenthesis in the formula, ensuring there is an equal, balanced amount in the right order.
    /// </summary>
    /// <param name="token">The current token being processed.</param>
    /// <param name="parenthesisCounter">Counter for the number of open and close parenthesis.</param>
    /// <param name="isNextTokenOperand">A boolean indicating if the next token is expected to be an operand.</param>
    /// <param name="hasTokenInsideParentheses">A boolean if there is at least one valid token inside parentheses.</param>
    /// <exception cref="FormulaFormatException">Exception thrown if there are mismatched,unbalanced, or empty parentheses.</exception>
    private void HandleParenthesis(string token, ref int parenthesisCounter, ref bool isNextTokenOperand, bool hasTokenInsideParentheses)
    {
        if (token == "(")
        {
            parenthesisCounter++;
            isNextTokenOperand = true;
            this.validatedTokens.Add(token);
            hasTokenInsideParentheses = false;
        }
        else if (token == ")")
        {
            parenthesisCounter--;
            if (parenthesisCounter < 0)
            {
                throw new FormulaFormatException("Mismatched number of parentheses found/ parentheses are not balanced.");
            }

            if (!hasTokenInsideParentheses)
            {
                throw new FormulaFormatException("Empty parentheses without any valid token inside is invalid.");
            }

            this.validatedTokens.Add(token);
            isNextTokenOperand = false;
        }
    }

    /// <summary>
    /// Validates operators in the formula based on the token beside it.
    /// </summary>
    /// <param name="token">The current token being processed.</param>
    /// <param name="isNextTokenOperand">A boolean indicating if the next token is expected to be an operand.</param>
    /// <exception cref="FormulaFormatException">Exception thrown if an unexpected operator is found.</exception>
    private void HandleOperator(string token, ref bool isNextTokenOperand)
    {
        if (!isNextTokenOperand)
        {
            this.validatedTokens.Add(token);
            isNextTokenOperand = true;
        }
        else
        {
            throw new FormulaFormatException("Unexpected operator present where it is not expected.");
        }
    }
}

/// <summary>
///   Used to report syntax errors in the argument to the Formula constructor.
/// </summary>
public class FormulaFormatException : Exception
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaFormatException"/> class.
    ///   <para>
    ///      Constructs a FormulaFormatException containing the explanatory message.
    ///   </para>
    /// </summary>
    /// <param name="message"> A developer defined message describing why the exception occurred.</param>
    public FormulaFormatException(string message)
        : base(message)
    {
        // All this does is call the base constructor. No extra code needed.
    }
}