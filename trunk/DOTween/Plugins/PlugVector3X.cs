// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/07 14:33
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
using DG.Tween.Core;
using DG.Tween.Plugins.Core;
using UnityEngine;

namespace DG.Tween.Plugins
{
    public struct PlugVector3X : IPluginSetter<Vector3, Vector3, Vector3XPlugin>
    {
        internal Vector3 endValue;
        internal Type pluginType;

        public PlugVector3X(float endValue)
        {
            this.endValue = new Vector3(endValue, 0, 0);
            pluginType = typeof(Vector3XPlugin);
        }

        public Type PluginType() { return pluginType; }
        public Vector3 EndValue() { return endValue; }
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // ||| CLASS |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class Vector3XPlugin : ABSTweenPlugin<Vector3, Vector3>
    {
        public override Vector3 ConvertT1toT2(Vector3 value)
        {
            return value;
        }

        public override Vector3 Calculate(MemberGetter<Vector3> getter, float elapsed, Vector3 startValue, Vector3 endValue, float duration, EaseFunction ease)
        {
            Vector3 res = getter();
            res.x = ease(elapsed, startValue.x, (endValue.x - startValue.x), duration, 0, 0);
            return res;
        }

        public override Vector3 GetRelativeEndValue(Vector3 startValue, Vector3 changeValue)
        {
            return startValue + changeValue;
        }
    }
}