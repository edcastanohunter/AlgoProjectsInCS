using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _01_Trees_BT_Drawing;

internal class BinaryNode<T>
    {
        internal T Value { get; set; }
        internal BinaryNode<T> LeftChild, RightChild;

        // New constants and properties go here...
        private const double NODE_RADIUS = 10;  // Radius of a node’s circle
        private const double X_SPACING = 20;    // Horizontal distance between neighboring subtrees
        private const double Y_SPACING = 20;    // Horizontal distance between parent and child subtree
        private const int Thickness = 2;
        internal Point Center { get; private set; }
        internal Rect SubtreeBounds { get; private set; }

        internal BinaryNode(T value)
        {
            Value = value;
            LeftChild = null;
            RightChild = null;
        }
        
        internal void AddLeft(BinaryNode<T> child)
        {
            LeftChild = child;
        }

        internal void AddRight(BinaryNode<T> child)
        {
            RightChild = child;
        }

        internal void SetSubtreeBounds(Rect subtreeBounds)
        {
            SubtreeBounds = subtreeBounds;
        }
        // Return an indented string representation of the node and its children.
        public override string ToString()
        {
            return ToString("");
        }

        // Recursively create a string representation of this node's subtree.
        // Display this value indented, followed by the left and right
        // values indented one more level.
        // End in a newline.
        public string ToString(string spaces)
        {
            // Create a string named result that initially holds the
            // current node's value followed by a new line.
            string result = string.Format("{0}{1}:\n", spaces, Value);

            // See if the node has any children.
            if ((LeftChild != null) || (RightChild != null))
            {
                // Add null or the child's value.
                if (LeftChild == null)
                    result += string.Format("{0}{1}null\n", spaces, "  ");
                else
                    result += LeftChild.ToString(spaces + "  ");

                // Add null or the child's value.
                if (RightChild == null)
                    result += string.Format("{0}{1}null\n", spaces, "  ");
                else
                    result += RightChild.ToString(spaces + "  ");
            }
            return result;
        }

        // Recursively search this node's subtree looking for the target value.
        // Return the node that contains the value or null.
        internal BinaryNode<T> FindNode(T target)
        {
            // See if this node contains the value.
            if (Value.Equals(target)) return this;

            // Search the left child subtree.
            BinaryNode<T> result = null;
            if (LeftChild != null)
                result = LeftChild.FindNode(target);
            if (result != null) return result;

            // Search the right child subtree.
            if (RightChild != null)
                result = RightChild.FindNode(target);
            if (result != null) return result;

            // We did not find the value. Return null.
            return null;
        }

        internal List<BinaryNode<T>> TraversePreorder()
        {
            List<BinaryNode<T>> result = new List<BinaryNode<T>>();

            // Add this node to the traversal.
            result.Add(this);

            // Add the child subtrees.
            if (LeftChild != null) result.AddRange(LeftChild.TraversePreorder());
            if (RightChild != null) result.AddRange(RightChild.TraversePreorder());
            return result;
        }
        internal List<BinaryNode<T>> TraverseInorder()
        {
            List<BinaryNode<T>> result = new List<BinaryNode<T>>();

            // Add the left subtree.
            if (LeftChild != null) result.AddRange(LeftChild.TraverseInorder());

            // Add this node.
            result.Add(this);

            // Add the right subtree.
            if (RightChild != null) result.AddRange(RightChild.TraverseInorder());
            return result;
        }
        internal List<BinaryNode<T>> TraversePostorder()
        {
            List<BinaryNode<T>> result = new List<BinaryNode<T>>();

            // Add the child subtrees.
            if (LeftChild != null) result.AddRange(LeftChild.TraversePostorder());
            if (RightChild != null) result.AddRange(RightChild.TraversePostorder());

            // Add this node.
            result.Add(this);
            return result;
        }

        internal List<BinaryNode<T>> TraverseBreadthFirst()
        {
            List<BinaryNode<T>> result = new List<BinaryNode<T>>();
            Queue<BinaryNode<T>> queue = new Queue<BinaryNode<T>>();

            // Start with the top node in the queue.
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                // Remove the top node from the queue and
                // add it to the result list.
                BinaryNode<T> node = queue.Dequeue();
                result.Add(node);

                // Add the node's children to the queue.
                if (node.LeftChild != null) queue.Enqueue(node.LeftChild);
                if (node.RightChild != null) queue.Enqueue(node.RightChild);
            }

            return result;
        }

        // New code goes here...
        
        /// <summary>
        /// simply calls ArrangeSubtree, DrawSubtreeLinks, and DrawSubtreeNodes.
        /// </summary>
        /// <param name="canvas"></param>
        internal void ArrangeAndDrawSubtree(Canvas canvas, double xmin, double ymin)
        {
            // Position the tree.
            ArrangeSubtree(xmin, ymin);

            // Draw the links.
            DrawSubtreeLinks(canvas);

            // Draw the nodes.
            DrawSubtreeNodes(canvas);
        }

        /// <summary>
        /// The ArrangeSubtree method is where you add code to position the node and the nodes in its subtree.
        /// </summary>
        internal void ArrangeSubtree(double xmin, double ymin)
        {
            
            // Calculate cy, the Y coordinate for this node.
            // This doesn't depend on the children.

            // If the node has no children, just place it here and return.
            if ((LeftChild == null) && (RightChild == null))
            {
                SubtreeBounds = new Rect(xmin, ymin, 2 * NODE_RADIUS, 2 * NODE_RADIUS);
                Center = new Point(
                    (SubtreeBounds.Left + SubtreeBounds.Right) / 2,
                    SubtreeBounds.Top + NODE_RADIUS);
                return;
            }

            // Set child_xmin and child_ymin to the
            // start position for child subtrees.
            double childXmin = xmin;
            double childYmin = ymin + 2 * NODE_RADIUS + Y_SPACING;
            var height = 0d;
            
            // Position the child subtrees.
            if (LeftChild != null)
            {
                // Arrange the left child subtree and update
                // child_xmin to allow room for its subtree.
                LeftChild.ArrangeSubtree(childXmin, childYmin);

                childXmin = LeftChild.SubtreeBounds.Right;
                // If we also have a right child,
                // add space between their subtrees.
                childXmin += RightChild != null ? X_SPACING : 0;
                
                height = LeftChild.SubtreeBounds.Bottom;
            }
            if (RightChild != null)
            {
                // Arrange the right child subtree.
                RightChild.ArrangeSubtree(childXmin, childYmin);
                childXmin = RightChild.SubtreeBounds.Right;
                
                // Update the height.
                if (RightChild.SubtreeBounds.Bottom > height)
                    height = RightChild.SubtreeBounds.Bottom;
                
            }

            SubtreeBounds = new Rect(xmin, ymin, childXmin - xmin, height - ymin);
            Center = new Point((SubtreeBounds.Left + SubtreeBounds.Right) / 2,
                SubtreeBounds.Top + NODE_RADIUS);
        }

        /// <summary>
        /// Draws the links in the node’s subtree
        /// </summary>
        internal void DrawSubtreeLinks(Canvas canvas)
        {
            if (LeftChild is not null)
            {
                LeftChild.DrawSubtreeLinks(canvas);
                canvas.DrawLine(Center, LeftChild.Center, Brushes.Navy, Thickness);
            }

            if (RightChild is not null)
            {
                RightChild.DrawSubtreeLinks(canvas);
                canvas.DrawLine(Center, RightChild.Center, Brushes.Navy, Thickness);
            }
            // Outline the subtree for debugging.
            canvas.DrawRectangle(SubtreeBounds, null, Brushes.Red, Thickness);
        }

        /// <summary>
        /// Draws the nodes in the node’s subtree
        /// </summary>
        internal void DrawSubtreeNodes(Canvas canvas)
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
            
            LeftChild?.DrawSubtreeNodes(canvas);
            RightChild?.DrawSubtreeNodes(canvas);
            
        }
        
    }
    