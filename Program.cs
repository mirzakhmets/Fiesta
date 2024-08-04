
using System;
using System.IO;

namespace Fiesta
{
  public class Program
  {
    public const string VD = "vd.bin";
    public static int V_COUNT;

    public static void Scan(string path, Trie trie)
    {
      foreach (string enumerateFile in Directory.EnumerateFiles(path))
      {
        try
        {
          Stream stream = (Stream) File.OpenRead(enumerateFile);
          if (enumerateFile.IndexOf("vd.bin") < 0 && trie.Query(Hash.GetHash(stream)))
          {
            ++Program.V_COUNT;
            Console.WriteLine("Threat found: " + enumerateFile + " in directory " + path);
          }
          stream.Close();
        }
        catch (Exception ex)
        {
          Console.WriteLine("File access error: " + enumerateFile);
        }
      }
      foreach (string enumerateDirectory in Directory.EnumerateDirectories(path))
        Program.Scan(enumerateDirectory, trie);
    }

    private static void Main(string[] args)
    {
      if (args.Length != 2)
      {
        Console.WriteLine("Usage: [ -q <file name> | -s <directory> ]");
        Console.WriteLine("\t-q - quarantine threat");
        Console.WriteLine("\t-s - scan directory");
      }
      else
      {
        if (!File.Exists("vd.bin"))
          File.Create("vd.bin");
        if (args[0].Equals("-q"))
        {
          try
          {
            FileStream fileStream1 = File.OpenRead(args[1]);
            byte[] hash = Hash.GetHash((Stream) fileStream1);
            fileStream1.Close();
            FileStream fileStream2 = File.Open("vd.bin", FileMode.Append, FileAccess.Write);
            fileStream2.Write(hash, 0, hash.Length);
            fileStream2.Close();
          }
          catch (Exception ex)
          {
            Console.WriteLine("File doesn't exist");
          }
        }
        else
        {
          FileStream fileStream = File.OpenRead("vd.bin");
          Trie trie = Trie.LoadTrie((Stream) fileStream);
          fileStream.Close();
          Program.V_COUNT = 0;
          Program.Scan(args[1], trie);
          if (Program.V_COUNT != 0)
            return;
          Console.WriteLine("No threats detected");
        }
      }
    }
  }
}
