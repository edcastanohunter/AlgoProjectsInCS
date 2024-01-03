﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace org_chart2
{
    internal class NaryNode<T>
    {
        internal T Value { get; set; }
        internal List<NaryNode<T>> Children;

        // Size and position values.
        private const double BOX_HALF_WIDTH = 80 / 2;
        private const double BOX_HALF_HEIGHT = 40 / 2;
        private const double X_SPACING = 20;    // Horizontal distance between neighboring subtrees
        private const double Y_SPACING = 20;    // Horizontal distance between parent and child subtree
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
            double cx, cy, child_xmin, child_ymin, xmax, ymax;

            // Calculate cy, the Y coordinate for this node.
            // This doesn't depend on the children.
            cy = ymin + BOX_HALF_HEIGHT;

            // If the node has no children, just place it here and return.
            if (IsLeaf())
            {
                cx = xmin + BOX_HALF_WIDTH;
                Center = new Point(cx, cy);
                SubtreeBounds = new Rect(xmin, ymin, 2 * BOX_HALF_WIDTH, 2 * BOX_HALF_HEIGHT);
                return;
            }

            // See if this is a twig.
            if (IsTwig())
            {
                // Position this node.
                cx = xmin + BOX_HALF_WIDTH;
                cy = ymin + BOX_HALF_HEIGHT;
                Center = new Point(cx, cy);

                // Put the child leaves below this node.
                // Set child_xmin and child_ymin to the
                // start position for child subtrees.
                child_xmin = xmin + X_SPACING;
                child_ymin = ymin + 2 * BOX_HALF_HEIGHT;

                foreach (NaryNode<T> child in Children)
                {
                    // Add vertical space before this child.
                    child_ymin += Y_SPACING;

                    // Position this child.
                    child.ArrangeSubtree(child_xmin, child_ymin);

                    // Add the child's height.
                    child_ymin += 2 * BOX_HALF_HEIGHT;
                }

                // Set this node's subtree bounds.
                xmax = child_xmin + 2 * BOX_HALF_WIDTH;
                ymax = child_ymin;
                SubtreeBounds = new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
                return;
            }


            // This is neither a leaf nor a twig.
            // Set child_xmin and child_ymin to the
            // start position for child subtrees.
            child_xmin = xmin;
            child_ymin = ymin + 2 * BOX_HALF_HEIGHT + Y_SPACING;

            // Set ymax equal to the largest Y position used.
            ymax = ymin + 2 * BOX_HALF_HEIGHT;

            // Position the child subtrees.
            foreach (NaryNode<T> child in Children)
            {
                // Position this child subtree.
                child.ArrangeSubtree(child_xmin, child_ymin);

                // Update child_xmin to allow room for the subtree
                // and space between the subtrees.
                child_xmin = child.SubtreeBounds.Right + X_SPACING;

                // Update the subtree bottom ymax.
                if (ymax < child.SubtreeBounds.Bottom)
                    ymax = child.SubtreeBounds.Bottom;
            }

            // Set xmax equal to child_xmin minus the horizontal
            // spacing we added after the last subtree.
            xmax = child_xmin - X_SPACING;

            // Use xmin, ymin, xmax, and ymax to set our subtree bounds.
            SubtreeBounds = new Rect(xmin, ymin, xmax - xmin, ymax - ymin);

            // Center this node over the subtree bounds.
            cx = (SubtreeBounds.Left + SubtreeBounds.Right) / 2;
            cy = ymin + BOX_HALF_HEIGHT;
            Center = new Point(cx, cy);
        }

        // Draw the subtree's links.
        private void DrawSubtreeLinks(Canvas canvas)
        {
            // See if we are a twig.
            if (IsTwig())
            {
                // Draw the vertical branch.
                ...

                // Draw the horizontal branches.
                ...
                foreach (NaryNode<T> child in Children)
                {
                    ...
                }
            }
            else
            {
                if (Children.Count == 1)
                {
                    // We have exactly one child. Just draw to it.
                    NaryNode<T> child = Children[0];
                    canvas.DrawLine(Center, child.Center, Brushes.Green, 1);
                }
                else if (Children.Count > 0)
                {
                    // We have more than one child. Draw vertical and horizontal branches.
                    // Find the Y coordinate of the center halfway to the children.
                    double ymid = (Center.Y + Children[0].Center.Y) / 2;

                    // Draw the vertical line to the center line.
                    canvas.DrawLine(Center, new Point(Center.X, ymid), Brushes.Green, 1);

                    // Draw the horizontal center line over the children.
                    int last_child = Children.Count - 1;
                    canvas.DrawLine(
                        new Point(Children[0].Center.X, ymid),
                        new Point(Children[last_child].Center.X, ymid),
                        Brushes.Green, 1);

                    // Draw lines from the center line to the children.
                    foreach (NaryNode<T> child in Children)
                    {
                        canvas.DrawLine(
                            new Point(child.Center.X, ymid),
                            child.Center,
                            Brushes.Green, 1);
                    }
                }

                // Recursively draw child subtree links.  
                foreach (NaryNode<T> child in Children)
                {
                    child.DrawSubtreeLinks(canvas);
                }
            }

            // Outline the subtree for debugging.
            //canvas.DrawRectangle(SubtreeBounds, null, Brushes.Red, 1);
        }

        // Draw the subtree's nodes.
        private void DrawSubtreeNodes(Canvas canvas)
        {
            // Draw the node.
            double x0 = Center.X - BOX_HALF_WIDTH;
            double y0 = Center.Y - BOX_HALF_HEIGHT;
            double x1 = Center.X + BOX_HALF_WIDTH;
            double y1 = Center.Y + BOX_HALF_HEIGHT;
            Rect rect = new Rect(x0, y0, x1 - x0, y1 - y0);

            // Fill with pink if this is a non-leaf node.
            Brush fill = Brushes.White;
            if (!IsLeaf()) fill = Brushes.Pink;

            canvas.DrawRectangle(rect, fill, Brushes.Black, 1);
            Label label = canvas.DrawLabel(rect, Value, null, Brushes.Red,
                HorizontalAlignment.Center,
                VerticalAlignment.Center,
                12, 0);

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

        // Return true if this node is a leaf.
        internal bool IsLeaf()
        {
            return (Children.Count == 0);
        }

        // Return true if this node's children are all leaves.
        internal bool IsTwig()
        {
            // If we are a leaf, then we are not a twig.
            if (IsLeaf()) return false;

            // See if all children are leaves.
            foreach (NaryNode<T> child in Children)
            {
                if (!child.IsLeaf()) return false;
            }

            // No children are non-leaves.
            return true;
        }
    }
}
