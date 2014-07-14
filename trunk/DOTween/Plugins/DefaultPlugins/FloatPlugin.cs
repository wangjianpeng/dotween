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

using DG.Tweening.Core;
using DG.Tweening.Plugins.Core;
using UnityEngine;

namespace DG.Tweening.Plugins.DefaultPlugins
{
    public class FloatPlugin : ABSTweenPlugin<float,float,PlugFloat.Options>
    {
        public override float ConvertT1toT2(PlugFloat.Options options, float value)
        {
            return value;
        }

        public override float GetRelativeEndValue(PlugFloat.Options options, float startValue, float changeValue)
        {
            return startValue + changeValue;
        }

        public override float GetChangeValue(PlugFloat.Options options, float startValue, float endValue)
        {
            return endValue - startValue;
        }

        public override float Evaluate(PlugFloat.Options options, bool isRelative, DOGetter<float> getter, float elapsed, float startValue, float changeValue, float duration, EaseFunction ease)
        {
            return options.snapping
                ? Mathf.Round(ease(elapsed, startValue, changeValue, duration, 0, 0))
                : ease(elapsed, startValue, changeValue, duration, 0, 0);
        }
    }
}