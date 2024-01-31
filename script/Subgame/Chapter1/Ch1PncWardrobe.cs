// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1PncWardrobe
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Minigames;
using LacieEngine.UI;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  public class Ch1PncWardrobe : PointAndClick
  {
    [Export(PropertyHint.None, "")]
    public Texture texCostumeDefault;
    [Export(PropertyHint.None, "")]
    public Texture texCostumeIndoors;
    [Export(PropertyHint.None, "")]
    public AnimatedTexture texCostumeDefaultShine;
    [Export(PropertyHint.None, "")]
    public AnimatedTexture texCostumeIndoorsShine;
    [Export(PropertyHint.None, "")]
    public Material matDiamondTransition;
    [LacieEngine.API.GetNode("Targets/snowglobe/anim/Animation")]
    private AnimationPlayer nSnowGlobeAnimation;
    [LacieEngine.API.GetNode("Costume_Overlay")]
    private TextureRect nCostumeOverlay;
    [LacieEngine.API.GetNode("Targets/window_day")]
    private Control nWindowDay;
    [LacieEngine.API.GetNode("Targets/window_evening")]
    private Control nWindowEvening;
    [LacieEngine.API.GetNode("Targets/window_night")]
    private Control nWindowNight;
    [LacieEngine.API.GetNode("Targets/rune")]
    private Control nRune;
    [LacieEngine.API.GetNode("Targets/salt")]
    private Control nSalt;
    [LacieEngine.API.GetNode("Targets/notebook")]
    private Control nNotebook;
    [LacieEngine.API.GetNode("Targets/doorknob")]
    private Control nDoorknob;
    private SimpleChoicesMenuContainer nCostumeMenu;
    private PVar varTookCh1RitualItems = (PVar) "ch1.home_took_wardrobe_items";
    private PEvent evtNoChange = (PEvent) "event_nochange";
    private PEvent evtChangeBounce = (PEvent) "ch1_event_change_indoors";

    public override void _Ready()
    {
      base._Ready();
      if (Game.Variables.GetVariable("general.part_of_day") == "day")
        this.nWindowDay.Visible = true;
      else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
        this.nWindowEvening.Visible = true;
      else if (Game.Variables.GetVariable("general.part_of_day") == "night")
        this.nWindowNight.Visible = true;
      this.nRune.Visible = !(bool) this.varTookCh1RitualItems.Value;
      this.nSalt.Visible = !(bool) this.varTookCh1RitualItems.Value;
      this.nNotebook.Visible = !(bool) this.varTookCh1RitualItems.Value;
      this.nDoorknob.Visible = !(bool) this.varTookCh1RitualItems.Value;
    }

    public void CurtainsOpen()
    {
    }

    public void CurtainsClose()
    {
    }

    public void ShakeSnowglobe() => this.nSnowGlobeAnimation.PlayFirstAnimation();

    public void ChangeCostumeMenu()
    {
      this.nCostumeMenu = new SimpleChoicesMenuContainer(new List<IMenuEntry>()
      {
        (IMenuEntry) new SimpleMenuEntry(Game.Language.GetCaption("system.menu.costume.default"), new Action(this.ChangeToDefaultCostume), (IMenuEntryList) null),
        (IMenuEntry) new SimpleMenuEntry(Game.Language.GetCaption("system.menu.costume.indoors"), new Action(this.ChangeToIndoorsCostume), (IMenuEntryList) null),
        (IMenuEntry) new SimpleMenuEntry(Game.Language.GetCaption("system.menu.costume.cancel"), new Action(this.CloseCostumeMenu), (IMenuEntryList) null)
      });
      this.nCostumeMenu.OnClose = new Action(this.CloseCostumeMenu);
      this.nCostumeMenu.DarkenBackground = true;
      Game.Screen.AddToLayer(IScreenManager.Layer.Minigame, (Node) this.nCostumeMenu);
      Game.InputProcessor = Inputs.Processor.Menu;
    }

    public void CloseCostumeMenu()
    {
      this.nCostumeMenu.DeleteIfValid();
      Game.InputProcessor = Inputs.Processor.Minigame;
    }

    public void ChangeToDefaultCostume()
    {
      this.nCostumeMenu.DeleteIfValid();
      this.ChangeCostume(this.texCostumeDefault, (Texture) this.texCostumeDefaultShine, "pnc_event_respawn");
    }

    public void ChangeToIndoorsCostume()
    {
      this.nCostumeMenu.DeleteIfValid();
      this.ChangeCostume(this.texCostumeIndoors, (Texture) this.texCostumeIndoorsShine, this.evtChangeBounce.Id);
    }

    private async void ChangeCostume(Texture textureCard, Texture textureShine, string evtEnd)
    {
      Ch1PncWardrobe ch1PncWardrobe = this;
      ch1PncWardrobe.nCursor.Visible = false;
      ch1PncWardrobe.nCostumeOverlay.Visible = true;
      ch1PncWardrobe.nCostumeOverlay.Texture = textureCard;
      ch1PncWardrobe.nCostumeOverlay.Material = ch1PncWardrobe.matDiamondTransition.Duplicate() as Material;
      Game.Animations.Play((LacieAnimation) new ShaderProgressAnimation((CanvasItem) ch1PncWardrobe.nCostumeOverlay, 1.2f));
      await DrkieUtil.DelaySeconds(1.3);
      TextureRect control = GDUtil.MakeNode<TextureRect>("Shine");
      control.Texture = textureShine;
      control.SetAnchorsGrowFrom(0.5f, 0.3f);
      ch1PncWardrobe.AddChild((Node) control);
      await DrkieUtil.DelaySeconds(3.0);
      await Game.Screen.FadeToBlack();
      Game.Minigames.End(evtEnd);
    }
  }
}
