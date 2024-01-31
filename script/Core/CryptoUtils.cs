// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.CryptoUtils
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace LacieEngine.Core
{
  public class CryptoUtils
  {
    public static byte[] Encrypt(byte[] input, string key)
    {
      byte[] numArray = new byte[16];
      using (Aes aes = Aes.Create())
      {
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = numArray;
        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
            cryptoStream.Write((ReadOnlySpan<byte>) input);
          return memoryStream.ToArray();
        }
      }
    }

    public static string Encrypt(string input, string key)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) memoryStream))
          streamWriter.Write(input);
        return Convert.ToBase64String(CryptoUtils.Encrypt(memoryStream.ToArray(), key));
      }
    }

    public static byte[] Decrypt(byte[] input, string key)
    {
      byte[] numArray = new byte[16];
      using (Aes aes = Aes.Create())
      {
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = numArray;
        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using (MemoryStream memoryStream = new MemoryStream(input))
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
          {
            using (MemoryStream destination = new MemoryStream())
            {
              cryptoStream.CopyTo((Stream) destination);
              return destination.ToArray();
            }
          }
        }
      }
    }

    public static string Decrypt(string input, string key)
    {
      using (MemoryStream memoryStream = new MemoryStream(CryptoUtils.Decrypt(Convert.FromBase64String(input), key)))
      {
        using (StreamReader streamReader = new StreamReader((Stream) memoryStream))
          return streamReader.ReadToEnd();
      }
    }

    public static string Sha1(string input)
    {
      using (SHA1Managed shA1Managed = new SHA1Managed())
      {
        byte[] hash = shA1Managed.ComputeHash(Encoding.UTF8.GetBytes(input));
        StringBuilder stringBuilder = new StringBuilder(hash.Length * 2);
        foreach (byte num in hash)
          stringBuilder.Append(num.ToString("x2"));
        return stringBuilder.ToString();
      }
    }
  }
}
