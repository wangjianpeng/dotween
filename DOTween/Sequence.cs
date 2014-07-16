// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/15 17:50

using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using UnityEngine;

namespace DG.Tweening
{
    public sealed class Sequence : Tween
    {
        // SETUP DATA ////////////////////////////////////////////////

        internal readonly List<Tween> sequencedTweens = new List<Tween>(); // Only Tweens (used for despawning)
        readonly List<ABSSequentiable> _sequencedObjs = new List<ABSSequentiable>(); // Tweens plus SequenceCallbacks
        bool _hasCallbacks;

        internal Sequence()
        {
            tweenType = TweenType.Sequence;
            Reset();
        }

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------

        public override void Reset()
        {
            base.Reset();

            sequencedTweens.Clear();
            _sequencedObjs.Clear();
            _hasCallbacks = false;
        }

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        // Called by DOTween when spawning/creating a new Sequence.
        internal static void Setup(Sequence s)
        {
            s.isPlaying = DOTween.defaultAutoPlayBehaviour == AutoPlay.All || DOTween.defaultAutoPlayBehaviour == AutoPlay.AutoPlaySequences;
            s.loopType = DOTween.defaultLoopType;
        }

        internal override bool Goto(UpdateData updateData)
        {
            return DoGoto(this, updateData);
        }

        internal static Sequence DoInsert(Sequence inSequence, Tween t, float atPosition)
        {
            TweenManager.AddActiveTweenToSequence(t);

            t.isSequenced = t.creationLocked = true;
            t.sequencedPosition = atPosition;
            t.sequencedEndPosition = t.sequencedPosition + t.duration;
            t.autoKill = false;
            t.delay = t.elapsedDelay = 0;
            t.delayComplete = true;
            t.onStart = t.onStepComplete = t.onComplete = null;

            float newDuration = atPosition + (t.loops == -1 ? t.duration : t.duration * t.loops);
            if (newDuration > inSequence.duration) inSequence.duration = newDuration;
            inSequence._sequencedObjs.Add(t);
            inSequence.sequencedTweens.Add(t);
            return inSequence;
        }

        internal static Sequence DoAppendInterval(Sequence inSequence, float interval)
        {
            inSequence.duration += interval;
            return inSequence;
        }

        internal static Sequence DoInsertCallback(Sequence inSequence, TweenCallback callback, float atPosition)
        {
            SequenceCallback c = new SequenceCallback(atPosition, callback);
            c.sequencedPosition = c.sequencedEndPosition = atPosition;
            inSequence._sequencedObjs.Add(c);
            inSequence._hasCallbacks = true;
            if (inSequence.duration < atPosition) inSequence.duration = atPosition;
            return inSequence;
        }

        // Called the moment the tween starts.
        // Returns TRUE in case of success (always TRUE for Sequences)
        internal static bool DoStartup(Sequence s)
        {
            s.startupDone = true;
            s.fullDuration = s.loops > -1 ? s.duration * s.loops : Mathf.Infinity;
            return true;
        }

        // Returns TRUE if the tween needs to be killed
        internal static bool DoGoto(Sequence s, UpdateData updateData)
        {
            throw new System.NotImplementedException();

//            // Startup
//            if (!s.startupDone) DoStartup(s);
//            // OnStart callback
//            if (!s.playedOnce && updateData.updateMode == UpdateMode.Update) {
//                s.playedOnce = true;
//                if (s.onStart != null) {
//                    s.onStart();
//                    // Tween might have been killed by onStart callback: verify
//                    if (!s.active) return true;
//                }
//            }
//
//            // Shared goto
//            float prevPosition = s.position;
//            int prevCompletedLoops = s.completedLoops;
//            GotoSharedResult gotoRes = DoGotoShared(s, updateData, true);
//
//            // Update routine
//            int cyclesToUpdate = gotoRes.newCompletedSteps + 1;
//            if (s.position >= s.duration) cyclesToUpdate--;
//            int cycle = 0;
//            float from = s.position;
//            float to;
//            while (cycle < cyclesToUpdate) {
//                bool isBackwardsCycle = s.loopType == LoopType.Yoyo && (s.completedLoops - cyclesToUpdate) % 2 == 0;
//                if (isBackwardsCycle) {
//                    if (cycle > 0) from = s.duration;
//                    to = 0;
//                } else {
//                    if (cycle > 0) from = 0;
//                    to = s.duration;
//                }
//                if (DoUpdateCycle(s, from, to, prevCompletedLoops + cycle)) return true;
//                cyclesToUpdate--;
//            }
        }

//        static bool DoUpdateCycle(Sequence s, float from, float to, int completedLoops)
//        {
//            
//        }

//        internal static bool DoGoto(Sequence s, UpdateData updateData)
//        {
//            float prevPosition = s.position;
//
//            // Startup
//            if (!s.startupDone) DoStartup(s);
//
//            GotoSharedResult gotoRes = DoGotoShared(s, updateData);
//            if (gotoRes.needsToBeKilled) return true;
//
//            // Update tweens
//            // Update eventual loop cycle callbacks elapsed in the meantime
//            if (s._hasCallbacks && updateData.updateMode == UpdateMode.Update) {
//                int cyclesToUpdate = gotoRes.newCompletedSteps;
//                if (s.position >= s.duration) cyclesToUpdate--;
//                if (cyclesToUpdate > 0) {
//                    float from = prevPosition;
//                    float to;
//                    bool firstCycle = true;
//                    while (cyclesToUpdate > 0) {
//                        bool isBackwardsCycle = s.loopType == LoopType.Yoyo && (s.completedLoops - cyclesToUpdate) % 2 == 0;
//                        if (s.isBackwards) isBackwardsCycle = !isBackwardsCycle;
//                        if (isBackwardsCycle) {
//                            if (!firstCycle) from = s.duration;
//                            to = 0;
//                        } else {
//                            if (!firstCycle) from = 0;
//                            to = s.duration;
//                        }
//                        if (DoUpdateCycle(s, from, to)) return true;
//                        cyclesToUpdate--;
//                        firstCycle = false;
//                    }
//                }
//            }
//
//            // Additional callbacks
//            if (gotoRes.newCompletedSteps > 0) {
//                // Already verified that onStepComplete is present
//                for (int i = 0; i < gotoRes.newCompletedSteps; ++i) s.onStepComplete();
//            }
//            if (s.isComplete && !gotoRes.wasComplete) {
//                if (s.onComplete != null) s.onComplete();
//            }
//
//            // Return
//            return s.autoKill && s.isComplete;
//        }
//
//        // Calls all callbacks in a loop cycle
//        static bool DoUpdateCycle(Sequence s, float from, float to)
//        {
//            bool isForwardCycle = from <= to;
//            if (isForwardCycle) {
//                int len = s._sequencedObjs.Count;
//                for (int i = 0; i < len; ++i) {
//                    ABSSequentiable sequentiable = s._sequencedObjs[i];
//                    if (sequentiable.tweenType == TweenType.Callback) sequentiable.onStart();
//                }
//            } else {
//                int len = s._sequencedObjs.Count - 1;
//                for (int i = len; i > -1; --i) {
//                    ABSSequentiable sequentiable = s._sequencedObjs[i];
//                    if (sequentiable.tweenType == TweenType.Callback) sequentiable.onStart();
//                }
//            }
//        }
    }
}