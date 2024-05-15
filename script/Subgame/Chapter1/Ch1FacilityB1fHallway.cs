// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1FacilityB1fHallway
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
    [Tool]
    public class Ch1FacilityB1fHallway : Ch1FacilityRoom
    {
        [LacieEngine.API.GetNode("Other/Events/misc_bucket")]
        private IToggleable nBucketEvt;
        [LacieEngine.API.GetNode("Background/Bucket")]
        private Node2D nBucket;
        [LacieEngine.API.GetNode("Other/Navigation")]
        private Navigation2D nNavigation;
        private PVar varBucketSpawned = (PVar)"ch1.facility_had_bucket_on_entry";
        private PVar varBucketTook = (PVar)"ch1.facility_took_bucket";
        private const int PRIMAL_SPAWNS = 3;

        public override void _BeforeFadeIn() => this.TrySpawnPrimal(this.nNavigation, 3);

        public override void _UpdateRoom()
        {
            base._UpdateRoom();
            this.nBucket.Visible = this.nBucketEvt.Enabled = (bool)this.varBucketSpawned.Value && !(bool)this.varBucketTook.Value;
        }

        public override int DecideEnemy()
        {
            return 1;
        }
    }
}
