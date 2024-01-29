using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1MkStairway : GameRoom
	{
		[GetNode("Background/WallStairwayPaintings/Painting")]
		private Sprite nMkPainting;

		private PVar varPaintingEyes = "ch1.mk_stressroom_eyes";

		public override void _UpdateRoom()
		{
			if (!varPaintingEyes.Value)
			{
				nMkPainting.Frame = 0;
			}
			else if (varPaintingEyes.Value == 1)
			{
				nMkPainting.Frame = 1;
			}
			else if (varPaintingEyes.Value == 2)
			{
				nMkPainting.Frame = 3;
			}
		}

		public void ShowBothEyes()
		{
			nMkPainting.Frame = 2;
		}
	}
}
