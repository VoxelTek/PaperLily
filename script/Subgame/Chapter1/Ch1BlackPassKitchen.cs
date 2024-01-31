// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1BlackPassKitchen
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1BlackPassKitchen : GameRoom
  {
    [LacieEngine.API.GetNode("Background/RedItem")]
    private Node2D nRedItemSpr;
    [LacieEngine.API.GetNode("Other/Events/misc_reditem")]
    private EventTrigger nRedItemEvent;
    [LacieEngine.API.GetNode("Main/chr_mom")]
    private Node2D nMom;
    [LacieEngine.API.GetNode("Main/MomCuttingCutscene")]
    private IAnimation2D nMomCuttingCutscene;
    [LacieEngine.API.GetNode("Main/MomCutting")]
    private IAnimation2D nMomCuttingLoop;
    private PVar varRedItemSpawned = (PVar) "general.blackpass_red_item_2_spawned";
    private PVar varFinishedQuest = (PVar) "ch1.blackpass_retrieved_knife";

    public override void _UpdateRoom()
    {
      bool flag = (bool) this.varRedItemSpawned.Value;
      this.nRedItemSpr.Visible = flag;
      this.nRedItemEvent.Enabled = flag;
      this.nMom.Visible = !(bool) this.varFinishedQuest.Value;
      this.nMomCuttingLoop.Node.Visible = (bool) this.varFinishedQuest.Value;
    }

    public async void PlayCuttingCutscene()
    {
      Ch1BlackPassKitchen baseNode = this;
      baseNode.nMom.Visible = false;
      baseNode.nMomCuttingCutscene.Node.Visible = true;
      await baseNode.DelaySeconds(1.0);
      baseNode.nMomCuttingCutscene.Play();
    }
  }
}
