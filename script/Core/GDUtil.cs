using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using Karambolo.PO;
using Newtonsoft.Json;

namespace LacieEngine.Core
{
	public static class GDUtil
	{
		private static readonly JsonSerializerSettings serializerSettings;

		private static readonly Regex vector2Regex;

		private static readonly Godot.Directory fileOpDir;

		static GDUtil()
		{
			vector2Regex = new Regex("\\(-?\\d+,-?\\d+\\)");
			fileOpDir = new Godot.Directory();
			serializerSettings = new JsonSerializerSettings();
			serializerSettings.NullValueHandling = NullValueHandling.Ignore;
		}

		public static object ReadBinaryFile(string filename, string encryptionKey = null)
		{
			Godot.File file = new Godot.File();
			if (!file.FileExists(filename))
			{
				return null;
			}
			file.Open(filename, Godot.File.ModeFlags.Read);
			using MemoryStream ms = new MemoryStream(file.GetBuffer((long)file.GetLen()));
			byte[] output = ms.ToArray();
			if (!encryptionKey.IsNullOrEmpty())
			{
				output = CryptoUtils.Decrypt(output, encryptionKey);
			}
			using MemoryStream ms2 = new MemoryStream(output);
			return new BinaryFormatter().Deserialize(ms2);
		}

		public static string ReadTextFile(string filename, string encryptionKey = null)
		{
			Godot.File file = new Godot.File();
			if (!file.FileExists(filename))
			{
				return null;
			}
			file.Open(filename, Godot.File.ModeFlags.Read);
			string text = file.GetAsText();
			file.Close();
			if (!encryptionKey.IsNullOrEmpty())
			{
				text = CryptoUtils.Decrypt(text, encryptionKey);
			}
			return text;
		}

		public static List<List<string>> ReadCsvFile(string filename)
		{
			Godot.File file = new Godot.File();
			List<List<string>> list = new List<List<string>>();
			if (!file.FileExists(filename))
			{
				Log.Error("Cannot read CSV file because it doesn't exist: ", filename);
				return null;
			}
			file.Open(filename, Godot.File.ModeFlags.Read);
			while (!file.EofReached())
			{
				list.Add(new List<string>(file.GetCsvLine()));
			}
			file.Close();
			return list;
		}

		public static POParseResult ReadPoFile(string filename)
		{
			POParserSettings pOParserSettings = new POParserSettings();
			pOParserSettings.StringDecodingOptions = new POStringDecodingOptions();
			pOParserSettings.StringDecodingOptions.KeepTranslationStringsPlatformIndependent = true;
			pOParserSettings.StringDecodingOptions.KeepKeyStringsPlatformIndependent = true;
			return new POParser(pOParserSettings).Parse(ReadTextFile(filename));
		}

		public static T ReadJsonFile<T>(string filename, string encryptionKey = null) where T : new()
		{
			string json = ReadTextFile(filename, encryptionKey);
			if (json == null)
			{
				Log.Error("Cannot read JSON file because it doesn't exist: ", filename);
				return default(T);
			}
			return JsonConvert.DeserializeObject<T>(json, serializerSettings);
		}

		public static System.Collections.Generic.Dictionary<string, string> ReadCaptionFile(string translationFile, string lang, string baseLang = null)
		{
			Log.Debug("Reading caption file: ", translationFile);
			List<List<string>> rows = ReadCsvFile(translationFile);
			System.Collections.Generic.Dictionary<string, string> keyValuePairs = new System.Collections.Generic.Dictionary<string, string>();
			if (rows.Count < 2 || rows[0].Count < 2)
			{
				Log.Error("Invalid translation CSV: ", translationFile);
				return keyValuePairs;
			}
			int langRow = -1;
			for (int k = 0; k < rows[0].Count; k++)
			{
				if (rows[0][k] == lang)
				{
					langRow = k;
					break;
				}
			}
			int baseRow = 0;
			if (baseLang != null)
			{
				for (int j = 0; j < rows[0].Count; j++)
				{
					if (rows[0][j] == baseLang)
					{
						baseRow = j;
						break;
					}
				}
			}
			if (langRow == -1)
			{
				return keyValuePairs;
			}
			for (int i = 1; i < rows.Count; i++)
			{
				if (rows[i].Count >= 2)
				{
					keyValuePairs[rows[i][baseRow]] = rows[i][langRow];
				}
			}
			return keyValuePairs;
		}

		public static void SaveTextFile(string content, string filename, string encryptionKey = null)
		{
			if (!encryptionKey.IsNullOrEmpty())
			{
				content = CryptoUtils.Encrypt(content, encryptionKey);
			}
			Godot.File file = new Godot.File();
			file.Open(filename, Godot.File.ModeFlags.Write);
			file.StoreString(content);
			file.Close();
		}

		public static void SaveJsonFile(object content, string filename, string encryptionKey = null, bool prettify = false)
		{
			string serializedContent = JsonConvert.SerializeObject(content, serializerSettings);
			if (prettify)
			{
				serializedContent = JsonPrettify(serializedContent);
			}
			SaveTextFile(serializedContent, filename, encryptionKey);
		}

		public static string JsonPrettify(string json)
		{
			using StringReader stringReader = new StringReader(json);
			using StringWriter stringWriter = new StringWriter();
			JsonTextReader jsonReader = new JsonTextReader(stringReader);
			JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
			jsonTextWriter.Formatting = Formatting.Indented;
			jsonTextWriter.Indentation = 1;
			jsonTextWriter.IndentChar = '\t';
			jsonTextWriter.WriteToken(jsonReader);
			return stringWriter.ToString() + "\n";
		}

		public static bool FileExists(string filename)
		{
			if (!ResourceLoader.Exists(filename))
			{
				return new Godot.File().FileExists(filename);
			}
			return true;
		}

		public static bool FolderExists(string folder)
		{
			folder.Replace("\\", "/");
			if (!folder.EndsWith("/"))
			{
				folder += "/";
			}
			return new Godot.Directory().DirExists(folder);
		}

		public static void EnsureFolderExists(string folder)
		{
			if (!FolderExists(folder))
			{
				new Godot.Directory().MakeDir(folder);
			}
		}

		public static List<string> ListFilesInPath(string folder, string prefix = null, string suffix = null, bool fullPath = true, bool recursive = false)
		{
			return ListContentInPath(folder, fullPath, prefix, suffix, listFiles: true, listDirs: false, recursive);
		}

		public static List<string> ListFilesInPath(string folder, string extension)
		{
			return ListContentInPath(folder, fullPath: true, null, extension, listFiles: true, listDirs: false, recursive: false);
		}

		public static List<string> ListFoldersInPath(string folder, bool fullPath = true)
		{
			return ListContentInPath(folder, fullPath, null, null, listFiles: false, listDirs: true, recursive: false);
		}

		private static List<string> ListContentInPath(string folder, bool fullPath, string prefix, string suffix, bool listFiles, bool listDirs, bool recursive)
		{
			folder = folder.Replace("\\", "/");
			if (!folder.EndsWith("/"))
			{
				folder += "/";
			}
			if (prefix == null)
			{
				prefix = "";
			}
			if (suffix == null)
			{
				suffix = "";
			}
			string regexPt = "^" + prefix + ".*" + suffix + "$";
			string regexPtCompiledResource = "^" + prefix + ".*" + suffix + ".converted.res" + "$";
			string regexPtCompiledAsset = "^" + prefix + ".*" + suffix + ".import" + "$";
			List<string> elements = new List<string>();
			using Godot.Directory directory = new Godot.Directory();
			directory.Open(folder);
			if (!PathsAreEqual(folder, directory.GetCurrentDir()))
			{
				return elements;
			}
			directory.ListDirBegin(skipNavigational: true, skipHidden: true);
			while (true)
			{
				string element = directory.GetNext();
				if (element.IsNullOrEmpty())
				{
					break;
				}
				if ((!directory.CurrentIsDir() || listDirs) && (directory.CurrentIsDir() || listFiles))
				{
					if (Regex.IsMatch(element.ToLower(), regexPt))
					{
						elements.Add(fullPath ? (folder + element) : element);
					}
					else if (Regex.IsMatch(element.ToLower(), regexPtCompiledResource))
					{
						elements.Add(fullPath ? (folder + element.StripSuffix(".converted.res")) : element.StripSuffix(".converted.res"));
					}
					else if (Regex.IsMatch(element.ToLower(), regexPtCompiledAsset))
					{
						elements.Add(fullPath ? (folder + element.StripSuffix(".import")) : element.StripSuffix(".import"));
					}
				}
				if (directory.CurrentIsDir() && recursive)
				{
					elements.AddRange(ListContentInPath(folder + element, fullPath, prefix, suffix, listFiles, listDirs, recursive));
				}
			}
			directory.ListDirEnd();
			return elements;
		}

		public static string FindFile(string filename, string folder)
		{
			folder = folder.Replace("\\", "/");
			if (!folder.EndsWith("/"))
			{
				folder += "/";
			}
			using Godot.Directory directory = new Godot.Directory();
			directory.Open(folder);
			if (!PathsAreEqual(folder, directory.GetCurrentDir()))
			{
				return null;
			}
			directory.ListDirBegin(skipNavigational: true, skipHidden: true);
			while (true)
			{
				string element = directory.GetNext();
				if (element.IsNullOrEmpty())
				{
					break;
				}
				if (directory.CurrentIsDir())
				{
					string recursiveFind = FindFile(filename, folder + element);
					if (recursiveFind != null)
					{
						return recursiveFind;
					}
				}
				else if (element == filename)
				{
					return folder + element;
				}
			}
			return null;
		}

		public static string GetFileNameFromPath(string path, bool removeExtension = false)
		{
			string fileName = path;
			if (path.Contains("/"))
			{
				int num = path.LastIndexOf("/") + 1;
				fileName = path.Substring(num, path.Length - num);
			}
			if (removeExtension && fileName.Contains("."))
			{
				fileName = fileName.Substring(0, fileName.LastIndexOf("."));
			}
			return fileName;
		}

		public static void CopyFile(string srcPath, string destPath)
		{
			fileOpDir.Copy(srcPath, destPath);
		}

		public static void DeleteFile(string filePath)
		{
			fileOpDir.Remove(filePath);
		}

		public static void ClearCache(string resourcePath)
		{
			ResourceLoader.Load(resourcePath).TakeOverPath("");
			string[] dependencies = ResourceLoader.GetDependencies(resourcePath);
			for (int i = 0; i < dependencies.Length; i++)
			{
				ClearCache(dependencies[i]);
			}
		}

		public static Color StringToColor(string colorStr)
		{
			if (colorStr.IsNullOrEmpty())
			{
				return Colors.White;
			}
			if (colorStr.StartsWith("#"))
			{
				return new Color(colorStr);
			}
			if (!colorStr.Contains(","))
			{
				return Color.ColorN(colorStr);
			}
			string[] colors = colorStr.Split(new string[2] { ",", " " }, 4, StringSplitOptions.RemoveEmptyEntries);
			return new Color(float.Parse(colors[0]), float.Parse(colors[1]), float.Parse(colors[2]), (colors.Length == 4) ? float.Parse(colors[3]) : 1f);
		}

		public static bool TryParseColor(string colorStr, out Color result)
		{
			try
			{
				result = StringToColor(colorStr);
				return true;
			}
			catch (Exception)
			{
				result = Colors.White;
				return false;
			}
		}

		public static Vector2 StringToVector2(string str)
		{
			if (str.IsNullOrEmpty())
			{
				return Vector2.Zero;
			}
			string[] vector = str.Split(new string[3] { ",", " ", "x" }, 2, StringSplitOptions.RemoveEmptyEntries);
			return new Vector2(float.Parse(vector[0]), float.Parse(vector[1]));
		}

		public static bool TryParseVector2(string str, out Vector2 output)
		{
			if (vector2Regex.IsMatch(str))
			{
				str = str.StripPrefix("(").StripSuffix(")");
				string[] components = str.Split(",");
				output = new Vector2(float.Parse(components[0]), float.Parse(components[1]));
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
			Godot.Collections.Array arr = new Godot.Collections.Array();
			foreach (object o in obj)
			{
				arr.Add(o);
			}
			return arr;
		}

		public static T MakeNode<T>(string name) where T : Node, new()
		{
			return new T
			{
				Name = name
			};
		}

		public static T MakeResource<T>() where T : Resource, new()
		{
			return new T();
		}

		public static CollisionShape2D MakeCollisionRect(Vector2 size, Vector2 offset)
		{
			CollisionShape2D collisionShape2D = MakeNode<CollisionShape2D>("CollisionShape");
			collisionShape2D.Shape = MakeRectangleShape(size);
			collisionShape2D.Position = offset;
			return collisionShape2D;
		}

		public static CollisionShape2D MakeCollisionRect(Vector2 size)
		{
			return MakeCollisionRect(size, Vector2.Zero);
		}

		public static CollisionShape2D MakeCollisionCircle(float radius, Vector2 offset)
		{
			CollisionShape2D collisionShape2D = MakeNode<CollisionShape2D>("CollisionShape");
			collisionShape2D.Shape = MakeCircleShape(radius);
			collisionShape2D.Position = offset;
			return collisionShape2D;
		}

		public static CollisionShape2D MakeCollisionCircle(float radius)
		{
			return MakeCollisionCircle(radius, Vector2.Zero);
		}

		public static CollisionShape2D MakeCollisionCapsule(Vector2 size, Vector2 offset)
		{
			CollisionShape2D collisionShape2D = MakeNode<CollisionShape2D>("CollisionShape");
			collisionShape2D.Shape = MakeCapsuleShape(size);
			collisionShape2D.Position = offset;
			collisionShape2D.RotationDegrees = ((size.x > size.y) ? 90 : 0);
			return collisionShape2D;
		}

		public static WorldEnvironment MakeWorldEnvironment()
		{
			WorldEnvironment worldEnvironment = MakeNode<WorldEnvironment>("Environment");
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
			await Game.Root.ToSignal(Game.Tree, "idle_frame");
			await Game.Root.ToSignal(Game.Tree, "idle_frame");
		}

		public static T ReadSetting<T>(string path)
		{
			try
			{
				return (T)ProjectSettings.GetSetting(path);
			}
			catch
			{
				Log.Error("Failed to load setting: ", path);
				return default(T);
			}
		}

		private static bool PathsAreEqual(string dir1, string dir2)
		{
			dir1 = dir1.ToLower().Trim();
			dir2 = dir2.ToLower().Trim();
			if (!dir1.EndsWith("/"))
			{
				dir1 += "/";
			}
			if (!dir2.EndsWith("/"))
			{
				dir2 += "/";
			}
			return dir1 == dir2;
		}

		private static RectangleShape2D MakeRectangleShape(Vector2 size)
		{
			return new RectangleShape2D
			{
				Extents = size / 2f
			};
		}

		private static CircleShape2D MakeCircleShape(float radius)
		{
			return new CircleShape2D
			{
				Radius = radius
			};
		}

		private static Shape2D MakeCapsuleShape(Vector2 size)
		{
			if (size.x == size.y)
			{
				return new CircleShape2D
				{
					Radius = size.x / 2f
				};
			}
			if (size.x > size.y)
			{
				return new CapsuleShape2D
				{
					Height = (size.x / 2f - size.y / 2f) * 2f,
					Radius = size.y / 2f
				};
			}
			return new CapsuleShape2D
			{
				Height = (size.y / 2f - size.x / 2f) * 2f,
				Radius = size.x / 2f
			};
		}
	}
}
