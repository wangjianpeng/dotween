// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/06/30 19:29
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 

using DG.Tween.Core.Easing;

namespace DG.Tween.Core
{
    public static class Utils
    {
        public static EaseFunction GetEaseFuncByType(EaseType easeType)
        {
            switch (easeType) {
                case EaseType.Linear: return Linear.EaseNone;
                case EaseType.InSine: return Sine.EaseIn;
                case EaseType.OutSine: return Sine.EaseOut;
                case EaseType.InOutSine: return Sine.EaseInOut;
                case EaseType.InQuad: return Quad.EaseIn;
                case EaseType.OutQuad: return Quad.EaseOut;
                case EaseType.InOutQuad: return Quad.EaseInOut;
                case EaseType.InCubic: return Cubic.EaseIn;
                case EaseType.OutCubic: return Cubic.EaseOut;
                case EaseType.InOutCubic: return Cubic.EaseInOut;
                case EaseType.InQuart: return Quart.EaseIn;
                case EaseType.OutQuart: return Quart.EaseOut;
                case EaseType.InOutQuart: return Quart.EaseInOut;
                case EaseType.InQuint: return Quint.EaseIn;
                case EaseType.OutQuint: return Quint.EaseOut;
                case EaseType.InOutQuint: return Quint.EaseInOut;
                case EaseType.InExpo: return Expo.EaseIn;
                case EaseType.OutExpo: return Expo.EaseOut;
                case EaseType.InOutExpo: return Expo.EaseInOut;
                case EaseType.InCirc: return Circ.EaseIn;
                case EaseType.OutCirc: return Circ.EaseOut;
                case EaseType.InOutCirc: return Circ.EaseInOut;
                case EaseType.InElastic: return Elastic.EaseIn;
                case EaseType.OutElastic: return Elastic.EaseOut;
                case EaseType.InOutElastic: return Elastic.EaseInOut;
                case EaseType.InBack: return Back.EaseIn;
                case EaseType.OutBack: return Back.EaseOut;
                case EaseType.InOutBack: return Back.EaseInOut;
                case EaseType.InBounce: return Bounce.EaseIn;
                case EaseType.OutBounce: return Bounce.EaseOut;
                case EaseType.InOutBounce: return Bounce.EaseInOut;
                default: return Quad.EaseOut;
            }
        }
    }
}