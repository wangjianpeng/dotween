// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/11 10:38
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
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // ||| PLUG ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public struct PlugVector3Y : IPlugSetter<Vector3, float, Vector3YPlugin, PlugVector3Y.Options>
    {
        readonly float _endValue;
        readonly MemberGetter<Vector3> _getter;
        readonly MemberSetter<Vector3> _setter;
        readonly Options _options;

        public PlugVector3Y(MemberGetter<Vector3> getter, MemberSetter<Vector3> setter, float endValue, Options options = new Options())
        {
            _getter = getter;
            _setter = setter;
            _endValue = endValue;
            _options = options;
        }

        public MemberGetter<Vector3> Getter() { return _getter; }
        public MemberSetter<Vector3> Setter() { return _setter; }
        public float EndValue() { return _endValue; }
        public Options GetOptions() { return _options; }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // ||| OPTIONS |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public struct Options
        {
            public bool snapping;

            public Options(bool snapping)
            {
                this.snapping = snapping;
            }
        }
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // ||| PLUGIN ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class Vector3YPlugin : ABSTweenPlugin<Vector3, float, PlugVector3Y.Options>
    {
        public override float ConvertT1toT2(PlugVector3Y.Options options, Vector3 value)
        {
            return value.y;
        }

        public override float GetRelativeEndValue(PlugVector3Y.Options options, float startValue, float changeValue)
        {
            return startValue + changeValue;
        }

        public override float GetChangeValue(PlugVector3Y.Options options, float startValue, float endValue)
        {
            return endValue - startValue;
        }

        public override Vector3 Evaluate(PlugVector3Y.Options options, bool isRelative, MemberGetter<Vector3> getter, float elapsed, float startValue, float changeValue, float duration, EaseFunction ease)
        {
            Vector3 res = getter();
            res.y = ease(elapsed, startValue, changeValue, duration, 0, 0);
            if (options.snapping) res.y = Mathf.Round(res.y);
            return res;
        }
    }
}