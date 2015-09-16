// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeNode.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   树节点.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Skymate;
    using Skymate.Extensions;

    /// <summary>
    /// 树节点.
    /// </summary>
    /// <typeparam name="T">
    /// 泛型
    /// </typeparam>
    public class TreeNode<T> : ICloneable<TreeNode<T>>
    {
        /// <summary>
        /// 子节点.
        /// </summary>
        private readonly LinkedList<TreeNode<T>> children = new LinkedList<TreeNode<T>>();

        /// <summary>
        /// 深度.
        /// </summary>
        private int? depth;

        /// <summary>
        /// 创建 <see cref="TreeNode{T}"/> 实例.
        /// </summary>
        /// <param name="value">
        /// 值.
        /// </param>
        public TreeNode(T value)
        {
            this.Value = value;
        }

        #region 属性

        /// <summary>
        /// Gets the parent.
        /// </summary>
        public TreeNode<T> Parent { get; private set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Gets the children.
        /// </summary>
        public IEnumerable<TreeNode<T>> Children
        {
            get
            {
                return this.children;
            }
        }

        /// <summary>
        /// Gets the leaf nodes.
        /// </summary>
        public IEnumerable<TreeNode<T>> LeafNodes
        {
            get
            {
                return this.children.Where(x => x.IsLeaf);
            }
        }

        /// <summary>
        /// Gets the non leaf nodes.
        /// </summary>
        public IEnumerable<TreeNode<T>> NonLeafNodes
        {
            get
            {
                return this.children.Where(x => !x.IsLeaf);
            }
        }

        /// <summary>
        /// Gets the first child.
        /// </summary>
        public TreeNode<T> FirstChild
        {
            get
            {
                var first = this.children.First;
                if (first != null)
                {
                    return first.Value;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the last child.
        /// </summary>
        public TreeNode<T> LastChild
        {
            get
            {
                var last = this.children.Last;
                if (last != null)
                {
                    return last.Value;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is leaf.
        /// </summary>
        public bool IsLeaf
        {
            get
            {
                return this.children.Count == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether has children.
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return this.children.Count > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is root.
        /// </summary>
        public bool IsRoot
        {
            get
            {
                return this.Parent == null;
            }
        }

        /// <summary>
        /// Gets the depth.
        /// </summary>
        public int Depth
        {
            get
            {
                if (this.depth.HasValue)
                {
                    return this.depth.Value;
                }

                var node = this;
                var i = -1;
                while (node != null && !node.IsRoot)
                {
                    i++;
                    node = node.Parent;
                }

                this.depth = i;

                return this.depth.Value;
            }
        }

        /// <summary>
        /// Gets the root.
        /// </summary>
        public TreeNode<T> Root
        {
            get
            {
                var root = this;
                while (root.Parent != null)
                {
                    root = root.Parent;
                }

                return root;
            }
        }

        /// <summary>
        /// Gets the next.
        /// </summary>
        public TreeNode<T> Next
        {
            get
            {
                if (this.Parent == null)
                {
                    return null;
                }

                var self = this.Parent.children.Find(this);
                var next = self != null ? self.Next : null;
                return next != null ? next.Value : null;
            }
        }

        /// <summary>
        /// Gets the previous.
        /// </summary>
        public TreeNode<T> Previous
        {
            get
            {
                if (this.Parent == null)
                {
                    return null;
                }

                var self = this.Parent.children.Find(this);
                var prev = self != null ? self.Previous : null;
                return prev != null ? prev.Value : null;
            }
        }

        #endregion

        #region 索引

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="i">
        /// The i.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>TreeNode</cref>
        ///     </see>
        ///     .
        /// </returns>
        public TreeNode<T> this[int i]
        {
            get
            {
                return this.children.ElementAt(i);
            }
        }

        #endregion

        #region 方法

        #region Append

        /// <summary>
        /// The append.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>TreeNode</cref>
        ///     </see>
        ///     .
        /// </returns>
        public TreeNode<T> Append(T value)
        {
            var node = new TreeNode<T>(value);
            this.AddChild(node, false);
            return node;
        }

        /// <summary>
        /// The append.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="clone">
        /// The clone.
        /// </param>
        public void Append(TreeNode<T> node, bool clone = true)
        {
            this.AddChild(node, clone);
        }

        /// <summary>
        /// The append many.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        public ICollection<TreeNode<T>> AppendMany(IEnumerable<T> values)
        {
            return values.Select(this.Append).AsReadOnly();
        }

        /// <summary>
        /// The append many.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The <see cref="TreeNode{T}"/>.
        /// </returns>
        public TreeNode<T>[] AppendMany(params T[] values)
        {
            return values.Select(this.Append).ToArray();
        }

        /// <summary>
        /// The append many.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        public void AppendMany(IEnumerable<TreeNode<T>> values)
        {
            values.Each(x => this.AddChild(x, true));
        }

        /// <summary>
        /// The append children of.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        public void AppendChildrenOf(TreeNode<T> node)
        {
            node.children.Each(x => this.AddChild(x, true));
        }

        #endregion Append

        #region Prepend

        /// <summary>
        /// The prepend.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="TreeNode{T}"/>.
        /// </returns>
        public TreeNode<T> Prepend(T value)
        {
            var node = new TreeNode<T>(value);
            this.AddChild(node, true, false);
            return node;
        }

        #endregion Prepend

        #region Insert[...]

        /// <summary>
        /// The insert after.
        /// </summary>
        /// <param name="refNode">
        /// The ref node.
        /// </param>
        public void InsertAfter(TreeNode<T> refNode)
        {
            this.Insert(refNode, true);
        }

        /// <summary>
        /// The insert before.
        /// </summary>
        /// <param name="refNode">
        /// The ref node.
        /// </param>
        public void InsertBefore(TreeNode<T> refNode)
        {
            this.Insert(refNode, false);
        }

        #endregion Insert[...]

        #region Select[...]

        /// <summary>
        /// The select node.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <returns>
        /// The <see cref="TreeNode{T}"/>.
        /// </returns>
        public TreeNode<T> SelectNode(Expression<Func<TreeNode<T>, bool>> predicate)
        {
            Guard.ArgumentNotNull(() => predicate);

            return this.FlattenNodes(predicate, false).FirstOrDefault();
        }

        /// <summary>
        /// Selects all nodes (recursively) with match the given <c>predicate</c>,
        /// but excluding self
        /// </summary>
        /// <param name="predicate">
        /// The predicate to match against
        /// </param>
        /// <returns>
        /// A readonly collection of node matches
        /// </returns>
        public ICollection<TreeNode<T>> SelectNodes(Expression<Func<TreeNode<T>, bool>> predicate)
        {
            Guard.ArgumentNotNull(() => predicate);

            var result = new List<TreeNode<T>>();

            var flattened = this.FlattenNodes(predicate, false);
            result.AddRange(flattened);

            return result.AsReadOnly();
        }

        #endregion Select[...]

        /// <summary>
        /// The remove node.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveNode(TreeNode<T> node)
        {
            node.TraverseTree(x => x.depth = null);
            return this.children.Remove(node);
        }

        /// <summary>
        /// The clear.
        /// </summary>
        public void Clear()
        {
            this.children.Clear();
        }

        /// <summary>
        /// The traverse.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        public void Traverse(Action<T> action)
        {
            action(this.Value);
            foreach (var child in this.children)
            {
                child.Traverse(action);
            }
        }

        /// <summary>
        /// The traverse tree.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        public void TraverseTree(Action<TreeNode<T>> action)
        {
            action(this);
            foreach (var child in this.children)
            {
                child.TraverseTree(action);
            }
        }

        /// <summary>
        /// The flatten.
        /// </summary>
        /// <param name="includeSelf">
        /// The include self.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public IEnumerable<T> Flatten(bool includeSelf = true)
        {
            return this.Flatten(null, includeSelf);
        }

        /// <summary>
        /// The flatten.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <param name="includeSelf">
        /// The include self.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public IEnumerable<T> Flatten(Expression<Func<T, bool>> expression, bool includeSelf = true)
        {
            IEnumerable<T> list;
            if (includeSelf)
            {
                list = new[] { this.Value };
            }
            else
            {
                list = Enumerable.Empty<T>();
            }

            var result = list.Union(this.children.SelectMany(x => x.Flatten()));
            if (expression != null)
            {
                result = result.Where(expression.Compile());
            }

            return result;
        }

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="TreeNode{T}"/>.
        /// </returns>
        public TreeNode<T> Find(T value)
        {
            // Guard.ArgumentNotNull(value, "value");
            if (this.Value.Equals(value))
            {
                return this;
            }

            TreeNode<T> item = null;

            foreach (var child in this.children)
            {
                item = child.Find(value);
                if (item != null)
                {
                    break;
                }
            }

            return item;
        }

        /// <summary>
        /// The clone.
        /// </summary>
        /// <returns>
        /// The <see cref="TreeNode{T}"/>.
        /// </returns>
        public TreeNode<T> Clone()
        {
            return this.Clone(true);
        }

        /// <summary>
        /// The clone.
        /// </summary>
        /// <param name="deep">
        /// The deep.
        /// </param>
        /// <returns>
        /// The <see cref="TreeNode{T}"/>.
        /// </returns>
        public TreeNode<T> Clone(bool deep)
        {
            T value = this.Value;

            if (value is ICloneable)
            {
                value = (T)((ICloneable)value).Clone();
            }

            var clone = new TreeNode<T>(value);
            if (deep)
            {
                clone.AppendChildrenOf(this);
            }

            return clone;
        }

        /// <summary>
        /// The clone.
        /// </summary>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        object ICloneable.Clone()
        {
            return this.Clone(true);
        }

        /// <summary>
        /// The flatten nodes.
        /// </summary>
        /// <param name="includeSelf">
        /// The include self.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        internal IEnumerable<TreeNode<T>> FlattenNodes(bool includeSelf = true)
        {
            return this.FlattenNodes(null, includeSelf);
        }

        /// <summary>
        /// The flatten nodes.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <param name="includeSelf">
        /// The include self.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        internal IEnumerable<TreeNode<T>> FlattenNodes(Expression<Func<TreeNode<T>, bool>> expression, bool includeSelf = true)
        {
            IEnumerable<TreeNode<T>> list;
            if (includeSelf)
            {
                list = new[] { this };
            }
            else
            {
                list = Enumerable.Empty<TreeNode<T>>();
            }

            var result = list.Union(this.children.SelectMany(x => x.FlattenNodes()));
            if (expression != null)
            {
                result = result.Where(expression.Compile());
            }

            return result;
        }


        /// <summary>
        /// The add child.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="clone">
        /// The clone.
        /// </param>
        /// <param name="append">
        /// The append.
        /// </param>
        private void AddChild(TreeNode<T> node, bool clone, bool append = true)
        {
            var newNode = node;
            if (clone)
            {
                newNode = node.Clone(true);
            }

            newNode.Parent = this;
            newNode.TraverseTree(x => x.depth = null);
            if (append)
            {
                this.children.AddLast(newNode);
            }
            else
            {
                this.children.AddFirst(newNode);
            }
        }


        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="refNode">
        /// The ref node.
        /// </param>
        /// <param name="after">
        /// The after.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        private void Insert(TreeNode<T> refNode, bool after)
        {
            Guard.ArgumentNotNull(() => refNode);

            if (refNode.Parent == null)
            {
                throw Error.Argument("refNode", "The reference node cannot be a root node and must be attached to the tree.");
            }

            var refLinkedList = refNode.Parent.children;
            var refNodeInternal = refLinkedList.Find(refNode);

            if (this.Parent != null)
            {
                var thisLinkedList = this.Parent.children;
                thisLinkedList.Remove(this);
            }

            if (after)
            {
                refLinkedList.AddAfter(refNodeInternal, this);
            }
            else
            {
                refLinkedList.AddBefore(refNodeInternal, this);
            }

            this.Parent = refNode.Parent;
            this.TraverseTree(x => x.depth = null);
        }


        #endregion 
    }
}