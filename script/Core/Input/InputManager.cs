using System;
using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Injectable]
	public class InputManager : Node, IInputManager, IModule
	{
		public event Action InputTypeChanged;

		public void Init()
		{
			base.Name = "InputMonitor";
			Game.Root.AddChild(this);
			Input.Singleton.Connect("joy_connection_changed", this, "ConnectionStatusChanged");
		}

		public override void _Input(InputEvent @event)
		{
			if (Game.InputProcessor == Inputs.Processor.None)
			{
				return;
			}
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
			if (Game.Settings.InputAutoSwitch)
			{
				if (InputMapper.CurrentProfile.Type == InputProfile.InputType.Controller && @event is InputEventKey)
				{
					Log.Debug("Keyboard detected, switching to keyboard profile.");
					InputMapper.SetProfile(Game.Settings.DefaultInputProfileKeyboard);
					this.InputTypeChanged?.Invoke();
				}
				else if (InputMapper.CurrentProfile.Type == InputProfile.InputType.Keyboard && @event is InputEventJoypadButton)
				{
					Log.Debug("Gamepad detected, switching to gamepad profile.");
					InputMapper.SetProfile(Game.Settings.DefaultInputProfileController);
					this.InputTypeChanged?.Invoke();
				}
			}
		}

		public void ConnectionStatusChanged(int deviceId, bool connected)
		{
			if (!connected && InputMapper.CurrentProfile.Type == InputProfile.InputType.Controller)
			{
				Log.Info("Controller ", deviceId, " disconnected. Pausing game.");
				if (Game.InputProcessor == Inputs.Processor.Player)
				{
					Game.Core.PauseGame();
				}
			}
		}
	}
}
