// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.LoadingScreen
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class LoadingScreen : Control
  {
    private AnimatedTextureRect nLoadingIndicator;

    public bool Instant { get; set; }

    public override void _EnterTree()
    {
      this.nLoadingIndicator = this.GetNode<AnimatedTextureRect>((NodePath) "LoadingIndicatorContainer/LoadingIndicator");
    }

    public override void _Ready() => this.ShowLoadingIndicator();

    public async void ShowLoadingIndicator()
    {
      if (!this.Instant)
        await DrkieUtil.DelaySeconds(0.5);
      if (!this.nLoadingIndicator.IsValid())
        return;
      this.nLoadingIndicator.Visible = true;
      this.nLoadingIndicator.Frame = 0;
      this.nLoadingIndicator.Playing = true;
    }
  }
}
