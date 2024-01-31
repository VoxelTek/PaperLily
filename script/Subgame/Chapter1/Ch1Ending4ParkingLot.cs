// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1Ending4ParkingLot
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Nodes;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1Ending4ParkingLot : GameRoom
  {
    [LacieEngine.API.GetNode("Lighting/room_light_red")]
    private RoomLighting nLightingR;
    [LacieEngine.API.GetNode("Lighting/room_light_yellow")]
    private RoomLighting nLightingY;
    [LacieEngine.API.GetNode("Lighting/room_light_green")]
    private RoomLighting nLightingG;
    private int _curLight;

    public void RotateLight()
    {
      switch (this._curLight++)
      {
        case 0:
          this.nLightingR.Apply();
          break;
        case 1:
          this.nLightingY.Apply();
          break;
        default:
          this.nLightingG.Apply();
          this._curLight = 0;
          break;
      }
    }
  }
}
