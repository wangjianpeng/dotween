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
using DG.Tween.Core;
using DG.Tween.Plugins.DefaultPlugins;
using UnityEngine;

namespace DG.Tween.Plugins.Core
{
    internal static class PluginsManager
    {
        // Default plugins. Contains internal dictionaries based on T2 types,
        // since there might be more plugins for the same type (like float to float and float to int)
        static readonly Dictionary<Type, Dictionary<Type, ITweenPlugin>> _DefaultPlugins = new Dictionary<Type, Dictionary<Type, ITweenPlugin>>(10);
        // Advanced and custom plugins
        static readonly Dictionary<Type, ITweenPlugin> _CustomPlugins = new Dictionary<Type, ITweenPlugin>(20);

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        internal static ABSTweenPlugin<T1,T2,TPlugOptions> GetDefaultPlugin<T1,T2,TPlugOptions>()
        {
            // TODO Improve

            Type t1 = typeof(T1);
            Type t2 = typeof(T2);
            bool hasT1 = _DefaultPlugins.ContainsKey(t1);
            bool hasT2 = hasT1 && _DefaultPlugins[t1].ContainsKey(t2);
            if (hasT2) return _DefaultPlugins[t1][t2] as ABSTweenPlugin<T1,T2,TPlugOptions>;

            // Retrieve correct custom plugin
            ITweenPlugin plugin = null;
            if (t1 == typeof(float)) {
                plugin = new FloatPlugin();
            } else if (t1 == typeof(Vector3)) {
                plugin = new Vector3Plugin();
            } else if (t1 == typeof(Quaternion)) {
                if (t2 == typeof(Quaternion)) Debugger.LogError("Quaternion tweens require a Vector3 endValue");
                else plugin = new QuaternionPlugin();
            }

            if (plugin != null) {
                if (!hasT1) _DefaultPlugins.Add(t1, new Dictionary<Type, ITweenPlugin>());
                _DefaultPlugins[t1].Add(t2, plugin);
                return plugin as ABSTweenPlugin<T1,T2,TPlugOptions>;
            }

            return null;
        }

        internal static ABSTweenPlugin<T1,T2,TPlugOptions> GetCustomPlugin<T1,T2,TPlugin,TPlugOptions>(IPlugSetter<T1,T2,TPlugin,TPlugOptions> plugSetter)
            where TPlugin : ITweenPlugin, new()
        {
            Type t = typeof(TPlugin);
            if (_CustomPlugins.ContainsKey(t)) return _CustomPlugins[t] as ABSTweenPlugin<T1,T2,TPlugOptions>;
            
            TPlugin plugin = new TPlugin();
            _CustomPlugins.Add(t, plugin);
            return plugin as ABSTweenPlugin<T1,T2,TPlugOptions>;
        }

        // Un-caches all plugins
        internal static void PurgeAll()
        {
            _DefaultPlugins.Clear();
            _CustomPlugins.Clear();
        }
    }
}