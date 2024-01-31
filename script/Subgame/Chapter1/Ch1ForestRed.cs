// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1ForestRed
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1ForestRed : GameRoom
  {
    [Inject]
    private IItemManager Items;
    [Export(PropertyHint.None, "")]
    private PackedScene scnLacieFall;
    [LacieEngine.API.GetNode("Main/chr_rune")]
    private NpcStaticIdleTurnable nRune;
    [LacieEngine.API.GetNode("TilesPath1")]
    private Node2D nPath1Tiles;
    [LacieEngine.API.GetNode("BgPath1")]
    private Node2D nPath1Bg;
    [LacieEngine.API.GetNode("TilesUnderwaterPath1")]
    private Node2D nTilesPath1Underwater;
    [LacieEngine.API.GetNode("TilesPath2")]
    private Node2D nPath2Tiles;
    [LacieEngine.API.GetNode("BgPath2")]
    private Node2D nPath2Bg;
    [LacieEngine.API.GetNode("TilesUnderwaterPath2")]
    private Node2D nTilesPath2Underwater;
    [LacieEngine.API.GetNode("TilesPath3")]
    private Node2D nPath3Tiles;
    [LacieEngine.API.GetNode("BgPath3")]
    private Node2D nPath3Bg;
    [LacieEngine.API.GetNode("TilesUnderwaterPath3")]
    private Node2D nTilesPath3Underwater;
    [LacieEngine.API.GetNode("Main/Lamp3")]
    private Node2D nLamp3;
    [LacieEngine.API.GetNode("Main/Lamp4")]
    private Node2D nLamp4;
    [LacieEngine.API.GetNode("Main/Lamp5")]
    private Node2D nLamp5;
    [LacieEngine.API.GetNode("Main/Lamp6")]
    private Node2D nLamp6;
    [LacieEngine.API.GetNode("Main/Lamp7")]
    private Node2D nLamp7;
    [LacieEngine.API.GetNode("Main/Lamp8")]
    private Node2D nLamp8;
    [LacieEngine.API.GetNode("Main/FurnitureBirdbox")]
    private Sprite nBirdbox;
    [LacieEngine.API.GetNode("Main/FurnitureBirdbox/Crows1")]
    private Particles2D nBirdsEscaping1;
    [LacieEngine.API.GetNode("Main/FurnitureBirdbox/Crows2")]
    private Particles2D nBirdsEscaping2;
    [LacieEngine.API.GetNode("Main/FurnitureBirdbox/Crow")]
    private Node2D nSaveCrow;
    [LacieEngine.API.GetNode("Lighting/LightPath1a")]
    private Node2D nLight1a;
    [LacieEngine.API.GetNode("Lighting/LightPath1b")]
    private Node2D nLight1b;
    [LacieEngine.API.GetNode("Lighting/LightPath2")]
    private Node2D nLight2;
    [LacieEngine.API.GetNode("Lighting/LightPath3")]
    private Node2D nLight3;
    private PVar varBirdboxOpen = (PVar) "ch1.forest_red_birdbox_open";
    private PVar varHasRunes = (PVar) "ch1.temp_player_has_runes";
    private PVar varDeathReaction = (PVar) "ch1.temp_rune_death_reaction";
    private PVar varRuneAllTalks = (PVar) "ch1.forest_rune_alltalks";

    public override void _UpdateRoom()
    {
      switch (Game.Variables.GetIntVariable("ch1.forest_red_entrance_stage"))
      {
        case 0:
          this.ShowPath1();
          break;
        case 1:
          this.ShowPath2();
          break;
        case 2:
          this.ShowPath3();
          break;
        case 3:
          this.ShowPath3();
          break;
        case 4:
          this.ShowNoPaths();
          break;
      }
      this.nSaveCrow.Visible = (bool) this.varBirdboxOpen.Value;
      this.nBirdbox.Frame = (bool) this.varBirdboxOpen.Value ? 1 : 0;
      if (!(bool) this.varRuneAllTalks.Value)
        return;
      this.nRune.TurningEnabled = true;
      this.nRune.TurnToDefault();
      this.nRune.TurningEnabled = false;
    }

    public void ShowPath1()
    {
      this.nPath1Tiles.Visible = true;
      this.nPath1Bg.Visible = true;
      this.nTilesPath1Underwater.Visible = true;
      this.nPath2Tiles.Visible = false;
      this.nPath2Bg.Visible = false;
      this.nTilesPath2Underwater.Visible = false;
      this.nLamp5.Visible = false;
      this.nLamp6.Visible = false;
      this.nPath3Tiles.Visible = false;
      this.nPath3Bg.Visible = false;
      this.nTilesPath3Underwater.Visible = false;
      this.nLamp7.Visible = false;
      this.nLamp8.Visible = true;
      this.nLight1a.Visible = true;
      this.nLight1b.Visible = true;
      this.nLight2.Visible = false;
      this.nLight3.Visible = false;
    }

    public void ShowPath2()
    {
      this.nPath1Tiles.Visible = true;
      this.nPath1Bg.Visible = true;
      this.nTilesPath1Underwater.Visible = true;
      this.nPath2Tiles.Visible = true;
      this.nPath2Bg.Visible = true;
      this.nTilesPath2Underwater.Visible = true;
      this.nPath3Tiles.Visible = false;
      this.nPath3Bg.Visible = false;
      this.nTilesPath3Underwater.Visible = false;
      this.nLamp7.Visible = false;
      this.nLamp8.Visible = true;
      this.nLight1a.Visible = true;
      this.nLight1b.Visible = true;
      this.nLight2.Visible = true;
      this.nLight3.Visible = false;
    }

    public void ShowPath3()
    {
      this.nPath1Tiles.Visible = true;
      this.nPath1Bg.Visible = true;
      this.nTilesPath1Underwater.Visible = true;
      this.nPath2Tiles.Visible = true;
      this.nPath2Bg.Visible = true;
      this.nTilesPath2Underwater.Visible = true;
      this.nPath3Tiles.Visible = true;
      this.nPath3Bg.Visible = true;
      this.nTilesPath3Underwater.Visible = true;
      this.nLamp7.Visible = true;
      this.nLamp8.Visible = true;
      this.nLight1a.Visible = true;
      this.nLight1b.Visible = true;
      this.nLight2.Visible = true;
      this.nLight3.Visible = true;
    }

    public void ShowNoPaths()
    {
      this.nPath1Tiles.Visible = false;
      this.nPath1Bg.Visible = false;
      this.nTilesPath1Underwater.Visible = false;
      this.nLamp3.Visible = false;
      this.nLamp4.Visible = false;
      this.nPath2Tiles.Visible = false;
      this.nPath2Bg.Visible = false;
      this.nTilesPath2Underwater.Visible = false;
      this.nLamp5.Visible = false;
      this.nLamp6.Visible = false;
      this.nLamp7.Visible = false;
      this.nLamp8.Visible = false;
      this.nPath3Tiles.Visible = false;
      this.nPath3Bg.Visible = false;
      this.nTilesPath3Underwater.Visible = false;
      this.nLight1a.Visible = false;
      this.nLight1b.Visible = false;
      this.nLight2.Visible = false;
      this.nLight3.Visible = false;
    }

    public async void OpenBirdBox()
    {
      Ch1ForestRed baseNode = this;
      baseNode.nBirdbox.Frame = 1;
      await baseNode.DelaySeconds(0.2);
      baseNode.nBirdsEscaping1.Emitting = true;
      baseNode.nBirdsEscaping2.Emitting = true;
      Node2D node2D = baseNode.scnLacieFall.Instance<Node2D>();
      node2D.Position = Game.Player.Node.Position;
      baseNode.GetMainLayer().AddChild((Node) node2D);
      Game.Player.Node.Visible = false;
    }

    public void UpdateRunesVar()
    {
      this.varHasRunes.NewValue = (PVar.PVarSetProxy) false;
      foreach (IItem ownedItem in this.Items.GetOwnedItems())
      {
        if (((IList<string>) ownedItem.Tags).Contains<string>("rune"))
          this.varHasRunes.NewValue = (PVar.PVarSetProxy) true;
      }
    }

    public void RandomizeDeathReaction()
    {
      this.varDeathReaction.NewValue = (PVar.PVarSetProxy) new Random().Next(0, 5);
    }
  }
}
