using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class InputBindingMenuEntry : IMenuEntry
	{
		private string caption;

		private string action;

		private Label captionLabel;

		private LabelWithImages valueLabel;

		public IMenuEntryList Parent { get; set; }

		public InputBindingMenuEntry(IMenuEntryList parentMenu, string caption, string action)
		{
			this.caption = caption;
			this.action = action;
			Parent = parentMenu;
		}

		public void Accept()
		{
			Game.InputProcessor = Inputs.Processor.Binding;
			if (!Game.Settings.InputProfileCustom)
			{
				InputMapper.CloneIntoCustomProfile(Game.Settings.InputProfile);
				Game.Settings.SetInputProfile("custom");
			}
			InputBindingScreen nScreen = new InputBindingScreen(action, caption, delegate
			{
				Parent.Update();
			});
			Game.Screen.AddToLayer(IScreenManager.Layer.Screen, nScreen);
		}

		public Control DrawEntry()
		{
			MarginContainer marginContainer = new MarginContainer();
			marginContainer.SetContainerMarginLeft(10);
			marginContainer.SetContainerMarginRight(10);
			captionLabel = new Label();
			captionLabel.Name = "Label";
			captionLabel.Text = caption;
			captionLabel.Align = Label.AlignEnum.Left;
			captionLabel.SetAnchorsPreset(Control.LayoutPreset.LeftWide);
			captionLabel.SetDefaultFontAndColor();
			valueLabel = GDUtil.MakeNode<LabelWithImages>("VolumeLabel");
			valueLabel.SetRightAlign();
			marginContainer.AddChild(captionLabel);
			marginContainer.AddChild(valueLabel);
			Update();
			return marginContainer;
		}

		public void Update()
		{
			valueLabel.Text = Inputs.Profiles[Game.Settings.InputProfile].GetAllCaptionsForAction(action);
		}
	}
}
