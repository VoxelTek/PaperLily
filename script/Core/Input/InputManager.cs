// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.InputManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using System;

#nullable disable
namespace LacieEngine.Core
{
  [Injectable]
  public class InputManager : Node, IInputManager, IModule
  {
    public event Action InputTypeChanged;

    public void Init()
    {
      this.Name = "InputMonitor";
      Game.Root.AddChild((Node) this);
      int num = (int) Input.Singleton.Connect("joy_connection_changed", (Godot.Object) this, "ConnectionStatusChanged");
    }

    public override void _Input(InputEvent @event)
    {
      if (Game.InputProcessor == Inputs.Processor.None)
        return;
      switch (Inputs.Handle(@event, Inputs.Processor.Any, Inputs.AllSystem))
      {
        case "system_full_screen":
          Game.Settings.SetFullScreen(!Game.Settings.FullScreen);
          break;
        case "system_mute_audio":
          Game.Settings.SetMuteAudio(!Game.Settings.MuteAudio);
          break;
        case "system_screenshot":
          Game.Screen.TakeScreenshot();
          break;
      }
      if (!Game.Settings.InputAutoSwitch)
        return;
      if (InputMapper.CurrentProfile.Type == InputProfile.InputType.Controller && @event is InputEventKey)
      {
        Log.Debug((object) "Keyboard detected, switching to keyboard profile.");
        InputMapper.SetProfile(Game.Settings.DefaultInputProfileKeyboard);
        Action inputTypeChanged = this.InputTypeChanged;
        if (inputTypeChanged == null)
          return;
        inputTypeChanged();
      }
      else
      {
        if (InputMapper.CurrentProfile.Type != InputProfile.InputType.Keyboard || !(@event is InputEventJoypadButton))
          return;
        Log.Debug((object) "Gamepad detected, switching to gamepad profile.");
        InputMapper.SetProfile(Game.Settings.DefaultInputProfileController);
        Action inputTypeChanged = this.InputTypeChanged;
        if (inputTypeChanged == null)
          return;
        inputTypeChanged();
      }
    }

    public void ConnectionStatusChanged(int deviceId, bool connected)
    {
      if (connected || InputMapper.CurrentProfile.Type != InputProfile.InputType.Controller)
        return;
      Log.Info((object) "Controller ", (object) deviceId, (object) " disconnected. Pausing game.");
      if (Game.InputProcessor != Inputs.Processor.Player)
        return;
      Game.Core.PauseGame();
    }
  }
}
