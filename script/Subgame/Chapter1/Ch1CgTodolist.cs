// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1CgTodolist
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  public class Ch1CgTodolist : Control, INodeWithInjections
  {
    [LacieEngine.API.GetNode("CrossOut1")]
    private CanvasItem nCrossOut1;
    [LacieEngine.API.GetNode("CrossOut2")]
    private CanvasItem nCrossOut2;
    [LacieEngine.API.GetNode("CrossOut3")]
    private CanvasItem nCrossOut3;
    [LacieEngine.API.GetNode("CrossOut4")]
    private CanvasItem nCrossOut4;
    [LacieEngine.API.GetNode("CrossOut5")]
    private CanvasItem nCrossOut5;
    private PVar varTask1Done = (PVar) "ch1.todolist_task1_done";
    private PVar varTask2Done = (PVar) "ch1.todolist_task2_done";
    private PVar varTask3Done = (PVar) "ch1.todolist_task3_done";
    private PVar varTask4Done = (PVar) "ch1.todolist_task4_done";
    private PVar varTask5Done = (PVar) "ch1.todolist_task5_done";

    public override void _Ready()
    {
      this.InjectNodes();
      this.nCrossOut1.Visible = (bool) this.varTask1Done.Value;
      this.nCrossOut2.Visible = (bool) this.varTask2Done.Value;
      this.nCrossOut3.Visible = (bool) this.varTask3Done.Value;
      this.nCrossOut4.Visible = (bool) this.varTask4Done.Value;
      this.nCrossOut5.Visible = (bool) this.varTask5Done.Value;
    }
  }
}
