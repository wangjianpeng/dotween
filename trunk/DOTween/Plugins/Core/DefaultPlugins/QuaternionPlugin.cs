// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/07 20:02
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

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Core.DefaultPlugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Core.DefaultPlugins
{
    public class QuaternionPlugin : ABSTweenPlugin<Quaternion,Vector3,NoOptions>
    {
        public override Vector3 ConvertT1toT2(TweenerCore<Quaternion,Vector3,NoOptions> t, Quaternion value)
        {
            return value.eulerAngles;
        }

        public override void SetRelativeEndValue(TweenerCore<Quaternion, Vector3, NoOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<Quaternion, Vector3, NoOptions> t)
        {
            t.changeValue = t.endValue - t.startValue;
        }

        public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, Vector3 changeValue)
        {
            return changeValue.magnitude / unitsXSecond;
        }

        public override Quaternion Evaluate(NoOptions options, Tween t, bool isRelative, DOGetter<Quaternion> getter, float elapsed, Vector3 startValue, Vector3 changeValue, float duration)
        {
            if (t.loopType == LoopType.Incremental) startValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);

            startValue.x = EaseManager.Evaluate(t, elapsed, startValue.x, changeValue.x, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            startValue.y = EaseManager.Evaluate(t, elapsed, startValue.y, changeValue.y, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            startValue.z = EaseManager.Evaluate(t, elapsed, startValue.z, changeValue.z, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            return Quaternion.Euler(startValue);
        }

        // ===================================================================================
        // SPECIFIC EXTAS --------------------------------------------------------------------

        internal void SetLocalAxisSetter(TweenerCore<Quaternion, Vector3, NoOptions> t)
        {
            Transform trans = t.target as Transform;
            if (trans != null) {
                // Transform target
                Quaternion localRot = trans.localRotation;
                t.setter = x => trans.localRotation = localRot * x;
            } else {
                // Rigidbody target
                Rigidbody rbody = t.target as Rigidbody;
                Quaternion localRot = rbody.transform.localRotation;
                t.setter = x => rbody.MoveRotation(localRot * x);
            }
        }
    }
}