```
Author:     Nandhini Ramanathan
Partner:    None
Start Date: 21-Sep-2024
Course:     CS 3505, University of Utah, School of Computing
GitHub ID:  NandhiniRam2005
Repo:       https://github.com/uofu-cs3500-20-fall2024/spreadsheet-NandhiniRam2005
Commit Date: 27-Sep-2024 9:20pm
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
dependent cells.

# Consulted Peers:

Joel Rodriguez

# References:

    1. HashSet<T> Class - https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?view=net-8.0
