// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1ForestArtist
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1ForestArtist : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private Texture texKettMad;
    [LacieEngine.API.GetNode("Main/MiscPot")]
    private Node2D nPot;
    [LacieEngine.API.GetNode("Other/Events/misc_pot")]
    private EventTrigger nPotEvent;
    private PVar varTookPot = (PVar) "ch1.forest_took_pot";

    public override void _UpdateRoom()
    {
      this.nPotEvent.Enabled = !(bool) this.varTookPot.Value;
      this.nPot.Visible = !(bool) this.varTookPot.Value;
    }

    public void KettMad()
    {
      WalkingCharacter registeredNpC = Game.Room.RegisteredNPCs["kett"] as WalkingCharacter;
      registeredNpC.OverrideTextureForState("stand", this.texKettMad);
      registeredNpC.OverrideTextureForState("walk", this.texKettMad);
    }
  }
}
