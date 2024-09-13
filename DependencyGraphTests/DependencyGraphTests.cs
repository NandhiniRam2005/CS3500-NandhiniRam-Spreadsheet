// <copyright file="DependencyGraphTests.cs" company="UofU-CS3500">
//   Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

/// <summary>
/// Author:    Nandhini Ramanathan
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
///    This file contains MS unit tests for the DependencyGraph class.
/// </summary>

namespace CS3500.DevelopmentTests;

using CS3500.DependencyGraph;

/// <summary>
/// This is a test class for DependencyGraphTest and is intended
/// to contain all DependencyGraphTest Unit Tests.
/// </summary>
[TestClass]
public class DependencyGraphExampleStressTests
{
    // --- Stress Tests ---

    /// <summary>
    /// This stress test is from the assignment starter code.
    /// This test performs a series of operations using all the methods in DependencyGraph
    /// to make sure that combining these tasks runs quickly and correctly. This test makes a
    /// graph of 200 nodes that depend on all teh nodes after it. Then, it removes some dependencies 
    /// in a pattern and adds more in another pattern. It removes some more dependencies and ensures
    /// getting these dependents and dependees returns the expected result.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void StressTest()
    {
        DependencyGraph dg = new();
        const int SIZE = 200;

        // Initialize an array of 200 strings by incrementing the char index starting from a to represent letters.
        string[] letters = new string[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            letters[i] = string.Empty + ((char)('a' + i));
        }

        // HashSets are initialized to represent the dependents and dependees for each letter.
        HashSet<string>[] dependents = new HashSet<string>[SIZE];
        HashSet<string>[] dependees = new HashSet<string>[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            dependents[i] = [];
            dependees[i] = [];
        }

        // Dependencies are added in a loop.
        // Each letter depends on all the letters that come after it.
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j++)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }

        // For each letter, remove the dependency on every fourth letter after it.
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 4; j < SIZE; j += 4)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }

        // For each letter, add dependencies back to every second letter.
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j += 2)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }

        // For every second letter, remove the dependency on every third letter after it.
        for (int i = 0; i < SIZE; i += 2)
        {
            for (int j = i + 3; j < SIZE; j += 3)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }

        // Make sure everything is right by checking that the actual dependents and dependees match the expected HashSets.
        for (int i = 0; i < SIZE; i++)
        {
            Assert.IsTrue(dependents[i].SetEquals(new
            HashSet<string>(dg.GetDependents(letters[i]))));
            Assert.IsTrue(dependees[i].SetEquals(new
            HashSet<string>(dg.GetDependees(letters[i]))));
        }
    }

    /// <summary>
    /// This attempted stress test is written by me.
    /// This test performs a series of operations using all the methods in DependencyGraph
    /// to make sure that combining these tasks runs quickly and correctly. This test makes a
    /// graph of 400 nodes that depend on every other node after it. Then, it removes some dependencies
    /// in a pattern and adds more in another pattern, then ensures getting these dependents and dependees
    /// returns the expected result.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Test_OverallScenarioStressTest_Outcome()
    {
        DependencyGraph dg = new();
        const int SIZE = 200;

        // Initialize an array of 400 strings called node and a number(index) attached to it.
        string[] nodes = new string[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            nodes[i] = "node" + i;
        }

        // HashSets are initialized to represent the dependents and dependees for each node.
        HashSet<string>[] dependents = new HashSet<string>[SIZE];
        HashSet<string>[] dependees = new HashSet<string>[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            dependents[i] = [];
            dependees[i] = [];
        }

        // // Add dependencies between every other node and all alternate nodes after it.
        for (int i = 0; i < SIZE; i+=2)
        {
            for (int j = i + 1; j < SIZE; j+=2)
            {
                dg.AddDependency(nodes[i], nodes[j]);
                dependents[i].Add(nodes[j]);
                dependees[j].Add(nodes[i]);
            }
        }

        // For each node, every third node removes the dependency to the next third node after it.
        for (int i = 0; i < SIZE; i += 3)
        {
            for (int j = i + 3; j < SIZE; j += 3)
            {
                dg.RemoveDependency(nodes[i], nodes[j]);
                dependents[i].Remove(nodes[j]);
                dependees[j].Remove(nodes[i]);
            }
        }

        // For each node, a random number of dependencies (between 1 and 10) are added to random nodes.
        Random random = new();
        for (int i = 0; i < SIZE; i++)
        {
            // Add 1 to 5 random dependencies for each node.
            int numberOfDependencies = random.Next(1, 10);
            for (int j = 0; j < numberOfDependencies; j++)
            {
                if (i + 1 < SIZE)
                {
                    // Pick random node after the current node.
                    int randomNode = random.Next(i + 1, SIZE);
                    dg.AddDependency(nodes[i], nodes[randomNode]);
                    dependents[i].Add(nodes[randomNode]);
                    dependees[randomNode].Add(nodes[i]);
                }
            }
        }

        // Make sure everything is right by checking that the actual dependents and dependees match the expected HashSets.
        for (int i = 0; i < SIZE; i++)
        {
            Assert.IsTrue(dependents[i].SetEquals(new
            HashSet<string>(dg.GetDependents(nodes[i]))));
            Assert.IsTrue(dependees[i].SetEquals(new
            HashSet<string>(dg.GetDependees(nodes[i]))));
        }
    }

    // --- Tests for the DependencyGraph constructor ---

    /// <summary>
    /// This tests makes sure that creating a new DependencyGraph is initialized with a size of 0.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraphConstructor_InitializeEmptyDependencyGraph_SizeIsZero()
    {
        DependencyGraph graph = new DependencyGraph();
        Assert.AreEqual(0, graph.Size);
    }

    /// <summary>
    /// This tests makes sure that creating a new empty DependencyGraph is initialized with no dependencies.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraphConstructor_InitializeNoDependencies_IsFalse()
    {
        DependencyGraph graph = new DependencyGraph();

        Assert.IsFalse(graph.HasDependents("n"));
        Assert.IsFalse(graph.HasDependees("k"));
    }

    /// <summary>
    /// This tests makes sure that creating a new empty DependencyGraph returns 0 when attempting to get dependencies.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraphConstructor_ZeroGetDependencies_AreEqual()
    {
        DependencyGraph graph = new DependencyGraph();

        Assert.AreEqual(0, graph.GetDependents("n").Count());
        Assert.AreEqual(0, graph.GetDependees("k").Count());
    }

    // --- Tests for the Size method ---

    /// <summary>
    /// This tests makes sure that the size method returns one when one dependency is added to the graph.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Size_OneDependencyAdded_SizeIsOne()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "k");
        Assert.AreEqual(1, graph.Size);
    }

    /// <summary>
    /// This tests makes sure that the size method returns zero when a dependency is added then removed.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Size_AddAndRemoveDependency_SizeIsZero()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "k");
        graph.RemoveDependency("n", "k");
        Assert.AreEqual(0, graph.Size);
    }

    // --- Tests for the HasDependents method ---

    /// <summary>
    /// This tests makes sure that the HasDependents method identifies that a node has a dependent.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void HasDependents_NodeWithDependents_IsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "k");
        Assert.IsTrue(graph.HasDependents("n"));
    }

    /// <summary>
    /// This tests makes sure that the HasDependents method identifies that a node does not have a dependent.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void HasDependents_NodeWithoutDependents_IsFalse()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "k");
        Assert.IsFalse(graph.HasDependents("k"));
    }

    // --- Tests for the HasDependees method ---

    /// <summary>
    /// This tests makes sure that the HasDependees method identifies that a node has a dependee.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void HasDependees_NodeWithDependees_IsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "k");
        Assert.IsTrue(graph.HasDependees("k"));
    }

    /// <summary>
    /// This tests makes sure that the HasDependees method identifies that a node does not have a dependee.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void HasDependees_NodeWithoutDependees_IsFalse()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "k");
        Assert.IsFalse(graph.HasDependees("n"));
    }

    // --- Tests for the GetDependents method ---

    /// <summary>
    /// This tests makes sure that the GetDependents method identifies the correct nodes that are dependents.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void GetDependents_SimpleGraphDependency_SetEqualsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "t");
        graph.AddDependency("k", "t");
        graph.AddDependency("n", "s");

        var actual = graph.GetDependents("n");
        var expected = new HashSet<string> { "t", "s" };

        Assert.IsTrue(expected.SetEquals(actual));
    }

    /// <summary>
    /// This tests makes sure that the GetDependents method returns an empty IEnumerable of type string when node has no dependents.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void GetDependents_NodeHasNoDependents_CountEqualsZero()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "t");
        graph.AddDependency("k", "t");

        Assert.AreEqual(0, graph.GetDependents("t").Count());
    }

    // --- Tests for the GetDependees method ---

    /// <summary>
    /// This tests makes sure that the GetDependees method identifies the correct nodes that are dependees.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void GetDependees_SimpleGraphDependency_SetEqualsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "t");
        graph.AddDependency("k", "t");

        // Change IEnumerable<string> return type to a HashSet so that it can be compared and tested.
        var actual = graph.GetDependees("t");
        var expected = new HashSet<string> { "n", "k" };

        Assert.IsTrue(expected.SetEquals(actual));
    }

    /// <summary>
    /// This tests makes sure that the GetDependees method returns an empty IEnumerable of type string when node has no dependees.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void GetDependees_NodeHasNoDependees_CountEqualsZero()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "t");
        graph.AddDependency("k", "t");

        Assert.AreEqual(0, graph.GetDependees("n").Count());
    }

    // --- Tests for the AddDependency method ---

    /// <summary>
    /// This tests makes sure that the size method returns one when a duplicate dependency is added to the graph.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Size_DuplicateDependencyAdded_SizeIsOne()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "k");
        graph.AddDependency("n", "k");
        Assert.AreEqual(1, graph.Size);
    }

    /// <summary>
    /// This tests makes sure that adding dependencies increases the size of the graph.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void AddDependency_AddMultipleDependencies()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "k");
        Assert.AreEqual(1, graph.Size);
        graph.AddDependency("t", "k");
        Assert.AreEqual(2, graph.Size);
        graph.AddDependency("j", "k");
        Assert.AreEqual(3, graph.Size);
    }

    // --- Tests for the RemoveDependency method ---

    /// <summary>
    /// This tests makes sure that removing dependencies decreases the size.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void RemoveDependency_RemoveDependencies_SizeDecreases()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "k");
        graph.AddDependency("n", "t");
        graph.AddDependency("t", "j");
        graph.AddDependency("n", "j");

        graph.RemoveDependency("n", "t");
        graph.RemoveDependency("t", "j");

        Assert.AreEqual(2, graph.Size);
    }

    /// <summary>
    /// This tests makes sure that removing a non existent dependency does not affect the size.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void RemoveDependency_RemovingNonExistentDependency_SizeIsZero()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.RemoveDependency("n", "k");
        Assert.AreEqual(0, graph.Size);
    }

    // --- Tests for the ReplaceDependents method ---

    /// <summary>
    /// This test replaces all the dependents of a node and makes sure the method has updated its dependents.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependents_ReplacesExistingDependents_SetEqualsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "k");
        graph.ReplaceDependents("n", new[] { "t", "s" });

        HashSet<string> expected = new HashSet<string> { "t", "s" };
        HashSet<string> actual = new HashSet<string>(graph.GetDependents("n"));

        Assert.IsTrue(expected.SetEquals(actual));
    }

    /// <summary>
    /// This test replaces all the dependents of a node that previously had no dependents and makes sure the method has updated its dependents.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependents_NodeWithNoPreviousDependents_SetEqualsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.ReplaceDependents("n", new[] { "j", "f" });

        HashSet<string> expected = new HashSet<string> { "j", "f" };
        HashSet<string> actual = new HashSet<string>(graph.GetDependents("n"));

        Assert.IsTrue(expected.SetEquals(actual));
    }

    /// <summary>
    /// This takes node that previously had dependents and replaces it with no dependents and makes sure the method has updated its dependents.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependents_EmptyDependentsList_SetEqualsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "k");
        graph.ReplaceDependents("n", new string[] { });

        HashSet<string> expectedDependents = new HashSet<string>();
        HashSet<string> actualDependents = new HashSet<string>(graph.GetDependents("n"));

        Assert.IsTrue(expectedDependents.SetEquals(actualDependents));
    }

    // --- Tests for the ReplaceDependees method ---

    /// <summary>
    /// This test replaces all the dependees of a node and makes sure the method has updated its dependees.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependees_ReplacesExistingDependees_SetEqualsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "k");
        graph.ReplaceDependees("k", new[] { "j" });

        HashSet<string> expected = new HashSet<string> { "j" };
        HashSet<string> actual = new HashSet<string>(graph.GetDependees("k"));

        Assert.IsTrue(expected.SetEquals(actual));
    }

    /// <summary>
    /// This test replaces all the dependees of a node that previously had no dependees and makes sure the method has updated its dependees.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependees_NodeWithNoPreviousDependees_SetEqualsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.ReplaceDependees("n", new[] { "j", "f" });

        HashSet<string> expected = new HashSet<string> { "j", "f" };
        HashSet<string> actual = new HashSet<string>(graph.GetDependees("n"));

        Assert.IsTrue(expected.SetEquals(actual));
    }

    /// <summary>
    /// This takes node that previously had dependees and replaces it with no dependees and makes sure the method has updated its dependees.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependees_EmptyDependeesList_SetEqualsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "k");
        graph.ReplaceDependees("k", new string[] { });

        HashSet<string> expectedDependents = new HashSet<string>();
        HashSet<string> actualDependents = new HashSet<string>(graph.GetDependees("n"));

        Assert.IsTrue(expectedDependents.SetEquals(actualDependents));
    }
}