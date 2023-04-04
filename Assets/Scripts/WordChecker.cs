using System;
using System.Collections.Generic;
using System.IO;

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
}