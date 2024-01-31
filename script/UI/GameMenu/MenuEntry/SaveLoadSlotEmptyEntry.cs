// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.SaveLoadSlotEmptyEntry
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class SaveLoadSlotEmptyEntry : SaveLoadSlotMenuEntry
  {
    private TextureRect BgTextureRect;

    public SaveLoadSlotEmptyEntry(
      SaveLoadMenu.Mode mode,
      SaveFileInformation saveInfo,
      IMenuEntryList parentMenu)
      : base(mode, saveInfo, parentMenu)
    {
    }

    public override Control DrawEntry()
    {
      Control control = new Control();
      control.RectMinSize = SaveLoadSlotMenuEntry.SaveSlotSize;
      this.BgTextureRect = GDUtil.MakeNode<TextureRect>("SaveSlotEmptyBg");
      this.BgTextureRect.Expand = true;
      this.BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/save_slot_empty.png");
      this.BgTextureRect.RectMinSize = SaveLoadSlotMenuEntry.SaveSlotSize;
      Label label = GDUtil.MakeNode<Label>("SlotName");
      label.Text = Game.Language.GetFormattedCaption("system.menu.save.slot", this.saveInfo.SlotNum);
      label.Align = Label.AlignEnum.Center;
      label.Valign = Label.VAlign.Center;
      label.SetAnchorsPreset(Control.LayoutPreset.Wide);
      label.SetDefaultFontAndColor();
      control.AddChild((Node) this.BgTextureRect);
      control.AddChild((Node) label);
      return control;
    }

    public override void Deselect()
    {
      this.BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/save_slot_empty.png");
    }

    public override void Select()
    {
      this.BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/save_slot_empty_selected.png");
    }
  }
}
