// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1BlackPassGarage
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;
using System;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1BlackPassGarage : GameRoom
  {
    [Inject]
    private IExtraFunctions Functions;
    [Export(PropertyHint.None, "")]
    private ShaderMaterial matShader;
    private PVar varSuccess = (PVar) "ch1.temp_blackpass_closetsearch_success";
    private PVar varAttempts = (PVar) "ch1.temp_blackpass_closetsearch_attempts";
    private PEvent evtFailToFind = (PEvent) "ch1_event_blackpass_garage_knife_fail";
    private const int MAX_ATTEMPTS = 10;
    private const float TIME_TO_FIND_KNIFE = 10f;
    private bool _dizzy;
    private float _elapsed;

    public override void _RoomProcess(float delta)
    {
      if (!this._dizzy)
        return;
      this._elapsed += delta;
      this.matShader.SetShaderParam("levels", (object) (float) ((double) this._elapsed / 13.0 * 10.0 + 10.0));
      this.matShader.SetShaderParam("spread", (object) ((double) this._elapsed / 13.0 * 0.1));
    }

    public void TryCloset()
    {
      this.varAttempts.NewValue = (PVar.PVarSetProxy) ((int) this.varAttempts.Value + 1);
      float num = DrkieUtil.EaseInCubic((float) ((double) (int) this.varAttempts.Value * 6.5 / 100.0));
      Log.Trace((object) "Chance of finding knife: ", (object) num);
      if (DrkieUtil.RollPercent((double) num * 100.0))
      {
        this.varSuccess.NewValue = (PVar.PVarSetProxy) true;
      }
      else
      {
        if (!(this.varAttempts.Value == 10))
          return;
        this.varSuccess.NewValue = (PVar.PVarSetProxy) true;
      }
    }

    public void StartSwirl()
    {
      this._dizzy = true;
      Game.Room.ApplyRoomShader((Material) this.matShader);
      this.Functions.StartTimer(10f, (Action) (() => this.evtFailToFind.Play()), true);
      this.Functions.GetTimer().Visible = false;
    }

    public void StopSwirl() => this._dizzy = false;

    public void LacieCollapse()
    {
      Game.Player.SetPlayerState(CharacterState.InObject);
      Game.Player.SpriteState = "collapse";
    }
  }
}
