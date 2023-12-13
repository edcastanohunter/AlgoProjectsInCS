namespace _01_Trees;

public class BinaryNode<T>
{
    private const string NullString = " null";
    public T NodeValue { get; set; }
    public BinaryNode<T>? LeftChild, RightChild;

    public BinaryNode(T nodeValue)
    {
        NodeValue = nodeValue;
        LeftChild = null;
        RightChild = null;
    }

    public void AddLeft(T leftValue)
    {
        LeftChild = new BinaryNode<T>(leftValue);
    }

    public void AddRight(T rightValue)
    {
        RightChild = new BinaryNode<T>(rightValue);
    }
    
    // Return a string representation of the node and its children.
    public override string ToString()
    {
        string result = string.Format("{0}:", NodeValue);

        if (LeftChild == null)
            result += NullString;
        else
            result += " " + LeftChild.NodeValue;

        if (RightChild == null)
            result += NullString;
        else
            result += " " + RightChild.NodeValue;

        return result;
    }
}

public static class BinaryNodeExtensions
{
    public static void TraversePreOrder<T>(this BinaryNode<T> node)
    {
        Console.WriteLine(node.NodeValue);
        if (node.LeftChild != null)
        {
            TraversePreOrder(node.LeftChild);
        }

        if (node.RightChild != null)
        {
            TraversePreOrder(node.RightChild);
        }
    }

    public static void TraverseInOrder<T>(this BinaryNode<T> node)
    {
        if (node.LeftChild != null)
        {
            TraverseInOrder(node.LeftChild);
        }

        Console.WriteLine(node.NodeValue);
        if (node.RightChild != null)
        {
            TraverseInOrder(node.RightChild);
        }
    }

    public static void TraversePostOrder<T>(this BinaryNode<T> node)
    {
        if (node.LeftChild != null)
        {
            TraversePostOrder(node.LeftChild);
        }

        if (node.RightChild != null)
        {
            TraversePostOrder(node.RightChild);
        }

        Console.WriteLine(node.NodeValue);
    }

    public static void TraverseDepthFirst<T>(this BinaryNode<T> root)
    {
        // Create a queue to hold children for later processing.
        Queue<BinaryNode<T>> children = new Queue<BinaryNode<T>>();

        // Place the root node on the queue.
        children.Enqueue(root);

        while (children.Any())
        {
            // Get the next node in the queue.
            var node = children.Dequeue();

            // Add the node's children to the queue.
            if (node.LeftChild != null)
            {
                children.Enqueue(node.LeftChild);
            }

            if (node.RightChild != null)
            {
                children.Enqueue(node.RightChild);
            }

            //Console.WriteLine($"{node.NodeValue}: {leftValue} {rightValue}");
            Console.WriteLine(node.ToString());
        }
    }
}