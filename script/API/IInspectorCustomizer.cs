using Godot;

namespace LacieEngine.API
{
	public interface IInspectorCustomizer
	{
		void _InspectorParseBegin(Object @object, IInspectorPlugin plugin)
		{
		}

		void _InspectorParseCategory(Object @object, string category)
		{
		}

		bool _InspectorParseProperty(Object @object, int type, string path, int hint, string hintText, int usage)
		{
			return false;
		}
	}
}
