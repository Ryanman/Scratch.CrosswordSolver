
namespace CrosswordSolver
{
    public class Trie
    {
        public TrieNode Head;

        public Trie()
        {
            Head = new TrieNode();
        }
        
        public TrieNode InsertNode(char c, TrieNode parent, bool isEndOfWord)
        {
            if (!parent.Children.ContainsKey(c))
            {
                var newNode = new TrieNode(c, parent, isEndOfWord);
                parent.Children.Add(c, newNode);
                return newNode;
            }
            return parent.Children[c];
        }
    }
}
