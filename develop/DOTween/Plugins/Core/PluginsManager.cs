// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/06 18:11
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

using System;
using UnityEngine;

namespace DG.Tween.Plugins.Core
{
    internal static class PluginsManager
    {
        static FloatPlugin _floatPlugin;
        static Vector3Plugin _vector3Plugin;

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        internal static ABSTweenPlugin<T> GetPlugin<T>()
        {
            // TODO Improve

            Type t = typeof(T);

            if (t == typeof(float)) {
                if (_floatPlugin == null) _floatPlugin = new FloatPlugin();
                return _floatPlugin as ABSTweenPlugin<T>;
            }
            if (t == typeof(Vector3)) {
                if (_vector3Plugin == null) _vector3Plugin = new Vector3Plugin();
                return _vector3Plugin as ABSTweenPlugin<T>;
            }

            return null;
        }

        // Un-caches all plugins
        internal static void PurgeAll()
        {
            _floatPlugin = null;
            _vector3Plugin = null;
        }
    }
}