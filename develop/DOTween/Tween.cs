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
        public TweenCallback onStart;
        public TweenCallback onStepComplete;
        public TweenCallback onComplete;
        // Fixed after creation
        internal bool autoKill;
        internal float duration;
        internal int loops;
        internal LoopType loopType;

        // SETUP DATA ////////////////////////////////////////////////

        internal UpdateType updateType;
        internal TweenType tweenType;
        internal Type type; // Only used by Tweeners
        internal bool active; // FALSE when tween is despawned - set only by TweenManager

        // PLAY DATA /////////////////////////////////////////////////

        internal bool creationLocked; // TRUE after the tween was updated the first time (even if it was delayed)
        internal bool startupDone; // Called when the tween begins, AFTER any delay is elapsed
        internal float position; // Time position within a single loop cycle
//        internal float elapsed; // Total elapsed time since beginning, loops included, delays excluded (becomes very imprecise with thousands of loops so better not use it at all)
        internal float fullDuration; // Total duration loops included
        internal int completedLoops;
        internal bool isPlaying; // Set by TweenManager when getting a new tween
        internal bool isComplete;

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------

        public virtual void Reset()
        {
            DoReset(this);
        }

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        // Called by TweenManager at each update.
        // Returns TRUE if the tween needs to be killed
        internal abstract bool Goto(float position, int completedLoops);

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
            t.updateType = UpdateType.Default;
            t.creationLocked = t.startupDone = false;
            t.position = t.fullDuration = t.completedLoops = 0;
//            t.elapsed = 0;
            t.isPlaying = t.isComplete = false;
        }
    }
}