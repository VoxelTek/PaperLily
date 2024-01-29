using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1BlackPassGarage : GameRoom
	{
		[Inject]
		private IExtraFunctions Functions;

		[Export(PropertyHint.None, "")]
		private ShaderMaterial matShader;

		private PVar varSuccess = "ch1.temp_blackpass_closetsearch_success";

		private PVar varAttempts = "ch1.temp_blackpass_closetsearch_attempts";

		private PEvent evtFailToFind = "ch1_event_blackpass_garage_knife_fail";

		private const int MAX_ATTEMPTS = 10;

		private const float TIME_TO_FIND_KNIFE = 10f;

		private bool _dizzy;

		private float _elapsed;

		public override void _RoomProcess(float delta)
		{
			if (_dizzy)
			{
				_elapsed += delta;
				matShader.SetShaderParam("levels", _elapsed / 13f * 10f + 10f);
				matShader.SetShaderParam("spread", (double)(_elapsed / 13f) * 0.1);
			}
		}

		public void TryCloset()
		{
			varAttempts.NewValue = (int)varAttempts.Value + 1;
			float chance = DrkieUtil.EaseInCubic((float)(int)varAttempts.Value * 6.5f / 100f);
			Log.Trace("Chance of finding knife: ", chance);
			if (DrkieUtil.RollPercent(chance * 100f))
			{
				varSuccess.NewValue = true;
			}
			else if (varAttempts.Value == 10)
			{
				varSuccess.NewValue = true;
			}
		}

		public void StartSwirl()
		{
			_dizzy = true;
			Game.Room.ApplyRoomShader(matShader);
			Functions.StartTimer(10f, delegate
			{
				evtFailToFind.Play();
			}, destroyTimerOnTimeout: true);
			Functions.GetTimer().Visible = false;
		}

		public void StopSwirl()
		{
			_dizzy = false;
		}

		public void LacieCollapse()
		{
			Game.Player.SetPlayerState(CharacterState.InObject);
			Game.Player.SpriteState = "collapse";
		}
	}
}
