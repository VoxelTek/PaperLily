using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class ObjectivesMenuEntry : IMenuEntry
	{
		private IObjective _objective;

		public IMenuEntryList Parent { get; set; }

		public ObjectivesMenuEntry(IObjective objective, IMenuEntryList parentMenu)
		{
			Parent = parentMenu;
			_objective = objective;
		}

		public Control DrawEntry()
		{
			MarginContainer marginContainer = GDUtil.MakeNode<MarginContainer>("LabelContainer");
			RichTextLabel label = GDUtil.MakeNode<RichTextLabel>("ObjectiveName");
			label.ScrollActive = false;
			label.BbcodeEnabled = true;
			label.FitContentHeight = true;
			label.SetDefaultFontAndColor();
			label.BbcodeText = "[img=11]res://assets/img/ui/bullet_point.png[/img] " + _objective.Name;
			label.SetAnchorsPreset(Control.LayoutPreset.Wide);
			marginContainer.AddChild(label);
			return marginContainer;
		}
	}
}
