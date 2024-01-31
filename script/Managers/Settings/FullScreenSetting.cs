// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.FullScreenSetting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Settings
{
  public class FullScreenSetting : Setting<bool>
  {
    private bool value;

    public FullScreenSetting()
    {
      this.Name = "system.settings.fullscreen";
      this.value = OS.WindowFullscreen;
    }

    public override string ValueLabel()
    {
      return !OS.WindowFullscreen ? "system.common.no" : "system.common.yes";
    }

    public override void Decrement() => this.value = !OS.WindowFullscreen;

    public override void Increment() => this.value = !OS.WindowFullscreen;

    public override void Apply() => Game.Settings.SetFullScreen(this.value);
  }
}
