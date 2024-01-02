namespace _01_Trees_Exhaustive;

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
        return ToString("");
    }

    public string ToString(string spaces)
    {
        string result = $"{spaces}{NodeValue}:";
        spaces += " ";

        if (LeftChild is not null || RightChild is not null)
        {
            result += (LeftChild != null) ? "\n" + LeftChild.ToString(spaces) : $"\n{spaces}None";
            result += (RightChild != null) ? "\n" + RightChild.ToString(spaces) : $"\n{spaces}None";
        }

        return result;
    }
}

public static class BinaryNodeExtensions
{
    public static bool FindValue<T>(this BinaryNode<T> tree, string value)
    {
        if (tree is null)
        {
            return false;
        }
        
        if (tree.NodeValue.ToString() == value)
        {
            return true;
        }

        return tree.LeftChild.FindValue(value) || tree.RightChild.FindValue(value);
    }
    public static IEnumerable<BinaryNode<T>> TraversePreOrder<T>(this BinaryNode<T> node)
    {
        var list = new List<BinaryNode<T>>();
        list.Add(node);
        if (node.LeftChild != null)
        {
            list.AddRange(node.LeftChild.TraversePreOrder());
        }

        if (node.RightChild != null)
        {
            list.AddRange(node.RightChild.TraversePreOrder());
        }

        return list;
    }

    public static IEnumerable<BinaryNode<T>> TraverseInOrder<T>(this BinaryNode<T> node)
    {
        var list = new List<BinaryNode<T>>();
        
        if (node.LeftChild != null)
        {
            list.AddRange(node.LeftChild.TraverseInOrder());
        }
        list.Add(node);
        if (node.RightChild != null)
        {
            list.AddRange( node.RightChild.TraverseInOrder());
        }

        return list;
    }

    public static IEnumerable<BinaryNode<T>> TraversePostOrder<T>(this BinaryNode<T> node)
    {
        var list = new List<BinaryNode<T>>();
        if (node.LeftChild != null)
        {
            list.AddRange(node.LeftChild.TraversePostOrder());
        }

        if (node.RightChild != null)
        {
            list.AddRange(node.RightChild.TraversePostOrder());
        }

        list.Add(node);

        return list;
    }

    public static IEnumerable<BinaryNode<T>> TraverseDepthFirst<T>(this BinaryNode<T> root)
    {
        // Create a queue to hold children for later processing.
        Queue<BinaryNode<T>> children = new Queue<BinaryNode<T>>();

        // Place the root node on the queue.
        children.Enqueue(root);
        var list = new List<BinaryNode<T>>();
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
            list.Add(node);
        }

        return list;
    }

    /// <summary>
    /// Add a node to this node's sorted subtree.
    /// </summary>
    /// <param name="value"></param>
    public static bool AddNode<T>(this BinaryNode<T> node, T value)
    {
        // this need to be improve since int hash is not deterministic
        if (value.GetHashCode() < node.NodeValue.GetHashCode()) 
        {
            if (node.LeftChild is null)
            {
                node.LeftChild = new BinaryNode<T>(value);
                return true;
            }
            
            return node.LeftChild.AddNode(value);
            
        }
        else
        {
            if (node.RightChild is null)
            {
                node.RightChild = new BinaryNode<T>(value);
                return true;
            }
            
            return node.RightChild.AddNode(value);
            
        }
    }
    
    /// <summary>
    /// Find a node 
    /// </summary>
    /// <param name="node">the node</param>
    /// <param name="value"></param>
    /// <typeparam name="T">Node's value type</typeparam>
    /// <returns></returns>
    public static BinaryNode<T> FindNode<T>(this BinaryNode<T> node, T value)
    {
        // this need to be improve since int hash is not deterministic
        if (EqualityComparer<T>.Default.Equals(value , node.NodeValue)) 
        {
            return node;
        }
        
        
        if (value.GetHashCode() < node.NodeValue.GetHashCode()) 
        {
            if (node.LeftChild is null)
            {
                return null;
            }
            
            return node.LeftChild.FindNode(value);
            
        }
        else
        {
            if (node.RightChild is null)
            {
                return null;
            }
            return node.RightChild.FindNode(value);
            
        }
    }

    /// <summary>
    /// Find the LCA for the two nodes.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static BinaryNode<int> FindLcaSortedTress(this BinaryNode<int> node, int value1, int value2)
    {
        // See if both nodes belong down the same child branch.
        if (value1 < node.NodeValue && value2 < node.NodeValue)
        {
            return node.LeftChild.FindLcaSortedTress(value1, value2);
        }

        if (value1 > node.NodeValue && value2 > node.NodeValue)
        {
            return node.RightChild.FindLcaSortedTress(value1, value2);
        }

        // This is the LCA.
        return node;
    }
}