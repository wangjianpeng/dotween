// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/06 16:33
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

using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.DefaultPlugins
{
    public class FloatPlugin : ABSTweenPlugin<float>
    {
        public override void SetStartValue(TweenerCore<float> t)
        {
            t.startValue = t.getter();
        }

        public override void Evaluate(TweenerCore<float> t, float elapsed)
        {
            if (!t.optionsBool0) {
                t.setter(Ease.Apply(t, elapsed, t.startValue, t.changeValue, t.duration, 0, 0));
            } else {
                // Snapping
                t.setter((float)Math.Round(Ease.Apply(t, elapsed, t.startValue, t.changeValue, t.duration, 0, 0)));
            }
        }
    }
}