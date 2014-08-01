// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/06 19:35
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
using DG.Tweening.Plugins.Core.DefaultPlugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Core.DefaultPlugins
{
    public class Vector3Plugin : ABSTweenPlugin<Vector3,Vector3,VectorOptions>
    {
        public override Vector3 ConvertT1toT2(VectorOptions options, Vector3 value)
        {
            return value;
        }

        public override Vector3 GetRelativeEndValue(VectorOptions options, Vector3 startValue, Vector3 changeValue)
        {
            return startValue + changeValue;
        }

        public override Vector3 GetChangeValue(VectorOptions options, Vector3 startValue, Vector3 endValue)
        {
            return endValue - startValue;
        }

        public override float GetSpeedBasedDuration(float unitsXSecond, Vector3 changeValue)
        {
            float res = changeValue.magnitude / unitsXSecond;
            if (res < 0) res = -res;
            return res;
        }

        public override Vector3 Evaluate(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, float elapsed, Vector3 startValue, Vector3 changeValue, float duration)
        {
            switch (options.axisConstraint) {
            case AxisConstraint.X:
                Vector3 resX = getter();
                resX.x = Ease.Apply(t, elapsed, startValue.x, changeValue.x, duration, 0, 0);
                if (options.snapping) resX.x = (float)Math.Round(resX.x);
                return resX;
            case AxisConstraint.Y:
                Vector3 resY = getter();
                resY.y = Ease.Apply(t, elapsed, startValue.y, changeValue.y, duration, 0, 0);
                if (options.snapping) resY.y = (float)Math.Round(resY.y);
                return resY;
            case AxisConstraint.Z:
                Vector3 resZ = getter();
                resZ.z = Ease.Apply(t, elapsed, startValue.z, changeValue.z, duration, 0, 0);
                if (options.snapping) resZ.z = (float)Math.Round(resZ.z);
                return resZ;
            default:
                startValue.x = Ease.Apply(t, elapsed, startValue.x, changeValue.x, duration, 0, 0);
                startValue.y = Ease.Apply(t, elapsed, startValue.y, changeValue.y, duration, 0, 0);
                startValue.z = Ease.Apply(t, elapsed, startValue.z, changeValue.z, duration, 0, 0);
                if (options.snapping) {
                    startValue.x = (float)Math.Round(startValue.x);
                    startValue.y = (float)Math.Round(startValue.y);
                    startValue.z = (float)Math.Round(startValue.z);
                }
                return startValue;
            }
        }
    }
}