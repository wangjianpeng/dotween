// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/11 10:23
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

using DG.Tweening.Core;
using DG.Tweening.Plugins;
using UnityEngine;

namespace DG.Tweening
{
    /// <summary>
    /// Shortcuts to DOTween plugins creations
    /// </summary>
    public static class Plug
    {
        ///////////////////////////////////////////////////////////////
        // DEFAULT PLUGINS (options only) /////////////////////////////

        /// <summary>Options for tweening a float</summary>
        /// <param name="snapping">If TRUE, the tween will snap all values to integers</param>
        public static PlugFloat.Options FloatOptions(bool snapping)
        {
            return new PlugFloat.Options(snapping);
        }

        /// <summary>Options for tweening a string</summary>
        /// <param name="scramble">If TRUE, the tween will use a scramble effect for each character</param>
        public static PlugString.Options StringOptions(bool scramble)
        {
            return new PlugString.Options(scramble);
        }

        /// <summary>Options for tweening a Vector2</summary>
        /// <param name="snapping">If TRUE, the tween will snap all values to integers</param>
        public static PlugVector.Options Vector2Options(bool snapping)
        {
            return new PlugVector.Options(AxisConstraint.None, snapping);
        }
        /// <summary>Options for tweening a Vector3</summary>
        /// <param name="snapping">If TRUE, the tween will snap all values to integers</param>
        public static PlugVector.Options Vector3Options(bool snapping)
        {
            return new PlugVector.Options(AxisConstraint.None, snapping);
        }
        /// <summary>Options for tweening a Vector4</summary>
        /// <param name="snapping">If TRUE, the tween will snap all values to integers</param>
        public static PlugVector.Options Vector4Options(bool snapping)
        {
            return new PlugVector.Options(AxisConstraint.None, snapping);
        }

        /// <summary>Options for tweening a Rect</summary>
        /// <param name="snapping">If TRUE, the tween will snap all values to integers</param>
        public static PlugRect.Options RectOptions(bool snapping)
        {
            return new PlugRect.Options(snapping);
        }

        ///////////////////////////////////////////////////////////////
        // CUSTOM PLUGINS /////////////////////////////////////////////

        // Vector2XYZ /////////////////////////////////////////////////

        /// <summary>Vector2X plugin. Tweens only the X value of a Vector2</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// Example usage with lambda: <code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// Example usage with lambda: <code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param>
        public static PlugVector2X Vector2X(DOGetter<Vector2> getter, DOSetter<Vector2> setter, float endValue)
        {
            return new PlugVector2X(getter, setter, endValue);
        }
        /// <summary>Vector2X plugin. Tweens only the X value of a Vector2</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// Example usage with lambda: <code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// Example usage with lambda: <code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param>
        /// <param name="options">Options. Example: <code>Plug.Vector2XOptions(true)</code></param>
        public static PlugVector2X Vector2X(DOGetter<Vector2> getter, DOSetter<Vector2> setter, float endValue, PlugVector2X.Options options)
        {
            return new PlugVector2X(getter, setter, endValue, options);
        }
        /// <summary>Options for tweening a Plug.Vector2X custom plugin</summary>
        /// <param name="snapping">If TRUE, the tween will snap all values to integers</param>
        public static PlugVector2X.Options Vector2XOptions(bool snapping)
        {
            return new PlugVector2X.Options(snapping);
        }

        /// <summary>Vector2Y plugin. Tweens only the Y value of a Vector2</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// Example usage with lambda: <code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// Example usage with lambda: <code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param>
        public static PlugVector2Y Vector2Y(DOGetter<Vector2> getter, DOSetter<Vector2> setter, float endValue)
        {
            return new PlugVector2Y(getter, setter, endValue);
        }
        /// <summary>Vector2Y plugin. Tweens only the Y value of a Vector2</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// Example usage with lambda: <code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// Example usage with lambda: <code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param>
        /// <param name="options">Options. Example: <code>Plug.Vector2YOptions(true)</code></param>
        public static PlugVector2Y Vector2Y(DOGetter<Vector2> getter, DOSetter<Vector2> setter, float endValue, PlugVector2Y.Options options)
        {
            return new PlugVector2Y(getter, setter, endValue, options);
        }
        /// <summary>Options for tweening a Plug.Vector2Y custom plugin</summary>
        /// <param name="snapping">If TRUE, the tween will snap all values to integers</param>
        public static PlugVector2Y.Options Vector2YOptions(bool snapping)
        {
            return new PlugVector2Y.Options(snapping);
        }

        // Vector3XYZ /////////////////////////////////////////////////

        /// <summary>Vector3X plugin. Tweens only the X value of a Vector3</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// Example usage with lambda: <code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// Example usage with lambda: <code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param>
        public static PlugVector3X Vector3X(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float endValue)
        {
            return new PlugVector3X(getter, setter, endValue);
        }
        /// <summary>Vector3X plugin. Tweens only the X value of a Vector3</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// Example usage with lambda: <code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// Example usage with lambda: <code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param>
        /// <param name="options">Options. Example: <code>Plug.Vector3XOptions(true)</code></param>
        public static PlugVector3X Vector3X(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float endValue, PlugVector3X.Options options)
        {
            return new PlugVector3X(getter, setter, endValue, options);
        }
        /// <summary>Options for tweening a Plug.Vector3X custom plugin</summary>
        /// <param name="snapping">If TRUE, the tween will snap all values to integers</param>
        public static PlugVector3X.Options Vector3XOptions(bool snapping)
        {
            return new PlugVector3X.Options(snapping);
        }

        /// <summary>Vector3Y plugin. Tweens only the Y value of a Vector3</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// Example usage with lambda: <code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// Example usage with lambda: <code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param>
        public static PlugVector3Y Vector3Y(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float endValue)
        {
            return new PlugVector3Y(getter, setter, endValue);
        }
        /// <summary>Vector3Y plugin. Tweens only the Y value of a Vector3</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// Example usage with lambda: <code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// Example usage with lambda: <code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param>
        /// <param name="options">Options. Example: <code>Plug.Vector3YOptions(true)</code></param>
        public static PlugVector3Y Vector3Y(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float endValue, PlugVector3Y.Options options)
        {
            return new PlugVector3Y(getter, setter, endValue, options);
        }
        /// <summary>Options for tweening a Plug.Vector3Y custom plugin</summary>
        /// <param name="snapping">If TRUE, the tween will snap all values to integers</param>
        public static PlugVector3Y.Options Vector3YOptions(bool snapping)
        {
            return new PlugVector3Y.Options(snapping);
        }

        /// <summary>Vector3Z plugin. Tweens only the Z value of a Vector3</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// Example usage with lambda: <code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// Example usage with lambda: <code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param>
        public static PlugVector3Z Vector3Z(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float endValue)
        {
            return new PlugVector3Z(getter, setter, endValue);
        }
        /// <summary>Vector3Z plugin. Tweens only the Z value of a Vector3</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// Example usage with lambda: <code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// Example usage with lambda: <code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param>
        /// <param name="options">Options. Example: <code>Plug.Vector3ZOptions(true)</code></param>
        public static PlugVector3Z Vector3Z(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float endValue, PlugVector3Z.Options options)
        {
            return new PlugVector3Z(getter, setter, endValue, options);
        }
        /// <summary>Options for tweening a Plug.Vector3Z custom plugin</summary>
        /// <param name="snapping">If TRUE, the tween will snap all values to integers</param>
        public static PlugVector3Z.Options Vector3ZOptions(bool snapping)
        {
            return new PlugVector3Z.Options(snapping);
        }

        // Alpha //////////////////////////////////////////////////////

        /// <summary>Alpha plugin. Tweens only the alpha value of a Color</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// Example usage with lambda: <code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// Example usage with lambda: <code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param>
        public static PlugAlpha Alpha(DOGetter<Color> getter, DOSetter<Color> setter, float endValue)
        {
            return new PlugAlpha(getter, setter, endValue);
        }
    }
}