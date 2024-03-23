// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.SaveFileInformation
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;
using System.Collections.Generic;

#nullable disable
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

        public static bool CanLoadRetrySave()
        {
            if (!GameState.SaveExists("retrysave"))
                return false;

            var saveFileInformation = GetSaveFileInformation("retrysave");
            if (!saveFileInformation.CanPlay())
                return false;

            foreach (var saveFileName in GDUtil.ListFilesInPath("user://save/", "slot", ".sav", true, false))
                if (GetSaveFileInformation(GDUtil.GetFileNameFromPath(saveFileName, true)).Date > saveFileInformation.Date)
                    return false;

            return true;
        }

        public bool CanPlay()
        {
            return !Empty && !Corrupted && ((!Event.IsNullOrEmpty() && Game.Events.Exists(Event)) || (Event.IsNullOrEmpty() && GDUtil.FileExists("res://resources/nodes/rooms/" + Room + ".tscn")));
        }

        public static SaveFileInformation GetSaveFileInformation(string saveName)
        {
            var info = new SaveFileInformation {
                Id = saveName,
                SlotNum = GetSlotNumFromSaveName(saveName)
            };
            if (!GameState.SaveExists(saveName))
            {
                info.Empty = true;
                return info;
            }
            try
            {
                return FillAsCurrentSave(info);
            }
            catch (Exception ex)
            {
                Log.Error("Save file ", saveName, " is corrupt: ", ex.Message);
                return FillAsCorrupted(info);
            }
        }

        private static SaveFileInformation FillAsCurrentSave(SaveFileInformation info)
        {
            var gameState = GDUtil.ReadJsonFile<GameState>("user://save/" + info.Id + ".sav", Game.Settings.SaveKey);
            if (gameState.Version.IsNullOrEmpty())
                gameState.Version = VER_LEGACY_DEFAULT;
            info.Party = gameState.Party.ToArray();
            info.Location = gameState.LocationStr.IsNullOrEmpty() ? "system.menu.save.location.unknown" : gameState.LocationStr;
            info.Image = gameState.LocationImg.IsNullOrEmpty() ? "res://assets/img/ui/save/unknown.png" : "res://assets/img/ui/save/" + gameState.LocationImg + ".png";
            info.Room = gameState.Room;
            info.Event = gameState.Event;
            info.Date = gameState.Date;
            info.Playtime = TimeSpan.FromMilliseconds(long.Parse(gameState.Playtime)).ToString("hh\\:mm\\:ss");
            return Version.Parse(gameState.Version) < Version.Parse(VER_LEGACY_CUTOFF) ? FillAsLegacySave(info) : info;
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
                return saveName.Replace("slot", "").PadLeft(2, '0');
            switch (saveName)
            {
                case "quicksave":
                    return "QS";
                case "autosave":
                    return "AU";
                default:
                    return "??";
            }
        }
    }
}
