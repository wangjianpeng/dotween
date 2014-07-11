//
// Quart.cs
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

namespace DG.Tween.Core.Easing
{
    /// <summary>
    /// This class contains a C# port of the easing equations created by Robert Penner (http://robertpenner.com/easing).
    /// </summary>
    public static class Quart
    {
        /// <summary>
        /// Easing equation function for a quartic (t^4) easing in: accelerating from zero velocity.
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
        /// <param name="unusedOvershootOrAmplitude">Unused: here to keep same delegate for all ease types.</param>
        /// <param name="unusedPeriod">Unused: here to keep same delegate for all ease types.</param>
        /// <returns>
        /// The eased value.
        /// </returns>
        public static float EaseIn(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
        {
            return changeValue*(time /= duration)*time*time*time + startValue;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing out: decelerating from zero velocity.
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
        /// <param name="unusedOvershootOrAmplitude">Unused: here to keep same delegate for all ease types.</param>
        /// <param name="unusedPeriod">Unused: here to keep same delegate for all ease types.</param>
        /// <returns>
        /// The eased value.
        /// </returns>
        public static float EaseOut(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
        {
            return -changeValue*((time = time/duration - 1)*time*time*time - 1) + startValue;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing in/out: acceleration until halfway, then deceleration.
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
        /// <param name="unusedOvershootOrAmplitude">Unused: here to keep same delegate for all ease types.</param>
        /// <param name="unusedPeriod">Unused: here to keep same delegate for all ease types.</param>
        /// <returns>
        /// The eased value.
        /// </returns>
        public static float EaseInOut(float time, float startValue, float changeValue, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
        {
            if ((time /= duration*0.5f) < 1)
            {
                return changeValue*0.5f*time*time*time*time + startValue;
            }
            return -changeValue*0.5f*((time -= 2)*time*time*time - 2) + startValue;
        }
    }
}
