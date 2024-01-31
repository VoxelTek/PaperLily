// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.IMinigame
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System.Threading.Tasks;

#nullable disable
namespace LacieEngine.Minigames
{
  public interface IMinigame
  {
    Node Node => (Node) this;

    bool CustomTransition => false;

    void Init();

    void Start()
    {
    }

    void AddUiElements(Control parent)
    {
    }

    Task TransitionIn() => (Task) null;

    Task TransitionOut() => (Task) null;
  }
}
