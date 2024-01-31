// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.PaperLily.PaperLilyFunctions
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;
using LacieEngine.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Subgame.PaperLily
{
  public static class PaperLilyFunctions
  {
    public static void RemoveInventory(PVar inventoryVar)
    {
      List<IItem> ownedItems = Game.Items.GetOwnedItems();
      List<string> stringList = new List<string>(ownedItems.Count);
      foreach (IItem obj in ownedItems)
      {
        if (!obj.Persistent)
          stringList.Add(obj.Id + "::" + Game.Items.GetOwnedAmount(obj.Id).ToString());
      }
      string str = JsonConvert.SerializeObject((object) stringList);
      inventoryVar.NewValue = (PVar.PVarSetProxy) str;
      Log.Debug((object) "Storing inventory: ", (object) inventoryVar.Value);
      Game.Items.ClearInventory();
    }

    public static void RestoreInventory(PVar inventoryVar)
    {
      Game.Items.ClearInventory();
      string str1 = (string) inventoryVar.Value;
      Log.Debug((object) "Restoring inventory: ", (object) str1);
      if (str1.IsNullOrEmpty())
        return;
      foreach (string str2 in JsonConvert.DeserializeObject<List<string>>(str1))
      {
        string[] strArray = str2.Split("::");
        Game.Items.AddItem(strArray[0], int.Parse(strArray[1]));
      }
      inventoryVar.NewValue = (PVar.PVarSetProxy) (string) null;
    }
  }
}
