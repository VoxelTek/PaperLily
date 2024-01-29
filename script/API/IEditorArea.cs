using Godot;

namespace LacieEngine.API
{
	public interface IEditorArea
	{
		Vector2 Area { get; set; }

		void Update();
	}
}
