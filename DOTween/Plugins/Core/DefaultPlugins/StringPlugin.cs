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

using System;
using System.Text;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Core.DefaultPlugins.Options;
using Random = UnityEngine.Random;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Core.DefaultPlugins
{
    // USING THIS PLUGIN WILL GENERATE GC ALLOCATIONS
    public class StringPlugin : ABSTweenPlugin<string, string, StringOptions>
    {
        static readonly StringBuilder _Buffer = new StringBuilder();

        public override string ConvertT1toT2(StringOptions options, string value)
        {
            return value;
        }

        public override string GetRelativeEndValue(StringOptions options, string startValue, string changeValue)
        {
            return changeValue;
        }

        public override string GetChangeValue(StringOptions options, string startValue, string endValue)
        {
            return endValue;
        }

        // ChangeValue is the same as endValue in this plugin
        public override string Evaluate(StringOptions options, Tween t, bool isRelative, DOGetter<string> getter, float elapsed, string startValue, string changeValue, float duration)
        {
            _Buffer.Remove(0, _Buffer.Length);
            int startValueLen = startValue.Length;
            int changeValueLen = changeValue.Length;
            int len = (int)Math.Round(Ease.Apply(t, elapsed, 0, changeValueLen, duration, 0, 0));

            if (isRelative) {
                _Buffer.Append(startValue);
                if (options.scramble) return _Buffer.Append(changeValue, 0, len).AppendScrambledChars(changeValueLen - len).ToString();
                return _Buffer.Append(changeValue, 0, len).ToString();
            }

            if (options.scramble) {
                return _Buffer.Append(changeValue, 0, len).AppendScrambledChars(changeValueLen - len).ToString();
            }

            int diff = startValueLen - changeValueLen;
            int startValueMaxLen = startValueLen;
            if (diff > 0) {
                // String to be replaced is longer than endValue: remove parts of it while tweening
                float perc = (float)len / changeValueLen;
                startValueMaxLen -= (int)(startValueMaxLen * perc);
            } else startValueMaxLen -= len;
            _Buffer.Append(changeValue, 0, len);
            if (len < changeValueLen && len < startValueLen) _Buffer.Append(startValue, len, startValueMaxLen);
            return _Buffer.ToString();
        }
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // ||| CLASS |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    internal static class StringPluginExtensions
    {
        static readonly char[] _ScrambledChars = new[] {
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','X','Y','Z',
//            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','x','y','z',
//            '1','2','3','4','5','6','7','8','9','0'
        };
        static readonly int _ScrambledCharsLen;
        static int _lastRndSeed;

        static StringPluginExtensions()
        {
            _ScrambledCharsLen = _ScrambledChars.Length;
            // Shuffle chars (uses Knuth shuggle algorithm)
            for (int i = 0; i < _ScrambledCharsLen; i++) {
                char tmp = _ScrambledChars[i];
                int r = Random.Range(i, _ScrambledCharsLen);
                _ScrambledChars[i] = _ScrambledChars[r];
                _ScrambledChars[r] = tmp;
            }
        }

        internal static StringBuilder AppendScrambledChars(this StringBuilder buffer, int length)
        {
            if (length <= 0) return buffer;

            // Make sure random seed is different from previous one used
            int rndSeed = _lastRndSeed;
            while (rndSeed == _lastRndSeed) {
                rndSeed = Random.Range(0, _ScrambledCharsLen);
            }
            _lastRndSeed = rndSeed;
            // Append
            for (int i = 0; i < length; ++i) {
                if (rndSeed >= _ScrambledCharsLen) rndSeed = 0;
                buffer.Append(_ScrambledChars[rndSeed]);
                rndSeed += 1;
            }
            return buffer;
        }
    }
}