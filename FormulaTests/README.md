```
Author:     Nandhini Ramanathan
Partner:    None
Start Date: 24-Aug-2024
Course:     CS 3505, University of Utah, School of Computing
GitHub ID:  NandhiniRam2005
Repo:       https://github.com/uofu-cs3500-20-fall2024/spreadsheet-NandhiniRam2005
Commit Date: 20-Sep-2024 9:20pm
Project:    FormulaSyntaxTests
Copyright:  CS 3500 and Nandhini Ramanathan - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:

I have added the FormulaGradingTests1 to this file to debug my code and have fixed them so all the autograder tests pass.
There were two tests failing, both needed the same fix by checking the last token in my formula and throwing a 
FormulaFormatException when the last token is an operator.

# Assignment Specific Topics
This assignment project is a MS unit tester class for the Formula class.
An EvaluationTest test suite has also been implemented to thoroughly test
the evaluate, ==, !=. Equals, and hashcode method in the Formula class.

# Consulted Peers:

Joel Rodriguez

# References:

    1. C# | Check if HashSet and the specified collection contain the same elements - https://www.geeksforgeeks.org/c-sharp-check-if-hashset-and-the-specified-collection-contain-the-same-elements/
    2. Assert.IsInstanceOfType Method - https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.testtools.unittesting.assert.isinstanceoftype?view=visualstudiosdk-2022
