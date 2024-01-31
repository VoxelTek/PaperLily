// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.ResolutionScaleSetting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Settings
{
  internal class ResolutionScaleSetting : Setting<int>
  {
    private static readonly Lazy<ResolutionScaleSetting> _lazyInstance = new Lazy<ResolutionScaleSetting>((Func<ResolutionScaleSetting>) (() => new ResolutionScaleSetting()));
    private static readonly int[] Options = new int[6]
    {
      1,
      2,
      3,
      4,
      6,
      8
    };
    private int selection;

    public static ResolutionScaleSetting Instance => ResolutionScaleSetting._lazyInstance.Value;

    private ResolutionScaleSetting()
    {
      this.Name = "system.settings.resolutionscale";
      this.Path = "lacie_engine/video/resolution_scale_pixel";
      this.Value = this.ReadValue();
      this.selection = -1;
      for (int index = 0; index < ResolutionScaleSetting.Options.Length; ++index)
      {
        if (ResolutionScaleSetting.Options[index] == this.Value)
          this.selection = index;
      }
    }

    public override string ValueLabel() => this.Value.ToString() + "x";

    public override void Decrement()
    {
      --this.selection;
      if (this.selection < 0)
        this.selection = ResolutionScaleSetting.Options.Length - 1;
      this.Value = ResolutionScaleSetting.Options[this.selection];
    }

    public override void Increment()
    {
      ++this.selection;
      if (this.selection >= ResolutionScaleSetting.Options.Length)
        this.selection = 0;
      this.Value = ResolutionScaleSetting.Options[this.selection];
    }

    public override void Apply()
    {
      Game.Screen?.RefreshPixelScaling();
      Game.Room?.RefreshPixelScaling();
      Game.Camera?.RefreshZoom();
    }
  }
}
