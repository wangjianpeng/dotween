// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/05 18:31
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

#pragma warning disable 1573
namespace DG.Tweening
{
    /// <summary>
    /// Miscellaneous extensions
    /// </summary>
    public static class Extensions
    {
        // ===================================================================================
        // TWEENERS + SEQUENCES --------------------------------------------------------------

        ///////////////////////////////////////////////////
        // Play operations ////////////////////////////////

        /// <summary>Completes the tween</summary>
        public static void Complete(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }

            TweenManager.Complete(t);
        }

        /// <summary>Flips the direction of this tween (backwards if it was going forward or viceversa)</summary>
        public static void Flip(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }

            TweenManager.Flip(t);
        }

        /// <summary>Send the tween to the given position in time</summary>
        /// <param name="to">Time position to reach
        /// (if higher than the whole tween duration the tween will simply reach its end)</param>
        /// <param name="andPlay">If TRUE will play the tween after reaching the given position, otherwise it will pause it</param>
        public static void Goto(this Tween t, float to, bool andPlay = false)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }

            if (to < 0) to = 0;
            TweenManager.Goto(t, to, andPlay);
        }

        /// <summary>Kills the tween</summary>
        public static void Kill(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }

            if (TweenManager.isUpdateLoop) {
                // Just mark it for killing, so the update loop will take care of it
                t.active = false;
            } else TweenManager.Despawn(t);
        }

        /// <summary>Pauses the tween</summary>
        public static T Pause<T>(this T t) where T : Tween
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }

            TweenManager.Pause(t);
            return t;
        }

        /// <summary>Plays the tween</summary>
        public static T Play<T>(this T t) where T : Tween
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }

            TweenManager.Play(t);
            return t;
        }

        /// <summary>Sets the tween in a forward direction and plays it</summary>
        public static void PlayBackwards(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }

            TweenManager.PlayBackwards(t);
        }

        /// <summary>Sets the tween in a backwards direction and plays it</summary>
        public static void PlayForward(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }

            TweenManager.PlayForward(t);
        }

        /// <summary>Restarts the tween from the beginning</summary>
        /// <param name="includeDelay">If TRUE includes the eventual tween delay, otherwise skips it</param>
        public static void Restart(this Tween t, bool includeDelay = true)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }

            TweenManager.Restart(t, includeDelay);
        }

        /// <summary>Rewinds the tween</summary>
        /// <param name="includeDelay">If TRUE includes the eventual tween delay, otherwise skips it</param>
        public static void Rewind(this Tween t, bool includeDelay = true)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }

            TweenManager.Rewind(t, includeDelay);
        }

        /// <summary>Plays the tween if it was paused, pauses it if it was playing</summary>
        public static void TogglePause(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }

            TweenManager.TogglePause(t);
        }

        ///////////////////////////////////////////////////
        // Info getters ///////////////////////////////////

        /// <summary>Returns the total number of loops completed by this tween</summary>
        public static int CompletedLoops(this Tween t)
        {
            return t.completedLoops;
        }

        /// <summary>Returns the current position of this tween</summary>
        public static float Position(this Tween t)
        {
            return t.position;
        }

        /// <summary>Returns the duration of this tween</summary>
        public static float Duration(this Tween t)
        {
            return t.duration;
        }

        /// <summary>Returns the full duration (loops included) of this tween</summary>
        public static float FullDuration(this Tween t)
        {
            return t.fullDuration;
        }

        /// <summary>Returns the total elapsed time for this tween</summary>
        public static float Elapsed(this Tween t)
        {
            int loopsToCount = t.position >= t.duration ? t.completedLoops - 1 : t.completedLoops;
            return (loopsToCount * t.duration) + t.position;
        }

        /// <summary>Returns FALSE if this tween has been killed</summary>
        public static bool IsActive(this Tween t)
        {
            return t.active;
        }

        /// <summary>Returns TRUE if this tween was reversed and is set to go backwards</summary>
        public static bool IsBackwards(this Tween t)
        {
            return t.isBackwards;
        }

        /// <summary>Returns TRUE if this tween is playing</summary>
        public static bool IsPlaying(this Tween t)
        {
            return t.isPlaying;
        }
    }
}