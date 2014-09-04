// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/03 19:36
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.PathCore;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins
{
    public struct PathOptions
    {
        public AxisConstraint lockPositionAxis;
        public bool isClosedPath;
        public int subdivisionsXSegment; // Subdivisions per each path segment
    }

    /// <summary>
    /// Path plugin works exclusively with Transforms
    /// </summary>
    public class PathPlugin : ABSTweenPlugin<Vector3, Path, PathOptions>
    {
        public static ABSTweenPlugin<Vector3, Path, PathOptions> Get()
        {
            return PluginsManager.GetCustomPlugin<PathPlugin, Vector3, Path, PathOptions>();
        }

        public override Path ConvertToStartValue(TweenerCore<Vector3, Path, PathOptions> t, Vector3 value)
        {
            // Simply sets the same path as start and endValue
            return t.endValue;
        }

        public override void SetRelativeEndValue(TweenerCore<Vector3, Path, PathOptions> t)
        {
            Vector3 startP = t.getter();
            int count = t.endValue.wps.Length;
            for (int i = 0; i < count; ++i) t.endValue.wps[i] += startP;
        }

        // Recreates waypoints with correct control points and eventual additional starting point
        // then sets the final path version
        public override void SetChangeValue(TweenerCore<Vector3, Path, PathOptions> t)
        {
            Path path = t.endValue;
            Vector3 currVal = t.getter();
            Vector3[] wps;
            int indMod = 1;
            int pAdd = (t.plugOptions.isClosedPath ? 1 : 0);
            int wpsLen = path.wps.Length;
            
            bool hasAdditionalStartingP = path.wps[0] != currVal; // Additional point in case starting waypoint doesn't coincide with current position
            if (hasAdditionalStartingP) {
                wps = new Vector3[wpsLen + 3 + pAdd];
                wps[1] = currVal;
                indMod = 2;
            } else {
                wps = new Vector3[wpsLen + 2 + pAdd];
            }
            for (int i = 0; i < wpsLen; ++i) wps[i + indMod] = path.wps[i];

            // Add control points and eventually close path
            wpsLen = wps.Length;
            if (t.plugOptions.isClosedPath) {
                wps[wpsLen - 2] = wps[1];
                wps[0] = wps[wpsLen - 3];
                wps[wpsLen - 1] = wps[2];
            } else {
                wps[0] = wps[1];
                Vector3 lastP = wps[wpsLen - 2];
                Vector3 diffV = lastP - wps[wpsLen - 3];
                wps[wpsLen - 1] = lastP + diffV;
            }

            // Manage eventual lockPositionAxis
            if (t.plugOptions.lockPositionAxis != AxisConstraint.None) {
                bool lockX = ((t.plugOptions.lockPositionAxis & AxisConstraint.X) == AxisConstraint.X);
                bool lockY = ((t.plugOptions.lockPositionAxis & AxisConstraint.Y) == AxisConstraint.Y);
                bool lockZ = ((t.plugOptions.lockPositionAxis & AxisConstraint.Z) == AxisConstraint.Z);
                for (int i = 0; i < wpsLen; ++i) {
                    Vector3 pt = wps[i];
                    wps[i] = new Vector3(
                        lockX ? currVal.x : pt.x,
                        lockY ? currVal.y : pt.y,
                        lockZ ? currVal.z : pt.z
                    );
                }
            }

            // Apply correct values to path and set needed data
            path.wps = wps;
            path.SetTimeToLenTables(wpsLen * t.plugOptions.subdivisionsXSegment);
        }

        public override float GetSpeedBasedDuration(PathOptions options, float unitsXSecond, Path changeValue)
        {
            return changeValue.length / unitsXSecond;
        }

        public override Vector3 Evaluate(PathOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, float elapsed, Path startValue, Path changeValue, float duration)
        {
            float pathPerc = EaseManager.Evaluate(t, elapsed, 0, 1, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            return changeValue.GetPoint(pathPerc, true);
        }
    }
}