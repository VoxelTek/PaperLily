// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.StringExtension
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace LacieEngine.Core
{
  public static class StringExtension
  {
    private static readonly string[] StringLineSplit = new string[3]
    {
      "\r\n",
      "\r",
      "\n"
    };
    private static readonly TextInfo TextInfo = new CultureInfo("en-US", false).TextInfo;

    public static bool In(this string str, params string[] list)
    {
      foreach (string str1 in list)
      {
        if (str1 == str)
          return true;
      }
      return false;
    }

    public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

    public static bool IsQuotedText(this string str) => str.StartsWith("\"") && str.EndsWith("\"");

    public static string StripPrefix(this string str, string prefix)
    {
      if (!str.StartsWith(prefix))
        return str;
      string str1 = str;
      int length = prefix.Length;
      return str1.Substring(length, str1.Length - length);
    }

    public static string StripSuffix(this string str, string suffix)
    {
      return !str.EndsWith(suffix) ? str : str.Substring(0, str.LastIndexOf(suffix));
    }

    public static string StripEdges(this string str, string edges)
    {
      return str.StripPrefix(edges).StripSuffix(edges);
    }

    public static string ToPascalCase(this string str)
    {
      return StringExtension.TextInfo.ToTitleCase(str).Replace(" ", string.Empty);
    }

    public static string[] SplitLines(this string str, bool keepEmpty = false)
    {
      string[] strArray = str.Split(StringExtension.StringLineSplit, StringSplitOptions.None);
      List<string> stringList = new List<string>();
      foreach (string str1 in strArray)
      {
        string str2 = str1.Trim();
        if (keepEmpty || !(str2 == string.Empty))
          stringList.Add(str2);
      }
      return stringList.ToArray();
    }

    public static int[] SplitInts(this string str, char separator)
    {
      string[] strArray = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
      List<int> intList = new List<int>();
      foreach (string str1 in strArray)
      {
        int result;
        if (int.TryParse(str1.Trim(), out result))
          intList.Add(result);
        else
          Log.Warn((object) "SplitInts: Invalid string to be converted to a number: ", (object) str1);
      }
      return intList.ToArray();
    }

    public static float[] SplitFloats(this string str, char separator)
    {
      string[] strArray = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
      List<float> floatList = new List<float>();
      foreach (string str1 in strArray)
      {
        float result;
        if (float.TryParse(str1.Trim(), out result))
          floatList.Add(result);
        else
          Log.Warn((object) "SplitFloats: Invalid string to be converted to a number: ", (object) str1);
      }
      return floatList.ToArray();
    }
  }
}
