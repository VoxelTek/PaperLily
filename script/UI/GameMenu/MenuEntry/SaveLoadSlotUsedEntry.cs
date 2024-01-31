// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.SaveLoadSlotUsedEntry
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class SaveLoadSlotUsedEntry : SaveLoadSlotMenuEntry
  {
    private static readonly Vector2 SaveDecorPos = new Vector2(4f, 4f);
    private static readonly Vector2 SaveLabelPos = new Vector2(4f, 1f);
    private static readonly Vector2 SaveRoomLabelPos = new Vector2(50f, 2f);
    private static readonly Vector2 SavePlaytimeLabelPos = new Vector2(50f, 18f);
    private static readonly Vector2 LocationImgPos = new Vector2(290f, 2f);
    private static readonly Color PrimaryColor = new Color("#BD274D");
    private TextureRect nSaveSlotBg;
    private TextureRect nSlotDecor;
    private Label nSlotNum;

    public SaveLoadSlotUsedEntry(
      SaveLoadMenu.Mode mode,
      SaveFileInformation saveInfo,
      IMenuEntryList parentMenu)
      : base(mode, saveInfo, parentMenu)
    {
    }

    public override Control DrawEntry()
    {
      Control control = new Control();
      control.SizeFlagsHorizontal = 0;
      control.SizeFlagsVertical = 0;
      control.RectMinSize = SaveLoadSlotMenuEntry.SaveSlotSize;
      this.nSaveSlotBg = GDUtil.MakeNode<TextureRect>("SaveSlotBg");
      this.nSaveSlotBg.Expand = true;
      this.nSaveSlotBg.Texture = GD.Load<Texture>("res://assets/img/ui/save_slot.png");
      this.nSaveSlotBg.RectMinSize = SaveLoadSlotMenuEntry.SaveSlotSize;
      this.nSlotDecor = GDUtil.MakeNode<TextureRect>("SaveSlotDecor");
      this.nSlotDecor.Expand = true;
      this.nSlotDecor.Texture = GD.Load<Texture>("res://assets/img/ui/save_decor.png");
      this.nSlotDecor.RectMinSize = this.nSlotDecor.Texture.GetSize() / 3f;
      this.nSlotDecor.RectPosition = SaveLoadSlotUsedEntry.SaveDecorPos;
      string path1 = this.saveInfo.Image;
      if (!ResourceLoader.Exists(path1))
        path1 = "res://assets/img/ui/save/unknown.png";
      TextureRect textureRect1 = GDUtil.MakeNode<TextureRect>("LocationImg");
      textureRect1.Expand = true;
      textureRect1.Texture = GD.Load<Texture>(path1);
      textureRect1.RectMinSize = textureRect1.Texture.GetSize() / 3f;
      textureRect1.RectPosition = SaveLoadSlotUsedEntry.LocationImgPos;
      HBoxContainer hboxContainer = GDUtil.MakeNode<HBoxContainer>("Characters");
      hboxContainer.AnchorRight = 0.8f;
      hboxContainer.AnchorBottom = 1f;
      hboxContainer.Alignment = BoxContainer.AlignMode.End;
      hboxContainer.GrowHorizontal = Control.GrowDirection.Begin;
      foreach (string str in this.saveInfo.Party)
      {
        string path2 = "res://assets/img/ui/chr_face/" + str + ".png";
        if (!ResourceLoader.Exists(path2))
          path2 = "res://assets/img/ui/chr_face/unknown.png";
        TextureRect textureRect2 = GDUtil.MakeNode<TextureRect>("Character");
        textureRect2.Expand = true;
        textureRect2.Texture = GD.Load<Texture>(path2);
        textureRect2.Material = GD.Load<Material>("res://resources/material/pixel_perfect.tres");
        textureRect2.RectMinSize = textureRect2.Texture.GetSize() * 0.8f;
        textureRect2.SizeFlagsHorizontal = 4;
        textureRect2.SizeFlagsVertical = 4;
        hboxContainer.AddChild((Node) textureRect2);
      }
      this.nSlotNum = GDUtil.MakeNode<Label>("SlotNum");
      this.nSlotNum.Text = this.saveInfo.SlotNum;
      this.nSlotNum.SetDefaultFontAndColor();
      this.nSlotNum.RectPosition = SaveLoadSlotUsedEntry.SaveLabelPos;
      Label label1 = GDUtil.MakeNode<Label>("RoomName");
      label1.Text = this.saveInfo.Location;
      label1.SetDefaultFontAndColor();
      label1.RectPosition = SaveLoadSlotUsedEntry.SaveRoomLabelPos;
      Label label2 = GDUtil.MakeNode<Label>("Playtime");
      label2.Text = this.saveInfo.Playtime;
      label2.SetDefaultFontAndColor();
      label2.RectPosition = SaveLoadSlotUsedEntry.SavePlaytimeLabelPos;
      control.AddChild((Node) this.nSaveSlotBg);
      control.AddChild((Node) textureRect1);
      control.AddChild((Node) hboxContainer);
      control.AddChild((Node) this.nSlotDecor);
      control.AddChild((Node) this.nSlotNum);
      control.AddChild((Node) label1);
      control.AddChild((Node) label2);
      return control;
    }

    public override void Deselect()
    {
      this.nSaveSlotBg.Texture = GD.Load<Texture>("res://assets/img/ui/save_slot.png");
      this.nSlotDecor.Visible = true;
      this.nSlotNum.SetDefaultFontAndColor();
    }

    public override void Select()
    {
      this.nSaveSlotBg.Texture = GD.Load<Texture>("res://assets/img/ui/save_slot_selected.png");
      this.nSlotDecor.Visible = false;
      this.nSlotNum.SetFontColor(SaveLoadSlotUsedEntry.PrimaryColor);
    }
  }
}
