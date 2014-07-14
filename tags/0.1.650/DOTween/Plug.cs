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

        // Float
        public static PlugFloat.Options FloatOptions(bool snapping)
        {
            return new PlugFloat.Options(snapping);
        }

        // String
        public static PlugString.Options StringOptions(bool scramble)
        {
            return new PlugString.Options(scramble);
        }

        // Vector2
        public static PlugVector2.Options Vector2Options(bool snapping)
        {
            return new PlugVector2.Options(snapping);
        }
        // Vector3
        public static PlugVector3.Options Vector3Options(bool snapping)
        {
            return new PlugVector3.Options(snapping);
        }
        // Vector4
        public static PlugVector4.Options Vector4Options(bool snapping)
        {
            return new PlugVector4.Options(snapping);
        }

        // Rect
        public static PlugRect.Options RectOptions(bool snapping)
        {
            return new PlugRect.Options(snapping);
        }

        ///////////////////////////////////////////////////////////////
        // CUSTOM PLUGINS /////////////////////////////////////////////

        // Vector2X
        public static PlugVector2X Vector2X(MemberGetter<Vector2> getter, MemberSetter<Vector2> setter, float endValue)
        {
            return new PlugVector2X(getter, setter, endValue);
        }
        public static PlugVector2X Vector2X(MemberGetter<Vector2> getter, MemberSetter<Vector2> setter, float endValue, PlugVector2X.Options options)
        {
            return new PlugVector2X(getter, setter, endValue, options);
        }
        public static PlugVector2X.Options Vector2XOptions(bool snapping)
        {
            return new PlugVector2X.Options(snapping);
        }
        // Vector2Y
        public static PlugVector2Y Vector2Y(MemberGetter<Vector2> getter, MemberSetter<Vector2> setter, float endValue)
        {
            return new PlugVector2Y(getter, setter, endValue);
        }
        public static PlugVector2Y Vector2Y(MemberGetter<Vector2> getter, MemberSetter<Vector2> setter, float endValue, PlugVector2Y.Options options)
        {
            return new PlugVector2Y(getter, setter, endValue, options);
        }
        public static PlugVector2Y.Options Vector2YOptions(bool snapping)
        {
            return new PlugVector2Y.Options(snapping);
        }

        // Vector3X
        public static PlugVector3X Vector3X(MemberGetter<Vector3> getter, MemberSetter<Vector3> setter, float endValue)
        {
            return new PlugVector3X(getter, setter, endValue);
        }
        public static PlugVector3X Vector3X(MemberGetter<Vector3> getter, MemberSetter<Vector3> setter, float endValue, PlugVector3X.Options options)
        {
            return new PlugVector3X(getter, setter, endValue, options);
        }
        public static PlugVector3X.Options Vector3XOptions(bool snapping)
        {
            return new PlugVector3X.Options(snapping);
        }
        // Vector3Y
        public static PlugVector3Y Vector3Y(MemberGetter<Vector3> getter, MemberSetter<Vector3> setter, float endValue)
        {
            return new PlugVector3Y(getter, setter, endValue);
        }
        public static PlugVector3Y Vector3Y(MemberGetter<Vector3> getter, MemberSetter<Vector3> setter, float endValue, PlugVector3Y.Options options)
        {
            return new PlugVector3Y(getter, setter, endValue, options);
        }
        public static PlugVector3Y.Options Vector3YOptions(bool snapping)
        {
            return new PlugVector3Y.Options(snapping);
        }
        // Vector3Z
        public static PlugVector3Z Vector3Z(MemberGetter<Vector3> getter, MemberSetter<Vector3> setter, float endValue)
        {
            return new PlugVector3Z(getter, setter, endValue);
        }
        public static PlugVector3Z Vector3Z(MemberGetter<Vector3> getter, MemberSetter<Vector3> setter, float endValue, PlugVector3Z.Options options)
        {
            return new PlugVector3Z(getter, setter, endValue, options);
        }
        public static PlugVector3Z.Options Vector3ZOptions(bool snapping)
        {
            return new PlugVector3Z.Options(snapping);
        }

        // Alpha (no options)
        public static PlugAlpha Alpha(MemberGetter<Color> getter, MemberSetter<Color> setter, float endValue)
        {
            return new PlugAlpha(getter, setter, endValue);
        }
    }
}