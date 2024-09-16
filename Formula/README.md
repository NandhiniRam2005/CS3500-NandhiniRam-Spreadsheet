```
Author:     Nandhini Ramanathan
Partner:    None
Start Date: 30-Aug-2024
Course:     CS 3505, University of Utah, School of Computing
GitHub ID:  NandhiniRam2005
Repo:       https://github.com/uofu-cs3500-20-fall2024/spreadsheet-NandhiniRam2005
Commit Date: 20-Sep-2024 9:20pm
Project:    Formula
Copyright:  CS 3500 and Nandhini Ramanathan - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:

For the ToSTring method in this Formula class, I initially thought of using a StringBuilder to concatenate 
the list of valid tokens into a string without any spaces. When I looked more into the string class in C#, I 
found that there is a string.join method that exists. I chose to use the string.join method because the code
is simpler, concise, and has better readability. As I looked into it more, The performance of both the STringBuilder 
and string.join method have a similar O(N) complexity. It was also easier since I didn't have to create a new object 
and write up a for loop. The string.join method just takes in a string separator (in this case no spaces) and the list
of string elements which was only one line of code.

# Assignment Specific Topics
This assignment focuses on creating a Formula object to evaluate mathematical expressions 
while adhering to specified rules. The constructor parses, validates syntax, and throws the 
FormulaFormatException when necessary. This class also includes the GetVariables and ToString methods.
Extra methods have been added to mathematically evaluate a formula whose syntax has been validated.

# Consulted Peers:

Joel Rodriguez

# References:

    1. C# Tutorial - https://www.w3schools.com/cs/index.php
    2. Recommended XML tags for C# documentation comments - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags?redirectedfrom=MSDN
    3. String Class - https://learn.microsoft.com/en-us/dotnet/api/system.string?view=net-8.0
    4. C# | How to get the last occurrence of the element in the List that match the specified conditions - https://www.geeksforgeeks.org/c-sharp-how-to-get-the-last-occurrence-of-the-element-in-the-list-that-match-the-specified-conditions/
    5. String.GetHashCode Method - https://learn.microsoft.com/en-us/dotnet/api/system.string.gethashcode?view=net-8.0
    6. out (C# Reference) - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/out
    7. Stack Class - https://learn.microsoft.com/en-us/dotnet/api/system.collections.stack?view=net-8.0

