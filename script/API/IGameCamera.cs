using Godot;

namespace LacieEngine.API
{
	public interface IGameCamera
	{
		Camera2D Node { get; }

		bool Current
		{
			get
			{
				return Node.Current;
			}
			set
			{
				Node.Current = value;
			}
		}

		float ZoomLevel { get; set; }

		void ApplyRoomSettings();

		void Shake(float time, float power);

		void Shake(float time);

		void Shake();

		void TrackNode(Node2D node);

		void TrackPlayer();

		void Unlock();

		void RefreshZoom();
	}
}
