using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[ExportType]
	public class ScreenProxyResource : Resource
	{
		[Export(PropertyHint.None, "")]
		public PackedScene Scene { get; private set; }
	}
}
