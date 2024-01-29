using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType(icon = "skull")]
	public class Ch1FairyHeadTrigger : Node2D, IEditorArea
	{
		public enum EAttackType
		{
			Ground,
			Lunge
		}

		[Export(PropertyHint.None, "")]
		public AudioStream sfxDeath;

		private CollisionObject2D _collisionNode;

		private CollisionShape2D _shapeNode;

		private bool _animLungeStarted;

		[Export(PropertyHint.None, "")]
		public string Event { get; set; } = "ch1_death_impact";


		[Export(PropertyHint.None, "")]
		public bool Enabled { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public EAttackType AttackType { get; set; }

		[Export(PropertyHint.None, "")]
		public NodePath HeadNode { get; private set; } = new NodePath();


		[Export(PropertyHint.None, "")]
		public NodePath AnimationNode { get; private set; } = new NodePath();


		[Export(PropertyHint.None, "")]
		public Vector2 Area { get; set; } = new Vector2(32f, 32f);


		public override void _EnterTree()
		{
			if (!Engine.EditorHint && !Game.Room.Cutscene)
			{
				_collisionNode = new Area2D();
				_collisionNode.Name = base.Name;
				_collisionNode.CollisionLayer = (Enabled ? 1u : 0u);
				_collisionNode.CollisionMask = 0u;
				AddChild(_collisionNode);
				_shapeNode = GDUtil.MakeCollisionRect(Area, this.GetPixelPerfectOffset());
				_collisionNode.AddChild(_shapeNode);
				_collisionNode.Connect("body_entered", this, "TriggerAttack");
			}
		}

		public override void _Process(float delta)
		{
			if (_animLungeStarted)
			{
				Node2D animationNode = GetNode(AnimationNode) as Node2D;
				animationNode.Position += Vector2.Down * delta * 700f;
				if (animationNode.Position.y >= Game.Player.Node.Position.y)
				{
					_animLungeStarted = false;
					animationNode.Position = animationNode.Position.ReplaceY(Game.Player.Node.Position.y - 1f);
					Game.Audio.PlaySfx(sfxDeath);
					Game.Events.PlayEvent(Event);
				}
			}
		}

		public void UpdateValues()
		{
			if (_collisionNode.IsValid())
			{
				_collisionNode.CollisionLayer = (Enabled ? 1u : 0u);
			}
			if (Engine.EditorHint)
			{
				Update();
			}
		}

		public async void TriggerAttack(Node _)
		{
			Game.InputProcessor = Inputs.Processor.None;
			if (AttackType == EAttackType.Ground)
			{
				(GetNode(HeadNode) as IAnimation2D).Play();
				await DrkieUtil.DelaySeconds(0.4);
				Node2D obj = GetNode(AnimationNode) as Node2D;
				obj.Position = Game.Player.Node.Position;
				obj.Visible = true;
				if (obj is IAnimation2D animation)
				{
					animation.Play();
				}
				await DrkieUtil.DelaySeconds(0.5);
				Game.Audio.PlaySfx(sfxDeath);
				Game.Events.PlayEvent(Event);
			}
			else if (AttackType == EAttackType.Lunge)
			{
				Node2D headNode = GetNode(HeadNode) as Node2D;
				headNode.Visible = false;
				Node2D obj2 = GetNode(AnimationNode) as Node2D;
				obj2.Position = headNode.Position;
				obj2.Visible = true;
				_animLungeStarted = true;
			}
		}

		void IEditorArea.Update()
		{
			Update();
		}
	}
}
