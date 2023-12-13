namespace _01_Trees;

public class NaryNode<T>
{
    private const string NullString = " null";
    public T NodeValue { get; set; }
    public List<NaryNode<T>>? Children;

    public NaryNode(T nodeValue)
    {
        NodeValue = nodeValue;
        Children = new List<NaryNode<T>>();
    }

    public void AddChild(T nodeValue)
    {
        Children?.Add(new NaryNode<T>(nodeValue));
    }
    
    public override string ToString()
    {
        string result = string.Format("{0}:", NodeValue);

        foreach (var node in Children)
        {
            result += " " + node.NodeValue;
        }

        return result;
    }
}

public static class NaryNodeExtensions
{
    public static void TraverseDepthFirst<T>(this NaryNode<T> root)
    {
        // Create a queue to hold children for later processing.
        Queue<NaryNode<T>> children = new Queue<NaryNode<T>>();

        // Place the root node on the queue.
        children.Enqueue(root);

        while (children.Any())
        {
            // Get the next node in the queue.
            var node = children.Dequeue();

            // Add the node's children to the queue.
            foreach (var child in node.Children)
            {
                children.Enqueue(child);
            }

            //Console.WriteLine($"{node.NodeValue}: {leftValue} {rightValue}");
            Console.WriteLine(node.ToString());
        }
    }
}