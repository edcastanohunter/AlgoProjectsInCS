// See https://aka.ms/new-console-template for more information

using _01_Trees_Exhaustive;


Console.WriteLine("Binary Tree");
RunBinaryTree();

Console.WriteLine("Nary Tree");
RunNaryTree();

// Binary tree creation 
// CreateBinaryTree();

static void CreateBinaryTree()
{
    var rnd = new Random();
    var min = 1;
    var max = 100;
    var rootNode = new BinaryNode<int>(rnd.Next(min, max));

    for (int i = 0; i < 5; i++)
    {
        rootNode.AddNode(rnd.Next(min, max));
    }
    
    PrintBinaryTreeArr(rootNode.TraverseDepthFirst(), "Breadth First");

    var found = false;
    while (!found)
    {
        var value = rnd.Next(min, max);
        var node = rootNode.FindNode(value);

        
        if (node is not null)
            found = true;

        Console.WriteLine($"Trying to fing {value} - {found}");
    }

    Console.WriteLine(rootNode.ToString());
}


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
    ;
    // Console.WriteLine(rootNode.ToString());

    // var value = "Root";
    // Console.WriteLine(rootNode.FindValue(value)? $"Found {value}": $"Value {value} not found");
    // value = "E";
    // Console.WriteLine(rootNode.FindValue(value)? $"Found {value}": $"Value {value} not found");
    // value = "F";
    // Console.WriteLine(rootNode.FindValue(value)? $"Found {value}": $"Value {value} not found");
    // value = "Q";
    // Console.WriteLine(rootNode.FindValue(value)? $"Found {value}": $"Value {value} not found");
    //
    // value = "F";
    // Console.WriteLine(bNode.FindValue(value)? $"Found {value}": $"Value {value} not found");

    /*
     Preorder:      Root A C D B E F
    Inorder:       C A D Root B F E
    Postorder:     C D A F E B Root
    Breadth First: Root A B C D E F
     */
    PrintBinaryTreeArr(rootNode.TraversePreOrder(), "Preorder");
    PrintBinaryTreeArr(rootNode.TraverseInOrder(), "Inorder");
    PrintBinaryTreeArr(rootNode.TraversePostOrder(), "Postorder");
    PrintBinaryTreeArr(rootNode.TraverseDepthFirst(), "Breadth First");


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
    //Console.WriteLine(rootNaryNode.ToString());
    
    // var value = "Root";
    // Console.WriteLine(rootNaryNode.FindValue(value)? $"Found {value}": $"Value {value} not found");
    // value = "E";
    // Console.WriteLine(rootNaryNode.FindValue(value)? $"Found {value}": $"Value {value} not found");
    // value = "F";
    // Console.WriteLine(rootNaryNode.FindValue(value)? $"Found {value}": $"Value {value} not found");
    // value = "Q";
    // Console.WriteLine(rootNaryNode.FindValue(value)? $"Found {value}": $"Value {value} not found");
    //
    // value = "F";
    // Console.WriteLine(cNode.FindValue(value)? $"Found {value}": $"Value {value} not found");
    
    /*
     Preorder:      Root A D G E B C F H I
    Postorder:     G D E A B H I F C Root
    Breadth-First: Root A B C D E F G H I
     * 
     */
    
    PrintNaryTreeArr(rootNaryNode.TraversePreOrder(), "Preorder");
    PrintNaryTreeArr(rootNaryNode.TraversePostOrder(), "Postorder");
    PrintNaryTreeArr(rootNaryNode.TraverseDepthFirst(), "Breadth First");
}


static void PrintBinaryTreeArr<T>(IEnumerable<BinaryNode<T>> nodeArr, string operation)
{
    string result;
    var length = 20;
    result = (operation + ":").PadRight(length).Substring(0, length);;
    foreach ( var node in nodeArr)
    {
        result += string.Format("{0} ", node.NodeValue);
    }
    Console.WriteLine(result);
}

static void PrintNaryTreeArr(IEnumerable<NaryNode<string>> nodeArr, string operation)
{
    string result;
    var length = 20;
    result = (operation + ":").PadRight(length).Substring(0, length);;
    foreach ( var node in nodeArr)
    {
        result += string.Format("{0} ", node.NodeValue);
    }
    Console.WriteLine(result);
}