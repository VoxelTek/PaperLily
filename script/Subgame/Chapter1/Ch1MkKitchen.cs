using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1MkKitchen : GameRoom
	{
		[GetNode("Main/cauldron/Cauldron/Liquid")]
		private Sprite nLiquid;

		[GetNode("Main/cauldron/Cauldron/Liquid/MkDarkness")]
		private Node2D nDarkness;

		[GetNode("Background/Cookiejar")]
		private Sprite nCookieJar;

		[GetNode("Background/WallStorage1/Spook")]
		private Sprite nSpook;

		private PVar varStirredPot = "ch1.mk_stirred_pot";

		private PVar varTookFoot = "ch1.mk_took_foot";

		private PVar varTookEye = "ch1.mk_took_eye_kitchen";

		private PVar varSeenSpoonman = "ch1.mk_seen_spoonman";

		private PVar varSerious = "general.serious";

		public override void _UpdateRoom()
		{
			nLiquid.Frame = (varTookFoot.Value ? 2 : (((bool)varStirredPot.Value) ? 1 : 0));
			nDarkness.Visible = nLiquid.Frame == 1;
			nCookieJar.Frame = (((bool)varTookEye.Value) ? 1 : 0);
			if ((bool)varSerious.Value && !varSeenSpoonman.Value)
			{
				nSpook.Visible = true;
			}
		}
	}
}
