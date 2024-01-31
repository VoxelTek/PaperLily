// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1Artist1f
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1Artist1f : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private PackedScene scnLacieFall;
    [Export(PropertyHint.None, "")]
    private PackedScene scnFlowerRed;
    [Export(PropertyHint.None, "")]
    private PackedScene scnFlowerGreen;
    [Export(PropertyHint.None, "")]
    private PackedScene scnFlowerYellow;
    [Export(PropertyHint.None, "")]
    private Texture texKettMad;
    [LacieEngine.API.GetNode("Background")]
    private Node2D nBackground;
    [LacieEngine.API.GetNode("Main/KettMad")]
    private SimpleAnimatedSprite nKettMad;
    [LacieEngine.API.GetNode("Main/KettSleep")]
    private Node2D nKettSleep;
    [LacieEngine.API.GetNode("Main/KettAwake")]
    private Node2D nKettAwake;
    [LacieEngine.API.GetNode("Other/Events/chr_kett_sleep")]
    private EventTrigger nKettSleepEvent;
    [LacieEngine.API.GetNode("Other/Events/chr_kett_awake")]
    private EventTrigger nKettAwakeEvent;
    [LacieEngine.API.GetNode("Other/Navigation")]
    private Navigation2D nNavigation;
    private PVar varKettCalm = (PVar) "ch1.forest_kett_calm_down";
    private PVar varKettChase = (PVar) "ch1.forest_kett_chase";
    private PVar varKettAwake = (PVar) "ch1.forest_kett_awake";
    private PVar varSpawnedRedFlower = (PVar) "ch1.forest_kett_spawned_flower_red";
    private PVar varSpawnedGreenFlower = (PVar) "ch1.forest_kett_spawned_flower_green";
    private PVar varSpawnedYellowFlower = (PVar) "ch1.forest_kett_spawned_flower_yellow";
    private PVar varPositionRedFlower = (PVar) "ch1.forest_kett_pos_flower_red";
    private PVar varPositionGreenFlower = (PVar) "ch1.forest_kett_pos_flower_green";
    private PVar varPositionYellowFlower = (PVar) "ch1.forest_kett_pos_flower_yellow";
    private NPCChaser nKettChasing;
    private Dictionary<string, Node2D> flowers = new Dictionary<string, Node2D>();

    public override void _BeforeFadeIn()
    {
      if ((bool) this.varKettChase.Value)
      {
        this.nKettChasing = new NPCChaser("kett");
        this.nKettChasing.Position = this.GetPoint("entrance");
        this.nKettChasing.MoveSpeedMultiplier = 1.3f;
        this.GetMainLayer().AddChild((Node) this.nKettChasing);
        this.nKettChasing.OverrideTextureForState("stand", this.texKettMad);
        this.nKettChasing.OverrideTextureForState("walk", this.texKettMad);
        this.nKettChasing.SetNavigation(this.nNavigation);
        this.nKettChasing.Caught += new Action(this.PlayerDeath);
      }
      if ((bool) this.varSpawnedRedFlower.Value)
        this.SpawnFlower("red", this.scnFlowerRed);
      if ((bool) this.varSpawnedGreenFlower.Value)
        this.SpawnFlower("green", this.scnFlowerGreen);
      if (!(bool) this.varSpawnedYellowFlower.Value)
        return;
      this.SpawnFlower("yellow", this.scnFlowerYellow);
    }

    public override void _AfterFadeIn()
    {
      if (!this.nKettChasing.IsValid())
        return;
      this.nKettChasing.Chasing = true;
    }

    public override void _UpdateRoom()
    {
      this.nKettAwake.Visible = false;
      this.nKettSleep.Visible = false;
      this.nKettMad.Visible = false;
      this.nKettSleepEvent.Enabled = false;
      this.nKettAwakeEvent.Enabled = false;
      if ((bool) this.varKettChase.Value)
        return;
      if ((bool) this.varKettAwake.Value)
      {
        this.nKettAwake.Visible = true;
        this.nKettAwakeEvent.Enabled = true;
      }
      else
      {
        this.nKettMad.Visible = !(bool) this.varKettCalm.Value;
        this.nKettSleep.Visible = (bool) this.varKettCalm.Value;
        this.nKettSleepEvent.Enabled = true;
      }
    }

    public async Task KettMadTurn()
    {
      Ch1Artist1f baseNode = this;
      baseNode.nKettMad.Playing = false;
      baseNode.nKettMad.AnimationFrames = "0,1,2,3,4,5";
      baseNode.nKettMad.FPS = 10f;
      baseNode.nKettMad.Loop = false;
      baseNode.nKettMad.Playing = true;
      await baseNode.DelaySeconds(0.05);
      Node2D node2D = baseNode.scnLacieFall.Instance<Node2D>();
      node2D.Position = Game.Player.Node.Position;
      baseNode.GetMainLayer().AddChild((Node) node2D);
      Game.Player.Node.Visible = false;
    }

    private void PauseChaser()
    {
      if (!this.nKettChasing.IsValid())
        return;
      this.nKettChasing.Chasing = false;
    }

    public void KettChomp()
    {
      this.nKettMad.Playing = false;
      this.nKettMad.Frame = 6;
    }

    public void KettBloomRed() => this.KettBloom("red", this.scnFlowerRed);

    public void KettBloomGreen() => this.KettBloom("green", this.scnFlowerGreen);

    public void KettBloomYellow() => this.KettBloom("yellow", this.scnFlowerYellow);

    public void RemoveFlowerRed() => this.DeleteFlower("red");

    public void RemoveFlowerGreen() => this.DeleteFlower("green");

    public void RemoveFlowerYellow() => this.DeleteFlower("yellow");

    public async void KettSniff()
    {
      Ch1Artist1f baseNode = this;
      NpcStaticIdleTurnable kett = (NpcStaticIdleTurnable) Game.Room.RegisteredNPCs["kett"];
      Direction dirBeforeAnimation = (Direction) kett.Direction;
      kett.SpriteState = "sniff";
      await baseNode.DelaySeconds(1.5);
      kett.SpriteState = "stand";
      kett.Turn(dirBeforeAnimation);
      kett = (NpcStaticIdleTurnable) null;
      dirBeforeAnimation = (Direction) null;
    }

    public async void KettSnack()
    {
      Ch1Artist1f baseNode = this;
      NpcStaticIdleTurnable kett = (NpcStaticIdleTurnable) Game.Room.RegisteredNPCs["kett"];
      Direction dirBeforeAnimation = (Direction) kett.Direction;
      kett.SpriteState = "snack";
      await baseNode.DelaySeconds(2.0);
      kett.SpriteState = "stand";
      kett.Turn(dirBeforeAnimation);
      kett = (NpcStaticIdleTurnable) null;
      dirBeforeAnimation = (Direction) null;
    }

    public void PlayerDeath() => Game.Events.PlayEvent("ch1_death_impact");

    private async void KettBloom(string color, PackedScene flowerScn)
    {
      Ch1Artist1f baseNode = this;
      NpcStaticIdleTurnable kett = (NpcStaticIdleTurnable) Game.Room.RegisteredNPCs["kett"];
      Direction dirBeforeAnimation = (Direction) kett.Direction;
      kett.SpriteState = "bloom_" + color;
      await baseNode.DelaySeconds(2.5);
      kett.SpriteState = "cut_" + color;
      await baseNode.DelaySeconds(2.5);
      ((PVar) ("ch1.forest_kett_spawned_flower_" + color)).NewValue = (PVar.PVarSetProxy) true;
      ((PVar) ("ch1.forest_kett_pos_flower_" + color)).NewValue = (PVar.PVarSetProxy) string.Join(",", (object) kett.Position.x, (object) kett.Position.y);
      baseNode.SpawnFlower(color, flowerScn);
      kett.SpriteState = "stand";
      kett.Turn(dirBeforeAnimation);
      kett = (NpcStaticIdleTurnable) null;
      dirBeforeAnimation = (Direction) null;
    }

    private void SpawnFlower(string color, PackedScene flowerScn)
    {
      Node2D node2D = flowerScn.Instance<Node2D>();
      PVar pvar = (PVar) ("ch1.forest_kett_pos_flower_" + color);
      node2D.Position = GDUtil.StringToVector2((string) pvar.Value);
      this.nBackground.AddChild((Node) node2D);
      this.flowers[color] = node2D;
    }

    private void DeleteFlower(string color)
    {
      if (!this.flowers.ContainsKey(color))
        return;
      this.flowers[color].DeleteIfValid();
    }
  }
}
