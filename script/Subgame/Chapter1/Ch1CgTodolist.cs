using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Subgame.Chapter1
{
	public class Ch1CgTodolist : Control, INodeWithInjections
	{
		[GetNode("CrossOut1")]
		private CanvasItem nCrossOut1;

		[GetNode("CrossOut2")]
		private CanvasItem nCrossOut2;

		[GetNode("CrossOut3")]
		private CanvasItem nCrossOut3;

		[GetNode("CrossOut4")]
		private CanvasItem nCrossOut4;

		[GetNode("CrossOut5")]
		private CanvasItem nCrossOut5;

		private PVar varTask1Done = "ch1.todolist_task1_done";

		private PVar varTask2Done = "ch1.todolist_task2_done";

		private PVar varTask3Done = "ch1.todolist_task3_done";

		private PVar varTask4Done = "ch1.todolist_task4_done";

		private PVar varTask5Done = "ch1.todolist_task5_done";

		public override void _Ready()
		{
			this.InjectNodes();
			nCrossOut1.Visible = varTask1Done.Value;
			nCrossOut2.Visible = varTask2Done.Value;
			nCrossOut3.Visible = varTask3Done.Value;
			nCrossOut4.Visible = varTask4Done.Value;
			nCrossOut5.Visible = varTask5Done.Value;
		}
	}
}
