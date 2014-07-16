// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/16 19:13

using DG.Tweening.Core.Enums;

namespace DG.Tweening.Core
{
    internal struct ApplyTweenData
    {
        internal float prevPosition;
        internal int prevCompletedLoops;
        internal int newCompletedSteps;
        internal bool useInversePosition;
        internal UpdateMode updateMode;

        public ApplyTweenData(float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode)
        {
            this.prevPosition = prevPosition;
            this.prevCompletedLoops = prevCompletedLoops;
            this.newCompletedSteps = newCompletedSteps;
            this.useInversePosition = useInversePosition;
            this.updateMode = updateMode;
        }
    }
}