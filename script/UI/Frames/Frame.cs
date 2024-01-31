// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.Frame
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public abstract class Frame : MarginContainer
  {
    public MarginContainer Container { get; protected set; }

    [Export(PropertyHint.None, "")]
    public Vector2 MinimumSize { get; set; } = Vector2.Zero;

    [Export(PropertyHint.None, "")]
    public int ContentMarginLeft { get; set; }

    [Export(PropertyHint.None, "")]
    public int ContentMarginTop { get; set; }

    [Export(PropertyHint.None, "")]
    public int ContentMarginRight { get; set; }

    [Export(PropertyHint.None, "")]
    public int ContentMarginBottom { get; set; }

    public string DecorBgTexture { get; set; }

    public Control.LayoutPreset DecorBgAlignment { get; set; }

    public Frame() => this.Container = GDUtil.MakeNode<MarginContainer>(nameof (Container));

    public void AddToFrame(Node child) => this.Container.AddChild(child);

    public void ClearFrame()
    {
      foreach (Node child in this.Container.GetChildren())
        child.Delete();
    }
  }
}
