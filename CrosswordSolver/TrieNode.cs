
using System.Collections.Generic;

namespace CrosswordSolver
{
    public class TrieNode
    {
        public char? Value { get; set; }
        public Dictionary<char, TrieNode> Children { get; set; } 
        public TrieNode Parent { get; set; } //Don't use this property but I figure any Trie should have it
        public bool IsWord { get; set; }

        public TrieNode()
        {
            Value = null;
            Children = new Dictionary<char,TrieNode>();
            IsWord = false;
            Parent = null;
        }
        public TrieNode(char value, TrieNode parent, bool isWord)
        {
            Value = value;
            Children = new Dictionary<char, TrieNode>();
            IsWord = isWord;
            Parent = parent;
        }

        public bool IsLeaf()
        {
            return Children.Count == 0;
        }

        public bool IsRoot()
        {
            return Parent == null;
        }

        public bool CharExists(char c)
        {
            return !IsLeaf()
                && Children.ContainsKey(c);
        }
    }
}
