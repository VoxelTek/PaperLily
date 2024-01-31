// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.FpsLabel
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class FpsLabel : Label
  {
    public override void _EnterTree()
    {
      this.Name = "FPS";
      this.SetFontColor(Godot.Colors.White);
      this.SetFontShadowColor(Godot.Colors.Black);
    }

    public override void _Process(float delta)
    {
      this.Text = Engine.GetFramesPerSecond().ToString() + " FPS";
    }
  }
}
