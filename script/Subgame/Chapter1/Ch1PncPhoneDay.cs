// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1PncPhoneDay
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  public class Ch1PncPhoneDay : Ch1PncPhone
  {
    public override void _Ready()
    {
      this.CorrectCombinations = new Dictionary<string, string>();
      this.CorrectCombinations.Add("195224#1", "ch1_event_phone_mother");
      base._Ready();
    }
  }
}
