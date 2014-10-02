// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/11 11:34
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.Text;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using Random = UnityEngine.Random;

#pragma warning disable 1591
namespace DG.Tweening.Plugins
{
    // USING THIS PLUGIN WILL GENERATE GC ALLOCATIONS
    public class StringPlugin : ABSTweenPlugin<string, string, StringOptions>
    {
        static readonly StringBuilder _Buffer = new StringBuilder();

        public override void SetFrom(TweenerCore<string, string, StringOptions> t, bool isRelative)
        {
            string prevEndVal = t.endValue;
            t.endValue = t.getter();
            t.startValue = prevEndVal;
            t.setter(t.startValue);
        }

        public override void Reset(TweenerCore<string, string, StringOptions> t)
        {
            t.startValue = t.endValue = t.changeValue = null;
        }

        public override string ConvertToStartValue(TweenerCore<string, string, StringOptions> t, string value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<string, string, StringOptions> t)
        {
            // Do nothing (endValue stays the same)
        }

        public override void SetChangeValue(TweenerCore<string, string, StringOptions> t)
        {
            t.changeValue = t.endValue;
        }

        public override float GetSpeedBasedDuration(StringOptions options, float unitsXSecond, string changeValue)
        {
            float res = changeValue.Length / unitsXSecond;
            if (res < 0) res = -res;
            return res;
        }

        // ChangeValue is the same as endValue in this plugin
        public override void EvaluateAndApply(StringOptions options, Tween t, bool isRelative, DOGetter<string> getter, DOSetter<string> setter, float elapsed, string startValue, string changeValue, float duration)
        {
            _Buffer.Remove(0, _Buffer.Length);

            // Incremental works only with relative tweens (otherwise the tween makes no sense)
            if (isRelative && t.loopType == LoopType.Incremental) {
                int iterations = t.isComplete ? t.completedLoops - 1 : t.completedLoops;
                if (iterations > 0) {
                    _Buffer.Append(startValue);
                    for (int i = 0; i < iterations; ++i) _Buffer.Append(changeValue);
                    startValue = _Buffer.ToString();
                    _Buffer.Remove(0, _Buffer.Length);
                }
            }

            int startValueLen = startValue.Length;
            int changeValueLen = changeValue.Length;
            int len = (int)Math.Round(EaseManager.Evaluate(t, elapsed, 0, changeValueLen, duration, t.easeOvershootOrAmplitude, t.easePeriod));
            if (len > changeValueLen) len = changeValueLen;
            else if (len < 0) len = 0;

            if (isRelative) {
                _Buffer.Append(startValue);
                if (options.scramble) {
                    setter(_Buffer.Append(changeValue, 0, len).AppendScrambledChars(changeValueLen - len).ToString());
                    return;
                }
                setter(_Buffer.Append(changeValue, 0, len).ToString());
                return;
            }

            if (options.scramble) {
                setter(_Buffer.Append(changeValue, 0, len).AppendScrambledChars(changeValueLen - len).ToString());
                return;
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
            setter(_Buffer.ToString());
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