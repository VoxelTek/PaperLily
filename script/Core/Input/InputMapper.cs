// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.InputMapper
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Core
{
  public class InputMapper
  {
    public static readonly uint[] Blacklist = new uint[20]
    {
      16777244U,
      16777245U,
      16777246U,
      16777247U,
      16777248U,
      16777249U,
      16777250U,
      16777251U,
      16777252U,
      16777253U,
      16777254U,
      16777255U,
      16777226U,
      16777260U,
      16777261U,
      16777239U,
      16777262U,
      16777243U,
      16777225U,
      134217728U
    };

    public static InputProfile CurrentProfile { get; private set; }

    public static void SetProfile(string profileName)
    {
      if (!Inputs.Profiles.ContainsKey(Game.Settings.DefaultInputProfile))
        Log.Error((object) "FATAL ERROR: Default input profile is missing! (", (object) profileName, (object) ")");
      else if (!Inputs.Profiles.ContainsKey(profileName))
      {
        Log.Error((object) "Invalid input profile ", (object) profileName, (object) " - reverting to default!");
        Game.Settings.SetInputProfile(Game.Settings.DefaultInputProfile);
        InputMapper.SetProfile(Game.Settings.DefaultInputProfile);
      }
      else
      {
        Log.Info((object) "Switching input profile to ", (object) profileName);
        InputMapper.ClearAllMappings();
        InputProfile profile = Inputs.Profiles[profileName];
        InputMapper.RegisterAllActions(profile);
        if (profile.Type == InputProfile.InputType.Keyboard)
          InputMapper.RegisterAllActions(Inputs.Profiles[Game.Settings.DefaultInputProfileController]);
        else if (profile.Type == InputProfile.InputType.Controller)
          InputMapper.RegisterAllActions(Inputs.Profiles[Game.Settings.DefaultInputProfileKeyboard]);
        InputMapper.RegisterAllActions(Inputs.SystemKeys);
        if (OS.IsDebugBuild())
          InputMapper.RegisterAllActions(Inputs.DebugKeys);
        foreach (string action in InputMap.GetActions())
          InputMap.ActionSetDeadzone(action, Game.Settings.JoystickDeadzone);
        InputMapper.CurrentProfile = profile;
      }
    }

    public static void CloneIntoCustomProfile(string profileName)
    {
      Inputs.Profiles["custom"] = new InputProfile(Inputs.Profiles[profileName])
      {
        Name = "custom",
        Caption = "system.settings.input.profile.custom"
      };
    }

    public static void SaveCustomProfile()
    {
      GDUtil.SaveJsonFile((object) new InputProfileDTO(Inputs.Profiles["custom"])
      {
        Order = (string) null,
        Captions = (List<InputCaptionDTO>) null
      }, "user://bindings.json");
    }

    private static void RegisterAllActions(InputProfile profile)
    {
      foreach (InputProfile.Mapping mapping in profile.Mappings)
        InputMap.ActionAddEvent(mapping.Action, mapping.Event);
    }

    private static void ClearAllMappings()
    {
      foreach (string action in InputMap.GetActions())
        InputMap.ActionEraseEvents(action);
    }
  }
}
