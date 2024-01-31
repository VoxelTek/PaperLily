using System;
using Godot;

namespace LacieEngine.Animation
{
    // Token: 0x02000144 RID: 324
    public class SlideInLeftAnimation : SlideInAnimation
    {
        // Token: 0x06000B5C RID: 2908 RVA: 0x0002D46E File Offset: 0x0002B66E
        public SlideInLeftAnimation(Control node, float? displacement = null, Vector2? to = null, float duration = 0.15f)
            : base(node, displacement, to, duration)
        {
        }

        // Token: 0x06000B5D RID: 2909 RVA: 0x0002D47C File Offset: 0x0002B67C
        public override void InitialCalculations()
        {
            this._to = this._pTo ?? this._node.RectPosition;
            this._from = this._to - new Vector2(this._pDisplacement ?? this._node.RectSize.x, 0f);
            this._track = this._to - this._from;
        }
    }
}
