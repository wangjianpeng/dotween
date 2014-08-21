// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/10 14:33
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

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Core.DefaultPlugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Core.DefaultPlugins
{
    public class ColorPlugin : ABSTweenPlugin<Color, Color, ColorOptions>
    {
        public override Color ConvertT1toT2(TweenerCore<Color, Color, ColorOptions> t, Color value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<Color, Color, ColorOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<Color, Color, ColorOptions> t)
        {
            t.changeValue = t.endValue - t.startValue;
        }

        public override float GetSpeedBasedDuration(ColorOptions options, float unitsXSecond, Color changeValue)
        {
            return 1f / unitsXSecond;
        }

        public override Color Evaluate(ColorOptions options, Tween t, bool isRelative, DOGetter<Color> getter, float elapsed, Color startValue, Color changeValue, float duration)
        {
            if (t.loopType == LoopType.Incremental) startValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);

            if (!options.alphaOnly) {
                startValue.r = EaseManager.Evaluate(t, elapsed, startValue.r, changeValue.r, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.g = EaseManager.Evaluate(t, elapsed, startValue.g, changeValue.g, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.b = EaseManager.Evaluate(t, elapsed, startValue.b, changeValue.b, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.a = EaseManager.Evaluate(t, elapsed, startValue.a, changeValue.a, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                return startValue;
            }

            // Alpha only
            Color res = getter();
            res.a = EaseManager.Evaluate(t, elapsed, startValue.a, changeValue.a, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            return res;
        }
    }
}