// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.PAchievement
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

#nullable disable
namespace LacieEngine.Core
{
  public class PAchievement
  {
    public string Id { get; private set; }

    private PAchievement(string achievementId) => this.Id = achievementId;

    public static implicit operator PAchievement(string achievementId)
    {
      return new PAchievement(achievementId);
    }

    public void Unlock() => Game.Achievements.Unlock(this.Id);
  }
}
