// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/16 11:43
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

#pragma warning disable 1591
namespace DG.Tweening.Core
{
    public abstract class ABSSequentiable
    {
        internal TweenType tweenType;
        internal float sequencedPosition; // position in Sequence
        internal float sequencedEndPosition; // end position in Sequence

        /// <summary>Called when the tween is set in a playing state the first time, after any eventual delay</summary>
        public TweenCallback onStart; // Used also by SequenceCallback as main callback
    }
}