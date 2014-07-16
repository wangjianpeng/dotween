// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/16 12:31
namespace DG.Tweening.Core
{
    internal struct GotoSharedResult
    {
        internal bool wasComplete;
        internal int newCompletedSteps;
        internal float updatePosition; // Different from position in case of Yoyo loop under certain circumstances

        public GotoSharedResult(bool wasComplete, int newCompletedSteps, float updatePosition)
        {
            this.wasComplete = wasComplete;
            this.newCompletedSteps = newCompletedSteps;
            this.updatePosition = updatePosition;
        }
    }
}