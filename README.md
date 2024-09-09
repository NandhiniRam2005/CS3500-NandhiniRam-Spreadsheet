```
Author:     Nandhini Ramanathan
Partner:    None
Start Date: 30-Aug-2024
Course:     CS 3505, University of Utah, School of Computing
GitHub ID:  NandhiniRam2005
Repo:       https://github.com/uofu-cs3500-20-fall2024/spreadsheet-NandhiniRam2005
Commit Date: 13-Sep-2024 9:20pm
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

Future extensions include using this dependency graph to evaluate and compute formulas across the spreadsheet 
and implementing a GUI for a visual element to the Spreadsheet.

# Time Expenditures:

    1. Assignment 1: Test Driven Development      Predicted Hours:   4        Actual Hours:   5
    2. Assignment 2: Formula Class                Predicted Hours:   7        Actual Hours:   9

                                                                              Hours spent -
                                                                                Effectively:      5  
                                                                                Debugging:        2
                                                                                Learning tools:   2

    2. Assignment 3: Dependency Graph Class       Predicted Hours:   9        Actual Hours:   

                                                                              Hours spent -
                                                                                Effectively:       
                                                                                Debugging:        
                                                                                Learning tools:   
                                                             
                                                             
