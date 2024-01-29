using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class LabelWithInputIcons : LabelWithImages
	{
		private string _captionKey;

		private string[] _inputs;

		public bool LookupCaption { get; set; } = true;


		public LabelWithInputIcons()
		{
		}

		public LabelWithInputIcons(string captionKey, params string[] inputs)
		{
			SetContent(captionKey, inputs);
		}

		public override void _Ready()
		{
			base._Ready();
			Game.Input.InputTypeChanged += RefreshCaptionAndIcons;
		}

		public override void _ExitTree()
		{
			Game.Input.InputTypeChanged -= RefreshCaptionAndIcons;
		}

		public void SetContent(string captionKey, params string[] inputs)
		{
			_captionKey = captionKey;
			_inputs = inputs ?? new string[0];
			RefreshCaptionAndIcons();
		}

		protected void RefreshCaptionAndIcons()
		{
			string[] inputsWithIcons = new string[_inputs.Length];
			for (int i = 0; i < _inputs.Length; i++)
			{
				inputsWithIcons[i] = Inputs.Caption(_inputs[i]);
			}
			if (LookupCaption)
			{
				base.Text = Game.Language.GetFormattedCaption(_captionKey, inputsWithIcons);
			}
			else
			{
				base.Text = DrkieUtil.FormatString(_captionKey, inputsWithIcons);
			}
		}
	}
}
