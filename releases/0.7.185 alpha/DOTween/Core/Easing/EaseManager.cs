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
// =============================================================
// Contains Daniele Giardini's C# port of the easing equations created
// by Robert Penner - http://robertpenner.com/easing, see license below:
// =============================================================
//
// TERMS OF USE - EASING EQUATIONS
//
// Open source under the BSD License.
//
// Copyright © 2001 Robert Penner
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must retain the above copyright notice,
// this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice,
// this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of the author nor the names of contributors may be used to endorse
// or promote products derived from this software without specific prior written permission.
// - THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
// IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
// EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core.Easing
{
    public static class EaseManager
    {
        const float _PiOver2 = Mathf.PI * 0.5f;
        const float _TwoPi = Mathf.PI * 2;

        public static float Evaluate(Tween t, float time, float startValue, float changeValue, float duration, float overshootOrAmplitude, float period)
        {
            switch (t.easeType) {
            case Ease.Linear:
                return changeValue * time / duration + startValue;
            case Ease.InSine:
                return -changeValue * (float)Math.Cos(time / duration * _PiOver2) + changeValue + startValue;
            case Ease.OutSine:
                return changeValue*(float)Math.Sin(time/duration*_PiOver2) + startValue;
            case Ease.InOutSine:
                return -changeValue * 0.5f * ((float)Math.Cos(Mathf.PI * time / duration) - 1) + startValue;
            case Ease.InQuad:
                return changeValue*(time /= duration)*time + startValue;
            case Ease.OutQuad:
                return -changeValue * (time /= duration) * (time - 2) + startValue;
            case Ease.InOutQuad:
                if ((time /= duration*0.5f) < 1) return changeValue*0.5f*time*time + startValue;
                return -changeValue*0.5f*((--time)*(time - 2) - 1) + startValue;
            case Ease.InCubic:
                return changeValue * (time /= duration) * time * time + startValue;
            case Ease.OutCubic:
                return changeValue * ((time = time / duration - 1) * time * time + 1) + startValue;
            case Ease.InOutCubic:
                if ((time /= duration*0.5f) < 1) return changeValue*0.5f*time*time*time + startValue;
                return changeValue*0.5f*((time -= 2)*time*time + 2) + startValue;
            case Ease.InQuart:
                return changeValue * (time /= duration) * time * time * time + startValue;
            case Ease.OutQuart:
                return -changeValue * ((time = time / duration - 1) * time * time * time - 1) + startValue;
            case Ease.InOutQuart:
                if ((time /= duration*0.5f) < 1) return changeValue*0.5f*time*time*time*time + startValue;
                return -changeValue*0.5f*((time -= 2)*time*time*time - 2) + startValue;
            case Ease.InQuint:
                return changeValue*(time /= duration)*time*time*time*time + startValue;
            case Ease.OutQuint:
                return changeValue*((time = time/duration - 1)*time*time*time*time + 1) + startValue;
            case Ease.InOutQuint:
                if ((time /= duration*0.5f) < 1) return changeValue*0.5f*time*time*time*time*time + startValue;
                return changeValue*0.5f*((time -= 2)*time*time*time*time + 2) + startValue;
            case Ease.InExpo:
                if (time == 0) return startValue;
                return changeValue*(float)Math.Pow(2, 10*(time/duration - 1)) + startValue - changeValue*0.001f;
            case Ease.OutExpo:
                if (time == duration) return startValue + changeValue;
                return changeValue*(-(float)Math.Pow(2, -10*time/duration) + 1) + startValue;
            case Ease.InOutExpo:
                if (time == 0) return startValue;
                if (time == duration) return startValue + changeValue;
                if ((time /= duration*0.5f) < 1) return changeValue*0.5f*(float)Math.Pow(2, 10*(time - 1)) + startValue;
                return changeValue*0.5f*(-(float)Math.Pow(2, -10*--time) + 2) + startValue;
            case Ease.InCirc:
                return -changeValue*((float)Math.Sqrt(1 - (time /= duration)*time) - 1) + startValue;
            case Ease.OutCirc:
                return changeValue * (float)Math.Sqrt(1 - (time = time / duration - 1) * time) + startValue;
            case Ease.InOutCirc:
                if ((time /= duration*0.5f) < 1) return -changeValue*0.5f*((float)Math.Sqrt(1 - time*time) - 1) + startValue;
                return changeValue*0.5f*((float)Math.Sqrt(1 - (time -= 2)*time) + 1) + startValue;
            case Ease.InElastic:
                float s0;
                if (time == 0) return startValue;
                if ((time /= duration) == 1) return startValue + changeValue;
                if (period == 0) period = duration*0.3f;
                if (overshootOrAmplitude == 0 || (changeValue > 0 && overshootOrAmplitude < changeValue) || (changeValue < 0 && overshootOrAmplitude < -changeValue)) {
                    overshootOrAmplitude = changeValue;
                    s0 = period/4;
                } else s0 = period / _TwoPi * (float)Math.Asin(changeValue / overshootOrAmplitude);
                return -(overshootOrAmplitude * (float)Math.Pow(2, 10 * (time -= 1)) * (float)Math.Sin((time * duration - s0) * _TwoPi / period)) + startValue;
            case Ease.OutElastic:
                float s1;
                if (time == 0) return startValue;
                if ((time /= duration) == 1) return startValue + changeValue;
                if (period == 0) period = duration*0.3f;
                if (overshootOrAmplitude == 0 || (changeValue > 0 && overshootOrAmplitude < changeValue) || (changeValue < 0 && overshootOrAmplitude < -changeValue)) {
                    overshootOrAmplitude = changeValue;
                    s1 = period/4;
                } else s1 = period / _TwoPi * (float)Math.Asin(changeValue / overshootOrAmplitude);
                return (overshootOrAmplitude * (float)Math.Pow(2, -10 * time) * (float)Math.Sin((time * duration - s1) * _TwoPi / period) + changeValue + startValue);
            case Ease.InOutElastic:
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
            case Ease.InBack:
                return changeValue * (time /= duration) * time * ((overshootOrAmplitude + 1) * time - overshootOrAmplitude) + startValue;
            case Ease.OutBack:
                return changeValue*((time = time/duration - 1)*time*((overshootOrAmplitude + 1)*time + overshootOrAmplitude) + 1) + startValue;
            case Ease.InOutBack:
                if ((time /= duration * 0.5f) < 1) return changeValue * 0.5f * (time * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time - overshootOrAmplitude)) + startValue;
                return changeValue / 2 * ((time -= 2) * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time + overshootOrAmplitude) + 2) + startValue;
            case Ease.InBounce:
                return Bounce.EaseIn(time, startValue, changeValue, duration, overshootOrAmplitude, period);
            case Ease.OutBounce:
                return Bounce.EaseOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
            case Ease.InOutBounce:
                return Bounce.EaseInOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
            case Ease.Custom:
                return t.customEase(time, startValue, changeValue, duration, overshootOrAmplitude, period);
            default:
                // OutQuad
                return -changeValue * (time /= duration) * (time - 2) + startValue;
            }
        }
    }
}