using System;
using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;

namespace LacieEngine.Minigames
{
	public class HpMeter : Control
	{
		[Export(PropertyHint.None, "")]
		public Texture texUi;

		[Export(PropertyHint.None, "")]
		public Texture texHpFull;

		[Export(PropertyHint.None, "")]
		public Texture texHpEmpty;

		private TextureRect[] _hpTextures;

		private int _maxHp;

		public string CharacterName { get; set; }

		public override void _EnterTree()
		{
			if (CharacterName == null)
			{
				string text2 = (CharacterName = Game.State.Party[0]);
			}
			_maxHp = Math.Max(Game.Variables.GetIntVariable("general.maxhp." + CharacterName), 1);
			TextureRect hpUi = GDUtil.MakeNode<TextureRect>("HpUi");
			hpUi.Texture = texUi;
			hpUi.RectPosition = new Vector2(8f, 8f);
			AddChild(hpUi);
			HBoxContainer hpContainer = GDUtil.MakeNode<HBoxContainer>("HpContainer");
			hpContainer.RectPosition = new Vector2(42f, 12f);
			hpContainer.SetSeparation(-8);
			_hpTextures = new TextureRect[_maxHp];
			for (int i = 0; i < _maxHp; i++)
			{
				_hpTextures[i] = GDUtil.MakeNode<TextureRect>("Hp" + i);
				_hpTextures[i].Texture = texHpFull;
				hpContainer.AddChild(_hpTextures[i]);
			}
			UpdateHp();
			hpUi.AddChild(hpContainer);
			Game.Animations.Play(new SlideInTopAnimation(hpUi));
		}

		public void UpdateHp()
		{
			int hp = Game.Variables.GetIntVariable("general.hp." + CharacterName);
			for (int i = 0; i < _maxHp; i++)
			{
				_hpTextures[i].Texture = ((hp > i) ? texHpFull : texHpEmpty);
			}
		}
	}
}
