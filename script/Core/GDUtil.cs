// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.GDUtil
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using Karambolo.PO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#nullable disable
namespace LacieEngine.Core
{
  public static class GDUtil
  {
    private static readonly JsonSerializerSettings serializerSettings;
    private static readonly Regex vector2Regex = new Regex("\\(-?\\d+,-?\\d+\\)");
    private static readonly Godot.Directory fileOpDir = new Godot.Directory();

    static GDUtil()
    {
      GDUtil.serializerSettings = new JsonSerializerSettings();
      GDUtil.serializerSettings.NullValueHandling = NullValueHandling.Ignore;
    }

    public static object ReadBinaryFile(string filename, string encryptionKey = null)
    {
      Godot.File file = new Godot.File();
      if (!file.FileExists(filename))
        return (object) null;
      int num = (int) file.Open(filename, Godot.File.ModeFlags.Read);
      using (MemoryStream memoryStream = new MemoryStream(file.GetBuffer((long) file.GetLen())))
      {
        byte[] numArray = memoryStream.ToArray();
        if (!encryptionKey.IsNullOrEmpty())
          numArray = CryptoUtils.Decrypt(numArray, encryptionKey);
        using (MemoryStream serializationStream = new MemoryStream(numArray))
          return new BinaryFormatter().Deserialize((Stream) serializationStream);
      }
    }

    public static string ReadTextFile(string filename, string encryptionKey = null)
    {
      Godot.File file = new Godot.File();
      if (!file.FileExists(filename))
        return (string) null;
      int num = (int) file.Open(filename, Godot.File.ModeFlags.Read);
      string input = file.GetAsText();
      file.Close();
      if (!encryptionKey.IsNullOrEmpty())
        input = CryptoUtils.Decrypt(input, encryptionKey);
      return input;
    }

    public static List<List<string>> ReadCsvFile(string filename)
    {
      Godot.File file = new Godot.File();
      List<List<string>> stringListList = new List<List<string>>();
      if (!file.FileExists(filename))
      {
        Log.Error((object) "Cannot read CSV file because it doesn't exist: ", (object) filename);
        return (List<List<string>>) null;
      }
      int num = (int) file.Open(filename, Godot.File.ModeFlags.Read);
      while (!file.EofReached())
        stringListList.Add(new List<string>((IEnumerable<string>) file.GetCsvLine()));
      file.Close();
      return stringListList;
    }

    public static POParseResult ReadPoFile(string filename)
    {
      POParserSettings settings = new POParserSettings()
      {
        StringDecodingOptions = new POStringDecodingOptions()
      };
      settings.StringDecodingOptions.KeepTranslationStringsPlatformIndependent = true;
      settings.StringDecodingOptions.KeepKeyStringsPlatformIndependent = true;
      return new POParser(settings).Parse(GDUtil.ReadTextFile(filename));
    }

    public static T ReadJsonFile<T>(string filename, string encryptionKey = null) where T : new()
    {
      string str = GDUtil.ReadTextFile(filename, encryptionKey);
      if (str != null)
        return JsonConvert.DeserializeObject<T>(str, GDUtil.serializerSettings);
      Log.Error((object) "Cannot read JSON file because it doesn't exist: ", (object) filename);
      return default (T);
    }

    public static System.Collections.Generic.Dictionary<string, string> ReadCaptionFile(
      string translationFile,
      string lang,
      string baseLang = null)
    {
      Log.Debug((object) "Reading caption file: ", (object) translationFile);
      List<List<string>> stringListList = GDUtil.ReadCsvFile(translationFile);
      System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
      if (stringListList.Count < 2 || stringListList[0].Count < 2)
      {
        Log.Error((object) "Invalid translation CSV: ", (object) translationFile);
        return dictionary;
      }
      int index1 = -1;
      for (int index2 = 0; index2 < stringListList[0].Count; ++index2)
      {
        if (stringListList[0][index2] == lang)
        {
          index1 = index2;
          break;
        }
      }
      int index3 = 0;
      if (baseLang != null)
      {
        for (int index4 = 0; index4 < stringListList[0].Count; ++index4)
        {
          if (stringListList[0][index4] == baseLang)
          {
            index3 = index4;
            break;
          }
        }
      }
      if (index1 == -1)
        return dictionary;
      for (int index5 = 1; index5 < stringListList.Count; ++index5)
      {
        if (stringListList[index5].Count >= 2)
          dictionary[stringListList[index5][index3]] = stringListList[index5][index1];
      }
      return dictionary;
    }

    public static void SaveTextFile(string content, string filename, string encryptionKey = null)
    {
      if (!encryptionKey.IsNullOrEmpty())
        content = CryptoUtils.Encrypt(content, encryptionKey);
      Godot.File file = new Godot.File();
      int num = (int) file.Open(filename, Godot.File.ModeFlags.Write);
      file.StoreString(content);
      file.Close();
    }

    public static void SaveJsonFile(
      object content,
      string filename,
      string encryptionKey = null,
      bool prettify = false)
    {
      string str = JsonConvert.SerializeObject(content, GDUtil.serializerSettings);
      if (prettify)
        str = GDUtil.JsonPrettify(str);
      GDUtil.SaveTextFile(str, filename, encryptionKey);
    }

    public static string JsonPrettify(string json)
    {
      using (StringReader reader1 = new StringReader(json))
      {
        using (StringWriter stringWriter = new StringWriter())
        {
          JsonTextReader reader2 = new JsonTextReader((TextReader) reader1);
          JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) stringWriter);
          jsonTextWriter.Formatting = Formatting.Indented;
          jsonTextWriter.Indentation = 1;
          jsonTextWriter.IndentChar = '\t';
          jsonTextWriter.WriteToken((JsonReader) reader2);
          return stringWriter.ToString() + "\n";
        }
      }
    }

    public static bool FileExists(string filename)
    {
      return ResourceLoader.Exists(filename) || new Godot.File().FileExists(filename);
    }

    public static bool FolderExists(string folder)
    {
      folder.Replace("\\", "/");
      if (!folder.EndsWith("/"))
        folder += "/";
      return new Godot.Directory().DirExists(folder);
    }

    public static void EnsureFolderExists(string folder)
    {
      if (GDUtil.FolderExists(folder))
        return;
      int num = (int) new Godot.Directory().MakeDir(folder);
    }

    public static List<string> ListFilesInPath(
      string folder,
      string prefix = null,
      string suffix = null,
      bool fullPath = true,
      bool recursive = false)
    {
      return GDUtil.ListContentInPath(folder, fullPath, prefix, suffix, true, false, recursive);
    }

    public static List<string> ListFilesInPath(string folder, string extension)
    {
      return GDUtil.ListContentInPath(folder, true, (string) null, extension, true, false, false);
    }

    public static List<string> ListFoldersInPath(string folder, bool fullPath = true)
    {
      return GDUtil.ListContentInPath(folder, fullPath, (string) null, (string) null, false, true, false);
    }

    private static List<string> ListContentInPath(
      string folder,
      bool fullPath,
      string prefix,
      string suffix,
      bool listFiles,
      bool listDirs,
      bool recursive)
    {
      folder = folder.Replace("\\", "/");
      if (!folder.EndsWith("/"))
        folder += "/";
      if (prefix == null)
        prefix = "";
      if (suffix == null)
        suffix = "";
      string pattern1 = "^" + prefix + ".*" + suffix + "$";
      string pattern2 = "^" + prefix + ".*" + suffix + ".converted.res" + "$";
      string pattern3 = "^" + prefix + ".*" + suffix + ".import" + "$";
      List<string> stringList = new List<string>();
      using (Godot.Directory directory = new Godot.Directory())
      {
        int num1 = (int) directory.Open(folder);
        if (!GDUtil.PathsAreEqual(folder, directory.GetCurrentDir()))
          return stringList;
        int num2 = (int) directory.ListDirBegin(true, true);
        while (true)
        {
          string next;
          do
          {
            next = directory.GetNext();
            if (!next.IsNullOrEmpty())
            {
              if ((!directory.CurrentIsDir() || listDirs) && (directory.CurrentIsDir() || listFiles))
              {
                if (Regex.IsMatch(next.ToLower(), pattern1))
                  stringList.Add(fullPath ? folder + next : next);
                else if (Regex.IsMatch(next.ToLower(), pattern2))
                  stringList.Add(fullPath ? folder + next.StripSuffix(".converted.res") : next.StripSuffix(".converted.res"));
                else if (Regex.IsMatch(next.ToLower(), pattern3))
                  stringList.Add(fullPath ? folder + next.StripSuffix(".import") : next.StripSuffix(".import"));
              }
            }
            else
              goto label_20;
          }
          while (!(directory.CurrentIsDir() & recursive));
          stringList.AddRange((IEnumerable<string>) GDUtil.ListContentInPath(folder + next, fullPath, prefix, suffix, listFiles, listDirs, recursive));
        }
label_20:
        directory.ListDirEnd();
        return stringList;
      }
    }

    public static string FindFile(string filename, string folder)
    {
      folder = folder.Replace("\\", "/");
      if (!folder.EndsWith("/"))
        folder += "/";
      using (Godot.Directory directory = new Godot.Directory())
      {
        int num1 = (int) directory.Open(folder);
        if (!GDUtil.PathsAreEqual(folder, directory.GetCurrentDir()))
          return (string) null;
        int num2 = (int) directory.ListDirBegin(true, true);
        string next;
        do
        {
          next = directory.GetNext();
          if (!next.IsNullOrEmpty())
          {
            if (directory.CurrentIsDir())
            {
              string file = GDUtil.FindFile(filename, folder + next);
              if (file != null)
                return file;
            }
          }
          else
            goto label_12;
        }
        while (!(next == filename));
        return folder + next;
label_12:
        return (string) null;
      }
    }

    public static string GetFileNameFromPath(string path, bool removeExtension = false)
    {
      string fileNameFromPath = path;
      if (path.Contains("/"))
      {
        string str = path;
        int startIndex = path.LastIndexOf("/") + 1;
        fileNameFromPath = str.Substring(startIndex, str.Length - startIndex);
      }
      if (removeExtension && fileNameFromPath.Contains("."))
        fileNameFromPath = fileNameFromPath.Substring(0, fileNameFromPath.LastIndexOf("."));
      return fileNameFromPath;
    }

    public static void CopyFile(string srcPath, string destPath)
    {
      int num = (int) GDUtil.fileOpDir.Copy(srcPath, destPath);
    }

    public static void DeleteFile(string filePath)
    {
      int num = (int) GDUtil.fileOpDir.Remove(filePath);
    }

    public static void ClearCache(string resourcePath)
    {
      ResourceLoader.Load(resourcePath).TakeOverPath("");
      foreach (string dependency in ResourceLoader.GetDependencies(resourcePath))
        GDUtil.ClearCache(dependency);
    }

    public static Color StringToColor(string colorStr)
    {
      if (colorStr.IsNullOrEmpty())
        return Godot.Colors.White;
      if (colorStr.StartsWith("#"))
        return new Color(colorStr);
      if (!colorStr.Contains(","))
        return Color.ColorN(colorStr);
      string[] strArray = colorStr.Split(new string[2]
      {
        ",",
        " "
      }, 4, StringSplitOptions.RemoveEmptyEntries);
      return new Color(float.Parse(strArray[0]), float.Parse(strArray[1]), float.Parse(strArray[2]), strArray.Length == 4 ? float.Parse(strArray[3]) : 1f);
    }

    public static bool TryParseColor(string colorStr, out Color result)
    {
      try
      {
        result = GDUtil.StringToColor(colorStr);
        return true;
      }
      catch (Exception ex)
      {
        result = Godot.Colors.White;
        return false;
      }
    }

    public static Vector2 StringToVector2(string str)
    {
      if (str.IsNullOrEmpty())
        return Vector2.Zero;
      string[] strArray = str.Split(new string[3]
      {
        ",",
        " ",
        "x"
      }, 2, StringSplitOptions.RemoveEmptyEntries);
      return new Vector2(float.Parse(strArray[0]), float.Parse(strArray[1]));
    }

    public static bool TryParseVector2(string str, out Vector2 output)
    {
      if (GDUtil.vector2Regex.IsMatch(str))
      {
        str = str.StripPrefix("(").StripSuffix(")");
        string[] strArray = str.Split(",");
        output = new Vector2(float.Parse(strArray[0]), float.Parse(strArray[1]));
        return true;
      }
      output = Vector2.Zero;
      return false;
    }

    public static T Instance<T>(string nodePath) where T : Node, new()
    {
      return GD.Load<PackedScene>(nodePath).Instance<T>();
    }

    public static Godot.Collections.Array MakeParamsArray(params object[] obj)
    {
      Godot.Collections.Array array = new Godot.Collections.Array();
      foreach (object obj1 in obj)
        array.Add(obj1);
      return array;
    }

    public static T MakeNode<T>(string name) where T : Node, new()
    {
      T obj = new T();
      obj.Name = name;
      return obj;
    }

    public static T MakeResource<T>() where T : Resource, new() => new T();

    public static CollisionShape2D MakeCollisionRect(Vector2 size, Vector2 offset)
    {
      CollisionShape2D collisionShape2D = GDUtil.MakeNode<CollisionShape2D>("CollisionShape");
      collisionShape2D.Shape = (Shape2D) GDUtil.MakeRectangleShape(size);
      collisionShape2D.Position = offset;
      return collisionShape2D;
    }

    public static CollisionShape2D MakeCollisionRect(Vector2 size)
    {
      return GDUtil.MakeCollisionRect(size, Vector2.Zero);
    }

    public static CollisionShape2D MakeCollisionCircle(float radius, Vector2 offset)
    {
      CollisionShape2D collisionShape2D = GDUtil.MakeNode<CollisionShape2D>("CollisionShape");
      collisionShape2D.Shape = (Shape2D) GDUtil.MakeCircleShape(radius);
      collisionShape2D.Position = offset;
      return collisionShape2D;
    }

    public static CollisionShape2D MakeCollisionCircle(float radius)
    {
      return GDUtil.MakeCollisionCircle(radius, Vector2.Zero);
    }

    public static CollisionShape2D MakeCollisionCapsule(Vector2 size, Vector2 offset)
    {
      CollisionShape2D collisionShape2D = GDUtil.MakeNode<CollisionShape2D>("CollisionShape");
      collisionShape2D.Shape = GDUtil.MakeCapsuleShape(size);
      collisionShape2D.Position = offset;
      collisionShape2D.RotationDegrees = (double) size.x > (double) size.y ? 90f : 0.0f;
      return collisionShape2D;
    }

    public static WorldEnvironment MakeWorldEnvironment()
    {
      WorldEnvironment worldEnvironment = GDUtil.MakeNode<WorldEnvironment>("Environment");
      worldEnvironment.Environment = new Godot.Environment();
      worldEnvironment.Environment.BackgroundMode = Godot.Environment.BGMode.Canvas;
      worldEnvironment.Environment.AdjustmentEnabled = true;
      worldEnvironment.Environment.GlowBlendMode = Godot.Environment.GlowBlendModeEnum.Screen;
      worldEnvironment.Environment.GlowBicubicUpscale = true;
      worldEnvironment.Environment.GlowHighQuality = true;
      worldEnvironment.Environment.GlowStrength = 0.7f;
      return worldEnvironment;
    }

    public static async Task DelayOneFrame()
    {
      object[] signal1 = await Game.Root.ToSignal((Godot.Object) Game.Tree, "idle_frame");
      object[] signal2 = await Game.Root.ToSignal((Godot.Object) Game.Tree, "idle_frame");
    }

    public static T ReadSetting<T>(string path)
    {
      try
      {
        return (T) ProjectSettings.GetSetting(path);
      }
      catch
      {
        Log.Error((object) "Failed to load setting: ", (object) path);
        return default (T);
      }
    }

    private static bool PathsAreEqual(string dir1, string dir2)
    {
      dir1 = dir1.ToLower().Trim();
      dir2 = dir2.ToLower().Trim();
      if (!dir1.EndsWith("/"))
        dir1 += "/";
      if (!dir2.EndsWith("/"))
        dir2 += "/";
      return dir1 == dir2;
    }

    private static RectangleShape2D MakeRectangleShape(Vector2 size)
    {
      return new RectangleShape2D() { Extents = size / 2f };
    }

    private static CircleShape2D MakeCircleShape(float radius)
    {
      return new CircleShape2D() { Radius = radius };
    }

    private static Shape2D MakeCapsuleShape(Vector2 size)
    {
      if ((double) size.x == (double) size.y)
        return (Shape2D) new CircleShape2D()
        {
          Radius = (size.x / 2f)
        };
      if ((double) size.x > (double) size.y)
        return (Shape2D) new CapsuleShape2D()
        {
          Height = (float) (((double) size.x / 2.0 - (double) size.y / 2.0) * 2.0),
          Radius = (size.y / 2f)
        };
      return (Shape2D) new CapsuleShape2D()
      {
        Height = (float) (((double) size.y / 2.0 - (double) size.x / 2.0) * 2.0),
        Radius = (size.x / 2f)
      };
    }
  }
}
