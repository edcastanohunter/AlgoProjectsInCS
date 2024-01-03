using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _01_Trees_NT_Drawing;

internal class NaryNode<T>
{
    internal T Value { get; set; }
    internal List<NaryNode<T>> Children;

    // Size and position values.
    private const double NODE_RADIUS = 10; // Radius of a node’s circle
    private const double X_SPACING = 20; // Horizontal distance between neighboring subtrees
    private const double Y_SPACING = 20; // Horizontal distance between parent and child subtree
    private const int Thickness = 2;
    
    internal Point Center { get; private set; }
    internal Rect SubtreeBounds { get; private set; }

    internal NaryNode(T value)
    {
        Value = value;
        Children = new List<NaryNode<T>>();
    }

    internal void AddChild(NaryNode<T> child)
    {
        Children.Add(child);
    }

    // Return an indented string representation of the node and its children.
    public override string ToString()
    {
        return ToString("");
    }

    // Recursively create a string representation of this node's subtree.
    // Display this value indented, followed its children indented one more level.
    // End in a newline.
    public string ToString(string spaces)
    {
        // Create a string named result that initially holds the
        // current node's value followed by a new line.
        string result = string.Format("{0}{1}:\n", spaces, Value);

        // Add the children representations
        // indented by one more level.
        foreach (NaryNode<T> child in Children)
            result += child.ToString(spaces + "  ");
        return result;
    }

    // Recursively search this node's subtree looking for the target value.
    // Return the node that contains the value or null.
    internal NaryNode<T> FindNode(T target)
    {
        // See if this node contains the value.
        if (Value.Equals(target)) return this;

        // Search the child subtrees.
        foreach (NaryNode<T> child in Children)
        {
            NaryNode<T> result = child.FindNode(target);
            if (result != null) return result;
        }

        // We did not find the value. Return null.
        return null;
    }

    internal List<NaryNode<T>> TraversePreorder()
    {
        List<NaryNode<T>> result = new List<NaryNode<T>>();

        // Add this node to the traversal.
        result.Add(this);

        // Add the children.
        foreach (NaryNode<T> child in Children)
        {
            result.AddRange(child.TraversePreorder());
        }

        return result;
    }

    internal List<NaryNode<T>> TraversePostorder()
    {
        List<NaryNode<T>> result = new List<NaryNode<T>>();

        // Add the children.
        foreach (NaryNode<T> child in Children)
        {
            result.AddRange(child.TraversePreorder());
        }

        // Add this node to the traversal.
        result.Add(this);
        return result;
    }

    internal List<NaryNode<T>> TraverseBreadthFirst()
    {
        List<NaryNode<T>> result = new List<NaryNode<T>>();
        Queue<NaryNode<T>> queue = new Queue<NaryNode<T>>();

        // Start with the top node in the queue.
        queue.Enqueue(this);
        while (queue.Count > 0)
        {
            // Remove the top node from the queue and
            // add it to the result list.
            NaryNode<T> node = queue.Dequeue();
            result.Add(node);

            // Add the node's children to the queue.
            foreach (NaryNode<T> child in node.Children)
                queue.Enqueue(child);
        }

        return result;
    }

    // Position the node's subtree.
    private void ArrangeSubtree(double xmin, double ymin)
    {
        double cx, cy;
        // Calculate cy, the Y coordinate for this node.
        // This doesn't depend on the children.


        // If the node has no children, just place it here and return.
        if (Children.Count == 0)
        {
            SubtreeBounds = new Rect(xmin, ymin, 2 * NODE_RADIUS, 2 * NODE_RADIUS);
            Center = new Point(
                (SubtreeBounds.Left + SubtreeBounds.Right) / 2,
                SubtreeBounds.Top + NODE_RADIUS);
            return;
        }

        // Set child_xmin and child_ymin to the
        // start position for child subtrees.
        double child_xmin = xmin;
        double child_ymin = ymin + 2 * NODE_RADIUS + Y_SPACING;
        
        // Set ymax equal to the largest Y position used.
        double ymax = ymin + 2 * NODE_RADIUS;

        var height = 0d;
        var isFirst = true;
        // Position the child subtrees.
        foreach (NaryNode<T> child in Children)
        {
            // Position this child subtree.
            child.ArrangeSubtree(child_xmin, child_ymin);
            
            // Update child_xmin to allow room for the subtree
            // and space between the subtrees.
            child_xmin = child.SubtreeBounds.Right;
            child_xmin += isFirst ? 0: X_SPACING;
            
            // Update the subtree bottom ymax.
            if (child.SubtreeBounds.Bottom > height)
                height = child.SubtreeBounds.Bottom;

            isFirst = false;
        }

        // Use xmin, ymin, xmax, and ymax to set our subtree bounds.
        SubtreeBounds = new Rect(xmin, ymin, child_xmin - xmin, height - ymin);

        // Center this node over the subtree bounds.
        Center = new Point((SubtreeBounds.Left + SubtreeBounds.Right) / 2,
            SubtreeBounds.Top + NODE_RADIUS);
        
    }

    // Draw the subtree's links.
    private void DrawSubtreeLinks(Canvas canvas)
    {
        // If we have exactly one child, just draw to it.
        if (Children.Count == 1)
        {
            Children[0].DrawSubtreeLinks(canvas);
            canvas.DrawLine(Center, Children[0].Center, Brushes.Navy, Thickness);
        }
        else if (Children.Count > 0)
        {
            // Else if we have more than one child,
            // draw vertical and horizontal branches.
            
            
            // Find the Y coordinate of the center
            // halfway to the children.
            var childNode = Children[0];
            var lastChild = Children.Last();
            var deltaY = Center.Y + (childNode.Center.Y - Center.Y) / 2;
            // Draw the vertical line to the center line.
            canvas.DrawLine(Center, Center with { Y = deltaY }, Brushes.Navy, Thickness);

            // Draw the horizontal center line over the children.
            canvas.DrawLine(childNode.Center with {Y = deltaY}, lastChild.Center with { Y = deltaY }, Brushes.Navy, Thickness);

            // Draw lines from the center line to the children.
            foreach (NaryNode<T> child in Children)
            {
                canvas.DrawLine(child.Center, child.Center with { Y = deltaY }, Brushes.Navy, Thickness);
            }

            // Recursively draw child subtree links.
            foreach (NaryNode<T> child in Children)
            {
                child.DrawSubtreeLinks(canvas);
            }
        }

        // Outline the subtree for debugging.
        canvas.DrawRectangle(SubtreeBounds, null, Brushes.Red, 1);
    }

    // Draw the subtree's nodes.
    private void DrawSubtreeNodes(Canvas canvas)
    {
        // Draw the node.
        var center = new Rect(Center.X - NODE_RADIUS, Center.Y - NODE_RADIUS, 2 * NODE_RADIUS,2 * NODE_RADIUS);
        canvas.DrawEllipse(center, Brushes.Aquamarine, Brushes.Green, Thickness);

        canvas.DrawLabel(center, Value,
            Brushes.Transparent,
            Brushes.Green,
            HorizontalAlignment.Center,
            VerticalAlignment.Top,
            14d,
            0d);
        
        // Draw the descendants' nodes.
        foreach (NaryNode<T> child in Children)
        {
            child.DrawSubtreeNodes(canvas);
        }
    }

    public void ArrangeAndDrawSubtree(Canvas canvas, double xmin, double ymin)
    {
        // Position the tree.
        ArrangeSubtree(xmin, ymin);

        // Draw the links.
        DrawSubtreeLinks(canvas);

        // Draw the nodes.
        DrawSubtreeNodes(canvas);
    }
}