// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerEvent
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerEvent
  {
    public string Id { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public string Room { get; set; }

    public bool ShouldTrackRepeats { get; set; }

    public bool AffectsPersistent { get; set; }

    public StoryPlayerCommand[] Dialogue { get; set; }

    public IEnumerable<string> GetDependencies()
    {
      ISet<string> dependencies = (ISet<string>) new HashSet<string>();
      foreach (StoryPlayerCommand storyPlayerCommand in this.Dialogue)
        dependencies.UnionWith((IEnumerable<string>) storyPlayerCommand.GetDependencies());
      return (IEnumerable<string>) dependencies;
    }
  }
}
