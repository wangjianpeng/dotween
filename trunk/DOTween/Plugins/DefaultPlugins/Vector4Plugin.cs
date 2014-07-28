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
using DG.Tweening.Plugins.Core;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.DefaultPlugins
{
    public class Vector4Plugin : ABSTweenPlugin<Vector4, Vector4, PlugVector.Options>
    {
        public override Vector4 ConvertT1toT2(PlugVector.Options options, Vector4 value)
        {
            return value;
        }

        public override Vector4 GetRelativeEndValue(PlugVector.Options options, Vector4 startValue, Vector4 changeValue)
        {
            return startValue + changeValue;
        }

        public override Vector4 GetChangeValue(PlugVector.Options options, Vector4 startValue, Vector4 endValue)
        {
            return endValue - startValue;
        }

        public override Vector4 Evaluate(PlugVector.Options options, Tween t, bool isRelative, DOGetter<Vector4> getter, float elapsed, Vector4 startValue, Vector4 changeValue, float duration)
        {
            startValue.x = Ease.Apply(t, elapsed, startValue.x, changeValue.x, duration, 0, 0);
            startValue.y = Ease.Apply(t, elapsed, startValue.y, changeValue.y, duration, 0, 0);
            startValue.z = Ease.Apply(t, elapsed, startValue.z, changeValue.z, duration, 0, 0);
            startValue.w = Ease.Apply(t, elapsed, startValue.w, changeValue.w, duration, 0, 0);
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