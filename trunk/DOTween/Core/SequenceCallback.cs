// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/16 11:38
namespace DG.Tweening.Core
{
    public class SequenceCallback : ISequentiable
    {
        public TweenType tweenType { get; private set; }
        internal TweenCallback callback;

        public SequenceCallback(TweenCallback callback)
        {
            tweenType = TweenType.Callback;
            this.callback = callback;
        }
    }
}