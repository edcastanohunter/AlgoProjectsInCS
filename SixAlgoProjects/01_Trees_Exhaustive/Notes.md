Traversal Uses
Tree traversals are often used by other algorithms to visit the tree’s nodes in
various orders. The following list describes a few situations when a particular
traversal might be handy:
■■ If you want to copy a tree, then you need to create each node before you
can create that node’s children. In that case, a preorder traversal is helpful
because it visits the original tree’s nodes before visiting their children.
■■ Preorder traversals are also useful for evaluating mathematical equations
written in Polish notation. (See https://en.wikipedia.org/wiki/
Polish _ notation.)
■■ If the tree is sorted, then an inorder traversal flattens the tree and visits
its nodes in their sorted order.
■■ Inorder traversals are also useful for evaluating normal mathematical
expressions. The section “Expression Evaluation,” later in this chapter,
explains that technique.
■■A breadth-first traversal lets you find a node that is as close to the root as
possible. For example, if you want to find a particular value in the tree
and that value may occur more than once, then a breadth-first traversal
will let you find the value that is closest to the root. (This same approach
is even more useful in some network algorithms, such as certain shortest
path algorithms.)
■■ Postorder traversals are useful for evaluating mathematical equations
written in reverse Polish notation. (See https://en.wikipedia.org/wiki/
Reverse_Polish_notation.)
■■ Postorder traversals are also useful for destroying trees in languages, such
as C and C++, which do not have garbage collection. In those languages,
you must free a node’s memory before you free any objects that might be
pointing to it. In a tree, a parent node holds references to its children, so
you must free the children first. A postorder traversal lets you visit the
nodes in the correct order.


## Lowest Common Ancestors
There are several ways that you can find the lowest common ancestor (LCA) of two nodes. Different 
LCA Algorithms work with different kinds of trees and produce different desired behaviors. For example, some 
algorithms work on sorted trees, other work when child nodes have links to their parents, and some preprocess the tree
to provide faster lookup of the lowest common ancestors later. 

### LCA's under different circumstances

#### Sorted Trees 


