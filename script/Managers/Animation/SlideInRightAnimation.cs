using System;
using Godot;

namespace LacieEngine.Animation
{
    // Token: 0x02000145 RID: 325
    public class SlideInRightAnimation : SlideInAnimation
    {
        // Token: 0x06000B5E RID: 2910 RVA: 0x0002D50E File Offset: 0x0002B70E
        public SlideInRightAnimation(Control node, float? displacement = null, Vector2? to = null, float duration = 0.15f)
            : base(node, displacement, to, duration)
        {
        }

        // Token: 0x06000B5F RID: 2911 RVA: 0x0002D51C File Offset: 0x0002B71C
        public override void InitialCalculations()
        {
            this._to = this._pTo ?? this._node.RectPosition;
            this._from = this._to + new Vector2(this._pDisplacement ?? this._node.RectSize.x, 0f);
            this._track = this._to - this._from;
        }
    }
}
