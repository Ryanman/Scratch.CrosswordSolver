using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CrosswordSolver
{
    class Program
    {
        private static Trie _Trie;
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string FileName = "english.csv";
        static void Main(string[] args)
        {
            Console.WriteLine("Loading Dictionary...");
            _Trie = new Trie();
            //Foreach word in file, insert
            //Fast enough that I'm not going to implement a progress bar here
            int count = 0;
            using (var sr = new StreamReader(File.OpenRead(FileName)))
            {
                while (!sr.EndOfStream)
                {
                    var word = sr.ReadLine();
                    InsertWord(word);
                    count++;
                }                
            }
            Console.Write(string.Format("Loading Complete! Loaded {0} words.\n\n",count));

            bool stillSearching = true;
            string template = "";
            while (stillSearching)
            {
                Console.Write("Enter word template to search for, using '-' as a wildcard: ");
                bool validInput = false;
                //Use a simple regex to ensure template works...
                while (!validInput)
                {
                    template = Console.ReadLine().ToUpper();
                    validInput = Regex.Match(template, @"^-|[a-zA-Z]|-$").Success; //I'll admit I'm not a regex expert, this seems to accept most inputs.
                    if (!validInput) Console.WriteLine("Incorrect Input! Please Try again:\n");
                }

                var words = GetPossibleWords(template);
                Console.WriteLine("Possible Words: ");
                foreach (var word in words)
                {
                    Console.WriteLine("\t{0}", word);
                }
                Console.WriteLine("\nSearch Complete. Press Enter to continue, or enter 'E' to exit");
                stillSearching = !(Console.ReadLine() == "E");
            }
            
            
        }

        private static List<string> GetPossibleWords(string template)
        {
            var curNode = _Trie.Head;
            var possibleWords = new List<string>();
            var sb = new StringBuilder();

            Search(template, 0, sb, possibleWords, curNode);

            return possibleWords;
        }

        private static void Search(string template, int depth, StringBuilder sb, List<string> possibleWords, TrieNode curNode)
        {
            if (curNode.Value != null)
            {
                sb.Append(curNode.Value);
            }
            if (depth == template.Length)
            {
                if (curNode.IsWord) possibleWords.Add(sb.ToString());
                RemoveLastChar(sb);//Move back up Trie
                return;
            }
            var nextCharacter = template[depth];
            if (nextCharacter == '-')
            {
                foreach(var letter in Alphabet)
                {
                    if (curNode.CharExists(letter))
                    {
                        Search(template, depth + 1, sb, possibleWords, curNode.Children[letter]);
                    }                    
                }
            }
            else if (curNode.CharExists(nextCharacter))
            {
                Search(template, depth + 1, sb, possibleWords, curNode.Children[nextCharacter]);
            }
            RemoveLastChar(sb);//Move back up Trie
        }

        private static void RemoveLastChar(StringBuilder sb)
        {
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
        }

        private static void InsertWord(string s)
        {
            var curNode = _Trie.Head;
            for (int i = 0; i < s.Length; i++) //For over foreach to mark the end of a word
            {
                var isEndOfWord = (i == s.Length - 1);
                curNode = _Trie.InsertNode(char.ToUpper(s[i]), curNode,isEndOfWord);
            }
        }
    }
}
