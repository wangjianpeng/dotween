// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/10 19:17
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
    public class RectPlugin : ABSTweenPlugin<Rect, Rect, RectOptions>
    {
        public override Rect ConvertT1toT2(TweenerCore<Rect, Rect, RectOptions> t, Rect value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<Rect, Rect, RectOptions> t)
        {
            t.endValue.x += t.startValue.x;
            t.endValue.y += t.startValue.y;
            t.endValue.width += t.startValue.width;
            t.endValue.height += t.startValue.height;
        }

        public override void SetChangeValue(TweenerCore<Rect, Rect, RectOptions> t)
        {
            t.changeValue = new Rect(
                t.endValue.x - t.startValue.x,
                t.endValue.y - t.startValue.y,
                t.endValue.width - t.startValue.width,
                t.endValue.height - t.startValue.height
            );
        }

        public override float GetSpeedBasedDuration(float unitsXSecond, Rect changeValue)
        {
            // Uses length of diagonal to calculate units.
            float diffW = changeValue.width;
            float diffH = changeValue.height;
            float diag = (float)Math.Sqrt(diffW * diffW + diffH * diffH);
            return diag / unitsXSecond;
        }

        public override Rect Evaluate(RectOptions options, Tween t, bool isRelative, DOGetter<Rect> getter, float elapsed, Rect startValue, Rect changeValue, float duration)
        {
            // Doens't support LoopType.Incremental

            startValue.x = EaseManager.Evaluate(t, elapsed, startValue.x, changeValue.x, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            startValue.y = EaseManager.Evaluate(t, elapsed, startValue.y, changeValue.y, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            startValue.width = EaseManager.Evaluate(t, elapsed, startValue.width, changeValue.width, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            startValue.height = EaseManager.Evaluate(t, elapsed, startValue.height, changeValue.height, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            if (options.snapping) {
                startValue.x = (float)Math.Round(startValue.x);
                startValue.y = (float)Math.Round(startValue.y);
                startValue.width = (float)Math.Round(startValue.width);
                startValue.height = (float)Math.Round(startValue.height);
            }
            return startValue;
        }
    }
}