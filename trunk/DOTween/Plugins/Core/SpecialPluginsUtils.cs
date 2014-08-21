// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/08/21 13:08
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
using DG.Tweening.Plugins.Core.DefaultPlugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins.Core
{
    // Utils for special plugins
    internal static class SpecialPluginsUtils
    {
        // Returns TRUE if it's successful, FALSE otherwise
        internal static bool SetLocalAxisSetter(TweenerCore<Quaternion, Vector3, NoOptions> t)
        {
            Transform trans = t.target as Transform;
            if (trans != null) {
                // Transform target
                Quaternion localRot = trans.localRotation;
                t.setter = x => trans.localRotation = localRot * x;
            } else {
                // Rigidbody target
                Rigidbody rbody = t.target as Rigidbody;
                if (rbody == null) return false;
                Quaternion localRot = rbody.transform.localRotation;
                t.setter = x => rbody.MoveRotation(localRot * x);
            }
            return true;
        }

        // Returns TRUE if it's successful, FALSE otherwise
        internal static bool SetShake(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
        {
            Camera target = t.target as Camera;
            if (target == null) return false;
            Vector3 startupPos;
            try {
                startupPos = t.getter();
            } catch { return false; }

            // Force specific settings
            t.isRelative = t.isSpeedBased = false;
            t.easeType = Ease.Linear;
            t.customEase = null;

            int len = t.endValue.Length;
            for (int i = 0; i < len; i++) {
                Vector3 endValue = t.endValue[i];
                endValue = target.transform.localRotation * endValue;
                t.endValue[i] = endValue + startupPos;
            }
            return true;
        }
    }
}