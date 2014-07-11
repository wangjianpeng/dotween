// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/11 11:34
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

namespace DG.Tween.Plugins.DefaultPlugins
{
    public class StringPlugin : ABSTweenPlugin<string, string, NoOptions>
    {
        public override string ConvertT1toT2(NoOptions options, string value)
        {
            return value;
        }

        public override string GetRelativeEndValue(NoOptions options, string startValue, string changeValue)
        {
            return changeValue;
        }

        public override string GetChangeValue(NoOptions options, string startValue, string endValue)
        {
            return endValue;
        }

        // ChangeValue is the same as endValue in this plugin
        public override string Evaluate(NoOptions options, bool isRelative, MemberGetter<string> getter, float elapsed, string startValue, string changeValue, float duration, EaseFunction ease)
        {
            int startValueLen = startValue.Length;
            int changeValueLen = changeValue.Length;
            int len = Mathf.RoundToInt(ease(elapsed, 0, changeValueLen, duration, 0, 0));

            if (isRelative) return startValue + changeValue.Substring(0, len);

            int diff = startValueLen - changeValueLen;
            int startValueMaxLen = startValueLen;
            if (diff > 0) {
                // String to be replaced is longer than endValue: remove parts of it while tweening
                float perc = (float)len / changeValueLen;
                startValueMaxLen -= (int)(startValueMaxLen * perc);
            } else startValueMaxLen -= len;
            return changeValue.Substring(0, len) + (len >= changeValueLen || len >= startValueLen ? "" : startValue.Substring(len, startValueMaxLen));
        }
    }
}