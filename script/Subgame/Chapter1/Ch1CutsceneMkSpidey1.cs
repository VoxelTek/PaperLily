// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1CutsceneMkSpidey1
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1CutsceneMkSpidey1 : GameRoomNested
  {
    [LacieEngine.API.GetNode("Room/Background/SpiderIn")]
    private IAnimation2D nSpiderIn;
    [LacieEngine.API.GetNode("Room/Background/SpiderIdle")]
    private IAnimation2D nSpiderIdle;

    public override void _BeforeFadeIn()
    {
      if (!(this.nSpiderIn.Node is Sprite node))
        return;
      node.Frame = 1;
    }

    public async void SpiderPopup()
    {
      Ch1CutsceneMkSpidey1 baseNode = this;
      baseNode.nSpiderIn.Play();
      await baseNode.DelaySeconds(1.0);
      baseNode.nSpiderIn.Node.Visible = false;
      baseNode.nSpiderIdle.Node.Visible = true;
      baseNode.nSpiderIdle.Play();
    }
  }
}
