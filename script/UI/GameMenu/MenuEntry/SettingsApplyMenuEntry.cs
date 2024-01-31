// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.SettingsApplyMenuEntry
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class SettingsApplyMenuEntry : IMenuEntry
  {
    public IMenuEntryList Parent { get; set; }

    public SettingsApplyMenuEntry(IMenuEntryList menu) => this.Parent = menu;

    public void Accept()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      Game.Settings.SaveSettings();
      this.Parent.Back();
    }

    public Control DrawEntry()
    {
      MarginContainer marginContainer = new MarginContainer();
      Label label = new Label();
      label.Name = "Label";
      label.Text = "system.common.done";
      label.Align = Label.AlignEnum.Center;
      label.SetAnchorsPreset(Control.LayoutPreset.Wide);
      label.SetDefaultFontAndColor();
      marginContainer.AddChild((Node) label);
      return (Control) marginContainer;
    }
  }
}
