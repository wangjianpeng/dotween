//
// Elastic.cs
//
// Author: Daniele Giardini (C# port of the easing equations created by Robert Penner - http://robertpenner.com/easing)
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

namespace DG.Tweening.Core.Easing
{
    /// <summary>
    /// This class contains a C# port of the easing equations created by Robert Penner (http://robertpenner.com/easing).
    /// </summary>
    public static class Elastic
    {
        const float TwoPi = Mathf.PI * 2;

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing in: accelerating from zero velocity.
        /// </summary>
        /// <param name="time">
        /// Current time (in frames or seconds).
        /// </param>
        /// <param name="startValue">
        /// Starting value.
        /// </param>
        /// <param name="changeValue">
        /// Change needed in value.
        /// </param>
        /// <param name="duration">
        /// Expected easing duration (in frames or seconds).
        /// </param>
        /// <returns>
        /// The eased value.
        /// </returns>
        public static float EaseIn(float time, float startValue, float changeValue, float duration)
        {
            return EaseIn(time, startValue, changeValue, duration, 0, 0);
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing in: accelerating from zero velocity.
        /// </summary>
        /// <param name="time">
        /// Current time (in frames or seconds).
        /// </param>
        /// <param name="startValue">
        /// Starting value.
        /// </param>
        /// <param name="changeValue">
        /// Change needed in value.
        /// </param>
        /// <param name="duration">
        /// Expected easing duration (in frames or seconds).
        /// </param>
        /// <param name="amplitude">
        /// Amplitude.
        /// </param>
        /// <param name="period">
        /// Period.
        /// </param>
        /// <returns>
        /// The eased value.
        /// </returns>
        public static float EaseIn(float time, float startValue, float changeValue, float duration, float amplitude, float period)
        {
            float s;
            if (time == 0)
            {
                return startValue;
            }
            if ((time /= duration) == 1)
            {
                return startValue + changeValue;
            }
            if (period == 0)
            {
                period = duration*0.3f;
            }
            if (amplitude == 0 || (changeValue > 0 && amplitude < changeValue) || (changeValue < 0 && amplitude < -changeValue))
            {
                amplitude = changeValue;
                s = period/4;
            }
            else
            {
                s = period/TwoPi*(float)Math.Asin(changeValue/amplitude);
            }
            return -(amplitude*(float)Math.Pow(2, 10*(time -= 1))*(float)Math.Sin((time*duration - s)*TwoPi/period)) + startValue;
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing out: decelerating from zero velocity.
        /// </summary>
        /// <param name="time">
        /// Current time (in frames or seconds).
        /// </param>
        /// <param name="startValue">
        /// Starting value.
        /// </param>
        /// <param name="changeValue">
        /// Change needed in value.
        /// </param>
        /// <param name="duration">
        /// Expected easing duration (in frames or seconds).
        /// </param>
        /// <returns>
        /// The eased value.
        /// </returns>
        public static float EaseOut(float time, float startValue, float changeValue, float duration)
        {
            return EaseOut(time, startValue, changeValue, duration, 0, 0);
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing out: decelerating from zero velocity.
        /// </summary>
        /// <param name="time">
        /// Current time (in frames or seconds).
        /// </param>
        /// <param name="startValue">
        /// Starting value.
        /// </param>
        /// <param name="changeValue">
        /// Change needed in value.
        /// </param>
        /// <param name="duration">
        /// Expected easing duration (in frames or seconds).
        /// </param>
        /// <param name="amplitude">
        /// Amplitude.
        /// </param>
        /// <param name="period">
        /// Period.
        /// </param>
        /// <returns>
        /// The eased value.
        /// </returns>
        public static float EaseOut(float time, float startValue, float changeValue, float duration, float amplitude, float period)
        {
            float s;
            if (time == 0)
            {
                return startValue;
            }
            if ((time /= duration) == 1)
            {
                return startValue + changeValue;
            }
            if (period == 0)
            {
                period = duration*0.3f;
            }
            if (amplitude == 0 || (changeValue > 0 && amplitude < changeValue) || (changeValue < 0 && amplitude < -changeValue))
            {
                amplitude = changeValue;
                s = period/4;
            }
            else
            {
                s = period/TwoPi*(float)Math.Asin(changeValue/amplitude);
            }
            return (amplitude*(float)Math.Pow(2, -10*time)*(float)Math.Sin((time*duration - s)*TwoPi/period) + changeValue + startValue);
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing in/out: acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="time">
        /// Current time (in frames or seconds).
        /// </param>
        /// <param name="startValue">
        /// Starting value.
        /// </param>
        /// <param name="changeValue">
        /// Change needed in value.
        /// </param>
        /// <param name="duration">
        /// Expected easing duration (in frames or seconds).
        /// </param>
        /// <returns>
        /// The eased value.
        /// </returns>
        public static float EaseInOut(float time, float startValue, float changeValue, float duration)
        {
            return EaseInOut(time, startValue, changeValue, duration, 0, 0);
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing in/out: acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="time">
        /// Current time (in frames or seconds).
        /// </param>
        /// <param name="startValue">
        /// Starting value.
        /// </param>
        /// <param name="changeValue">
        /// Change needed in value.
        /// </param>
        /// <param name="duration">
        /// Expected easing duration (in frames or seconds).
        /// </param>
        /// <param name="amplitude">
        /// Amplitude.
        /// </param>
        /// <param name="period">
        /// Period.
        /// </param>
        /// <returns>
        /// The eased value.
        /// </returns>
        public static float EaseInOut(float time, float startValue, float changeValue, float duration, float amplitude, float period)
        {
            float s;
            if (time == 0)
            {
                return startValue;
            }
            if ((time /= duration*0.5f) == 2)
            {
                return startValue + changeValue;
            }
            if (period == 0)
            {
                period = duration*(0.3f*1.5f);
            }
            if (amplitude == 0 || (changeValue > 0 && amplitude < changeValue) || (changeValue < 0 && amplitude < -changeValue))
            {
                amplitude = changeValue;
                s = period/4;
            }
            else
            {
                s = period/TwoPi*(float)Math.Asin(changeValue/amplitude);
            }
            if (time < 1)
            {
                return -0.5f*(amplitude*(float)Math.Pow(2, 10*(time -= 1))*(float)Math.Sin((time*duration - s)*TwoPi/period)) + startValue;
            }
            return amplitude*(float)Math.Pow(2, -10*(time -= 1))*(float)Math.Sin((time*duration - s)*TwoPi/period)*0.5f + changeValue + startValue;
        }
    }
}
