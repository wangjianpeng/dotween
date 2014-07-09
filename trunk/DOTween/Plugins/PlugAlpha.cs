// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/07 19:22
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

using DG.Tween.Core;
using DG.Tween.Plugins.Core;
using UnityEngine;

namespace DG.Tween.Plugins
{
    public struct PlugAlpha : IPlugSetter<Color, Color, AlphaPlugin, NoOptions>
    {
        readonly Color _endValue;
        readonly MemberGetter<Color> _getter;
        readonly MemberSetter<Color> _setter;

        public PlugAlpha(MemberGetter<Color> getter, MemberSetter<Color> setter, float endValue)
        {
            _getter = getter;
            _setter = setter;
            _endValue = new Color(0, 0, 0, endValue);
        }

        public MemberGetter<Color> Getter() { return _getter; }
        public MemberSetter<Color> Setter() { return _setter; }
        public Color EndValue() { return _endValue; }
        public NoOptions GetOptions() { return new NoOptions(); }
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // ||| CLASS |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class AlphaPlugin : ABSTweenPlugin<Color, Color, NoOptions>
    {
        public override Color ConvertT1toT2(NoOptions options, Color value)
        {
            return value;
        }

        public override Color Calculate(NoOptions options, MemberGetter<Color> getter, float elapsed, Color startValue, Color endValue, float duration, EaseFunction ease)
        {
            Color res = getter();
            res.a = ease(elapsed, startValue.a, (endValue.a - startValue.a), duration, 0, 0);
            return res;
        }

        public override Color GetRelativeEndValue(NoOptions options, Color startValue, Color changeValue)
        {
            return startValue + changeValue;
        }
    }
}