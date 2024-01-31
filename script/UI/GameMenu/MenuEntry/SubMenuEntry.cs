// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.SubMenuEntry
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.UI
{
  public class SubMenuEntry : SimpleVerticalMenu, IMenuEntry
  {
    private string _caption;

    public SubMenuEntry(string caption, IMenuEntryList parentMenu, IMenuEntryContainer container)
    {
      this._caption = caption;
      this.Parent = parentMenu;
      this.Container = container;
      this.Entries = new List<IMenuEntry>();
    }

    public void Accept()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
      this.Root();
    }

    public Control DrawEntry()
    {
      MarginContainer marginContainer = new MarginContainer();
      Label label = new Label();
      label.Name = "Label";
      label.Text = this._caption;
      label.Align = Label.AlignEnum.Center;
      label.SetAnchorsPreset(Control.LayoutPreset.Wide);
      label.SetDefaultFontAndColor();
      marginContainer.AddChild((Node) label);
      return (Control) marginContainer;
    }

    public void AddEntry(IMenuEntry entry)
    {
      entry.Parent = (IMenuEntryList) this;
      this.Entries.Add(entry);
    }
  }
}
