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
    public enum OrientType
    {
        None,
        ToPath,
        LookAtTransform,
        LookAtPosition
    }

    public struct PathOptions
    {
        public PathMode mode;
        public OrientType orientType;
        public AxisConstraint lockPositionAxis, lockRotationAxis;
        public bool isClosedPath;
        public Vector3 lookAtPosition;
        public Transform lookAtTransform;
        public float lookAhead;
        public bool hasCustomForwardDirection;
        public Quaternion forward;
        public bool useLocalPosition;
        public Transform parent; // Only used with OrientType.ToPath and useLocalPosition set as TRUE
        
        internal float startupZRot; // Used to store Z value in case of lock Z, in order to rotate things differently
    }

    /// <summary>
    /// Path plugin works exclusively with Transforms
    /// </summary>
    public class PathPlugin : ABSTweenPlugin<Vector3, Path, PathOptions>
    {
        public const float MinLookAhead = 0.0001f;

        public override void Reset(TweenerCore<Vector3, Path, PathOptions> t)
        {
            t.endValue.Destroy(); // Clear path
            t.startValue = t.endValue = t.changeValue = null;
        }

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
            Transform trans = (Transform)t.target;
            path.wps = wps;
            path.subdivisions = wpsLen * path.subdivisionsXSegment;
            path.SetTimeToLenTables();
            t.plugOptions.startupZRot = trans.eulerAngles.z;
            if (t.plugOptions.orientType == OrientType.ToPath && t.plugOptions.useLocalPosition) t.plugOptions.parent = trans.parent;

            // Set changeValue as a reference to endValue
            t.changeValue = t.endValue;
        }

        public override float GetSpeedBasedDuration(PathOptions options, float unitsXSecond, Path changeValue)
        {
            return changeValue.length / unitsXSecond;
        }

        public override void EvaluateAndApply(PathOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, DOSetter<Vector3> setter, float elapsed, Path startValue, Path changeValue, float duration)
        {
            float pathPerc = EaseManager.Evaluate(t, elapsed, 0, 1, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            float constantPathPerc = changeValue.ConvertToConstantPathPerc(pathPerc);
            Vector3 newPos = changeValue.GetPoint(constantPathPerc);
            changeValue.targetPosition = newPos; // Used to draw editor gizmos
            setter(newPos);

            if (options.orientType != OrientType.None) {
                Transform trans = (Transform)t.target;
                Quaternion newRot = Quaternion.identity;

                switch (options.orientType) {
                case OrientType.LookAtPosition:
                    changeValue.lookAtPosition = options.lookAtPosition; // Used to draw editor gizmos
                    newRot = Quaternion.LookRotation(options.lookAtPosition - trans.position, trans.up);
                    break;
                case OrientType.LookAtTransform:
                    if (options.lookAtTransform != null) {
                        changeValue.lookAtPosition = options.lookAtTransform.position; // Used to draw editor gizmos
                        newRot = Quaternion.LookRotation(options.lookAtTransform.position - trans.position, trans.up);
                    }
                    break;
                case OrientType.ToPath:
                    Vector3 lookAtP;
                    if (changeValue.type == PathType.Linear && options.lookAhead <= MinLookAhead) {
                        // Calculate lookAhead so that it doesn't turn until it starts moving on next waypoint
                        lookAtP = newPos + changeValue.wps[changeValue.linearWPIndex] - changeValue.wps[changeValue.linearWPIndex - 1];
                    } else {
                        float lookAheadPerc = constantPathPerc + options.lookAhead;
                        if (lookAheadPerc > 1) lookAheadPerc = (options.isClosedPath ? lookAheadPerc - 1 : 1.00001f);
                        lookAtP = changeValue.GetPoint(lookAheadPerc);
                    }
                    Vector3 transUp = trans.up;
                    // Apply basic modification for local position movement
                    if (options.useLocalPosition && options.parent != null) lookAtP = options.parent.TransformPoint(lookAtP);
                    // LookAt axis constraint
                    if (options.lockRotationAxis != AxisConstraint.None) {
                        if ((options.lockRotationAxis & AxisConstraint.X) == AxisConstraint.X) {
                            Vector3 v0 = trans.InverseTransformPoint(lookAtP);
                            v0.y = 0;
                            lookAtP = trans.TransformPoint(v0);
                            transUp = options.useLocalPosition && options.parent != null ? options.parent.up : Vector3.up;
                        }
                        if ((options.lockRotationAxis & AxisConstraint.Y) == AxisConstraint.Y) {
                            Vector3 v0 = trans.InverseTransformPoint(lookAtP);
                            if (v0.z < 0) v0.z = -v0.z;
                            v0.x = 0;
                            lookAtP = trans.TransformPoint(v0);
                        }
                        if ((options.lockRotationAxis & AxisConstraint.Z) == AxisConstraint.Z) {
                            // Fix to allow racing loops to keep cars straight and not flip it
                            if (options.useLocalPosition && options.parent != null) transUp = options.parent.TransformDirection(Vector3.up);
                            else transUp = trans.TransformDirection(Vector3.up);
                            transUp.z = options.startupZRot;
                        }
                    }
                    // Eventual 2D mode
                    if (options.mode == PathMode.Full3D) newRot = Quaternion.LookRotation(lookAtP - trans.position, transUp);
                    else {
                        // 2D path
                        float rotY = 0;
                        float rotZ = Utils.Angle2D(trans.position, lookAtP);
                        if (rotZ < 0) rotZ = 360 + rotZ;
                        if (options.mode == PathMode.Sidescroller2D) {
                            // Manage Y and modified Z rotation
                            rotY = lookAtP.x < trans.position.x ? 180 : 0;
                            if (rotZ > 90 && rotZ < 270) rotZ = 180 - rotZ;
                        }
                        newRot = Quaternion.Euler(0, rotY, rotZ);
                    }
                    break;
                }

                if (options.hasCustomForwardDirection) newRot *= options.forward;
                trans.rotation = newRot;
            }
        }
    }
}