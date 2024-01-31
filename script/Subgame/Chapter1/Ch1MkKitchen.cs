// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1MkKitchen
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1MkKitchen : GameRoom
  {
    [LacieEngine.API.GetNode("Main/cauldron/Cauldron/Liquid")]
    private Sprite nLiquid;
    [LacieEngine.API.GetNode("Main/cauldron/Cauldron/Liquid/MkDarkness")]
    private Node2D nDarkness;
    [LacieEngine.API.GetNode("Background/Cookiejar")]
    private Sprite nCookieJar;
    [LacieEngine.API.GetNode("Background/WallStorage1/Spook")]
    private Sprite nSpook;
    private PVar varStirredPot = (PVar) "ch1.mk_stirred_pot";
    private PVar varTookFoot = (PVar) "ch1.mk_took_foot";
    private PVar varTookEye = (PVar) "ch1.mk_took_eye_kitchen";
    private PVar varSeenSpoonman = (PVar) "ch1.mk_seen_spoonman";
    private PVar varSerious = (PVar) "general.serious";

    public override void _UpdateRoom()
    {
      this.nLiquid.Frame = (bool) this.varTookFoot.Value ? 2 : ((bool) this.varStirredPot.Value ? 1 : 0);
      this.nDarkness.Visible = this.nLiquid.Frame == 1;
      this.nCookieJar.Frame = (bool) this.varTookEye.Value ? 1 : 0;
      if (!(bool) this.varSerious.Value || (bool) this.varSeenSpoonman.Value)
        return;
      this.nSpook.Visible = true;
    }
  }
}
