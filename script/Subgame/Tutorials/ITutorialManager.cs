// Decompiled with JetBrains decompiler
// Type: LacieEngine.Modules.Tutorials.ITutorialManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Modules.Tutorials
{
  [InjectableInterface(unique = true)]
  public interface ITutorialManager : IModule
  {
    event Action TutorialStarted;

    event Action TutorialEnded;

    void ShowTutorial(string tutorialId, bool animate);

    void ShowTutorial(string tutorialId);

    void HideTutorial();

    bool Exists(string id);

    void LoadResourcesForTutorial(string tutorialId);

    IList<string> GetDependencies(string tutorialId);
  }
}
