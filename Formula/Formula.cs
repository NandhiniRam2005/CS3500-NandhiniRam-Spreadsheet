// <copyright file="Formula.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors> Nandhini Ramanathan, Professor Joe, Danny, and Jim </authors>
// <date> August 25,2023 </date>

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
///   <para>
///     For Assignment Two, you are to implement the following functionality:
///   </para>
///   <list type="bullet">
///     <item>
///        Formula Constructor which checks the syntax of a formula.
///     </item>
///     <item>
///        Get Variables
///     </item>
///     <item>
///        ToString
///     </item>
///   </list>
/// </summary>
public class Formula
{
    /// <summary>
    ///   All variables are letters followed by numbers.  This pattern
    ///   represents valid variable name strings.
    /// </summary>
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";

    /// <summary>
    ///   This list represents the tokens from the given formula.
    /// </summary>
    private List<string> validatedTokens;

    /// <summary>
    ///  This is a string representation of a canonical form of the formula.
    /// </summary>
    private string canonicalFormula;

    /// <summary>
    ///   Initializes a new instance of the <see cref="Formula"/> class.
    ///   <para>
    ///     Creates a Formula from a string that consists of an infix expression written as
    ///     described in the class comment.  If the expression is syntactically incorrect,
    ///     throws a FormulaFormatException with an explanatory Message.  See the assignment
    ///     specifications for the syntax rules you are to implement.
    ///   </para>
    ///   <para>
    ///     Non Exhaustive Example Errors:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///        Invalid variable name, e.g., x, x1x  (Note: x1 is valid, but would be normalized to X1)
    ///     </item>
    ///     <item>
    ///        Empty formula, e.g., string.Empty
    ///     </item>
    ///     <item>
    ///        Mismatched Parentheses, e.g., "(("
    ///     </item>
    ///     <item>
    ///        Invalid Following Rule, e.g., "2x+5"
    ///     </item>
    ///   </list>
    /// </summary>
    /// <param name="formula"> The string representation of the formula to be created.</param>
    public Formula(string formula)
    {
        double validatedNumericalValue;
        int parenthesisCounter = 0;
        bool isNextTokenOperand = true;
        bool hasTokenInsideParentheses = false;
        this.validatedTokens = new ();
        this.canonicalFormula = string.Empty;

        if (string.IsNullOrWhiteSpace(formula))
        {
            throw new FormulaFormatException("Formula cannot be empty or contain only whitespace.");
        }

        // Enumerate tokens in the formula
        List<string> tokensInFormula = GetTokens(formula);

        foreach (string token in tokensInFormula)
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
                    throw new FormulaFormatException("Unexpected variable where an operand is not expected.");
                }
            }
            else if (double.TryParse(token, out validatedNumericalValue))
            {
                if (isNextTokenOperand)
                {
                    this.validatedTokens.Add(validatedNumericalValue.ToString());
                    isNextTokenOperand = false;
                    hasTokenInsideParentheses = true;
                }
                else
                {
                    throw new FormulaFormatException("Unexpected number where an operand is not expected.");
                }
            }
            else if (token == "(")
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
                    throw new FormulaFormatException("Mismatched parentheses");
                }

                if (!hasTokenInsideParentheses)
                {
                    throw new FormulaFormatException("Empty parentheses are not allowed.");
                }

                this.validatedTokens.Add(token);
                isNextTokenOperand = false;
            }
            else if (token == "+" || token == "-" || token == "*" || token == "/")
            {
                if (!isNextTokenOperand)
                {
                    this.validatedTokens.Add(token);
                    isNextTokenOperand = true;
                }
                else
                {
                    throw new FormulaFormatException("Unexpected operator where an operand is expected.");
                }
            }
            else
            {
                throw new FormulaFormatException("Invalid token in formula.");
            }
        }

        if (parenthesisCounter != 0)
        {
            throw new FormulaFormatException("Mismatched parentheses.");
        }

        // Join the strings in the validatedTokens list without any spaces and store it in the canonicalFormula string varaible
        this.canonicalFormula = string.Join(string.Empty, this.validatedTokens);
    }

    /// <summary>
    ///   <para>
    ///     Returns a set of all the variables in the formula.
    ///   </para>
    ///   <remarks>
    ///     Important: no variable may appear more than once in the returned set, even
    ///     if it is used more than once in the Formula.
    ///   </remarks>
    ///   <para>
    ///     For example, if N is a method that converts all the letters in a string to upper case:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>new("x1+y1*z1").GetVariables() should enumerate "X1", "Y1", and "Z1".</item>
    ///     <item>new("x1+X1"   ).GetVariables() should enumerate "X1".</item>
    ///   </list>
    /// </summary>
    /// <returns> the set of variables (string names) representing the variables referenced by the formula. </returns>
    public ISet<string> GetVariables()
    {
        // Create a HashSet to hold unique variables
        HashSet<string> variables = new ();

        // Iterate through tokens and add variables to the set
        foreach (string token in this.validatedTokens)
        {
            if (IsVar(token))
            {
                variables.Add(token.ToUpper());
            }
        }

        return variables;
    }

    /// <returns>
    ///  A canonical version (string) of the formula. All "equal" formulas
    ///   should have the same value here.
    /// </returns>
    /// <summary>
    ///   <para>
    ///     Returns a string representation of a canonical form of the formula.
    ///   </para>
    ///   <para>
    ///     The string will contain no spaces.
    ///   </para>
    ///   <para>
    ///     If the string is passed to the Formula constructor, the new Formula f 
    ///     will be such that this.ToString() == f.ToString().
    ///   </para>
    ///   <para>
    ///     All of the variables in the string will be normalized.  This
    ///     means capital letters.
    ///   </para>
    ///   <para>
    ///       For example:
    ///   </para>
    ///   <code>
    ///       new("x1 + y1").ToString() should return "X1+Y1"
    ///       new("X1 + 5.0000").ToString() should return "X1+5".
    ///   </code>
    ///   <para>
    ///     This code should execute in O(1) time.
    ///   <para>
    /// </summary>
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
        List<string> results = [];

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
    /// <param name="message"> A developer defined message describing why the exception occured.</param>
    public FormulaFormatException(string message)
        : base(message)
    {
        // All this does is call the base constructor. No extra code needed.
    }
}