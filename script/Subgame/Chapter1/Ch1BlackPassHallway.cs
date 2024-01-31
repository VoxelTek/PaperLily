// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1BlackPassHallway
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1BlackPassHallway : GameRoom
  {
    [LacieEngine.API.GetNode("Main/LockersRow3/FurnitureLockers6/RedItem")]
    private Node2D nRedItemSpr;
    [LacieEngine.API.GetNode("Other/Events/misc_reditem")]
    private EventTrigger nRedItemEvent;
    [LacieEngine.API.GetNode("Background/Windows")]
    private Node2D nWindow;
    [LacieEngine.API.GetNode("Main/LockersRow4/FurnitureWardrobe")]
    private Sprite nWardrobe;
    [LacieEngine.API.GetNode("Other/Events/misc_wardrobe")]
    private EventTrigger nWardrobeEvent;
    [LacieEngine.API.GetNode("Main/LockersRow3/FurnitureLockers6/LacieLocker")]
    private Sprite nLacieLocker;
    [LacieEngine.API.GetNode("Other/Events/misc_locker_mess")]
    private EventTrigger nLockerMess;
    private PVar varRedItemSpawned = (PVar) "general.blackpass_red_item_1_spawned";
    private PVar varCurtainsClosed = (PVar) "ch1.blackpass_hallway_curtains_closed";
    private PVar varLockerOpen = (PVar) "ch1.blackpass_opened_locker";
    private PVar varVisits = (PVar) "general.blackpass_visits";

    public override void _UpdateRoom()
    {
      this.nRedItemSpr.Visible = (bool) this.varLockerOpen.Value && (bool) this.varRedItemSpawned.Value;
      this.nRedItemEvent.Enabled = (bool) this.varRedItemSpawned.Value;
      this.nWindow.Visible = !(bool) this.varCurtainsClosed.Value;
      if ((int) this.varVisits.Value >= 3)
      {
        this.nWardrobe.Visible = false;
        this.nWardrobeEvent.Enabled = false;
      }
      this.nLacieLocker.Frame = (bool) this.varLockerOpen.Value ? 2 : 0;
      this.nLockerMess.Enabled = (bool) this.varLockerOpen.Value;
    }

    public void OpenLocker() => this.nLacieLocker.Frame = 2;

    public async void OpenWardrobe()
    {
      Ch1BlackPassHallway baseNode = this;
      baseNode.nWardrobe.Frame = 1;
      await baseNode.DelaySeconds(0.5);
      baseNode.nWardrobe.Frame = 2;
    }
  }
}
