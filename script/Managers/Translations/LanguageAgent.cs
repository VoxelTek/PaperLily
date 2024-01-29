using System.Collections.Generic;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.Translations
{
	public class LanguageAgent : Node
	{
		private Dictionary<string, Resource> overrideCache;

		public LanguageAgent()
		{
			base.Name = "LanguageAgent";
			overrideCache = new Dictionary<string, Resource>();
		}

		public void OverrideResource(string path, string takeOverPath)
		{
			Log.Debug("Overriding resource: ", path, " will take over ", takeOverPath);
			GDUtil.ClearCache(takeOverPath);
			overrideCache[takeOverPath] = ResourceLoader.Load(path, null, noCache: true);
			overrideCache[takeOverPath].TakeOverPath(takeOverPath);
		}

		public void ClearOverrides()
		{
			foreach (string key in overrideCache.Keys)
			{
				GDUtil.ClearCache(key);
			}
			overrideCache.Clear();
		}
	}
}
