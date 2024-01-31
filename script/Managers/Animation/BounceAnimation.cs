using System;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.Animation
{
    // Token: 0x02000138 RID: 312
    public class BounceAnimation : LacieAnimation
    {
        // Token: 0x06000B19 RID: 2841 RVA: 0x0002CA60 File Offset: 0x0002AC60
        public BounceAnimation(Control node, Direction direction, float? amplitude = null, float? frequency = null)
        {
            base.Duration = float.MaxValue;
            this._node = node;
            base.Node = node;
            this._direction = direction.ToVector();
            this._pAmplitude = amplitude;
            this._pFrequency = frequency ?? 0.5f;
        }

        // Token: 0x06000B1A RID: 2842 RVA: 0x0002CAC4 File Offset: 0x0002ACC4
        public override void InitialCalculations()
        {
            this._originalPosition = this._node.RectPosition;
            this._amplitude = (double)(this._pAmplitude ?? (this._node.RectSize.Length() / 2f));
            this._frequency = 3.141592653589793 * (double)(1f / this._pFrequency);
        }

        // Token: 0x06000B1B RID: 2843 RVA: 0x0002CB38 File Offset: 0x0002AD38
        public override void UpdateToFirstFrame()
        {
        }

        // Token: 0x06000B1C RID: 2844 RVA: 0x0002CB3C File Offset: 0x0002AD3C
        public override void UpdateToCurrentFrame()
        {
            this._node.RectPosition = this._originalPosition + this._direction * (float)Math.Abs(Math.Sin((double)base.Elapsed * this._frequency) * this._amplitude);
        }

        // Token: 0x06000B1D RID: 2845 RVA: 0x0002CB8A File Offset: 0x0002AD8A
        public override void UpdateToFinalFrame()
        {
        }

        // Token: 0x04000950 RID: 2384
        public const float DEFAULT_FREQUENCY = 0.5f;

        // Token: 0x04000951 RID: 2385
        protected Control _node;

        // Token: 0x04000952 RID: 2386
        protected Vector2 _direction;

        // Token: 0x04000953 RID: 2387
        protected Vector2 _originalPosition;

        // Token: 0x04000954 RID: 2388
        protected float? _pAmplitude;

        // Token: 0x04000955 RID: 2389
        protected double _amplitude;

        // Token: 0x04000956 RID: 2390
        protected float _pFrequency;

        // Token: 0x04000957 RID: 2391
        protected double _frequency;
    }
}
