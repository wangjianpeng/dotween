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

using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.DefaultPlugins
{
    public class ColorPlugin : ABSTweenPlugin
    {
        Color _res;

        public override void SetStartValue(TweenerCore t)
        {
            t.startValueV4 = GetTargetValue(t);
        }

        public override void Evaluate(TweenerCore t, float elapsed)
        {
            if (!t.optionsBool0) {
                _res.r = Ease.Apply(t, elapsed, t.startValueV4.x, t.changeValueV4.x, t.duration, 0, 0);
                _res.g = Ease.Apply(t, elapsed, t.startValueV4.y, t.changeValueV4.y, t.duration, 0, 0);
                _res.b = Ease.Apply(t, elapsed, t.startValueV4.z, t.changeValueV4.z, t.duration, 0, 0);
                _res.a = Ease.Apply(t, elapsed, t.startValueV4.w, t.changeValueV4.w, t.duration, 0, 0);
            } else {
                // Alpha only
                _res = GetTargetValue(t);
                _res.a = Ease.Apply(t, elapsed, t.startValueV4.w, t.startValueV4.w, t.duration, 0, 0);
            }
            // Apply to eventual known type
            if (t.targetType == TargetType.MaterialColor) t.targetMaterial.color = _res;
            else t.setterVector4(_res);
        }

        static Color GetTargetValue(TweenerCore t)
        {
            if (t.targetType == TargetType.MaterialColor) return t.targetMaterial.color;
            return t.getterVector4();
        }
    }
}