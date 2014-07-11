// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/05 18:31
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

using DG.Tween.Core;

namespace DG.Tween
{
    public static class Extensions
    {
        // ===================================================================================
        // TWEENER + SEQUENCES ---------------------------------------------------------------

        // Play operations

        public static void Complete(this Tween t)
        {
            TweenManager.Complete(t);
        }
        
        public static void Flip(this Tween t)
        {
            TweenManager.Flip(t);
        }

        public static void Goto(this Tween t, float to, bool andPlay = false)
        {
            TweenManager.Goto(t, to, andPlay);
        }

        public static void Kill(this Tween t)
        {
            if (TweenManager.IsActive(t)) {
                if (TweenManager.isUpdateLoop && TweenManager.updateLoopType == t.updateType) {
                    // Just mark it for killing, so the update loop will take care of it
                    t.active = false;
                } else TweenManager.Despawn(t);
            }
        }

        public static Tween Pause(this Tween t)
        {
            TweenManager.Pause(t);
            return t;
        }

        public static void Play(this Tween t)
        {
            TweenManager.Play(t);
        }

        public static void PlayBackwards(this Tween t)
        {
            TweenManager.PlayBackwards(t);
        }

        public static void PlayForward(this Tween t)
        {
            TweenManager.PlayForward(t);
        }

        public static void Restart(this Tween t, bool includeDelay = true)
        {
            TweenManager.Restart(t, includeDelay);
        }

        public static void Rewind(this Tween t, bool includeDelay = true)
        {
            TweenManager.Rewind(t, includeDelay);
        }

        // Info getters

        public static int CompletedLoops(this Tween t)
        {
            return t.completedLoops;
        }

        public static float Position(this Tween t)
        {
            return t.position;
        }

        public static float Elapsed(this Tween t)
        {
            int loopsToCount = t.position >= t.duration ? t.completedLoops - 1 : t.completedLoops;
            return (loopsToCount * t.duration) + t.position;
        }
    }
}