// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.ResolutionSetting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Settings
{
  public class ResolutionSetting : Setting<Vector2>
  {
    private static readonly Vector2[] StandardResolutions = new Vector2[6]
    {
      new Vector2(640f, 360f),
      new Vector2(1280f, 720f),
      new Vector2(1920f, 1080f),
      new Vector2(2560f, 1440f),
      new Vector2(3200f, 1800f),
      new Vector2(3840f, 2160f)
    };
    private Vector2 value;

    public ResolutionSetting()
    {
      this.Name = "system.settings.resolution";
      this.ApplyCurrentResolution();
    }

    public override string ValueLabel()
    {
      Vector2 size = Game.Root.Size;
      return size.x.ToString() + "x" + (object) size.y;
    }

    public override void Decrement()
    {
      Vector2 size = Game.Root.Size;
      this.value = ResolutionSetting.StandardResolutions[0];
      foreach (Vector2 standardResolution in ResolutionSetting.StandardResolutions)
      {
        if (standardResolution >= size)
          break;
        this.value = standardResolution;
      }
    }

    public override void Increment()
    {
      Vector2 size = Game.Root.Size;
      foreach (Vector2 standardResolution in ResolutionSetting.StandardResolutions)
      {
        this.value = standardResolution;
        if (standardResolution > size)
          break;
      }
    }

    public override void Apply()
    {
      Game.Settings.SetFullScreen(false);
      Game.Settings.SetResolution(this.value);
    }

    public void ApplyCurrentResolution()
    {
      this.value = OS.WindowSize;
      Game.Settings.SetFullScreen(OS.WindowFullscreen);
      Game.Settings.SetResolution(OS.WindowSize);
    }
  }
}
