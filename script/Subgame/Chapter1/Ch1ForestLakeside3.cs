// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1ForestLakeside3
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1ForestLakeside3 : GameRoom
  {
    [LacieEngine.API.GetNode("Background/HiddenDoor")]
    private SimpleAnimatedSprite nHiddenDoor;
    [LacieEngine.API.GetNode("Background/LogMovable")]
    private Node2D nLog;
    [LacieEngine.API.GetNode("Ground/LogInWater")]
    private Node2D nLogWater;
    [LacieEngine.API.GetNode("Underwater/MiscLogUnderwater")]
    private Node2D nLogWaterShadow;
    [LacieEngine.API.GetNode("Other/Events/misc_log_push")]
    private EventTrigger nPushLogEvt;
    [LacieEngine.API.GetNode("Other/Events/misc_water")]
    private EventTrigger nWaterEvt;
    private PVar varPushedLog = (PVar) "ch1.forest_lakeside_pushed_log";

    public override void _UpdateRoom()
    {
      bool flag = (bool) this.varPushedLog.Value;
      this.nLog.Visible = !flag;
      this.nLogWater.Visible = flag;
      this.nLogWaterShadow.Visible = flag;
      this.nPushLogEvt.Enabled = !flag;
      this.nWaterEvt.Enabled = !flag;
    }

    public void OpenHiddenDoor1() => Game.Player.VisualNode.Modulate = Godot.Colors.Transparent;

    public async void OpenHiddenDoor2()
    {
      Ch1ForestLakeside3 baseNode = this;
      baseNode.nHiddenDoor.Playing = true;
      await baseNode.DelaySeconds(2.0);
      Game.Animations.Play((LacieAnimation) new FadeInAnimation((CanvasItem) Game.Player.VisualNode, 1f));
      await baseNode.DelaySeconds(1.5);
      if (Game.Player.Node is IWalker node)
        node.Walk(0, 32, Direction.Down, IWalker.MoveSpeed.Normal);
      await baseNode.DelaySeconds(1.0);
      baseNode.nHiddenDoor.AnimationFrames = "4,3,2,1,0";
      baseNode.nHiddenDoor.Playing = true;
    }
  }
}
