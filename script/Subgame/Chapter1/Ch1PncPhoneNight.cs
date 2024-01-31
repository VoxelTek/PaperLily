// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1PncPhoneNight
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  public class Ch1PncPhoneNight : Ch1PncPhone
  {
    public override void _Ready()
    {
      this.CorrectCombinations = new Dictionary<string, string>();
      this.CorrectCombinations.Add("195224#1", "ch1_event_phone_mother_night");
      this.CorrectCombinations.Add("999281#0", "ch1_event_phone_ritual");
      base._Ready();
    }
  }
}
