// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.ICharacterManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.API
{
  [InjectableInterface(unique = true)]
  public interface ICharacterManager : IModule
  {
    ICharacter Get(string id);

    bool Exists(string id);

    bool IsMoodValid(string id, string mood);

    void LoadResourcesForCharacter(string characterId);

    void OverrideCharacterName(string characterId, string newName);

    void RemoveOverrideCharacterName(string characterId);

    void OverrideMood(string characterId, string mood, string newMood);

    void RemoveOverrideMoods(string characterId);

    Texture GetMoodTexture(string characterId, string mood);

    IList<string> GetDependencies(string characterId);
  }
}
