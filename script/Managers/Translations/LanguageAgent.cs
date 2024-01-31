// Decompiled with JetBrains decompiler
// Type: LacieEngine.Translations.LanguageAgent
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Translations
{
  public class LanguageAgent : Node
  {
    private Dictionary<string, Resource> overrideCache;

    public LanguageAgent()
    {
      this.Name = nameof (LanguageAgent);
      this.overrideCache = new Dictionary<string, Resource>();
    }

    public void OverrideResource(string path, string takeOverPath)
    {
      Log.Debug((object) "Overriding resource: ", (object) path, (object) " will take over ", (object) takeOverPath);
      GDUtil.ClearCache(takeOverPath);
      this.overrideCache[takeOverPath] = ResourceLoader.Load(path, (string) null, true);
      this.overrideCache[takeOverPath].TakeOverPath(takeOverPath);
    }

    public void ClearOverrides()
    {
      foreach (string key in this.overrideCache.Keys)
        GDUtil.ClearCache(key);
      this.overrideCache.Clear();
    }
  }
}
