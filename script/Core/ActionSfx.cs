using System;
using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
    [Tool]
    [ExportType]
    public class ActionSfx : Node, IAction, IToggleable
    {
        [Export(PropertyHint.None, "")]
        public bool Enabled { get; set; } = true;

        [Export(PropertyHint.None, "")]
        public AudioStream Sfx { get; set; }

        public override void _Ready()
        {
            if (Engine.EditorHint)
            {
                return;
            }
            Game.Room.RegisteredRoomActions[base.Name] = this;
        }

        public void Execute()
        {
            if (!this.Enabled)
            {
                return;
            }
            if (this.Sfx == null)
            {
                return;
            }
            Game.Audio.PlaySfx(this.Sfx);
        }
    }
}