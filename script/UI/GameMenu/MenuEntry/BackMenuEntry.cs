// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.BackMenuEntry
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class BackMenuEntry : IMenuEntry
  {
    private string caption;

    public IMenuEntryList Parent { get; set; }

    public BackMenuEntry(string caption, IMenuEntryList parentMenu)
    {
      this.caption = caption;
      this.Parent = parentMenu;
    }

    public Control DrawEntry()
    {
      MarginContainer marginContainer = new MarginContainer();
      Label label = new Label();
      label.Name = "Label";
      label.Text = this.caption;
      label.Align = Label.AlignEnum.Center;
      label.SetAnchorsPreset(Control.LayoutPreset.Wide);
      label.SetDefaultFontAndColor();
      marginContainer.AddChild((Node) label);
      return (Control) marginContainer;
    }

    public void Accept()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation2.ogg");
      this.Parent.Back();
    }
  }
}
