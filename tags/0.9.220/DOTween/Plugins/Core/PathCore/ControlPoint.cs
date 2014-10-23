// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/10/07 11:28
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
    /// <summary>
    /// Path control point
    /// </summary>
    internal struct ControlPoint
    {
        internal Vector3 a, b;

        public ControlPoint(Vector3 a, Vector3 b)
        {
            this.a = a;
            this.b = b;
        }
    }
}