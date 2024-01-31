// Decompiled with JetBrains decompiler
// Type: LacieEngine.Animation.AnimationManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Animation
{
  [Injectable]
  public class AnimationManager : Node, IAnimationManager, IModule
  {
	private LinkedList<LacieAnimation> _animations;
	private LinkedList<LacieAnimation> _animationQueue;

	public void Init()
	{
	  this.Name = nameof (AnimationManager);
	  this._animations = new LinkedList<LacieAnimation>();
	  this._animationQueue = new LinkedList<LacieAnimation>();
	  Game.Root.AddChild((Node) this);
	}

	public override void _Process(float delta)
	{
	  if (!this._animationQueue.IsEmpty<LacieAnimation>())
	  {
		this._animations.AddRange<LacieAnimation>((IEnumerable<LacieAnimation>) this._animationQueue);
		this._animationQueue.Clear();
	  }
	  this._animations.RemoveAll<LacieAnimation>((Func<LacieAnimation, bool>) (anim => anim.Finished));
	  foreach (LacieAnimation animation in this._animations)
	  {
		if (animation.Playing)
		  animation.Play(delta);
	  }
	}

	public void Play(LacieAnimation animation)
	{
	  this._animationQueue.AddLast(animation);
	  animation.Begin();
	}

	public async void PlayDelayed(LacieAnimation animation, float delay)
	{
	  await DrkieUtil.DelaySeconds((double) delay);
	  this.Play(animation);
	}

	public void StopAnimations(Node node)
	{
	  this.StopAnimationsInList(this._animations, node);
	  this.StopAnimationsInList(this._animationQueue, node);
	}

	public void StopAnimations<T>(Node node) where T : LacieAnimation
	{
	  this.StopAnimationsInList(this._animations, node, typeof (T));
	  this.StopAnimationsInList(this._animationQueue, node, typeof (T));
	}

	private void StopAnimationsInList(LinkedList<LacieAnimation> list, Node node, System.Type type = null)
	{
	  foreach (LacieAnimation lacieAnimation in list)
	  {
		if (lacieAnimation.Node == node && (type == (System.Type) null || lacieAnimation.GetType() == type))
		  lacieAnimation.Finish();
	  }
	}
  }
}
