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
        static readonly Dictionary<Type, ITweenPlugin> _CustomPlugins = new Dictionary<Type, ITweenPlugin>(30);

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        internal static ABSTweenPlugin GetDefaultPlugin(DefaultPluginType pluginType)
        {
            // Retrieve correct custom plugin
            switch (pluginType) {
            case DefaultPluginType.Int:
                if (_intPlugin == null) _intPlugin = new IntPlugin();
                return _intPlugin;
            case DefaultPluginType.Uint:
                if (_uintPlugin == null) _uintPlugin = new UintPlugin();
                return _uintPlugin;
            case DefaultPluginType.Vector2:
                if (_vector2Plugin == null) _vector2Plugin = new Vector2Plugin();
                return _vector2Plugin;
            case DefaultPluginType.Vector3:
                if (_vector3Plugin == null) _vector3Plugin = new Vector3Plugin();
                return _vector3Plugin;
            case DefaultPluginType.Vector4:
                if (_vector4Plugin == null) _vector4Plugin = new Vector4Plugin();
                return _vector4Plugin;
            case DefaultPluginType.Quaternion:
                if (_quaternionPlugin == null) _quaternionPlugin = new QuaternionPlugin();
                return _quaternionPlugin;
            case DefaultPluginType.Color:
                if (_colorPlugin == null) _colorPlugin = new ColorPlugin();
                return _colorPlugin;
            case DefaultPluginType.Rect:
                if (_rectPlugin == null) _rectPlugin = new RectPlugin();
                return _rectPlugin;
            case DefaultPluginType.RectOffset:
                if (_rectOffsetPlugin == null) _rectOffsetPlugin = new RectOffsetPlugin();
                return _rectOffsetPlugin;
            case DefaultPluginType.String:
                if (_stringPlugin == null) _stringPlugin = new StringPlugin();
                return _stringPlugin;
            default:
                if (_floatPlugin == null) _floatPlugin = new FloatPlugin();
                return _floatPlugin;
            }
        }

//        internal static ABSTweenPlugin<T1,T2,TPlugOptions> GetCustomPlugin<T1,T2,TPlugin,TPlugOptions>(IPlugSetter<T1,T2,TPlugin,TPlugOptions> plugSetter)
//            where TPlugin : ITweenPlugin, new()
//        {
//            Type t = typeof(TPlugin);
//            if (_CustomPlugins.ContainsKey(t)) return _CustomPlugins[t] as ABSTweenPlugin<T1,T2,TPlugOptions>;
//            
//            TPlugin plugin = new TPlugin();
//            _CustomPlugins.Add(t, plugin);
//            return plugin as ABSTweenPlugin<T1,T2,TPlugOptions>;
//        }

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


            _CustomPlugins.Clear();
        }
    }
}