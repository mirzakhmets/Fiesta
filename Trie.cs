
using System.IO;

namespace Fiesta
{
  public class Trie
  {
    public TrieNode MainNode = new TrieNode();

    public void AddHash(byte[] hash)
    {
      TrieNode trieNode1 = this.MainNode;
      for (int index = 0; index < hash.Length; ++index)
      {
        TrieNode trieNode2 = trieNode1;
        trieNode1 = trieNode2.Children[(int) hash[index]];
        if (trieNode1 == null)
        {
          trieNode1 = new TrieNode();
          trieNode2.Children[(int) hash[index]] = trieNode1;
        }
      }
    }

    public bool Query(byte[] query)
    {
      TrieNode trieNode = this.MainNode;
      for (int index = 0; index < query.Length; ++index)
      {
        trieNode = trieNode.Children[(int) query[index]];
        if (trieNode == null)
          return false;
      }
      return true;
    }

    public static Trie LoadTrie(Stream stream)
    {
      Trie trie = new Trie();
      TrieNode trieNode = trie.MainNode;
      int num = 0;
      int index;
      while ((index = stream.ReadByte()) != -1)
      {
        if (trieNode.Children[index] == null)
          trieNode.Children[index] = new TrieNode();
        num = (num + 1) % 128;
        trieNode = num != 0 ? trieNode.Children[index] : trie.MainNode;
      }
      return trie;
    }
  }
}
