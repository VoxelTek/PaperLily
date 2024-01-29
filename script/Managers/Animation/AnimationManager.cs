using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Animation
{
	[Injectable]
	public class AnimationManager : Node, IAnimationManager, IModule
	{
		private LinkedList<LacieAnimation> _animations;

		private LinkedList<LacieAnimation> _animationQueue;

		public void Init()
		{
			base.Name = "AnimationManager";
			_animations = new LinkedList<LacieAnimation>();
			_animationQueue = new LinkedList<LacieAnimation>();
			Game.Root.AddChild(this);
		}

		public override void _Process(float delta)
		{
			if (!_animationQueue.IsEmpty())
			{
				_animations.AddRange(_animationQueue);
				_animationQueue.Clear();
			}
			_animations.RemoveAll((LacieAnimation anim) => anim.Finished);
			foreach (LacieAnimation animation in _animations)
			{
				if (animation.Playing)
				{
					animation.Play(delta);
				}
			}
		}

		public void Play(LacieAnimation animation)
		{
			_animationQueue.AddLast(animation);
			animation.Begin();
		}

		public async void PlayDelayed(LacieAnimation animation, float delay)
		{
			await DrkieUtil.DelaySeconds(delay);
			Play(animation);
		}

		public void StopAnimations(Node node)
		{
			StopAnimationsInList(_animations, node);
			StopAnimationsInList(_animationQueue, node);
		}

		public void StopAnimations<T>(Node node) where T : LacieAnimation
		{
			StopAnimationsInList(_animations, node, typeof(T));
			StopAnimationsInList(_animationQueue, node, typeof(T));
		}

		private void StopAnimationsInList(LinkedList<LacieAnimation> list, Node node, Type type = null)
		{
			foreach (LacieAnimation animation in list)
			{
				if (animation.Node == node && (type == null || animation.GetType() == type))
				{
					animation.Finish();
				}
			}
		}
	}
}
