// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.SceneProvider
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Core
{
  public class SceneProvider : Resource
  {
    [Export(PropertyHint.None, "")]
    public PackedScene FirstScreen { get; private set; }

    [Export(PropertyHint.None, "")]
    public PackedScene TitleScreen { get; private set; }

    [Export(PropertyHint.None, "")]
    public PackedScene CreditsScreen { get; private set; }

    [Export(PropertyHint.None, "")]
    public PackedScene DeathScreen { get; private set; }

    [Export(PropertyHint.None, "")]
    public PackedScene LoadingScreen { get; private set; }

    [Export(PropertyHint.None, "")]
    public PackedScene SavingScreen { get; private set; }
  }
}
