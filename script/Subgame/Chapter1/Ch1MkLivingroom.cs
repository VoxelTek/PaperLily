using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1MkLivingroom : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private AudioStream bgmLongplay;

		[GetNode("Background/WallFireplace/Fire")]
		private Node2D nFire;

		[GetNode("Main/plants_left/MiscPlant4")]
		private Sprite nPlant4;

		private PVar varFireOn = "ch1.mk_fireplace_on";

		private PVar varLongplay = "ch1.mk_gramophone_playing";

		private PVar varPlantsDead = "ch1.mk_plants_dead";

		private PVar varUsedFlower1 = "ch1.mk_took_flowers_livingroom";

		private PVar varUsedFlower2 = "ch1.mk_took_flowers_hallway";

		private PVar varUsedFlower3 = "ch1.mk_took_flowers_bathroom";

		public override void _UpdateRoom()
		{
			nFire.Visible = varFireOn.Value;
			if ((bool)varLongplay.Value)
			{
				Game.Audio.PlayBgm(bgmLongplay, 1f, crossFade: true);
			}
			if ((bool)varPlantsDead.Value)
			{
				nPlant4.Frame = 3;
			}
			else if ((bool)varUsedFlower1.Value || (bool)varUsedFlower2.Value)
			{
				nPlant4.Frame = 1;
			}
			else if ((bool)varUsedFlower3.Value)
			{
				nPlant4.Frame = 2;
			}
			else
			{
				nPlant4.Frame = 0;
			}
		}
	}
}
