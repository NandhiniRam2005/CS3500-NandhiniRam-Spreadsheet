```
Author:     Nandhini Ramanathan
Partner:    None
Start Date: 18-Oct-2024
Course:     CS 3505, University of Utah, School of Computing
GitHub ID:  NandhiniRam2005
Repo:       https://github.com/uofu-cs3500-20-fall2024/spreadsheet-NandhiniRam2005
Commit Date: 18-Oct-2024 9:20pm
Project:    Spreadsheet
Copyright:  CS 3500 and Nandhini Ramanathan - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:

None.

# Assignment Specific Topics
The Spreadsheet project aims to implement a basic spreadsheet application that supports cell management and dependency tracking. 
It soon aims to allow users to interact with a grid of cells, each identified by a unique name, and to calculate results based 
on user-defined formulas while ensuring there are no circular dependencies among the cells.

The spreadsheet uses a dependency graph to manage relationships between cells. When a cell's contents are updated, the spreadsheet 
tracks which other cells depend on it. This mechanism ensures that any change in a cell's value automatically triggers updates in 
dependent cells. This class includes an indexer, which provides easy access to cells using their unique names. The added application 
now supports loading and saving spreadsheet data from and to files too.

# Examples of Good Software Practices (GSP)

1. Code Reuse

In the Spreadsheet implementation, I utilized helper methods to encapsulate repeated logic, such as to set cell contents by making a 
helper, which all three private SetCellContents methods call. Throughout the Spreadsheet project, minimized code duplication, common functionality, 
such as cell validation and error handling, was centralized within dedicated helper methods. This approach not only reduces redundancy but also 
simplifies maintenance, as changes to core logic only need to be made in one place. Consequently, it enhances code readability and ensures consistent 
behavior across the application.

2. Testing Strategies

Robust testing strategies were implemented to ensure the reliability of the Spreadsheet application. I adopted a Test-Driven Development (TDD) 
approach, writing unit tests before implementing methods. This ensured that each method meets its specifications from the outset. Additionally, 
I included edge cases in my tests to validate the system’s behavior under various scenarios. This proactive testing approach helps catch issues 
early in the development process, improving overall software quality. I also combined open box testing and kept all my tests in the end to produce 
well rounded tests that test edge, corner and normal cases which then I used to make stress tests (using PS5 autograder tests as reference).

3. Well-Named and Documented Methods

I employed abstract classes and interfaces to define common behaviors for different components, such as cells (cell class) and formulas (formula class). 
This abstraction allows for flexibility in implementation while ensuring that different parts of the system adhere to the same contracts. It also
made the code a lot more readable.

Other Good Software Practices Achieved:

- DRY (Don't Repeat Yourself): Centralized common logic in helper methods to minimize duplication.
- Separation of Concerns: Organized the codebase to separate spreadsheet logic from user interface and data management functionalities.
- Well-Named and Commented Methods: Used clear method names and comments to enhance code readability and understanding.
- Regression Testing: I had old tests that I just edited to fulfill the SetContentsOfCell method format and also did TDD.

# Consulted Peers:

Joel Rodriguez

# References:

    1. HashSet<T> Class - https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?view=net-8.0
    2. How to write .NET objects as JSON (serialize) - https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/how-to
    3. How to read JSON as .NET objects (deserialize) - https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/deserialization
