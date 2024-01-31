// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerItemCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Translations;
using LacieEngine.UI;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerItemCommand : StoryPlayerCommand
  {
    private static readonly Vector2 ItemIconSize = new Vector2(40f, 40f);
    [Inject]
    [NonSerialized]
    private IItemManager Items;

    public StoryPlayerItemCommand.ItemOperation Operation { get; set; }

    public string Item { get; set; }

    public string NewName { get; set; }

    public string NewDescription { get; set; }

    public int? Amount { get; set; }

    public bool Silent { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      int amount = this.Amount ?? 1;
      if (this.Operation == StoryPlayerItemCommand.ItemOperation.Add && !this.Silent)
      {
        this.Items.AddItem(this.Item, amount);
        string formattedCaption = Game.Language.GetFormattedCaption("system.storyplayer.item.get", this.Items.Get(this.Item).Name, amount > 1 ? " x" + amount.ToString() : "");
        storyPlayer.UI.HideDialogueBox();
        string icon = this.Items.Get(this.Item).Icon;
        Control itemPopup = StoryPlayerItemCommand.CreateItemPopup(formattedCaption, icon);
        storyPlayer.nItemPopup.AddChild((Node) itemPopup);
        storyPlayer.nItemPopup.Visible = true;
        storyPlayer.Characters.HideAllCharacters();
        storyPlayer.Characters.ShowCharacter((string) null);
        storyPlayer.SetNextBlockContinue();
        storyPlayer.AdvanceDisabled = false;
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_item.ogg");
      }
      else if (this.Operation == StoryPlayerItemCommand.ItemOperation.Add && this.Silent)
      {
        this.Items.AddItem(this.Item, amount);
        storyPlayer.SetNextBlockContinue();
        storyPlayer.Next();
      }
      else if (this.Operation == StoryPlayerItemCommand.ItemOperation.Remove)
      {
        this.Items.RemoveItem(this.Item, amount);
        storyPlayer.SetNextBlockContinue();
        storyPlayer.Next();
      }
      else
      {
        if (this.Operation != StoryPlayerItemCommand.ItemOperation.Rename)
          return;
        this.Items.OverrideItemNameAndDescription(this.Item, this.NewName, this.NewDescription);
        storyPlayer.SetNextBlockContinue();
        storyPlayer.Next();
      }
    }

    public override IList<string> GetDependencies()
    {
      return (IList<string>) new List<string>(1)
      {
        "res://assets/sprite/common/item/" + this.Items.Get(this.Item).Icon + ".png"
      };
    }

    public override void FindCaptions(TranslatableMessages captions)
    {
      if (this.Operation != StoryPlayerItemCommand.ItemOperation.Rename)
        return;
      if (!this.NewName.IsNullOrEmpty())
        captions.Add(this.NewName, this.ItemRenameNameContext());
      if (this.NewDescription.IsNullOrEmpty())
        return;
      captions.Add(this.NewDescription, this.ItemRenameDescriptionContext());
    }

    public override void OverrideCaptions(ICaptionSet captions)
    {
      if (this.Operation != StoryPlayerItemCommand.ItemOperation.Rename)
        return;
      if (!this.NewName.IsNullOrEmpty() && captions.ContainsKey(this.NewName))
        this.NewName = captions.Get(this.NewName, this.ItemRenameNameContext());
      if (this.NewDescription.IsNullOrEmpty() || !captions.ContainsKey(this.NewDescription))
        return;
      this.NewDescription = captions.Get(this.NewDescription, this.ItemRenameDescriptionContext());
    }

    private string ItemRenameNameContext()
    {
      return "events." + this.Event.Id + ".item.rename.name." + this.Item;
    }

    private string ItemRenameDescriptionContext()
    {
      return "events." + this.Event.Id + ".item.rename.desc." + this.Item;
    }

    private static Control CreateItemPopup(string text, string iconPath)
    {
      Frame itemPopup = Game.Settings.UiProvider.MakeDefaultFrame();
      itemPopup.ContentMarginTop = 5;
      itemPopup.ContentMarginBottom = 5;
      itemPopup.ContentMarginLeft -= 5;
      HBoxContainer hboxContainer = new HBoxContainer();
      hboxContainer.SetSeparation(0);
      TextureRect textureRect = new TextureRect();
      textureRect.RectMinSize = StoryPlayerItemCommand.ItemIconSize;
      textureRect.Expand = true;
      textureRect.Texture = GD.Load("res://assets/sprite/common/item/" + iconPath + ".png") as Texture;
      textureRect.Texture.Flags = 4U;
      textureRect.Material = GD.Load<Material>("res://resources/material/pixel_perfect.tres");
      Label label = GDUtil.MakeNode<Label>("Info");
      label.Text = text;
      label.Align = Label.AlignEnum.Center;
      label.SetDefaultFontAndColor();
      hboxContainer.AddChild((Node) textureRect);
      hboxContainer.AddChild((Node) label);
      itemPopup.AddToFrame((Node) hboxContainer);
      return (Control) itemPopup;
    }

    public enum ItemOperation
    {
      Add,
      Remove,
      Rename,
    }
  }
}
