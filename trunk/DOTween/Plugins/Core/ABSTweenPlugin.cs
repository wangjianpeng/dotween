// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/07 00:41
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

using DG.Tween.Core;

namespace DG.Tween.Plugins.Core
{
    public abstract class ABSTweenPlugin<T1,T2,TPlugOptions> : ITweenPlugin
    {
        // getter is there because some plugins might need it
        public abstract T2 ConvertT1toT2(TPlugOptions options, T1 value);
        public abstract T2 GetRelativeEndValue(TPlugOptions options, T2 startValue, T2 changeValue);
        public abstract T2 GetChangeValue(TPlugOptions options, T2 startValue, T2 endValue);
        public abstract T1 Calculate(TPlugOptions options, MemberGetter<T1> getter, float elapsed, T2 startValue, T2 changeValue, float duration, EaseFunction ease);
    }
}