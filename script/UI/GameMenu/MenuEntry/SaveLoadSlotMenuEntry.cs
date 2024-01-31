// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.SaveLoadSlotMenuEntry
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.UI
{
  public abstract class SaveLoadSlotMenuEntry : IMenuEntry
  {
    protected static readonly Vector2 SaveSlotSize = new Vector2(1095f, 114f) / 3f;
    protected SaveLoadMenu.Mode mode;
    protected SaveFileInformation saveInfo;

    public IMenuEntryList Parent { get; set; }

    public SaveLoadSlotMenuEntry(
      SaveLoadMenu.Mode mode,
      SaveFileInformation saveInfo,
      IMenuEntryList parentMenu)
    {
      this.mode = mode;
      this.saveInfo = saveInfo;
      this.Parent = parentMenu;
    }

    public abstract Control DrawEntry();

    public abstract void Deselect();

    public abstract void Select();

    public void Accept()
    {
      if (this.mode == SaveLoadMenu.Mode.Save)
      {
        if (!this.saveInfo.Empty)
        {
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
          AreYouSureContainer areYouSure = new AreYouSureContainer()
          {
            OnYes = (Action) (() => this.ReallySave())
          };
          areYouSure.OnClose = (Action) (() => areYouSure.Delete());
          areYouSure.Text = "system.menu.save.overwrite.areyousure";
          Game.Screen.AddToLayer(IScreenManager.Layer.Screen, (Node) areYouSure);
        }
        else
          this.ReallySave();
      }
      else if (!this.saveInfo.Empty && !this.saveInfo.Corrupted && (!this.saveInfo.Event.IsNullOrEmpty() && Game.Events.Exists(this.saveInfo.Event) || this.saveInfo.Event.IsNullOrEmpty() && ResourceLoader.Exists("res://resources/nodes/rooms/" + this.saveInfo.Room + ".tscn")))
      {
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
        Game.Core.StartGameFromSave(this.saveInfo.Id);
      }
      else
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
    }

    private void ReallySave()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      GameState.Save(this.saveInfo.Id);
      this.Parent.Back();
    }

    public enum Mode
    {
      Save,
      Load,
    }
  }
}
