using Godot;
using LacieEngine.API;

namespace LacieEngine.Modules.Tutorials
{
	[ExportType]
	public class Tutorial : Resource
	{
		public string Id { get; internal set; }

		[Export(PropertyHint.None, "")]
		public string Name { get; set; }

		[Export(PropertyHint.None, "")]
		public string Text { get; set; }

		[Export(PropertyHint.None, "")]
		public Texture Image { get; set; }

		[Export(PropertyHint.None, "")]
		public string[] Inputs { get; set; }

		[Export(PropertyHint.None, "")]
		public Tutorial NextPage { get; set; }
	}
}
