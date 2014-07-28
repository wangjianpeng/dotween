// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/07 13:03
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

using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;

namespace DG.Tweening
{
    /// <summary>
    /// Shared by Tweeners and Sequences
    /// </summary>
    public abstract class Tween : ABSSequentiable
    {
        // OPTIONS ///////////////////////////////////////////////////

        // Modifiable at runtime
        /// <summary>TimeScale for the tween</summary>
        public float timeScale;
        /// <summary>If TRUE the tween wil go backwards</summary>
        public bool isBackwards;
        /// <summary>Int id (usable for filtering with DOTween static methods)</summary>
        public int id = -1;
        /// <summary>String id (usable for filtering with DOTween static methods)</summary>
        public string stringId;
        /// <summary>Object id (usable for filtering with DOTween static methods)</summary>
        public object objId;
        // Update type (changed via TweenManager.SetUpdateType)
        internal UpdateType updateType;
//        public TweenCallback onStart; // (in ABSSequentiable) When the tween is set in a PLAY state the first time, AFTER any eventual delay
        /// <summary>Called the moment the tween completes one loop cycle</summary>
        public TweenCallback onStepComplete;
        /// <summary>Called the moment the tween reaches completion (loops included)</summary>
        public TweenCallback onComplete;
        
        // Fixed after creation
        internal bool isFrom;
        internal bool isSpeedBased;
        internal bool autoKill;
        internal float duration;
        internal int loops;
        internal LoopType loopType;
        // Tweeners-only (shared by Sequences only for compatibility reasons, otherwise not used)
        internal float delay;
        internal bool isRelative;
        internal EaseType easeType;
        internal EaseFunction easeCurveEval; // Used only for AnimationCurve ease

        // SETUP DATA ////////////////////////////////////////////////

        internal Type typeofT1; // Only used by Tweeners
        internal Type typeofT2; // Only used by Tweeners
        internal Type typeofTPlugOptions; // Only used by Tweeners
        internal bool active; // FALSE when tween is despawned - set only by TweenManager
        internal bool isSequenced; // Set by Sequence when adding a Tween to it
        internal int activeId = -1; // Index inside its active list (touched only by TweenManager)

        // PLAY DATA /////////////////////////////////////////////////

        internal bool creationLocked; // TRUE after the tween was updated the first time (even if it was delayed), or when added to a Sequence
        internal bool startupDone; // TRUE the first time the actual tween starts, AFTER any delay has elapsed (unless it's a FROM tween)
        internal bool playedOnce; // TRUE after the tween was set in a play state at least once, AFTER any delay is elapsed
        internal float position; // Time position within a single loop cycle
        internal float fullDuration; // Total duration loops included
        internal int completedLoops;
        internal bool isPlaying; // Set by TweenManager when getting a new tween
        internal bool isComplete;
        internal float elapsedDelay; // Amount of eventual delay elapsed (shared by Sequences only for compatibility reasons, otherwise not used)
        internal bool delayComplete = true; // TRUE when the delay has elapsed or isn't set, also set by Delay extension method (shared by Sequences only for compatibility reasons, otherwise not used)

        // ===================================================================================
        // INTERNAL + ABSTRACT METHODS -------------------------------------------------------

        // Doesn't reset active state and activeId, since those are only touched by TweenManager
        internal virtual void Reset()
        {
            timeScale = 1;
            isBackwards = false;
            id = -1;
            stringId = null;
            objId = null;
            updateType = UpdateType.Default;
            onStart = onComplete = onStepComplete = null;

            isFrom = isSpeedBased = false;
            autoKill = DOTween.defaultAutoKill;
            duration = 0;
            loops = 1;
            loopType = LoopType.Restart;
            delay = 0;
            isRelative = false;
            easeCurveEval = null;
            isSequenced = false;
            creationLocked = startupDone = playedOnce = false;
            position = fullDuration = completedLoops = 0;
            isPlaying = isComplete = false;
            elapsedDelay = 0;
            delayComplete = true;
        }

        // Called by TweenManager in case a tween has a delay that needs to be updated.
        // Returns the eventual time in excess compared to the tween's delay time.
        // Shared also by Sequences even if they don't use it, in order to make it compatible with Tween.
        internal virtual float UpdateDelay(float elapsed) { return 0; }

        // Called the moment the tween starts.
        // For tweeners, that means AFTER any delay has elapsed
        // (unless it's a FROM tween, in which case it will be called BEFORE any eventual delay).
        // Returns TRUE in case of success,
        // FALSE if there are missing references and the tween needs to be killed
        internal abstract bool Startup();

        // Applies the tween set by DoGoto.
        // Returns TRUE if the tween needs to be killed
        internal abstract bool ApplyTween(float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode);

        // ===================================================================================
        // INTERNAL STATIC METHODS -----------------------------------------------------------

        // Instead of advancing the tween from the previous position each time,
        // uses the given position to calculate running time since startup, and places the tween there like a Goto.
        // Executes regardless of whether the tween is playing.
        // Returns TRUE if the tween needs to be killed
        internal static bool DoGoto(Tween t, float toPosition, int toCompletedLoops, UpdateMode updateMode)
        {
            // Startup
            if (!t.startupDone) {
                if (!t.Startup()) return true;
            }
            // OnStart callback
            if (!t.playedOnce && updateMode == UpdateMode.Update) {
                t.playedOnce = true;
                if (t.onStart != null) {
                    t.onStart();
                    // Tween might have been killed by onStart callback: verify
                    if (!t.active) return true;
                }
            }

            float prevPosition = t.position;
            int prevCompletedLoops = t.completedLoops;
            t.completedLoops = toCompletedLoops;
            bool wasRewinded = t.position <= 0 && prevCompletedLoops <= 0;
            bool wasComplete = t.isComplete;
            // Determine if it will be complete after update
            if (t.loops != -1) t.isComplete = t.completedLoops == t.loops;
            // Calculate newCompletedSteps only if an onStepComplete callback is present and might be called
            int newCompletedSteps = 0;
            if (t.onStepComplete != null && updateMode == UpdateMode.Update) {
                if (t.isBackwards) {
                    newCompletedSteps = t.completedLoops < prevCompletedLoops ? prevCompletedLoops - t.completedLoops : (toPosition <= 0 && !wasRewinded ? 1 : 0);
                    if (wasComplete) newCompletedSteps--;
                } else newCompletedSteps = t.completedLoops > prevCompletedLoops ? t.completedLoops - prevCompletedLoops : 0;
            }

            // Set position (makes position 0 equal to position "end" when looping)
            t.position = toPosition;
            if (t.position > t.duration) t.position = t.duration;
            else if (t.position <= 0) {
                if (t.completedLoops > 0 || t.isComplete) t.position = t.duration;
                else t.position = 0;
            }
            // Set playing state after update
            if (t.isPlaying) {
                if (!t.isBackwards) t.isPlaying = !t.isComplete; // Reached the end
                else t.isPlaying = !(t.completedLoops == 0 && t.position <= 0); // Rewinded
            }

            // updatePosition is different in case of Yoyo loop under certain circumstances
            bool useInversePosition = t.loopType == LoopType.Yoyo
                && (t.position < t.duration ? t.completedLoops % 2 != 0 : t.completedLoops % 2 == 0);

            // Get values from plugin and set them
            if (t.ApplyTween(prevPosition, prevCompletedLoops, newCompletedSteps, useInversePosition, updateMode)) return true;

            // Additional callbacks
            if (newCompletedSteps > 0) {
                // Already verified that onStepComplete is present
                for (int i = 0; i < newCompletedSteps; ++i) t.onStepComplete();
            }
            if (t.isComplete && !wasComplete) {
                if (t.onComplete != null) t.onComplete();
            }

            // Return
            return t.autoKill && t.isComplete;
        }
    }
}