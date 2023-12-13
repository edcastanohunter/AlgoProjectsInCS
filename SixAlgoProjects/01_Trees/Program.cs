// See https://aka.ms/new-console-template for more information
using _01_Trees;

RunNaryTree();
//RunBinaryTree();

static void RunBinaryTree()
{
    var rootNode = new BinaryNode<string>("Root");
    rootNode.AddLeft("A");
    rootNode.AddRight("B");

    var aNode = rootNode.LeftChild;
    aNode?.AddLeft("C");
    aNode?.AddRight("D");

    var bNode = rootNode.RightChild;
    bNode?.AddRight("E");

    var eNode = bNode.RightChild;
    eNode?.AddLeft("F");

    //Console.WriteLine("------------ Breadth-First -------------");
    //rootNode.TraverseDepthFirst();
    Console.WriteLine(rootNode.ToString());
}

static void RunNaryTree()
{
    var rootNaryNode = new NaryNode<string>("Root");
    
    // Second level nodes
    rootNaryNode.AddChild("A");
    rootNaryNode.AddChild("B");
    rootNaryNode.AddChild("C");
    
    var aNode = rootNaryNode.Children[0];
    aNode.AddChild("D");
    aNode.AddChild("E");

    var dNode = aNode.Children[0];
    dNode.AddChild("G");

    var cNode = rootNaryNode.Children[2];
    cNode.AddChild("F");

    var fNode = cNode.Children.First();
    fNode.AddChild("H");
    fNode.AddChild("I");
    
    // Console.WriteLine("------------ Breadth-First -------------");
    // rootNaryNode.TraverseDepthFirst();
    Console.WriteLine(rootNaryNode.ToString());
}