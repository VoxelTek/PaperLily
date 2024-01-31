// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.InventoryMenu
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Items;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.UI
{
  public class InventoryMenu : IMenuEntryList
  {
    private const int MinItemSlots = 15;
    private const int Columns = 5;
    private TitledMenuFrame nFrame;
    private RichTextLabel nDescription;
    private List<InventoryEntry> _entries;

    public IMenuEntryContainer Container { get; set; }

    public IMenuEntryList Parent { get; set; }

    public List<IMenuEntry> Entries { get; private set; }

    public int Selection { get; set; }

    public Action OnBack { get; set; }

    public InventoryMenu(
      IMenuEntryContainer container,
      TitledMenuFrame frame,
      RichTextLabel description)
    {
      this.Container = container;
      this.Entries = new List<IMenuEntry>();
      this.nFrame = frame;
      this.nDescription = description;
      List<IItem> ownedItems = Game.Items.GetOwnedItems();
      this._entries = new List<InventoryEntry>();
      foreach (IItem obj in ownedItems)
      {
        if (!obj.Hidden)
          this._entries.Add(new InventoryEntry(obj.Id, Game.Items.GetOwnedAmount(obj.Id)));
      }
      foreach (InventoryEntry entry in this._entries)
        this.Entries.Add((IMenuEntry) new InventoryMenuEntry(entry, (IMenuEntryList) this));
      int num = Math.Max(15, 5 * (int) Math.Ceiling((double) this.Entries.Count / 5.0));
      for (int count = this.Entries.Count; count < num; ++count)
        this.Entries.Add((IMenuEntry) new InventoryMenuEmptyEntry((IMenuEntryList) this));
    }

    public void HighlightSelection()
    {
      foreach (IInventoryMenuEntry entry in this.Entries)
        entry.Deselect();
      this.nFrame.TitleVisible = false;
      this.nDescription.Text = "";
      if (this.Selection <= -1)
        return;
      ((IInventoryMenuEntry) this.Entries[this.Selection]).Select();
      if (this._entries.Count <= this.Selection)
        return;
      this.nFrame.TitleVisible = true;
      this.nFrame.TitleText = this._entries[this.Selection].Item.Name;
      this.nDescription.Text = this._entries[this.Selection].Item.Description;
    }

    public Control DrawContent()
    {
      CenterContainer centerContainer = GDUtil.MakeNode<CenterContainer>("CenterContainer");
      centerContainer.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
      GridContainer box = GDUtil.MakeNode<GridContainer>("EntryGrid");
      box.Columns = 5;
      box.SetSeparation(10, 10);
      int num1 = 0;
      int num2 = 15;
      foreach (IMenuEntry entry in this.Entries)
      {
        MarginContainer marginContainer = GDUtil.MakeNode<MarginContainer>("Option" + num1++.ToString());
        Control control = entry.DrawEntry();
        marginContainer.AddChild((Node) control);
        box.AddChild((Node) marginContainer);
        --num2;
      }
      centerContainer.AddChild((Node) box);
      return (Control) centerContainer;
    }

    public void HandleInput(InputEvent @event)
    {
      switch (Inputs.Handle(@event, Inputs.Processor.Menu, Inputs.AllUi, true))
      {
        case "input_up":
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
          this.Selection -= 4;
          if (--this.Selection <= -1)
            this.Selection = this.Entries.Count + this.Selection;
          this.HighlightSelection();
          break;
        case "input_down":
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
          this.Selection += 5;
          if (this.Selection >= this.Entries.Count)
            this.Selection %= 5;
          this.HighlightSelection();
          break;
        case "input_left":
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
          if (this.Selection-- % 5 == 0)
            this.Selection += 5;
          this.HighlightSelection();
          break;
        case "input_right":
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
          if (++this.Selection % 5 == 0)
            this.Selection -= 5;
          this.HighlightSelection();
          break;
        case "input_action":
          this.Entries[this.Selection].Accept();
          break;
        case "input_cancel":
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation2.ogg");
          this.Back();
          break;
      }
    }

    public void Back()
    {
      if (this.OnBack != null)
      {
        this.OnBack();
      }
      else
      {
        if (this.Parent == null)
          return;
        this.Parent.Root();
      }
    }
  }
}
