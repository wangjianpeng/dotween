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
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.DefaultPlugins;
using UnityEngine;

namespace DG.Tweening.Plugins.Core
{
    internal static class PluginsManager
    {
        // Default plugins
        static readonly Dictionary<DefaultPluginType, ITweenPlugin> _DefaultPlugins = new Dictionary<DefaultPluginType, ITweenPlugin>(16);
        // Advanced and custom plugins
        static readonly Dictionary<Type, ITweenPlugin> _CustomPlugins = new Dictionary<Type, ITweenPlugin>(30);

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        internal static ABSTweenPlugin<T> GetDefaultPlugin<T>(DefaultPluginType pluginType)
        {
            ITweenPlugin plugin;
            _DefaultPlugins.TryGetValue(pluginType, out plugin);
            if (plugin != null) return plugin as ABSTweenPlugin<T>;

            // Retrieve correct custom plugin
            switch (pluginType) {
            case DefaultPluginType.Float:
                plugin = new FloatPlugin();
                break;
            case DefaultPluginType.Int:
                plugin = new IntPlugin();
                break;
            case DefaultPluginType.Uint:
                plugin = new UintPlugin();
                break;
            case DefaultPluginType.Vector2:
                plugin = new Vector2Plugin();
                break;
            case DefaultPluginType.Vector3:
                plugin = new Vector3Plugin();
                break;
            case DefaultPluginType.Vector4:
                plugin = new Vector4Plugin();
                break;
            case DefaultPluginType.Quaternion:
                plugin = new QuaternionPlugin();
                break;
            case DefaultPluginType.Color:
                plugin = new ColorPlugin();
                break;
            case DefaultPluginType.Rect:
                plugin = new RectPlugin();
                break;
            case DefaultPluginType.RectOffset:
                plugin = new RectOffsetPlugin();
                break;
            case DefaultPluginType.String:
                plugin = new StringPlugin();
                break;
            }

            _DefaultPlugins.Add(pluginType, plugin);
            return plugin as ABSTweenPlugin<T>;
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