// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/15 17:50

using System.Collections.Generic;
using DG.Tweening.Core;

namespace DG.Tweening
{
    public sealed class Sequence : Tween
    {
        // SETUP DATA ////////////////////////////////////////////////

        internal readonly List<Tween> sequencedTweens = new List<Tween>(); // Only Tweens
        readonly List<ISequentiable> _sequencedObjs = new List<ISequentiable>(); // Tweens plus SequenceCallbacks
        readonly List<float> _tweensPositions = new List<float>();

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
            _tweensPositions.Clear();
        }

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        // Called by DOTween when spawning/creating a new Sequence.
        internal static void Setup(Sequence s)
        {
            s.isPlaying = DOTween.defaultAutoPlayBehaviour == AutoPlay.All;
        }

        internal override bool Goto(UpdateData updateData)
        {
            return DoGoto(this, updateData);
        }

        internal static Sequence DoAppend(Sequence inSequence, Tween t)
        {
            return DoInsert(inSequence, t, inSequence.duration);
        }

        internal static Sequence DoInsert(Sequence inSequence, Tween t, float atPosition)
        {
            TweenManager.AddActiveTweenToSequence(t);

            t.isSequenced = t.creationLocked = true;
            t.autoKill = false;
            t.delay = t.elapsedDelay = 0;
            t.delayComplete = true;

            inSequence.duration += t.loops == -1 ? t.duration : t.duration * t.loops;
            inSequence._sequencedObjs.Add(t);
            inSequence.sequencedTweens.Add(t);
            return inSequence;
        }

        internal static Sequence DoAppendInterval(Sequence inSequence, float interval)
        {
            inSequence.duration += interval;
            return inSequence;
        }

        internal static bool DoStartup(Sequence s)
        {
            throw new System.NotImplementedException();
        }

        internal static bool DoGoto(Sequence s, UpdateData updateData)
        {
            throw new System.NotImplementedException();
        }
    }
}