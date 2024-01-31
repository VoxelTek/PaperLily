// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.InputTypeSetting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Settings
{
  internal class InputTypeSetting : Setting<string>
  {
    private readonly List<string> Options;
    private string value;
    private int selection;

    public InputTypeSetting()
    {
      this.Name = "system.settings.input.profile";
      this.Options = new List<string>();
      List<InputProfile> inputProfileList = new List<InputProfile>((IEnumerable<InputProfile>) Inputs.Profiles.Values);
      inputProfileList.Sort();
      foreach (InputProfile inputProfile in inputProfileList)
      {
        if (!inputProfile.Name.StartsWith("_"))
          this.Options.Add(inputProfile.Name);
      }
      this.value = Game.Settings.InputProfile;
      for (int index = 0; index < this.Options.Count; ++index)
      {
        if (this.Options[index] == this.value)
          this.selection = index;
      }
    }

    public override string ValueLabel()
    {
      return Game.Language.GetFormattedCaption("system.settings.input.profile.caption", Game.Language.GetCaption(Inputs.Profiles[Game.Settings.InputProfile].Type == InputProfile.InputType.Keyboard ? "system.common.keyboard" : "system.common.controller"), Game.Language.GetCaption(Inputs.Profiles[Game.Settings.InputProfile].Caption));
    }

    public override void Decrement()
    {
      --this.selection;
      if (this.selection < 0)
        this.selection = this.Options.Count - 1;
      this.value = this.Options[this.selection];
    }

    public override void Increment()
    {
      ++this.selection;
      if (this.selection >= this.Options.Count)
        this.selection = 0;
      this.value = this.Options[this.selection];
    }

    public override void Apply() => Game.Settings.SetInputProfile(this.value);
  }
}
