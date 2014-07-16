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
using DG.Tweening.Core.Easing;
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
        public float timeScale;
        public bool isBackwards;
        public int id = -1;
        public string stringId;
        public UnityEngine.Object unityObjectId;
//        public TweenCallback onStart; // (in ABSSequentiable) When the tween is set in a PLAY state the first time, AFTER any eventual delay
        public TweenCallback onStepComplete;
        public TweenCallback onComplete;
        // Fixed after creation
        internal bool autoKill;
        internal float duration;
        internal int loops;
        internal LoopType loopType;
        // Tweeners-only (shared by Sequences only for compatibility reasons, otherwise not used)
        internal float delay;
        internal bool isRelative;
        internal EaseFunction ease;
        internal EaseCurve easeCurve; // Stored in case of AnimationCurve ease

        // SETUP DATA ////////////////////////////////////////////////

        internal UpdateType updateType;
        internal Type typeofT1; // Only used by Tweeners
        internal Type typeofT2; // Only used by Tweeners
        internal Type typeofTPlugOptions; // Only used by Tweeners
        internal bool active; // FALSE when tween is despawned - set only by TweenManager
        internal bool isSequenced; // Set by Sequence when adding a Tween to it

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
        // PUBLIC METHODS --------------------------------------------------------------------

        public virtual void Reset()
        {
            DoReset(this);
        }

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        // Called by TweenManager in case a tween has a delay that needs to be updated.
        // Returns the eventual time in excess compared to the tween's delay time.
        // Shared also by Sequences even if they don't use it, in order to make it compatible with Tween.
        internal virtual float UpdateDelay(float elapsed) { return 0; }

        // Called by TweenManager at each update.
        // Returns TRUE if the tween needs to be killed
        internal abstract bool Goto(UpdateData updateData);

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

//        // Goto part shared by Tweeners and Sequences
//        static internal GotoSharedResult DoGotoShared(Tween t, UpdateData updateData, bool getOnlyNewCompletedSteps = false)
//        {
//            
//
//            return new GotoSharedResult(wasComplete, newCompletedSteps, updatePosition);
//        }

        // ===================================================================================
        // METHODS ---------------------------------------------------------------------------

        // Doesn't reset active state, since that is touched only by TweenManager
        static void DoReset(Tween t)
        {
            t.autoKill = DOTween.autoKill;
            t.timeScale = 1;
            t.isBackwards = false;
            t.unityObjectId = null;
            t.stringId = null;
            t.id = -1;
            t.onStart = t.onComplete = null;

            t.duration = 0;
            t.loops = 1;
            t.loopType = LoopType.Restart;
            t.delay = 0;
            t.isRelative = false;
            t.ease = null;
            t.easeCurve = null;
            t.updateType = UpdateType.Default;
            t.isSequenced = false;
            t.creationLocked = t.startupDone = t.playedOnce = false;
            t.position = t.fullDuration = t.completedLoops = 0;
            t.isPlaying = t.isComplete = false;
            t.elapsedDelay = 0;
            t.delayComplete = true;
        }
    }
}