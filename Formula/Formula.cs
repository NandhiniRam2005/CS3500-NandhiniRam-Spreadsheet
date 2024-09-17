// <copyright file="Formula.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

/// <summary>
/// Author:    Nandhini Ramanathan, Professor Joe, Danny, and Jim
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
///    This file defines the Formula class, which represents mathematical formulas
///    in standard infix notation. The class is responsible for validating formulas,
///    extracting variables, and converting the formula into a canonical string form
///    with different public and private helper methods. This file can also now evaluate
///    validated formulas.
/// </summary>

namespace CS3500.Formula;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
///   <para>
///     This class represents formulas written in standard infix notation using standard precedence
///     rules.  The allowed symbols are non-negative numbers written using double-precision
///     floating-point syntax; variables that consist of one ore more letters followed by
///     one or more numbers; parentheses; and the four operator symbols +, -, *, and /. This class
///     can also now evaluate validated formulas and returns a Formula Error object when a variable
///     is unknown or when division by 0 occurs.
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

        // Check if the last token is invalid (operator)
        string lastToken = tokensInFormula.Last();
        if (lastToken == "+" || lastToken == "-" || lastToken == "*" || lastToken == "/")
        {
            throw new FormulaFormatException("Formula cannot end with an operator.");
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

    /// <summary>
    ///   <para>
    ///     Reports whether f1 == f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are the same.</returns>
#pragma warning disable SA1201 // Elements should appear in the correct order
    public static bool operator ==(Formula f1, Formula f2)
    {
        // Check if both objects are null
        if (ReferenceEquals(f1, null) && ReferenceEquals(f2, null))
        {
            return true;
        }

        // Check if one is null and the other is not
        if (ReferenceEquals(f1, null) || ReferenceEquals(f2, null))
        {
            return false;
        }

        // Use the Equals method for comparison
        return f1.Equals(f2);
    }
#pragma warning restore SA1201 // Elements should appear in the correct order

    /// <summary>
    ///   <para>
    ///     Reports whether f1 != f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are not equal to each other.</returns>
    public static bool operator !=(Formula f1, Formula f2)
    {
        return !(f1 == f2);
    }

    /// <summary>
    ///   <para>
    ///     Determines if two formula objects represent the same formula.
    ///   </para>
    ///   <para>
    ///     By definition, if the parameter is null or does not reference
    ///     a Formula Object then return false.
    ///   </para>
    ///   <para>
    ///     Two Formulas are considered equal if their canonical string representations
    ///     (as defined by ToString) are equal.
    ///   </para>
    /// </summary>
    /// <param name="obj"> The other object.</param>
    /// <returns>
    ///   True if the two objects represent the same formula.
    /// </returns>
    public override bool Equals(object? obj)
    {
        // Check if the object is null or not of the same type
        if (obj == null || !(obj is Formula))
        {
            return false;
        }

        // Compare using the canonical string representation
        Formula formula = (Formula)obj;
        return this.ToString() == formula.ToString();
    }

    /// <summary>
    ///   <para>
    ///     Evaluates this Formula, using the lookup delegate to determine the values of
    ///     variables.
    ///   </para>
    ///   <remarks>
    ///     When the lookup method is called, it will always be passed a Normalized (capitalized)
    ///     variable name.  The lookup method will throw an ArgumentException if there is
    ///     not a definition for that variable token.
    ///   </remarks>
    ///   <para>
    ///     If no undefined variables or divisions by zero are encountered when evaluating
    ///     this Formula, the numeric value of the formula is returned.  Otherwise, a
    ///     FormulaError is returned (with a meaningful explanation as the Reason property).
    ///   </para>
    ///   <para>
    ///     This method should never throw an exception.
    ///   </para>
    /// </summary>
    /// <param name="lookup">
    ///   <para>
    ///     Given a variable symbol as its parameter, lookup returns the variable's (double) value
    ///     (if it has one) or throws an ArgumentException (otherwise).  This method should expect
    ///     variable names to be capitalized.
    ///   </para>
    /// </param>
    /// <returns> Either a double or a formula error, based on evaluating the formula.</returns>
    public object Evaluate(Lookup lookup)
    {
        // Stack for storing numeric values (numbers or variable values).
        Stack<double> valueStack = new Stack<double>();

        // Stack for storing operators and parentheses.
        Stack<string> operatorStack = new Stack<string>();

        // Loop through each token in the validated formula.
        foreach (string token in validatedTokens)
        {
            // If the token is a number, process it.
            if (double.TryParse(token, out double number))
            {
                if (!ProcessValue(number, valueStack, operatorStack, out string errorMessage))
                {
                    return new FormulaError(errorMessage);
                }
            }

            // If the token is a variable, use the lookup to get its value and process it.
            else if (IsVar(token))
            {
                double variableValue;
                try
                {
                    // Attempt to get the variable's value using the provided lookup.
                    variableValue = lookup(token);
                }
                catch (ArgumentException)
                {
                    return new FormulaError($"{token} variable is unknown or undefined.");
                }

                if (!ProcessValue(variableValue, valueStack, operatorStack, out string errorMessage))
                {
                    return new FormulaError(errorMessage);
                }
            }

            // If the token is + or -, handle addition and subtraction.
            else if (token == "+" || token == "-")
            {
                ProcessPlusOrMinus(valueStack, operatorStack);

                // Push the current + or - token onto the operator stack for later processing.
                operatorStack.Push(token);
            }

            // If the token is * or /, push it onto the operator stack to be handled later.
            else if (token == "*" || token == "/")
            {
                operatorStack.Push(token);
            }

            // If the token is a left parenthesis, push it onto the operator stack.
            else if (token == "(")
            {
                operatorStack.Push(token);
            }

            // If the token is a right parenthesis, process the expression inside the parenthesis.
            else if (token == ")")
            {
                if (!ProcessParenthesis(valueStack, operatorStack, out string errorMessage))
                {
                    return new FormulaError(errorMessage);
                }
            }
        }

        // Final processing after all tokens have been handled.
        if (operatorStack.Count == 0)
        {
            // If the operator stack is empty, there should be exactly one value in the value stack (the result).
            return valueStack.Pop();
        }
        else
        {
            // If there is an operator left, it should be a single + or - with two values in the stack.
            string additionOrSubtractionOperator = operatorStack.Pop();
            double rightOperand = valueStack.Pop();
            double leftOperand = valueStack.Pop();

            // Perform the final addition or subtraction.
            if (additionOrSubtractionOperator == "+")
            {
                return leftOperand + rightOperand;
            }
            else
            {
                return leftOperand - rightOperand;
            }
        }
    }

    /// <summary>
    ///   <para>
    ///     Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
    ///     case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two
    ///     randomly-generated unequal Formulas have the same hash code should be extremely small.
    ///   </para>
    /// </summary>
    /// <returns> The hashcode for the object. </returns>
    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    /// <summary>
    /// Processes a value (either a number or variable) token and handling multiplication and division if needed.
    /// </summary>
    /// <param name="value">The value (number or variable) to process.</param>
    /// <param name="valueStack">The stack used to store numeric values.</param>
    /// <param name="operatorStack">The stack used to store operators.</param>
    /// <param name="errorMessage">An output parameter that returns the error message if an error occurs.</param>
    /// <returns>True if processing was successful, otherwise false.</returns>
    private bool ProcessValue(double value, Stack<double> valueStack, Stack<string> operatorStack, out string errorMessage)
    {
        errorMessage = string.Empty;
        double result;

        // Check if the operator stack has * or / on top, which means they need to be processed.
        if (operatorStack.Count > 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
        {
            string multiplicationOrDivisionOperator = operatorStack.Pop();
            double leftOperand = valueStack.Pop();

            // Handle division by zero.
            if (multiplicationOrDivisionOperator == "/" && value == 0)
            {
                errorMessage = "Cannot divide by zero.";
                return false;
            }

            // Perform the multiplication or division and push the result back onto the value stack.
            if (multiplicationOrDivisionOperator == "*")
            {
                result = leftOperand * value;
            }
            else
            {
                result = leftOperand / value;
            }

            valueStack.Push(result);
        }
        else
        {
            // If there's no * or / to apply, simply push the value onto the value stack.
            valueStack.Push(value);
        }

        return true;
    }

    /// <summary>
    /// Processes a plus or minus token, performing any remaining addition or subtraction on the value stack.
    /// </summary>
    /// <param name="valueStack">The stack used to store numeric values.</param>
    /// <param name="operatorStack">The stack used to store operators.</param>
    private void ProcessPlusOrMinus(Stack<double> valueStack, Stack<string> operatorStack)
    {
        double result;

        // If there is a + or - on top of the operator stack, apply it to the top two values in the value stack.
        if (operatorStack.Count > 0 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
        {
            string additionOrSubtractionOperator = operatorStack.Pop();
            double rightOperand = valueStack.Pop();
            double leftOperand = valueStack.Pop();

            // Perform the addition or subtraction.
            if (additionOrSubtractionOperator == "+")
            {
                result = leftOperand + rightOperand;
            }
            else
            {
                result = leftOperand - rightOperand;
            }

            // Push the result back onto the value stack.
            valueStack.Push(result);
        }
    }

    /// <summary>
    /// Processes a parenthesis token, evaluating the expression inside the parenthesis.
    /// </summary>
    /// <param name="valueStack">The stack used to store numeric values.</param>
    /// <param name="operatorStack">The stack used to store operators.</param>
    /// <param name="errorMessage">An output parameter that returns the error message if an error occurs.</param>
    /// <returns>True if processing was successful, otherwise false.</returns>
    private bool ProcessParenthesis(Stack<double> valueStack, Stack<string> operatorStack, out string errorMessage)
    {
        errorMessage = string.Empty;

        // If there is a + or - on top of the operator stack, apply it to the top two values.
        ProcessPlusOrMinus(valueStack, operatorStack);

        // Pop the right parenthesis from the operator stack (We know there is a parentheses
        // since the Evaluate method checks this token already so we don't need another check).
        operatorStack.Pop();

        // If there is a * or / now at the top of the operator stack, apply it to the top two values.
        if (operatorStack.Count > 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
        {
            string multiplicationOrDivisionOperator = operatorStack.Pop();
            double rightOperand = valueStack.Pop();
            double leftOperand = valueStack.Pop();

            // Handle division by zero.
            if (multiplicationOrDivisionOperator == "/" && rightOperand == 0)
            {
                errorMessage = "Cannot divide by zero.";
                return false;
            }

            // Perform the multiplication or division.
            double result;
            if (multiplicationOrDivisionOperator == "*")
            {
                result = leftOperand * rightOperand;
            }
            else
            {
                result = leftOperand / rightOperand;
            }

            // Push the result back onto the value stack.
            valueStack.Push(result);
        }

        return true;
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
    }
}

/// <summary>
/// Used as a possible return value of the Formula.Evaluate method.
/// </summary>
public class FormulaError
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaError"/> class.
    ///   <para>
    ///     Constructs a FormulaError containing the explanatory reason.
    ///   </para>
    /// </summary>
    /// <param name="message"> Contains a message for why the error occurred.</param>
    public FormulaError(string message)
    {
        Reason = message;
    }

    /// <summary>
    ///  Gets the reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }
}

/// <summary>
///   Any method meeting this type signature can be used for
///   looking up the value of a variable.  In general the expected behavior is that
///   the Lookup method will "know" about all variables in a formula
///   and return their appropriate value.
/// </summary>
/// <exception cref="ArgumentException">
///   If a variable name is provided that is not recognized by the implementing method,
///   then the method should throw an ArgumentException.
/// </exception>
/// <param name="variableName">
///   The name of the variable (e.g., "A1") to lookup.
/// </param>
/// <returns> The value of the given variable (if one exists). </returns>
public delegate double Lookup(string variableName);