// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/01/29 12:57

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;

namespace DG.Tweening
{
    /// <summary>
    /// Creates virtual tweens that can be used to change other elements via their OnUpdate calls
    /// </summary>
    public static class DOVirtual
    {
        #region Virtual Tweens

        /// <summary>
        /// Tweens a virtual float.
        /// You can add regular setting to the generated tween,
        /// but do not use <code>SetUpdate</code> or you will overwrite the onVirtualUpdate parameter
        /// </summary>
        /// <param name="from">The value to start from</param>
        /// <param name="to">The value to tween to</param>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="onVirtualUpdate">A callback which must accept a parameter of type float, called at each update</param>
        /// <returns></returns>
        public static Tweener Float(float from, float to, float duration, TweenCallback<float> onVirtualUpdate)
        {
            float val = from;
            return DOTween.To(() => val, x => val = x, to, duration).OnUpdate(() => onVirtualUpdate(val));
        }

        #endregion

//        #region Virtual Functions
//
//        public static float EaseValueAtTime(float time, float duration, Ease easeType)
//        {
//            return EaseManager.
//        }
//
//        #endregion
    }
}