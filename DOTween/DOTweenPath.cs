// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/11 19:42
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening
{
    public enum HandlesDrawMode
    {
        Orthographic,
        Perspective
    }

    public enum HandlesType
    {
        Free,
        Full
    }

    /// <summary>
    /// Attach this to a GameObject to create and assign a path to it
    /// </summary>
    [AddComponentMenu("DOTween/Path")]
    public class DOTweenPath : MonoBehaviour
    {
        public bool isClosedPath;

        // Internal options
        public List<Vector3> wps = new List<Vector3>();
//        public Path path;

        // Editor-only options
        public HandlesType handlesType;
        public HandlesDrawMode handlesDrawMode;
        public float perspectiveHandleSize = 0.5f;
        public bool showIndexes = true;
        public Color pathColor = new Color(1, 1, 1, 0.5f);
    }
}