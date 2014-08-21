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
        public override Vector3 ConvertT1toT2(TweenerCore<Vector3, Vector3, VectorOptions> t, Vector3 value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<Vector3, Vector3, VectorOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<Vector3, Vector3, VectorOptions> t)
        {
            switch (t.plugOptions.axisConstraint) {
            case AxisConstraint.X:
                t.changeValue = new Vector3(t.endValue.x - t.startValue.x, 0, 0);
                break;
            case AxisConstraint.Y:
                t.changeValue = new Vector3(0, t.endValue.y - t.startValue.y, 0);
                break;
            case AxisConstraint.Z:
                t.changeValue = new Vector3(0, 0, t.endValue.z - t.startValue.z);
                break;
            default:
                t.changeValue = t.endValue - t.startValue;
                break;
            }
        }

        public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector3 changeValue)
        {
            return changeValue.magnitude / unitsXSecond;
        }

        public override Vector3 Evaluate(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, float elapsed, Vector3 startValue, Vector3 changeValue, float duration)
        {
            if (t.loopType == LoopType.Incremental) startValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);

            switch (options.axisConstraint) {
            case AxisConstraint.X:
                Vector3 resX = getter();
                resX.x = EaseManager.Evaluate(t, elapsed, startValue.x, changeValue.x, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                if (options.snapping) resX.x = (float)Math.Round(resX.x);
                return resX;
            case AxisConstraint.Y:
                Vector3 resY = getter();
                resY.y = EaseManager.Evaluate(t, elapsed, startValue.y, changeValue.y, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                if (options.snapping) resY.y = (float)Math.Round(resY.y);
                return resY;
            case AxisConstraint.Z:
                Vector3 resZ = getter();
                resZ.z = EaseManager.Evaluate(t, elapsed, startValue.z, changeValue.z, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                if (options.snapping) resZ.z = (float)Math.Round(resZ.z);
                return resZ;
            default:
                startValue.x = EaseManager.Evaluate(t, elapsed, startValue.x, changeValue.x, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.y = EaseManager.Evaluate(t, elapsed, startValue.y, changeValue.y, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.z = EaseManager.Evaluate(t, elapsed, startValue.z, changeValue.z, duration, t.easeOvershootOrAmplitude, t.easePeriod);
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