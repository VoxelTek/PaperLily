// Decompiled with JetBrains decompiler
// Type: LacieEngine.Modules.Tutorials.Tutorial
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;

#nullable disable
namespace LacieEngine.Modules.Tutorials
{
  [ExportType]
  [Tool]
  public class Tutorial : Resource
  {
    public string Id { get; internal set; }

    [Export(PropertyHint.None, "")]
    public string Name { get; set; }

    [Export(PropertyHint.None, "")]
    public string Text { get; set; }

    [Export(PropertyHint.None, "")]
    public Texture Image { get; set; }

    [Export(PropertyHint.None, "")]
    public string[] Inputs { get; set; }

    [Export(PropertyHint.None, "")]
    public Tutorial NextPage { get; set; }
  }
}
