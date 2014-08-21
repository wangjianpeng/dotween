// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/08/20 15:05
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

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Core.DefaultPlugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Core.DefaultPlugins
{
    /// <summary>
    /// This plugin generates some GC allocations at startup
    /// </summary>
    public class Vector3ArrayPlugin : ABSTweenPlugin<Vector3, Vector3[], VectorOptions>
    {
        public override Vector3[] ConvertT1toT2(TweenerCore<Vector3, Vector3[], VectorOptions> t, Vector3 value)
        {
            int len = t.endValue.Length;
            Vector3[] res = new Vector3[len];
            for (int i = 0; i < len; i++) {
                if (i == 0) res[i] = value;
                else res[i] = t.endValue[i - 1];
            }
            return res;
        }

        public override void SetRelativeEndValue(TweenerCore<Vector3, Vector3[], VectorOptions> t)
        {
            int len = t.endValue.Length;
            for (int i = 0; i < len; ++i) t.endValue[i] = t.startValue[i] + t.endValue[i];
        }

        public override void SetChangeValue(TweenerCore<Vector3, Vector3[], VectorOptions> t)
        {
            int len = t.endValue.Length;
            t.changeValue = new Vector3[len];
            for (int i = 0; i < len; ++i) t.changeValue[i] = t.endValue[i] - t.startValue[i];
        }

        public override float GetSpeedBasedDuration(float unitsXSecond, Vector3[] changeValue)
        {
            float totMagnitude = 0;
            int len = changeValue.Length;
            for (int i = 0; i < len; ++i) totMagnitude += changeValue[i].magnitude;
            return totMagnitude / unitsXSecond;
        }

        public override Vector3 Evaluate(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, float elapsed, Vector3[] startValue, Vector3[] changeValue, float duration)
        {
            float elapsedPerc = EaseManager.Evaluate(t, elapsed, 0, 1, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            
            int len = changeValue.Length;
            int indexToTween = (int)Mathf.Floor(len * elapsedPerc);
            if (indexToTween == len) indexToTween -= 1;
            
            float innerTweenDuration = duration / len;
            float innerTweenElapsed = elapsed - (innerTweenDuration * indexToTween);

            Vector3 res = changeValue[indexToTween];
            return res * innerTweenElapsed / innerTweenDuration + startValue[indexToTween];
        }
    }
}