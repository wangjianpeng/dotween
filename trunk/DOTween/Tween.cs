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
using DG.Tween.Core;
using UnityEngine;

namespace DG.Tween
{
    /// <summary>
    /// Shared by Tweeners and Sequences
    /// </summary>
    public abstract class Tween
    {
        // OPTIONS ///////////////////////////////////////////////////

        // Modifiable at runtime
        public float timeScale;
        public bool isBackwards;
        public int id = -1;
        public string stringId;
        public UnityEngine.Object unityObjectId;
        public TweenCallback onStart; // When the tween starts, AFTER any eventual delay
        public TweenCallback onStepComplete;
        public TweenCallback onComplete;
        // Fixed after creation
        internal bool autoKill;
        internal float duration;
        internal int loops;
        internal LoopType loopType;
        internal float delay; // Shared by Sequences only for compatibility reasons, otherwise not used

        // SETUP DATA ////////////////////////////////////////////////

        internal UpdateType updateType;
        internal TweenType tweenType;
        internal Type type; // Only used by Tweeners
        internal bool active; // FALSE when tween is despawned - set only by TweenManager

        // PLAY DATA /////////////////////////////////////////////////

        internal bool creationLocked; // TRUE after the tween was updated the first time (even if it was delayed)
        internal bool startupDone; // TRUE after the tween begins, AFTER any delay is elapsed
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
        internal abstract float UpdateDelay(float elapsed);

        // Called by TweenManager at each update.
        // Returns TRUE if the tween needs to be killed
        internal abstract bool Goto(UpdateData updateData);

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
            t.updateType = UpdateType.Default;
            t.creationLocked = t.startupDone = false;
            t.position = t.fullDuration = t.completedLoops = 0;
            t.isPlaying = t.isComplete = false;
            t.elapsedDelay = 0;
            t.delayComplete = false;
        }
    }
}