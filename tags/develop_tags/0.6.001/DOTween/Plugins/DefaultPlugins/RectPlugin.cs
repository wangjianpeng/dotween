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
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.DefaultPlugins
{
    public class RectPlugin : ABSTweenPlugin<Rect>
    {
        Rect _res;

        public override void SetStartValue(TweenerCore<Rect> t)
        {
            Rect r = t.getter();
            t.startValueV4 = new Vector4(r.x, r.y, r.width, r.height);
        }

        public override void Evaluate(TweenerCore<Rect> t, float elapsed)
        {
            _res.x = Ease.Apply(t, elapsed, t.startValueV4.x, t.changeValueV4.x, t.duration, 0, 0);
            _res.y = Ease.Apply(t, elapsed, t.startValueV4.y, t.changeValueV4.y, t.duration, 0, 0);
            _res.width = Ease.Apply(t, elapsed, t.startValueV4.z, t.changeValueV4.z, t.duration, 0, 0);
            _res.height = Ease.Apply(t, elapsed, t.startValueV4.z, t.changeValueV4.z, t.duration, 0, 0);
            
            if (t.optionsBool0) {
                // Snapping
                _res.x = (float)Math.Round(_res.x);
                _res.y = (float)Math.Round(_res.y);
                _res.width = (float)Math.Round(_res.width);
                _res.height = (float)Math.Round(_res.height);
            }

            t.setter(_res);
        }
    }
}