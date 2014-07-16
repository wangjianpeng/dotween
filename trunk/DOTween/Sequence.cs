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

        // CALLED BY Tween the moment the tween starts.
        // Returns TRUE in case of success (always TRUE for Sequences)
        internal override bool Startup()
        {
            return DoStartup(this);
        }

        internal override bool ApplyTween(float updatePosition)
        {
            return DoApplyTween(this, updatePosition);
        }

        // Called by DOTween when spawning/creating a new Sequence.
        internal static void Setup(Sequence s)
        {
            s.isPlaying = DOTween.defaultAutoPlayBehaviour == AutoPlay.All || DOTween.defaultAutoPlayBehaviour == AutoPlay.AutoPlaySequences;
            s.loopType = DOTween.defaultLoopType;
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

        internal static bool DoStartup(Sequence s)
        {
            s.startupDone = true;
            s.fullDuration = s.loops > -1 ? s.duration * s.loops : Mathf.Infinity;
            return true;
        }

        // Applies the tween set by DoGoto.
        // Returns TRUE if the tween needs to be killed
        internal static bool DoApplyTween(Sequence s, float updatePosition)
        {
            throw new System.NotImplementedException();
        }
    }
}