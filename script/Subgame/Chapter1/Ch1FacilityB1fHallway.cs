using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1FacilityB1fHallway : Ch1FacilityRoom
	{
		[GetNode("Other/Events/misc_bucket")]
		private IToggleable nBucketEvt;

		[GetNode("Background/Bucket")]
		private Node2D nBucket;

		[GetNode("Other/Navigation")]
		private Navigation2D nNavigation;

		private PVar varBucketSpawned = "ch1.facility_had_bucket_on_entry";

		private PVar varBucketTook = "ch1.facility_took_bucket";

		private const int PRIMAL_SPAWNS = 3;

		public override void _BeforeFadeIn()
		{
			TrySpawnPrimal(nNavigation, 3);
		}

		public override void _UpdateRoom()
		{
			base._UpdateRoom();
			Node2D node2D = nBucket;
			bool visible = (nBucketEvt.Enabled = (bool)varBucketSpawned.Value && !varBucketTook.Value);
			node2D.Visible = visible;
		}
	}
}
