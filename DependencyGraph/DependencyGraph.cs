// <copyright file="DependencyGraph.cs" company="UofU-CS3500">
//   Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta
// Version 1.3 - H. James de St. Germain Fall 2024
// (Clarified meaning of dependent and dependee.)
// (Clarified names in solution/project structure.)

/// <summary>
/// Author:    Nandhini Ramanathan, Joe Zachary, Daniel Kopta, and H. James de St. Germain
/// Partner:   None
/// Date:      September 13,2024
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
/// This file implements a DependencyGraph that manages dependencies between strings
/// in a directed graph.
/// </summary>

namespace CS3500.DependencyGraph;

/// <summary>
///   <para>
///     (s1,t1) is an ordered pair of strings, meaning t1 depends on s1.
///     (in other words: s1 must be evaluated before t1.)
///   </para>
///   <para>
///     A DependencyGraph can be modeled as a set of ordered pairs of strings.
///     Two ordered pairs (s1,t1) and (s2,t2) are considered equal if and only
///     if s1 equals s2 and t1 equals t2.
///   </para>
///   <para>
///     Given a DependencyGraph DG:
///   </para>
///   <list type="number">
///     <item>
///       If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
///       (The set of things that depend on s.)
///     </item>
///     <item>
///       If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
///       (The set of things that s depends on.)
///     </item>
///   </list>
///   <para>
///      For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}.
///   </para>
///   <code>
///     dependents("a") = {"b", "c"}
///     dependents("b") = {"d"}
///     dependents("c") = {}
///     dependents("d") = {"d"}
///     dependees("a")  = {}
///     dependees("b")  = {"a"}
///     dependees("c")  = {"a"}
///     dependees("d")  = {"b", "d"}
///   </code>
/// </summary>
public class DependencyGraph
{
    // A dictionary where the key is a node and the value is a HashSet of nodes that depend on it.
    private Dictionary<string, HashSet<string>> dependentsMap;

    // A dictionary where the key is a node and the value is a HashSet of nodes that the key depends on.
    private Dictionary<string, HashSet<string>> dependeesMap;

    // The number of dependencies (pairs) in the graph.
    private int size;

    /// <summary>
    ///   Initializes a new instance of the <see cref="DependencyGraph"/> class.
    ///   The initial DependencyGraph is empty.
    /// </summary>
    public DependencyGraph()
    {
        dependentsMap = new Dictionary<string, HashSet<string>>();
        dependeesMap = new Dictionary<string, HashSet<string>>();

        size = 0;
    }

    /// <summary>
    /// Gets the number of ordered pairs in the DependencyGraph.
    /// </summary>
    public int Size
    {
        get { return size; }
    }

    /// <summary>
    ///   Reports whether the given node has dependents (i.e., other nodes depend on it).
    /// </summary>
    /// <param name="nodeName"> The name of the node.</param>
    /// <returns> true if the node has dependents. </returns>
    public bool HasDependents(string nodeName)
    {
        // Check whether the nodeName is in the Map and check if its value (HashSet) is not empty.
        return dependentsMap.ContainsKey(nodeName) && dependentsMap[nodeName].Count > 0;
    }

    /// <summary>
    ///   Reports whether the given node has dependees (i.e., depends on one or more other nodes).
    /// </summary>
    /// <returns> true if the node has dependees.</returns>
    /// <param name="nodeName">The name of the node.</param>
    public bool HasDependees(string nodeName)
    {
        // Check whether the nodeName is in the Map and check if its value (HashSet) is not empty.
        return dependeesMap.ContainsKey(nodeName) && dependeesMap[nodeName].Count > 0;
    }

    /// <summary>
    ///   <para>
    ///     Returns the dependents of the node with the given name.
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependents of nodeName. </returns>
    public IEnumerable<string> GetDependents(string nodeName)
    {
        if (dependentsMap.ContainsKey(nodeName))
        {
            return dependentsMap[nodeName];
        }
        else
        {
            return new HashSet<string>();
        }
    }

    /// <summary>
    ///   <para>
    ///     Returns the dependees of the node with the given name.
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependees of nodeName. </returns>
    public IEnumerable<string> GetDependees(string nodeName)
    {
        if (dependeesMap.ContainsKey(nodeName))
        {
            return dependeesMap[nodeName];
        }
        else
        {
            return new HashSet<string>();
        }
    }

    /// <summary>
    /// <para>
    ///   Adds the ordered pair (dependee, dependent), if it doesn't already exist (otherwise nothing happens).
    /// </para>
    /// <para>
    ///   This can be thought of as: dependee must be evaluated before dependent.
    /// </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first. </param>
    /// <param name="dependent"> The name of the node that cannot be evaluated until after the other node has been. </param>
    public void AddDependency(string dependee, string dependent)
    {
        bool dependencyAdded = false;

        // Add dependent to dependentsMap
        if (!dependentsMap.ContainsKey(dependee))
        {
            dependentsMap[dependee] = new HashSet<string>();
        }

        if (dependentsMap[dependee].Add(dependent))
        {
            dependencyAdded = true;
        }

        // Add dependee to dependeesMap
        if (!dependeesMap.ContainsKey(dependent))
        {
            dependeesMap[dependent] = new HashSet<string>();
        }

        if (dependeesMap[dependent].Add(dependee))
        {
            dependencyAdded = true;
        }

        // Update size
        if (dependencyAdded)
        {
            size++;
        }
    }

    /// <summary>
    ///   <para>
    ///     Removes the ordered pair (dependee, dependent), if it exists (otherwise nothing happens).
    ///   </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first. </param>
    /// <param name="dependent"> The name of the node that cannot be evaluated until the other node has been. </param>
    public void RemoveDependency(string dependee, string dependent)
    {
        bool dependencyRemoved = false;

        // Remove dependent from dependentsMap
        if (dependentsMap.ContainsKey(dependee) && dependentsMap[dependee].Contains(dependent))
        {
            dependentsMap[dependee].Remove(dependent);
            dependencyRemoved = true;

            // If there are no more dependents, remove the dependee entry from the map
            if (dependentsMap[dependee].Count == 0)
            {
                dependentsMap.Remove(dependee);
            }
        }

        // Remove dependee from dependeesMap
        if (dependeesMap.ContainsKey(dependent) && dependeesMap[dependent].Contains(dependee))
        {
            dependeesMap[dependent].Remove(dependee);
            dependencyRemoved = true;

            // If there are no more dependees, remove the dependent entry from the map
            if (dependeesMap[dependent].Count == 0)
            {
                dependeesMap.Remove(dependent);
            }
        }

        // Update size
        if (dependencyRemoved)
        {
            size--;
        }
    }

    /// <summary>
    ///   Removes all existing ordered pairs of the form (nodeName, *).  Then, for each
    ///   t in newDependents, adds the ordered pair (nodeName, t).
    /// </summary>
    /// <param name="nodeName"> The name of the node who's dependents are being replaced. </param>
    /// <param name="newDependents"> The new dependents for nodeName. </param>
    public void ReplaceDependents(string nodeName, IEnumerable<string> newDependents)
    {
        // Remove all existing dependents of nodeName
        if (dependentsMap.ContainsKey(nodeName))
        {
            foreach (string oldDependent in dependentsMap[nodeName])
            {
                RemoveDependency(nodeName, oldDependent);
            }
        }

        // Add new dependents
        foreach (var dependent in newDependents)
        {
            AddDependency(nodeName, dependent);
        }
    }

    /// <summary>
    ///   <para>
    ///     Removes all existing ordered pairs of the form (*, nodeName).  Then, for each
    ///     t in newDependees, adds the ordered pair (t, nodeName).
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The name of the node who's dependees are being replaced. </param>
    /// <param name="newDependees"> The new dependees for nodeName. Could be empty.</param>
    public void ReplaceDependees(string nodeName, IEnumerable<string> newDependees)
    {
        // Remove all existing dependees of nodeName
        if (dependeesMap.ContainsKey(nodeName))
        {
            foreach (string oldDependee in dependeesMap[nodeName])
            {
                RemoveDependency(oldDependee, nodeName);
            }
        }

        // Add new dependees
        foreach (var dependee in newDependees)
        {
            AddDependency(dependee, nodeName);
        }
    }
}
