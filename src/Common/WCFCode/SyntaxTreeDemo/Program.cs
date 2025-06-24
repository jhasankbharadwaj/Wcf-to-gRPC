using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;

public class SerializableNode
{
    public string Kind { get; set; }
    public string Text { get; set; }
    public List<SerializableNode> Children { get; set; } = new List<SerializableNode>();

    // For convenience in the Python script, we'll indicate if it's a leaf.
    public bool IsTerminal => Children.Count == 0;
}

public class Program
{
    public static void Main(string[] args)
    {
        string codeToAnalyze = File.ReadAllText("/workspaces/Wcf-to-gRPC/src/Common/WCFCode/c#class/Person.cs");
        SyntaxTree tree = CSharpSyntaxTree.ParseText(codeToAnalyze);
        var root = tree.GetRoot();

        // Build our custom serializable tree
        SerializableNode serializableTree = BuildSerializableTree(root);

        // Serialize to JSON
        string json = JsonConvert.SerializeObject(serializableTree, Formatting.Indented);
        File.WriteAllText("/workspaces/Wcf-to-gRPC/src/Encoder/roslyn_ast.json", json);

        Console.WriteLine("AST has been generated and saved to roslyn_ast.json");
    }

    private static SerializableNode BuildSerializableTree(SyntaxNodeOrToken nodeOrToken)
    {
        var node = new SerializableNode
        {
            Kind = nodeOrToken.Kind().ToString(),
            // We only care about the text of tokens (leaves), not the full text of non-terminal nodes.
            Text = nodeOrToken.IsToken ? nodeOrToken.AsToken().Text : "" 
        };

        foreach (var child in nodeOrToken.ChildNodesAndTokens())
        {
            node.Children.Add(BuildSerializableTree(child));
        }

        // Clean up: For non-terminals, we don't need the Text.
        // For terminals (tokens), Text is useful but Kind can be more specific.
        if (!node.IsTerminal)
        {
            node.Text = null; // We only want text at the leaves.
        }
        else
        {
             // Normalize tokens for better processing later.
             node.Text = node.Text.Trim().Replace("|", "<s>").Replace(" ", "<sp>"); // Sanitize
        }

        return node;
    }
}



