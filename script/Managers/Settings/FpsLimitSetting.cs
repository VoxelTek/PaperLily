// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.FpsLimitSetting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Settings
{
  internal class FpsLimitSetting : Setting<string>
  {
    public const string FPS_AUTO = "auto";
    public const string FPS_VSYNC = "vsync";
    public const string FPS_VSYNC_COMPOSITOR = "vsync-comp";
    public const string FPS_GSYNC = "gsync";
    public const string FPS_UNLOCKED = "unlocked";
    private const int MAX_PHYSICS_FPS = 60;
    private static readonly string[] Options = new string[17]
    {
      "auto",
      "vsync",
      "vsync-comp",
      "gsync",
      "unlocked",
      "30",
      "40",
      "50",
      "60",
      "75",
      "90",
      "100",
      "120",
      "144",
      "155",
      "165",
      "240"
    };
    private static readonly Lazy<FpsLimitSetting> _lazyInstance = new Lazy<FpsLimitSetting>((Func<FpsLimitSetting>) (() => new FpsLimitSetting()));
    private FpsLimitSetting.FpsAgent nFpsAgent;
    private int selection;

    public static FpsLimitSetting Instance => FpsLimitSetting._lazyInstance.Value;

    private FpsLimitSetting()
    {
      this.Name = "system.settings.fpslimit";
      this.Path = "lacie_engine/video/fps_limit";
      this.Value = this.ReadValue();
      this.selection = -1;
      for (int index = 0; index < FpsLimitSetting.Options.Length; ++index)
      {
        if (FpsLimitSetting.Options[index] == this.Value)
          this.selection = index;
      }
    }

    public override string ValueLabel()
    {
      string str;
      switch (this.Value)
      {
        case "auto":
          str = "system.common.auto";
          break;
        case "gsync":
          str = "system.settings.gsync";
          break;
        case "vsync":
          str = "system.settings.vsync";
          break;
        case "vsync-comp":
          str = "system.settings.compvsync";
          break;
        case "unlocked":
          str = "system.settings.fpsunlocked";
          break;
        default:
          str = this.Value;
          break;
      }
      return str;
    }

    public override void Decrement()
    {
      --this.selection;
      if (this.selection < 0)
        this.selection = FpsLimitSetting.Options.Length - 1;
      this.Value = FpsLimitSetting.Options[this.selection];
    }

    public override void Increment()
    {
      ++this.selection;
      if (this.selection >= FpsLimitSetting.Options.Length)
        this.selection = 0;
      this.Value = FpsLimitSetting.Options[this.selection];
    }

    public override void Apply()
    {
      switch (this.Value)
      {
        case "auto":
          OS.VsyncEnabled = false;
          OS.VsyncViaCompositor = false;
          this.EnableFpsAgent();
          break;
        case "gsync":
          OS.VsyncEnabled = true;
          OS.VsyncViaCompositor = false;
          this.EnableFpsAgent();
          break;
        case "vsync":
          this.DisableFpsAgent();
          OS.VsyncEnabled = true;
          OS.VsyncViaCompositor = false;
          Engine.TargetFps = 0;
          Engine.IterationsPerSecond = Math.Min(FpsLimitSetting.GetTargetRefreshRate(), 60);
          break;
        case "vsync-comp":
          this.DisableFpsAgent();
          OS.VsyncEnabled = true;
          OS.VsyncViaCompositor = true;
          Engine.TargetFps = 0;
          Engine.IterationsPerSecond = Math.Min(FpsLimitSetting.GetTargetRefreshRate(), 60);
          break;
        case "unlocked":
          this.DisableFpsAgent();
          OS.VsyncEnabled = false;
          OS.VsyncViaCompositor = false;
          Engine.TargetFps = 0;
          Engine.IterationsPerSecond = 60;
          break;
        default:
          this.DisableFpsAgent();
          OS.VsyncEnabled = false;
          OS.VsyncViaCompositor = false;
          Engine.TargetFps = int.Parse(this.Value);
          Engine.IterationsPerSecond = Math.Min(int.Parse(this.Value), 60);
          break;
      }
    }

    public static int GetTargetRefreshRate()
    {
      return (double) OS.GetScreenRefreshRate() == -1.0 ? 60 : (int) Math.Ceiling((double) OS.GetScreenRefreshRate());
    }

    private void EnableFpsAgent()
    {
      if (this.nFpsAgent.IsValid())
        return;
      this.nFpsAgent = new FpsLimitSetting.FpsAgent();
      this.nFpsAgent.Init();
    }

    private void DisableFpsAgent()
    {
      this.nFpsAgent.DeleteIfValid();
      this.nFpsAgent = (FpsLimitSetting.FpsAgent) null;
    }

    public class FpsAgent : Node
    {
      private const float Tick = 1f;
      private float _elapsed;
      private int _lastValue = -1;

      public void Init()
      {
        this.Name = nameof (FpsAgent);
        Game.Root.AddChild((Node) this);
        this.AdjustFps();
      }

      public override void _Process(float delta)
      {
        this._elapsed += delta;
        if ((double) this._elapsed < 1.0)
          return;
        this._elapsed = 0.0f;
        this.AdjustFps();
      }

      private void AdjustFps()
      {
        int targetRefreshRate = FpsLimitSetting.GetTargetRefreshRate();
        if (FpsLimitSetting.Instance.Value == "gsync")
          targetRefreshRate -= 2;
        if (this._lastValue == targetRefreshRate)
          return;
        Engine.TargetFps = this._lastValue = targetRefreshRate;
        Engine.IterationsPerSecond = Math.Min(targetRefreshRate, 60);
      }
    }
  }
}
