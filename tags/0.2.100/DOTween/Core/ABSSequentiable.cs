// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/16 11:43
namespace DG.Tweening.Core
{
    public abstract class ABSSequentiable
    {
        internal TweenType tweenType;
        internal float sequencedPosition; // position in Sequence
        internal float sequencedEndPosition; // end position in Sequence
        public TweenCallback onStart; // Used also by SequenceCallback as main callback
    }
}