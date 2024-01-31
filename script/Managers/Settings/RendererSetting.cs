// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.RendererSetting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Settings
{
  internal class RendererSetting : Setting<string>
  {
    private static readonly string[] Options = new string[2]
    {
      "GLES2",
      "GLES3"
    };
    private string value;
    private bool changed;
    private int selection;

    public override string CaptionLabel()
    {
      OS.VideoDriver currentVideoDriver;
      if (this.changed)
      {
        string str = this.value;
        currentVideoDriver = OS.GetCurrentVideoDriver();
        string upper = currentVideoDriver.ToString().ToUpper();
        if (str != upper)
          return "system.settings.renderer";
      }
      string caption = Game.Language.GetCaption("system.settings.renderer");
      ILanguageManager language = Game.Language;
      string[] strArray = new string[1];
      currentVideoDriver = OS.GetCurrentVideoDriver();
      strArray[0] = currentVideoDriver.ToString().ToUpper();
      string formattedCaption = language.GetFormattedCaption("system.settings.current", strArray);
      return caption + " (" + formattedCaption + ")";
    }

    public RendererSetting()
    {
      this.changed = false;
      this.value = Game.Settings.Renderer;
      for (int index = 0; index < RendererSetting.Options.Length; ++index)
      {
        if (RendererSetting.Options[index] == this.value)
          this.selection = index;
      }
    }

    public override string ValueLabel()
    {
      string key = !(this.value == "GLES2") ? "system.settings.renderer.gles3" : "system.settings.renderer.gles2";
      return this.changed && this.value != OS.GetCurrentVideoDriver().ToString().ToUpper() ? Game.Language.GetCaption(key) + " (" + Game.Language.GetCaption("system.settings.restartrequired") + ")" : key;
    }

    public override void Decrement()
    {
      this.changed = true;
      --this.selection;
      if (this.selection < 0)
        this.selection = RendererSetting.Options.Length - 1;
      this.value = RendererSetting.Options[this.selection];
    }

    public override void Increment()
    {
      this.changed = true;
      ++this.selection;
      if (this.selection >= RendererSetting.Options.Length)
        this.selection = 0;
      this.value = RendererSetting.Options[this.selection];
    }

    public override void Apply() => Game.Settings.SetRenderer(this.value);
  }
}
