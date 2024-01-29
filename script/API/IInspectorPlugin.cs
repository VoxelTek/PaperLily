using Godot;

namespace LacieEngine.API
{
	public interface IInspectorPlugin
	{
		void AddCustomControl(Control control);

		Texture GetIcon(string name);
	}
}
