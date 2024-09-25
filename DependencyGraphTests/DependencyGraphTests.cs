// <copyright file="DependencyGraphTests.cs" company="UofU-CS3500">
//   Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

namespace CS3500.DevelopmentTests;

using CS3500;

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
/// File Contents:
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
        const int SIZE = 700;

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

    /// <summary>
    /// This tests makes sure that the HasDependents and HasDependees method identifies that a node can have both
    /// dependees and dependents.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void HasDependentsAndDependees_MultipleDependencies_IsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("a", "b");
        graph.AddDependency("a", "c");
        graph.AddDependency("b", "d");

        Assert.IsTrue(graph.HasDependents("a"));

        // b has dependees and dependents.
        Assert.IsTrue(graph.HasDependees("b"));
        Assert.IsTrue(graph.HasDependents("b"));

        Assert.IsTrue(graph.HasDependees("c"));
        Assert.IsTrue(graph.HasDependees("d"));
    }

    /// <summary>
    /// This tests makes sure that the HasDependents and HasDependees method identifies that a node that is added and
    /// then removed should not have any dependents or dependees anymore.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void HasDependentsAndDependees_AddAndRemoveDependency_IsFalse()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("a", "b");
        graph.RemoveDependency("a", "b");

        Assert.IsFalse(graph.HasDependents("a"));
        Assert.IsFalse(graph.HasDependees("b"));
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

    /// <summary>
    /// This tests makes sure that the GetDependents method has a size of one when a duplicate node is added.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void GetDependents_DuplicateDependencyAdded_CountEqualsOne()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "t");
        graph.AddDependency("n", "t");

        Assert.AreEqual(1, graph.GetDependents("n").Count());
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
    /// This tests makes sure that the GetDependees method has a size of one when a duplicate node is added.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void GetDependees_DuplicateDependencyAdded_CountEqualsOne()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "t");
        graph.AddDependency("n", "t");

        Assert.AreEqual(1, graph.GetDependees("t").Count());
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

    /// <summary>
    /// This tests makes sure that removing a node twice does not result in ana error and the size is zero.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void RemoveDependency_RemovingDependencyTwice_SizeIsZero()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "k");
        graph.RemoveDependency("n", "k");
        graph.RemoveDependency("n", "k");
        Assert.AreEqual(0, graph.Size);
    }

    /// <summary>
    /// This tests makes sure that removing multiple all dependees has the expected result.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void RemoveDependency_RemoveAllDependencies_HasDependecyIsFalse()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "r");
        graph.AddDependency("n", "k");

        graph.RemoveDependency("n", "r");
        graph.RemoveDependency("n", "k");

        Assert.IsFalse(graph.HasDependents("n"));
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

    /// <summary>
    /// This tests replaces dependents for multiple nodes and makes sure they return the expected result.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependents_MultipleNodes_SetEqualsIsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("a", "b");
        graph.AddDependency("c", "d");

        graph.ReplaceDependents("a", new[] { "x", "y" });
        graph.ReplaceDependents("c", new[] { "z" });

        HashSet<string> expectedA = new HashSet<string> { "x", "y" };
        HashSet<string> expectedC = new HashSet<string> { "z" };

        Assert.IsTrue(expectedA.SetEquals(graph.GetDependents("a")));
        Assert.IsTrue(expectedC.SetEquals(graph.GetDependents("c")));
    }

    /// <summary>
    /// This test replaces dependents and adds duplicate dependents, result should ahve no duplicates.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependents_WithDuplicates_SetEqualsIsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.ReplaceDependents("a", new[] { "b", "b", "c" });

        HashSet<string> expected = new HashSet<string> { "b", "c" };
        Assert.IsTrue(expected.SetEquals(graph.GetDependents("a")));
    }

    /// <summary>
    /// This tests replaces dependents and ensures old dependents are removed.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependents_RemovesOldDependents_SetEqualsIsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("a", "b");
        graph.AddDependency("a", "c");

        graph.ReplaceDependents("a", new[] { "x" });

        HashSet<string> expected = new HashSet<string> { "x" };
        Assert.IsTrue(expected.SetEquals(graph.GetDependents("a")));
    }

    /// <summary>
    /// This test replaces dependents with an empty array on a node with multiple existing dependents.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependents_WithEmptyArray_CountIsZero()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("a", "b");
        graph.AddDependency("a", "c");

        graph.ReplaceDependents("a", new string[] { });

        Assert.AreEqual(0, graph.GetDependents("a").Count());
    }

    /// <summary>
    /// This test replaces dependents on a node that is not present in the graph, so the node is added.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependents_OnNonExistingNode_SetEqualsIsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.ReplaceDependents("x", new[] { "y", "z" });

        HashSet<string> expected = new HashSet<string> { "y", "z" };
        Assert.IsTrue(expected.SetEquals(graph.GetDependents("x")));
    }

    /// <summary>
    /// This test makes sure that replacing dependents with a single dependent works correctly.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependents_WithSingleDependent_SetEqualsIsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("a", "b");

        graph.ReplaceDependents("a", new[] { "x" });

        HashSet<string> expected = new HashSet<string> { "x" };
        Assert.IsTrue(expected.SetEquals(graph.GetDependents("a")));
    }

    /// <summary>
    /// This test replaces dependents on multiple levels of dependencies and makes sure the result is expected.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependents_WithMultipleLevels_CorrectlyUpdatesDependencies()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("a", "b");
        graph.AddDependency("b", "c");

        graph.ReplaceDependents("a", new[] { "d" });

        HashSet<string> expected = new HashSet<string> { "d" };
        Assert.IsTrue(expected.SetEquals(graph.GetDependents("a")));
        Assert.AreEqual(1, graph.GetDependents("a").Count());
    }

    /// <summary>
    /// This test replaces dependents and ensures that dependents are correctly updated even when they are already present in other nodes.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependents_WithSharedDependents_SetEqualsIsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("a", "b");
        graph.AddDependency("c", "b");

        graph.ReplaceDependents("a", new[] { "x", "y" });

        HashSet<string> expectedA = new HashSet<string> { "x", "y" };
        HashSet<string> expectedC = new HashSet<string> { "b" };

        Assert.IsTrue(expectedA.SetEquals(graph.GetDependents("a")));
        Assert.IsTrue(expectedC.SetEquals(graph.GetDependents("c")));
    }

    /// <summary>
    /// This test replaces dependents when a dependent already exists as a dependee.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependents_WithExistingDependee_SetEqualsIsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("a", "b");
        graph.ReplaceDependents("a", new[] { "b" });

        HashSet<string> expected = new HashSet<string> { "b" };
        Assert.IsTrue(expected.SetEquals(graph.GetDependents("a")));
    }

    // --- Tests for the ReplaceDependees method ---

    /// <summary>
    /// This test replaces all the dependees of a node and ensures the method has updated its dependees.
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
    /// This test replaces all the dependees of a node that previously had no dependees and ensures the method has updated its dependees.
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
    /// This test replaces dependees for a node that previously had dependees with an empty list and ensures the method has updated its dependees.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependees_EmptyDependeesList_SetEqualsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("n", "k");
        graph.ReplaceDependees("k", new string[] { });

        HashSet<string> expectedDependees = new HashSet<string>();
        HashSet<string> actualDependees = new HashSet<string>(graph.GetDependees("k"));

        Assert.IsTrue(expectedDependees.SetEquals(actualDependees));
    }

    /// <summary>
    /// This test replaces dependees for multiple nodes and ensures they return the expected result.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependees_MultipleNodes_SetEqualsIsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("b", "a");
        graph.AddDependency("d", "c");

        graph.ReplaceDependees("a", new[] { "x", "y" });
        graph.ReplaceDependees("c", new[] { "z" });

        HashSet<string> expectedA = new HashSet<string> { "x", "y" };
        HashSet<string> expectedC = new HashSet<string> { "z" };

        Assert.IsTrue(expectedA.SetEquals(graph.GetDependees("a")));
        Assert.IsTrue(expectedC.SetEquals(graph.GetDependees("c")));
    }

    /// <summary>
    /// This test replaces dependees and adds duplicate dependees; the result should have no duplicates.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependees_WithDuplicates_SetEqualsIsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.ReplaceDependees("a", new[] { "b", "b", "c" });

        HashSet<string> expected = new HashSet<string> { "b", "c" };
        Assert.IsTrue(expected.SetEquals(graph.GetDependees("a")));
    }

    /// <summary>
    /// This test replaces dependees and ensures old dependees are removed.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependees_RemovesOldDependees_SetEqualsIsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("b", "a");
        graph.AddDependency("c", "a");

        graph.ReplaceDependees("a", new[] { "x" });

        HashSet<string> expected = new HashSet<string> { "x" };
        Assert.IsTrue(expected.SetEquals(graph.GetDependees("a")));
    }

    /// <summary>
    /// This test replaces dependees with an empty array on a node with multiple existing dependees.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependees_WithEmptyArray_CountIsZero()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("b", "a");
        graph.AddDependency("c", "a");

        graph.ReplaceDependees("a", new string[] { });

        Assert.AreEqual(0, graph.GetDependees("a").Count());
    }

    /// <summary>
    /// This test replaces dependees on a node that is not present in the graph, so the node is added.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependees_OnNonExistingNode_SetEqualsIsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.ReplaceDependees("x", new[] { "y", "z" });

        HashSet<string> expected = new HashSet<string> { "y", "z" };
        Assert.IsTrue(expected.SetEquals(graph.GetDependees("x")));
    }

    /// <summary>
    /// This test makes sure that replacing dependees with a single dependee works correctly.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependees_WithSingleDependee_SetEqualsIsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("b", "a");

        graph.ReplaceDependees("a", new[] { "x" });

        HashSet<string> expected = new HashSet<string> { "x" };
        Assert.IsTrue(expected.SetEquals(graph.GetDependees("a")));
    }

    /// <summary>
    /// This test replaces dependees on multiple levels of dependencies and ensures the result is expected.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependees_WithMultipleLevels_CorrectlyUpdatesDependees()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("b", "a");
        graph.AddDependency("c", "b");

        graph.ReplaceDependees("a", new[] { "d" });

        HashSet<string> expected = new HashSet<string> { "d" };
        Assert.IsTrue(expected.SetEquals(graph.GetDependees("a")));
        Assert.AreEqual(1, graph.GetDependees("a").Count());
    }

    /// <summary>
    /// This test replaces dependees and ensures that dependees are correctly updated even when they are already present in other nodes.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependees_WithSharedDependees_SetEqualsIsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("b", "a");
        graph.AddDependency("b", "c");

        graph.ReplaceDependees("a", new[] { "x", "y" });

        HashSet<string> expectedA = new HashSet<string> { "x", "y" };
        HashSet<string> expectedC = new HashSet<string> { "b" };

        Assert.IsTrue(expectedA.SetEquals(graph.GetDependees("a")));
        Assert.IsTrue(expectedC.SetEquals(graph.GetDependees("c")));
    }

    /// <summary>
    /// This test replaces dependees when a dependee already exists as a dependent.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void ReplaceDependees_WithExistingDependent_SetEqualsIsTrue()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("b", "a");
        graph.ReplaceDependees("a", new[] { "b" });

        HashSet<string> expected = new HashSet<string> { "b" };
        Assert.IsTrue(expected.SetEquals(graph.GetDependees("a")));
    }
}