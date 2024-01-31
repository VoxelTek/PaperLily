// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.CreditsScreen2
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using LacieEngine.UI;

#nullable disable
namespace LacieEngine.Rooms
{
  public class CreditsScreen2 : SimpleScreen
  {
    public override void _Ready()
    {
      base._Ready();
      this.SetProcessInput(false);
      this.Timers();
    }

    private async void Timers()
    {
      CreditsScreen2 creditsScreen2 = this;
      await creditsScreen2.DelaySeconds(1.3);
      Game.Animations.Play((LacieAnimation) new PanAnimationControl((Control) creditsScreen2, Vector2.Zero, Vector2.Up * (creditsScreen2.RectSize.y - 360f), 20.5f));
      await creditsScreen2.DelaySeconds(25.0);
      creditsScreen2.GoToNextScreen();
    }
  }
}
