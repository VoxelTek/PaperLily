// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.SteamUpdater
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using Steamworks;

#nullable disable
namespace LacieEngine.Nodes
{
  public class SteamUpdater : Node
  {
	protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

	public SteamUpdater()
	{
	  this.Name = nameof (SteamUpdater);
	  this.PauseMode = Node.PauseModeEnum.Process;
	}

	public override void _Ready()
	{
	  this.m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(this.OnGameOverlayActivated));
	}

	public override void _Process(float delta) => SteamAPI.RunCallbacks();

	public override void _Notification(int what)
	{
	  if (what != 1006)
		return;
	  SteamAPI.Shutdown();
	}

	public void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
	{
	  if (pCallback.m_bActive != (byte) 0)
	  {
		if (InputMapper.CurrentProfile.Type != InputProfile.InputType.Controller || Game.InputProcessor != Inputs.Processor.Player)
		  return;
		Game.Core.PauseGame();
	  }
	  else
		Game.Tree.Paused = false;
	}
  }
}
