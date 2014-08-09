// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/10 16:53
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
    public class Vector4Plugin : ABSTweenPlugin<Vector4, Vector4, VectorOptions>
    {
        public override Vector4 ConvertT1toT2(VectorOptions options, Vector4 value)
        {
            return value;
        }

        public override Vector4 GetRelativeEndValue(VectorOptions options, Vector4 startValue, Vector4 changeValue)
        {
            return startValue + changeValue;
        }

        public override Vector4 GetChangeValue(VectorOptions options, Vector4 startValue, Vector4 endValue)
        {
            return endValue - startValue;
        }

        public override float GetSpeedBasedDuration(float unitsXSecond, Vector4 changeValue)
        {
            float res = changeValue.magnitude / unitsXSecond;
            if (res < 0) res = -res;
            return res;
        }

        public override Vector4 Evaluate(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector4> getter, float elapsed, Vector4 startValue, Vector4 changeValue, float duration)
        {
            if (t.loopType == LoopType.Incremental) startValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);

            startValue.x = EaseManager.Evaluate(t, elapsed, startValue.x, changeValue.x, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            startValue.y = EaseManager.Evaluate(t, elapsed, startValue.y, changeValue.y, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            startValue.z = EaseManager.Evaluate(t, elapsed, startValue.z, changeValue.z, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            startValue.w = EaseManager.Evaluate(t, elapsed, startValue.w, changeValue.w, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            if (options.snapping) {
                startValue.x = (float)Math.Round(startValue.x);
                startValue.y = (float)Math.Round(startValue.y);
                startValue.z = (float)Math.Round(startValue.z);
                startValue.w = (float)Math.Round(startValue.w);
            }
            return startValue;
        }
    }
}