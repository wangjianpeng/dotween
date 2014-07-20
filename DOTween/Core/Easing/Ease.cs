// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/19 14:11
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
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core.Easing
{
    public static class Ease
    {
        const float _PiOver2 = Mathf.PI * 0.5f;
        const float _TwoPi = Mathf.PI * 2;

        public static float Apply(Tween t, float time, float startValue, float changeValue, float duration, float overshootOrAmplitude, float period)
        {
            switch (t.easeType) {
            case EaseType.Linear:
                return changeValue * time / duration + startValue;
            case EaseType.InSine:
                return -changeValue * (float)Math.Cos(time / duration * _PiOver2) + changeValue + startValue;
            case EaseType.OutSine:
                return changeValue*(float)Math.Sin(time/duration*_PiOver2) + startValue;
            case EaseType.InOutSine:
                return -changeValue * 0.5f * ((float)Math.Cos(Mathf.PI * time / duration) - 1) + startValue;
            case EaseType.InQuad:
                return changeValue*(time /= duration)*time + startValue;
            case EaseType.OutQuad:
                return -changeValue * (time /= duration) * (time - 2) + startValue;
            case EaseType.InOutQuad:
                if ((time /= duration*0.5f) < 1) return changeValue*0.5f*time*time + startValue;
                return -changeValue*0.5f*((--time)*(time - 2) - 1) + startValue;
            case EaseType.InCubic:
                return changeValue * (time /= duration) * time * time + startValue;
            case EaseType.OutCubic:
                return changeValue * ((time = time / duration - 1) * time * time + 1) + startValue;
            case EaseType.InOutCubic:
                if ((time /= duration*0.5f) < 1) return changeValue*0.5f*time*time*time + startValue;
                return changeValue*0.5f*((time -= 2)*time*time + 2) + startValue;
            case EaseType.InQuart:
                return changeValue * (time /= duration) * time * time * time + startValue;
            case EaseType.OutQuart:
                return -changeValue * ((time = time / duration - 1) * time * time * time - 1) + startValue;
            case EaseType.InOutQuart:
                if ((time /= duration*0.5f) < 1) return changeValue*0.5f*time*time*time*time + startValue;
                return -changeValue*0.5f*((time -= 2)*time*time*time - 2) + startValue;
            case EaseType.InQuint:
                return changeValue*(time /= duration)*time*time*time*time + startValue;
            case EaseType.OutQuint:
                return changeValue*((time = time/duration - 1)*time*time*time*time + 1) + startValue;
            case EaseType.InOutQuint:
                if ((time /= duration*0.5f) < 1) return changeValue*0.5f*time*time*time*time*time + startValue;
                return changeValue*0.5f*((time -= 2)*time*time*time*time + 2) + startValue;
            case EaseType.InExpo:
                if (time == 0) return startValue;
                return changeValue*(float)Math.Pow(2, 10*(time/duration - 1)) + startValue - changeValue*0.001f;
            case EaseType.OutExpo:
                if (time == duration) return startValue + changeValue;
                return changeValue*(-(float)Math.Pow(2, -10*time/duration) + 1) + startValue;
            case EaseType.InOutExpo:
                if (time == 0) return startValue;
                if (time == duration) return startValue + changeValue;
                if ((time /= duration*0.5f) < 1) return changeValue*0.5f*(float)Math.Pow(2, 10*(time - 1)) + startValue;
                return changeValue*0.5f*(-(float)Math.Pow(2, -10*--time) + 2) + startValue;
            case EaseType.InCirc:
                return -changeValue*((float)Math.Sqrt(1 - (time /= duration)*time) - 1) + startValue;
            case EaseType.OutCirc:
                return changeValue * (float)Math.Sqrt(1 - (time = time / duration - 1) * time) + startValue;
            case EaseType.InOutCirc:
                if ((time /= duration*0.5f) < 1) return -changeValue*0.5f*((float)Math.Sqrt(1 - time*time) - 1) + startValue;
                return changeValue*0.5f*((float)Math.Sqrt(1 - (time -= 2)*time) + 1) + startValue;
            case EaseType.InElastic:
                float s0;
                if (time == 0) return startValue;
                if ((time /= duration) == 1) return startValue + changeValue;
                if (period == 0) period = duration*0.3f;
                if (overshootOrAmplitude == 0 || (changeValue > 0 && overshootOrAmplitude < changeValue) || (changeValue < 0 && overshootOrAmplitude < -changeValue)) {
                    overshootOrAmplitude = changeValue;
                    s0 = period/4;
                } else s0 = period / _TwoPi * (float)Math.Asin(changeValue / overshootOrAmplitude);
                return -(overshootOrAmplitude * (float)Math.Pow(2, 10 * (time -= 1)) * (float)Math.Sin((time * duration - s0) * _TwoPi / period)) + startValue;
            case EaseType.OutElastic:
                float s1;
                if (time == 0) return startValue;
                if ((time /= duration) == 1) return startValue + changeValue;
                if (period == 0) period = duration*0.3f;
                if (overshootOrAmplitude == 0 || (changeValue > 0 && overshootOrAmplitude < changeValue) || (changeValue < 0 && overshootOrAmplitude < -changeValue)) {
                    overshootOrAmplitude = changeValue;
                    s1 = period/4;
                } else s1 = period / _TwoPi * (float)Math.Asin(changeValue / overshootOrAmplitude);
                return (overshootOrAmplitude * (float)Math.Pow(2, -10 * time) * (float)Math.Sin((time * duration - s1) * _TwoPi / period) + changeValue + startValue);
            case EaseType.InOutElastic:
                float s;
                if (time == 0) return startValue;
                if ((time /= duration*0.5f) == 2) return startValue + changeValue;
                if (period == 0) period = duration*(0.3f*1.5f);
                if (overshootOrAmplitude == 0 || (changeValue > 0 && overshootOrAmplitude < changeValue) || (changeValue < 0 && overshootOrAmplitude < -changeValue)) {
                    overshootOrAmplitude = changeValue;
                    s = period/4;
                } else s = period / _TwoPi * (float)Math.Asin(changeValue / overshootOrAmplitude);
                if (time < 1) return -0.5f * (overshootOrAmplitude * (float)Math.Pow(2, 10 * (time -= 1)) * (float)Math.Sin((time * duration - s) * _TwoPi / period)) + startValue;
                return overshootOrAmplitude * (float)Math.Pow(2, -10 * (time -= 1)) * (float)Math.Sin((time * duration - s) * _TwoPi / period) * 0.5f + changeValue + startValue;
            case EaseType.InBack:
                return changeValue * (time /= duration) * time * ((overshootOrAmplitude + 1) * time - overshootOrAmplitude) + startValue;
            case EaseType.OutBack:
                return changeValue*((time = time/duration - 1)*time*((overshootOrAmplitude + 1)*time + overshootOrAmplitude) + 1) + startValue;
            case EaseType.InOutBack:
                if ((time /= duration * 0.5f) < 1) return changeValue * 0.5f * (time * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time - overshootOrAmplitude)) + startValue;
                return changeValue / 2 * ((time -= 2) * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time + overshootOrAmplitude) + 2) + startValue;
            case EaseType.InBounce:
                return Bounce.EaseIn(time, startValue, changeValue, duration, overshootOrAmplitude, period);
            case EaseType.OutBounce:
                return Bounce.EaseOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
            case EaseType.InOutBounce:
                return Bounce.EaseInOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
            case EaseType.AnimationCurve:
                return t.easeCurveEval(time, startValue, changeValue, duration, overshootOrAmplitude, period);
            default:
                // OutQuad
                return -changeValue * (time /= duration) * (time - 2) + startValue;
            }
        }
    }
}