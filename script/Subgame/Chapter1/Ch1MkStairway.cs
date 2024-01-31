// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1MkStairway
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1MkStairway : GameRoom
  {
    [LacieEngine.API.GetNode("Background/WallStairwayPaintings/Painting")]
    private Sprite nMkPainting;
    private PVar varPaintingEyes = (PVar) "ch1.mk_stressroom_eyes";

    public override void _UpdateRoom()
    {
      if (!(bool) this.varPaintingEyes.Value)
        this.nMkPainting.Frame = 0;
      else if (this.varPaintingEyes.Value == 1)
      {
        this.nMkPainting.Frame = 1;
      }
      else
      {
        if (!(this.varPaintingEyes.Value == 2))
          return;
        this.nMkPainting.Frame = 3;
      }
    }

    public void ShowBothEyes() => this.nMkPainting.Frame = 2;
  }
}
