```
Author:     Nandhini Ramanathan
Partner:    None
Start Date: 30-Aug-2024
Course:     CS 3505, University of Utah, School of Computing
GitHub ID:  NandhiniRam2005
Repo:       https://github.com/uofu-cs3500-20-fall2024/spreadsheet-NandhiniRam2005
Commit Date: 27-Sep-2024 9:20pm
Solution:   Spreadsheet
Copyright:  CS 3500 and Nandhini Ramanathan - This work may not be copied for use in Academic Coursework.
```

# Overview of the Spreadsheet functionality

The Spreadsheet program is currently capable of constructing a formula and checking its validity.
It checks the tokens in the formula including variables, numeric values, parenthesis, and normalizes 
them when necessary, throwing a FormulaFormatException when invalid tokens are present with a meaningful message.

The program has been extended to include the implementation of a Dependency Graph. The Dependency Graph
manages relationships between cells, where one cell can depend on another. This graph enables efficient 
updates to cells when dependencies change, ensuring proper recalculations.

The program has been extended to include the implementation of evaluating formulas. It takes a validated
formula (tokens validated in constructor) and mathematically evaluates them. The == and != operators, and 
the Equals method have been implemented in the Formula class to define Equality between Formula objects.

A Spreadsheet class has been added to store cell contents, managing dependencies between cells, and 
ensuring there are no circular dependencies. It incorporated the Formula and DependencyGraph class together 
to accomplish this.

Future extensions include implementing a GUI for a visual element to the Spreadsheet.

# Examples of Good Software Practices (GSP)

1. DRY (Don't Repeat Yourself)

In the Spreadsheet implementation, I utilized helper methods to encapsulate repeated logic, such as to set cell contents
and dependency management to check for a circular exception. This not only simplifies the code to make it more readable 
but also enhances maintainability, as changes can be made in one place without affecting multiple code sections.

2. Separation of Concerns

Each class in the solution has a well-defined responsibility. The Formula class handles formula construction and validation, 
while the DependencyGraph class manages cell dependencies. This separation allows for easier testing and debugging, as each 
component can be modified independently.

3. Well-Named and Documented Methods

Throughout my code, I have prioritized using descriptive names for methods and variables. For example, methods like 
HasCircularDependencyHelper and SetCellContentsHelper clearly communicate their purpose, making the code more intuitive. Additionally, 
I have included XML documentation comments for all classes and methods to provide further clarity on their functionality.

Other Good Software Practices Achieved:

- Complexity management through breaking down complex algorithms into smaller, well-named helper methods like the Evaluate method, 
  Formula constructor, and the set cell contents method in the Spreadsheet class.
- Code reuse by utilizing existing C# libraries and avoiding redundant implementations like the Dictionary and reusing my DependencyGraph 
  class for my Spreadsheet class.
- Adherence to style guidelines as defined in the .editorconfig and stylecop file by making sure there are no more warnings.
- Versioning with meaningful commit messages to track changes effectively when starting a new assignment.

# Time Expenditures:

    1. Assignment 1: Test Driven Development      Predicted Hours:   4        Actual Hours:   5
    2. Assignment 2: Formula Class                Predicted Hours:   7        Actual Hours:   9

                                                                              Hours spent -
                                                                                Effectively:      5  
                                                                                Debugging:        2
                                                                                Learning tools:   2

    3. Assignment 3: Dependency Graph Class       Predicted Hours:   9        Actual Hours:   10

                                                                              Hours spent -
                                                                                Effectively:      7    
                                                                                Debugging:        2   
                                                                                Learning tools:   1

    4. Assignment 4: Evaluating a Formula         Predicted Hours:   8        Actual Hours:   11

                                                                              Hours spent -
                                                                                Effectively:      8    
                                                                                Debugging:        2   
                                                                                Learning tools:   1

    5. Assignment 5: Onward to a Spreadsheet      Predicted Hours:   7        Actual Hours:   8

                                                                              Hours spent -
                                                                                Effectively:      6    
                                                                                Debugging:        1   
                                                                                Learning tools:   1
                                                             
                                                             
