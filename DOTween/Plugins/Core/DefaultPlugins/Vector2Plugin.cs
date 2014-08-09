// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/10 16:51
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 

using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Core.DefaultPlugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Core.DefaultPlugins
{
    public class Vector2Plugin : ABSTweenPlugin<Vector2, Vector2, VectorOptions>
    {
        public override Vector2 ConvertT1toT2(VectorOptions options, Vector2 value)
        {
            return value;
        }

        public override Vector2 GetRelativeEndValue(VectorOptions options, Vector2 startValue, Vector2 changeValue)
        {
            return startValue + changeValue;
        }

        public override Vector2 GetChangeValue(VectorOptions options, Vector2 startValue, Vector2 endValue)
        {
            return endValue - startValue;
        }

        public override float GetSpeedBasedDuration(float unitsXSecond, Vector2 changeValue)
        {
            float res = changeValue.magnitude / unitsXSecond;
            if (res < 0) res = -res;
            return res;
        }

        public override Vector2 Evaluate(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector2> getter, float elapsed, Vector2 startValue, Vector2 changeValue, float duration)
        {
            if (t.loopType == LoopType.Incremental) startValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);

            switch (options.axisConstraint) {
            case AxisConstraint.X:
                Vector2 resX = getter();
                resX.x = EaseManager.Evaluate(t, elapsed, startValue.x, changeValue.x, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                if (options.snapping) resX.x = (float)Math.Round(resX.x);
                return resX;
            case AxisConstraint.Y:
                Vector2 resY = getter();
                resY.y = EaseManager.Evaluate(t, elapsed, startValue.y, changeValue.y, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                if (options.snapping) resY.y = (float)Math.Round(resY.y);
                return resY;
            default:
                startValue.x = EaseManager.Evaluate(t, elapsed, startValue.x, changeValue.x, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.y = EaseManager.Evaluate(t, elapsed, startValue.y, changeValue.y, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                if (options.snapping) {
                    startValue.x = (float)Math.Round(startValue.x);
                    startValue.y = (float)Math.Round(startValue.y);
                }
                return startValue;
            }
        }
    }
}