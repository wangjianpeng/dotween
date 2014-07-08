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
using DG.Tween.Plugins.DefaultPlugins;
using UnityEngine;

namespace DG.Tween.Plugins.Core
{
    internal static class PluginsManager
    {
        // Default plugins
//        static FloatPlugin _floatPlugin;
//        static Vector3Plugin _vector3Plugin;
        static readonly Dictionary<Type, ITweenPlugin> _DefaultPlugins = new Dictionary<Type, ITweenPlugin>(10);
        // Advanced and custom plugins
        static readonly Dictionary<Type, ITweenPlugin> _CustomPlugins = new Dictionary<Type, ITweenPlugin>(20);

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        internal static ABSTweenPlugin<T1,T2> GetDefaultPlugin<T1,T2>()
        {
            // TODO Improve

            Type t = typeof(T1);
            if (_DefaultPlugins.ContainsKey(t)) return _DefaultPlugins[t] as ABSTweenPlugin<T1,T2>;

            ITweenPlugin plugin = null;
            if (t == typeof(float)) {
                plugin = new FloatPlugin();
            } else if (t == typeof(Vector3)) {
                plugin = new Vector3Plugin();
            }

            if (plugin != null) {
                _DefaultPlugins.Add(t, plugin);
                return plugin as ABSTweenPlugin<T1,T2>;
            }

            return null;
        }

        internal static ABSTweenPlugin<T1,T2> GetCustomPlugin<T1,T2,TPlugin>(IPlugSetter<T1,T2,TPlugin> plugSetter)
            where TPlugin : ITweenPlugin, new()
        {
            Type t = typeof(TPlugin);
            if (_CustomPlugins.ContainsKey(t)) return _CustomPlugins[t] as ABSTweenPlugin<T1,T2>;
            
            TPlugin plugin = new TPlugin();
            _CustomPlugins.Add(t, plugin);
            return plugin as ABSTweenPlugin<T1,T2>;
        }

        // Un-caches all plugins
        internal static void PurgeAll()
        {
            _DefaultPlugins.Clear();
            _CustomPlugins.Clear();
        }
    }
}