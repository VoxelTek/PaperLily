// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Translations;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public abstract class StoryPlayerCommand
  {
    [field: NonSerialized]
    public StoryPlayerEvent Event { get; set; }

    public int Index { get; set; }

    public string RawCommand { get; set; }

    public int IndentLv { get; set; } = -1;

    public abstract void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer);

    public virtual void Load()
    {
      foreach (string dependency in (IEnumerable<string>) this.GetDependencies())
        Game.Memory.Cache(dependency);
    }

    public virtual IList<string> GetDependencies() => (IList<string>) Array.Empty<string>();

    public virtual bool Validate(out List<string> messages)
    {
      messages = (List<string>) null;
      return true;
    }

    public virtual void FindCaptions(TranslatableMessages captions)
    {
    }

    public virtual void OverrideCaptions(ICaptionSet captions)
    {
    }
  }
}
