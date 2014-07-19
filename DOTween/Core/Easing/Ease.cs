// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/19 14:11
namespace DG.Tweening.Core.Easing
{
    public static class Ease
    {
        public static float Apply(Tween t, float time, float startValue, float changeValue, float duration, float overshootOrAmplitude, float period)
        {
            switch (t.easeType) {
                case EaseType.Linear: return Linear.EaseNone(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InSine: return Sine.EaseIn(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.OutSine: return Sine.EaseOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InOutSine: return Sine.EaseInOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InQuad: return Quad.EaseIn(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.OutQuad: return Quad.EaseOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InOutQuad: return Quad.EaseInOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InCubic: return Cubic.EaseIn(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.OutCubic: return Cubic.EaseOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InOutCubic: return Cubic.EaseInOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InQuart: return Quart.EaseIn(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.OutQuart: return Quart.EaseOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InOutQuart: return Quart.EaseInOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InQuint: return Quint.EaseIn(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.OutQuint: return Quint.EaseOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InOutQuint: return Quint.EaseInOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InExpo: return Expo.EaseIn(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.OutExpo: return Expo.EaseOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InOutExpo: return Expo.EaseInOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InCirc: return Circ.EaseIn(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.OutCirc: return Circ.EaseOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InOutCirc: return Circ.EaseInOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InElastic: return Elastic.EaseIn(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.OutElastic: return Elastic.EaseOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InOutElastic: return Elastic.EaseInOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InBack: return Back.EaseIn(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.OutBack: return Back.EaseOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InOutBack: return Back.EaseInOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InBounce: return Bounce.EaseIn(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.OutBounce: return Bounce.EaseOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.InOutBounce: return Bounce.EaseInOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                case EaseType.AnimationCurve: return t.easeCurveEval(time, startValue, changeValue, duration, overshootOrAmplitude, period);
                default: return Quad.EaseOut(time, startValue, changeValue, duration, overshootOrAmplitude, period);
            }
        }
    }
}