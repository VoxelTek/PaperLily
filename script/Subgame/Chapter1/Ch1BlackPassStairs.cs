// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1BlackPassStairs
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1BlackPassStairs : GameRoom
  {
    [LacieEngine.API.GetNode("Background/Item1")]
    private Node2D nItem1Bg;
    [LacieEngine.API.GetNode("Background/Item2")]
    private Node2D nItem2Bg;
    [LacieEngine.API.GetNode("Background/Item3")]
    private Node2D nItem3Bg;
    [LacieEngine.API.GetNode("Other/Events/misc_item_1")]
    private EventTrigger nItem1Evt;
    [LacieEngine.API.GetNode("Other/Events/misc_item_2")]
    private EventTrigger nItem2Evt;
    [LacieEngine.API.GetNode("Other/Events/misc_item_3")]
    private EventTrigger nItem3Evt;
    private PVar varRedItem1 = (PVar) "general.blackpass_red_item_1_took";
    private PVar varRedItem2 = (PVar) "general.blackpass_red_item_2_took";
    private PVar varRedItem3 = (PVar) "general.blackpass_red_item_3_took";

    public override void _UpdateRoom()
    {
      int ownedRedItems = this.GetOwnedRedItems();
      this.nItem1Bg.Visible = ownedRedItems >= 1;
      this.nItem2Bg.Visible = ownedRedItems >= 2;
      this.nItem3Bg.Visible = ownedRedItems >= 3;
      this.nItem1Evt.Enabled = ownedRedItems == 1;
      this.nItem2Evt.Enabled = ownedRedItems == 2;
      this.nItem3Evt.Enabled = ownedRedItems == 3;
    }

    private int GetOwnedRedItems()
    {
      int ownedRedItems = 0;
      if ((bool) this.varRedItem1.Value)
        ++ownedRedItems;
      if ((bool) this.varRedItem2.Value)
        ++ownedRedItems;
      if ((bool) this.varRedItem3.Value)
        ++ownedRedItems;
      return ownedRedItems;
    }
  }
}
