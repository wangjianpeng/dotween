// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/07 19:22
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

using System;
using DG.Tween.Plugins.Core;
using DG.Tween.Plugins.CustomPlugins;
using UnityEngine;

namespace DG.Tween.Plugins
{
    public class PlugAlpha : IPluginSetter<Color, AlphaPlugin>
    {
        internal Color endValue;
        internal Type pluginType;

        public PlugAlpha(float endValue)
        {
            this.endValue = new Color(0, 0, 0, endValue);
            pluginType = typeof(AlphaPlugin);
        }

        public Type PluginType()
        {
            return pluginType;
        }

        public Color EndValue()
        {
            return endValue;
        }
    }
}