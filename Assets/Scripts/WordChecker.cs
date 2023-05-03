using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class TrieNode
{
    public bool is_leaf;
    public TrieNode[] children;

    public TrieNode()
    {
        is_leaf = false;
        children = new TrieNode[26];
    }
}

class Trie
{
    private TrieNode root;

    public Trie()
    {
        root = new TrieNode();
    }

    public void Insert(string word)
    {
        TrieNode curr = root;

        foreach (char c in word)
        {
            int index = c - 'a';
            if (index >= 0 && index < 26)
            {
                if (curr.children[index] == null)
                {
                    curr.children[index] = new TrieNode();
                }

                curr = curr.children[index];
            }
        }

        curr.is_leaf = true;
    }

    public bool Search(string word)
    {
        TrieNode curr = root;

        foreach (char c in word)
        {
            int index = c - 'A';

            if (curr.children[index] == null)
            {
                return false;
            }

            curr = curr.children[index];
        }

        return curr.is_leaf;
    }

    private void Destroy(TrieNode root)
    {
        if (root == null)
        {
            return;
        }

        foreach (TrieNode child in root.children)
        {
            Destroy(child);
        }

        root = null;
    }

    public void Destroy()
    {
        Destroy(root);
    }

    public char[] GetNextLetters(string prefix, int wordLength)
    {
        if (wordLength - prefix.Length == 0)
        {
            return new char[0];
        }
        else if (prefix.Length == 0) {
            char[] alphabet = Enumerable.Range('A', 26).Select(c => (char) c).ToArray();
            return alphabet;
        }
        string[] words = GetWordsStartingWith(prefix, wordLength);
        char[] letters = Array.ConvertAll(words, word => word[prefix.Length]);
        return letters.Distinct().ToArray();
    }

    public string[] GetWordsStartingWith(string prefix, int wordLength)
    {
        List<string> words = new List<string>();
        TrieNode curr = root;

        // Encontra o nó correspondente ao prefixo
        foreach (char c in prefix)
        {
            int index = c - 'a';
            if (index >= 0 && index < 26)
            {
                if (curr.children[index] == null)
                {
                    // Se o nó correspondente ao prefixo não existir, retorna um array vazio
                    return new string[0];
                }

                curr = curr.children[index];
            }
        }
        
        int remainingLength = wordLength - prefix.Length;
        // Obtém as palavras que começam com o prefixo e possuem o comprimento especificado
        GetWordsStartingWithRecursive(curr, prefix, remainingLength, words);

        return words.ToArray();
    }

    private void GetWordsStartingWithRecursive(TrieNode node, string prefix, int remainingLength, List<string> words)
    {
        if (remainingLength == 0)
        {
            if (node.is_leaf)
            {
                words.Add(prefix);
            }
            return;
        }

        // Verifica os filhos do nó atual para obter as próximas letras
        for (int i = 0; i < 26; i++)
        {
            if (node.children[i] != null)
            {
                char c = (char)('a' + i);
                GetWordsStartingWithRecursive(node.children[i], prefix + c, remainingLength - 1, words);
            }
        }
    }

}

public class WordChecker
{
    private Trie trie = new Trie();

    public WordChecker(string filePath)
    {
        // Load words into trie when WordChecker object is created
        using (StreamReader file = new StreamReader(filePath))
        {
            if (file == null)
            {
                Console.Error.WriteLine("Error: Unable to open file");
                Environment.Exit(1);
            }

            string line;

            while ((line = file.ReadLine()) != null)
            {
                trie.Insert(line);
            }

            file.Close();
        }
    }

    public bool IsWordInTrie(string word)
    {
        return trie.Search(word);
    }

    public Char[] PossibleLettersInTrie(string prefix, int length)
    {
        return trie.GetNextLetters(prefix.ToLower(), length);
    }

    public string[] PossibleWordsInTrie(string prefix, int length)
    {
        return trie.GetWordsStartingWith(prefix.ToLower(), length);
    }
}