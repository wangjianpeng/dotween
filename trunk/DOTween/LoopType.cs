// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/05 13:35
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
namespace DG.Tweening
{
    /// <summary>
    /// Types of loop
    /// </summary>
    public enum LoopType
    {
        /// <summary>Each loop cycle restarts from the beginning</summary>
        Restart,
        /// <summary>The tween moves forward and backwards at alternate cycles</summary>
        Yoyo,
        /// <summary>Continuously increments the tween at the end of each loop cycle (A to B, B to B+(A-B), and so on),
        /// thus always moving "onward".
        /// <para>Doesn't work with Rect, String and RectOffset values, where it will revert to Restart</para></summary>
        Incremental
    }
}