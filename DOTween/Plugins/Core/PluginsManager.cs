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
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins.DefaultPlugins;
using UnityEngine;

namespace DG.Tweening.Plugins.Core
{
    internal static class PluginsManager
    {
        // Default plugins
        static FloatPlugin _floatPlugin;
        static IntPlugin _intPlugin;
        static UintPlugin _uintPlugin;
        static Vector2Plugin _vector2Plugin;
        static Vector3Plugin _vector3Plugin;
        static Vector4Plugin _vector4Plugin;
        static QuaternionPlugin _quaternionPlugin;
        static ColorPlugin _colorPlugin;
        static RectPlugin _rectPlugin;
        static RectOffsetPlugin _rectOffsetPlugin;
        static StringPlugin _stringPlugin;

        // Advanced and custom plugins
        const int _MaxCustomPlugins = 20;
        static Dictionary<Type, ITweenPlugin> _customPlugins;

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        internal static ABSTweenPlugin<T1,T2,TPlugOptions> GetDefaultPlugin<T1,T2,TPlugOptions>()
        {
            Type t1 = typeof(T1);
            Type t2 = typeof(T2);
            ITweenPlugin plugin = null;

            if (t1 == typeof(Vector3)) {
                if (_vector3Plugin == null) _vector3Plugin = new Vector3Plugin();
                plugin = _vector3Plugin;
            } else if (t1 == typeof(Quaternion)) {
                if (t2 == typeof(Quaternion)) Debugger.LogError("Quaternion tweens require a Vector3 endValue");
                else {
                    if (_quaternionPlugin == null) _quaternionPlugin = new QuaternionPlugin();
                    plugin = _quaternionPlugin;
                }
            } else if (t1 == typeof(Vector2)) {
                if (_vector2Plugin == null) _vector2Plugin = new Vector2Plugin();
                plugin = _vector2Plugin;
            } else if (t1 == typeof(float)) {
                if (_floatPlugin == null) _floatPlugin = new FloatPlugin();
                plugin = _floatPlugin;
            } else if (t1 == typeof(Color)) {
                if (_colorPlugin == null) _colorPlugin = new ColorPlugin();
                plugin = _colorPlugin;
            } else if (t1 == typeof(int)) {
                if (_intPlugin == null) _intPlugin = new IntPlugin();
                plugin = _intPlugin;
            } else if (t1 == typeof(Vector4)) {
                if (_vector4Plugin == null) _vector4Plugin = new Vector4Plugin();
                plugin = _vector4Plugin;
            } else if (t1 == typeof(Rect)) {
                if (_rectPlugin == null) _rectPlugin = new RectPlugin();
                plugin = _rectPlugin;
            } else if (t1 == typeof(RectOffset)) {
                if (_rectOffsetPlugin == null) _rectOffsetPlugin = new RectOffsetPlugin();
                plugin = _rectOffsetPlugin;
            } else if (t1 == typeof(uint)) {
                if (_uintPlugin == null) _uintPlugin = new UintPlugin();
                plugin = _uintPlugin;
            } else if (t1 == typeof(string)) {
                if (_stringPlugin == null) _stringPlugin = new StringPlugin();
                plugin = _stringPlugin;
            }

            if (plugin != null) return plugin as ABSTweenPlugin<T1, T2, TPlugOptions>;

            return null;
        }

        internal static ABSTweenPlugin<T1,T2,TPlugOptions> GetCustomPlugin<T1,T2,TPlugin,TPlugOptions>(IPlugSetter<T1,T2,TPlugin,TPlugOptions> plugSetter)
            where TPlugin : ITweenPlugin, new()
        {
            Type t = typeof(TPlugin);
            ITweenPlugin plugin;

            if (_customPlugins == null) _customPlugins = new Dictionary<Type, ITweenPlugin>(_MaxCustomPlugins);
            else if (_customPlugins.TryGetValue(t, out plugin)) return plugin as ABSTweenPlugin<T1, T2, TPlugOptions>;

            plugin = new TPlugin();
            _customPlugins.Add(t, plugin);
            return plugin as ABSTweenPlugin<T1, T2, TPlugOptions>;
        }

        // Un-caches all plugins
        internal static void PurgeAll()
        {
            _floatPlugin = null;
            _intPlugin = null;
            _uintPlugin = null;
            _vector2Plugin = null;
            _vector3Plugin = null;
            _vector4Plugin = null;
            _quaternionPlugin = null;
            _colorPlugin = null;
            _rectPlugin = null;
            _rectOffsetPlugin = null;
            _stringPlugin = null;

            _customPlugins.Clear();
        }
    }
}