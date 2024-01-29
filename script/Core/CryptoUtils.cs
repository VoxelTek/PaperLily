using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LacieEngine.Core
{
	public class CryptoUtils
	{
		public static byte[] Encrypt(byte[] input, string key)
		{
			byte[] iv = new byte[16];
			using Aes aes = Aes.Create();
			aes.Key = Encoding.UTF8.GetBytes(key);
			aes.IV = iv;
			ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
			using MemoryStream memoryStream = new MemoryStream();
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
			{
				cryptoStream.Write(input);
			}
			return memoryStream.ToArray();
		}

		public static string Encrypt(string input, string key)
		{
			using MemoryStream memoryStream = new MemoryStream();
			using (StreamWriter streamWriter = new StreamWriter(memoryStream))
			{
				streamWriter.Write(input);
			}
			return Convert.ToBase64String(Encrypt(memoryStream.ToArray(), key));
		}

		public static byte[] Decrypt(byte[] input, string key)
		{
			byte[] iv = new byte[16];
			using Aes aes = Aes.Create();
			aes.Key = Encoding.UTF8.GetBytes(key);
			aes.IV = iv;
			ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
			using MemoryStream inputStream = new MemoryStream(input);
			using CryptoStream cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read);
			using MemoryStream outputStream = new MemoryStream();
			cryptoStream.CopyTo(outputStream);
			return outputStream.ToArray();
		}

		public static string Decrypt(string input, string key)
		{
			using MemoryStream memoryStream = new MemoryStream(Decrypt(Convert.FromBase64String(input), key));
			using StreamReader streamReader = new StreamReader(memoryStream);
			return streamReader.ReadToEnd();
		}

		public static string Sha1(string input)
		{
			using SHA1Managed sha1 = new SHA1Managed();
			byte[] array = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
			StringBuilder sb = new StringBuilder(array.Length * 2);
			byte[] array2 = array;
			foreach (byte b in array2)
			{
				sb.Append(b.ToString("x2"));
			}
			return sb.ToString();
		}
	}
}
