using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public abstract class SaveLoadSlotMenuEntry : IMenuEntry
	{
		public enum Mode
		{
			Save,
			Load
		}

		protected static readonly Vector2 SaveSlotSize = new Vector2(1095f, 114f) / 3f;

		protected SaveLoadMenu.Mode mode;

		protected SaveFileInformation saveInfo;

		public IMenuEntryList Parent { get; set; }

		public SaveLoadSlotMenuEntry(SaveLoadMenu.Mode mode, SaveFileInformation saveInfo, IMenuEntryList parentMenu)
		{
			this.mode = mode;
			this.saveInfo = saveInfo;
			Parent = parentMenu;
		}

		public abstract Control DrawEntry();

		public abstract void Deselect();

		public abstract void Select();

		public void Accept()
		{
			if (mode == SaveLoadMenu.Mode.Save)
			{
				if (!saveInfo.Empty)
				{
					Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
					AreYouSureContainer areYouSure = new AreYouSureContainer();
					areYouSure.OnYes = delegate
					{
						ReallySave();
					};
					areYouSure.OnClose = delegate
					{
						areYouSure.Delete();
					};
					areYouSure.Text = "system.menu.save.overwrite.areyousure";
					Game.Screen.AddToLayer(IScreenManager.Layer.Screen, areYouSure);
				}
				else
				{
					ReallySave();
				}
			}
			else if (!saveInfo.Empty && !saveInfo.Corrupted && ((!saveInfo.Event.IsNullOrEmpty() && Game.Events.Exists(saveInfo.Event)) || (saveInfo.Event.IsNullOrEmpty() && ResourceLoader.Exists("res://resources/nodes/rooms/" + saveInfo.Room + ".tscn"))))
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
				Game.Core.StartGameFromSave(saveInfo.Id);
			}
			else
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
			}
		}

		private void ReallySave()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			GameState.Save(saveInfo.Id);
			Parent.Back();
		}
	}
}
