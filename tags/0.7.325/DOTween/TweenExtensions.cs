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
using UnityEngine;

#pragma warning disable 1573
namespace DG.Tweening
{
    /// <summary>
    /// Methods that extend Tween objects and allow to control or get data from them
    /// </summary>
    public static class TweenExtensions
    {
        // ===================================================================================
        // TWEENERS + SEQUENCES --------------------------------------------------------------

        #region Runtime Operations

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

        /// <summary>Sets the tween in a backwards direction and plays it</summary>
        public static void PlayBackwards(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }

            TweenManager.PlayBackwards(t);
        }

        /// <summary>Sets the tween in a forward direction and plays it</summary>
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
        #endregion

        #region Yield Coroutines

        /// <summary>
        /// Creates a yield instruction that waits until the tween is killed or complete.
        /// It can be used inside a coroutine as a yield.
        /// <para>Example usage:</para><code>yield return myTween.WaitForCompletion();</code>
        /// </summary>
        public static YieldInstruction WaitForCompletion(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }

            return DOTween.instance.StartCoroutine(DOTween.instance.WaitForCompletion(t));
        }

        /// <summary>
        /// Creates a yield instruction that waits until the tween is killed.
        /// It can be used inside a coroutine as a yield.
        /// <para>Example usage:</para><code>yield return myTween.WaitForKill();</code>
        /// </summary>
        public static YieldInstruction WaitForKill(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }

            return DOTween.instance.StartCoroutine(DOTween.instance.WaitForKill(t));
        }

        /// <summary>
        /// Creates a yield instruction that waits until the tween is killed or has gone through the given amount of loops.
        /// It can be used inside a coroutine as a yield.
        /// <para>Example usage:</para><code>yield return myTween.WaitForElapsedLoops(2);</code>
        /// </summary>
        /// <param name="elapsedLoops">Elapsed loops to wait for</param>
        public static YieldInstruction WaitForElapsedLoops(this Tween t, int elapsedLoops)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }

            return DOTween.instance.StartCoroutine(DOTween.instance.WaitForElapsedLoops(t, elapsedLoops));
        }

        /// <summary>
        /// Creates a yield instruction that waits until the tween is killed or has reached the given position (loops included, delays excluded).
        /// It can be used inside a coroutine as a yield.
        /// <para>Example usage:</para><code>yield return myTween.WaitForPosition(2.5f);</code>
        /// </summary>
        /// <param name="position">Position (loops included, delays excluded) to wait for</param>
        public static YieldInstruction WaitForPosition(this Tween t, float position)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }

            return DOTween.instance.StartCoroutine(DOTween.instance.WaitForPosition(t, position));
        }

        /// <summary>
        /// Creates a yield instruction that waits until the tween is killed or started
        /// (meaning when the tween is set in a playing state the first time, after any eventual delay).
        /// It can be used inside a coroutine as a yield.
        /// <para>Example usage:</para><code>yield return myTween.WaitForStart();</code>
        /// </summary>
        public static Coroutine WaitForStart(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }

            return DOTween.instance.StartCoroutine(DOTween.instance.WaitForStart(t));
        }
        #endregion

        #region Info Getters

        /// <summary>Returns the total number of loops completed by this tween</summary>
        public static int CompletedLoops(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return 0;
            }

            return t.completedLoops;
        }

        /// <summary>Returns the duration of this tween (delays excluded)</summary>
        /// <param name="includeLoops">If TRUE returns the full duration loops included,
        ///  otherwise the duration of a single loop cycle</param>
        public static float Duration(this Tween t, bool includeLoops = true)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return 0;
            }

            // Calculate duration here instead than getting fullDuration because fullDuration might
            // not be set yet, since it's set inside DoStartup
            if (includeLoops) return t.loops == -1 ? Mathf.Infinity : t.duration * t.loops;
            return t.duration;
        }

        /// <summary>Returns the elapsed time for this tween (delays exluded)</summary>
        /// <param name="includeLoops">If TRUE returns the elapsed time since startup loops included,
        ///  otherwise the elapsed time within the current loop cycle</param>
        public static float Elapsed(this Tween t, bool includeLoops = true)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return 0;
            }

            if (includeLoops) {
                int loopsToCount = t.position >= t.duration ? t.completedLoops - 1 : t.completedLoops;
                return (loopsToCount * t.duration) + t.position;
            }
            return t.position;
        }
        /// <summary>Returns the elapsed percentage (0 to 1) of this tween (delays exluded)</summary>
        /// <param name="includeLoops">If TRUE returns the elapsed percentage since startup loops included,
        ///  otherwise the elapsed percentage within the current loop cycle</param>
        public static float ElapsedPercentage(this Tween t, bool includeLoops = true)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return 0;
            }

            if (includeLoops) {
                int loopsToCount = t.position >= t.duration ? t.completedLoops - 1 : t.completedLoops;
                return ((loopsToCount * t.duration) + t.position) / t.fullDuration;
            }
            return t.position / t.duration;
        }

        /// <summary>Returns FALSE if this tween has been killed.
        /// <para>BEWARE: if this tween is recyclable it might have been spawned again for another use and thus return TRUE anyway.</para>
        /// When working with recyclable tweens you should take care to know when a tween has been killed and manually set your references to NULL.
        /// If you want to be sure your references are set to NULL when a tween is killed you can use the <code>OnKill</code> callback like this:
        /// <para><code>.OnKill(()=> myTweenReference = null)</code></para></summary>
        public static bool IsActive(this Tween t)
        {
            return t.active;
        }

        /// <summary>Returns TRUE if this tween was reversed and is set to go backwards</summary>
        public static bool IsBackwards(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return false;
            }

            return t.isBackwards;
        }

        /// <summary>Returns TRUE if this tween is playing</summary>
        public static bool IsPlaying(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return false;
            }

            return t.isPlaying;
        }
        #endregion
    }
}