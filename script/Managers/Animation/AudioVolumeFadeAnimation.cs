using Godot;

namespace LacieEngine.Animation
{
	public class AudioVolumeFadeAnimation : LacieAnimation
	{
		private float _initialVolume;

		private float _finalVolume;

		private AudioStreamPlayer _node;

		private float _track;

		public AudioVolumeFadeAnimation(AudioStreamPlayer node, float duration, float initialVolume, float finalVolume)
		{
			base.Node = (_node = node);
			base.Duration = duration;
			_initialVolume = initialVolume;
			_finalVolume = finalVolume;
		}

		public override void InitialCalculations()
		{
			_track = _finalVolume - _initialVolume;
		}

		public override void UpdateToFirstFrame()
		{
			_node.VolumeDb = GD.Linear2Db(_initialVolume);
		}

		public override void UpdateToCurrentFrame()
		{
			float progress = base.Elapsed / base.Duration;
			_node.VolumeDb = GD.Linear2Db(_initialVolume + _track * progress);
		}

		public override void UpdateToFinalFrame()
		{
			_node.VolumeDb = GD.Linear2Db(_finalVolume);
		}
	}
}
