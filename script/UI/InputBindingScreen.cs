// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.InputBindingScreen
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.UI
{
  public class InputBindingScreen : Control
  {
    private string _action;
    private Action _refreshCallback;

    private InputBindingScreen()
    {
    }

    public InputBindingScreen(string action, string actionCaption, Action refreshCallback)
    {
      this.Name = nameof (InputBindingScreen);
      this._action = action;
      this._refreshCallback = refreshCallback;
      this.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
      string formattedCaption = Game.Language.GetFormattedCaption(Inputs.Profiles[Game.Settings.InputProfile].Type != InputProfile.InputType.Keyboard ? "system.settings.input.pressbutton" : "system.settings.input.presskey", Game.Language.GetCaption(actionCaption));
      ColorRect colorRect = new ColorRect();
      colorRect.Color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
      colorRect.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
      Control infoBox = (Control) UIUtil.CreateInfoBox(formattedCaption);
      CenterContainer centerContainer = new CenterContainer();
      centerContainer.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
      centerContainer.AddChild((Node) infoBox);
      this.AddChild((Node) colorRect);
      this.AddChild((Node) centerContainer);
    }

    public override void _Input(InputEvent @event)
    {
      if (Game.InputProcessor != Inputs.Processor.Binding || !@event.IsPressed())
        return;
      Game.Tree.SetInputAsHandled();
      InputProfile profile = Inputs.Profiles[Game.Settings.InputProfile];
      if (profile.Type == InputProfile.InputType.Keyboard)
      {
        if (!(@event is InputEventKey event1))
          return;
        foreach (KeyList keyList in InputMapper.Blacklist)
        {
          if ((KeyList) event1.Scancode == keyList)
            return;
        }
        Inputs.Profiles["custom"].UnassignMapping(this._action);
        Inputs.Profiles["custom"].UnassignEvent((InputEvent) event1);
        Inputs.Profiles["custom"].AddKeyMapping(this._action, event1.Scancode);
      }
      else if (profile.Type == InputProfile.InputType.Controller)
      {
        switch (@event)
        {
          case InputEventJoypadButton event2:
            Inputs.Profiles["custom"].UnassignMapping(this._action);
            Inputs.Profiles["custom"].UnassignEvent((InputEvent) event2);
            Inputs.Profiles["custom"].AddButtonMapping(this._action, event2.ButtonIndex);
            break;
          case InputEventJoypadMotion event3:
            Inputs.Profiles["custom"].UnassignMapping(this._action);
            Inputs.Profiles["custom"].UnassignEvent((InputEvent) event3);
            Inputs.Profiles["custom"].AddAxisMapping(this._action, event3.Axis, (double) event3.AxisValue > 0.0 ? 1 : -1);
            break;
          case InputEventKey inputEventKey:
            if (inputEventKey.Scancode != 16777217U)
              return;
            this.ReturnToMenu();
            break;
          default:
            return;
        }
      }
      this.ReturnToMenu();
    }

    private void ReturnToMenu()
    {
      Game.InputProcessor = Inputs.Processor.Menu;
      this.Delete();
      this._refreshCallback();
    }
  }
}
