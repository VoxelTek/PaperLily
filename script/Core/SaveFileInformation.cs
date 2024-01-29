using System;

namespace LacieEngine.Core
{
	public class SaveFileInformation
	{
		private const string VER_LEGACY_DEFAULT = "1.4.0";

		private const string VER_LEGACY_CUTOFF = "1.5.0";

		public string Id { get; private set; }

		public string SlotNum { get; private set; }

		public string[] Party { get; private set; }

		public string Location { get; private set; }

		public string Image { get; private set; }

		public string Room { get; private set; }

		public string Event { get; private set; }

		public DateTime Date { get; private set; }

		public string Playtime { get; private set; }

		public bool Empty { get; private set; }

		public bool Corrupted { get; private set; }

		private SaveFileInformation()
		{
		}

		public static SaveFileInformation GetSaveFileInformation(string saveName)
		{
			SaveFileInformation info = new SaveFileInformation();
			info.Id = saveName;
			info.SlotNum = GetSlotNumFromSaveName(saveName);
			if (!GameState.SaveExists(saveName))
			{
				info.Empty = true;
				return info;
			}
			try
			{
				info = FillAsCurrentSave(info);
				return info;
			}
			catch (Exception e)
			{
				Log.Error("Save file ", saveName, " is corrupt: ", e.Message);
				return FillAsCorrupted(info);
			}
		}

		private static SaveFileInformation FillAsCurrentSave(SaveFileInformation info)
		{
			GameState gs = GDUtil.ReadJsonFile<GameState>("user://save/" + info.Id + ".sav", Game.Settings.SaveKey);
			if (gs.Version.IsNullOrEmpty())
			{
				gs.Version = "1.4.0";
			}
			info.Party = gs.Party.ToArray();
			info.Location = (gs.LocationStr.IsNullOrEmpty() ? "system.menu.save.location.unknown" : gs.LocationStr);
			info.Image = (gs.LocationImg.IsNullOrEmpty() ? "res://assets/img/ui/save/unknown.png" : ("res://assets/img/ui/save/" + gs.LocationImg + ".png"));
			info.Room = gs.Room;
			info.Event = gs.Event;
			info.Date = gs.Date;
			info.Playtime = TimeSpan.FromMilliseconds(long.Parse(gs.Playtime)).ToString("hh\\:mm\\:ss");
			if (Version.Parse(gs.Version) < Version.Parse("1.5.0"))
			{
				return FillAsLegacySave(info);
			}
			return info;
		}

		private static SaveFileInformation FillAsLegacySave(SaveFileInformation info)
		{
			if (!info.Room.StartsWith("Prologue"))
			{
				info.Location = "system.menu.save.obsolete";
				info.Corrupted = true;
			}
			return info;
		}

		private static SaveFileInformation FillAsCorrupted(SaveFileInformation info)
		{
			info.Corrupted = true;
			info.Party = new string[0];
			info.Location = "system.menu.save.corrupted";
			info.Image = "res://assets/img/ui/save/unknown.png";
			info.Date = DateTime.Now;
			info.Playtime = "00:00:00";
			return info;
		}

		private static string GetSlotNumFromSaveName(string saveName)
		{
			if (saveName.StartsWith("slot"))
			{
				return saveName.Replace("slot", "").PadLeft(2, '0');
			}
			if (saveName == "quicksave")
			{
				return "QS";
			}
			if (saveName == "autosave")
			{
				return "AU";
			}
			return "??";
		}
	}
}
