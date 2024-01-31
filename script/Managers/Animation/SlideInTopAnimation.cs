using System;
using Godot;

namespace LacieEngine.Animation
{
    // Token: 0x02000146 RID: 326
    public class SlideInTopAnimation : SlideInAnimation
    {
        // Token: 0x06000B60 RID: 2912 RVA: 0x0002D5AE File Offset: 0x0002B7AE
        public SlideInTopAnimation(Control node, float? displacement = null, Vector2? to = null, float duration = 0.15f)
            : base(node, displacement, to, duration)
        {
        }

        // Token: 0x06000B61 RID: 2913 RVA: 0x0002D5BC File Offset: 0x0002B7BC
        public override void InitialCalculations()
        {
            this._to = this._pTo ?? this._node.RectPosition;
            this._from = this._to - new Vector2(0f, this._pDisplacement ?? this._node.RectSize.y);
            this._track = this._to - this._from;
        }
    }
}
