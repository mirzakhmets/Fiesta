
using System.IO;

namespace Fiesta
{
  public class Hash
  {
    public const int HASH_SIZE = 128;

    public static byte[] GetHash(Stream stream)
    {
      int index1 = 0;
      byte[] hash = new byte[128];
      for (int index2 = 0; index2 < hash.Length; ++index2)
        hash[index2] = (byte) 0;
      int num;
      while ((num = stream.ReadByte()) != -1)
      {
        hash[index1] = (byte) ((uint) hash[index1] ^ (uint) num);
        index1 = (index1 + 1) % 128;
      }
      return hash;
    }
  }
}
