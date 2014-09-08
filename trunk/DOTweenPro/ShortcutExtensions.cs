// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/08/27 20:54
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.PathCore;
using UnityEngine;

namespace DG.Tweening
{
    /// <summary>
    /// Methods that extend known Unity objects and allow to directly create and control tweens from their instances
    /// </summary>
    public static class ShortcutExtensions
    {
        #region Transform

        public static TweenerCore<Vector3, Path, PathOptions> DOPath(this Transform target, Vector3[] path, float duration, PathType pathType = PathType.Linear, PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
        {
            if (resolution < 1) resolution = 1;
            TweenerCore<Vector3, Path, PathOptions> t = DOTween.To(PathPlugin.Get(), () => target.position, x => target.position = x, new Path(pathType, path, resolution, gizmoColor), duration)
                .SetTarget(target);

            t.plugOptions.mode = pathMode;
            return t;
        }

        /// <summary>Tweens a Transform's localPosition in a spiral shape.
        /// Also stores the transform as the tween's target so it can be used for filtered operations</summary>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="axis">The axis around which the spiral will rotate</param>
        /// <param name="mode">The type of spiral movement</param>
        /// <param name="speed">Speed of the rotations</param>
        /// <param name="frequency">Frequency of the rotation. Higher values lead to wider spirals</param>
        /// <param name="depth">Indicates how much the tween should move along the spiral's axis</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener DOSpiral(this Transform target, float duration, Vector3? axis = null, SpiralMode mode = SpiralMode.Expand, float speed = 1, float frequency = 10, float depth = 0, bool snapping = false)
        {
            if (Mathf.Approximately(speed, 0)) speed = 1;
            if (axis == null || axis == Vector3.zero) axis = Vector3.forward;

            TweenerCore<Vector3, Vector3, SpiralOptions> t = DOTween.To(SpiralPlugin.Get(), () => target.localPosition, x => target.localPosition = x, (Vector3)axis, duration)
                .SetTarget(target);

            t.plugOptions.mode = mode;
            t.plugOptions.speed = speed;
            t.plugOptions.frequency = frequency;
            t.plugOptions.depth = depth;
            t.plugOptions.snapping = snapping;
            return t;
        }

        #endregion

        #region Rigidbody

        /// <summary>Tweens a Rigidbody's position in a spiral shape.
        /// Also stores the transform as the tween's target so it can be used for filtered operations</summary>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="axis">The axis around which the spiral will rotate</param>
        /// <param name="mode">The type of spiral movement</param>
        /// <param name="speed">Speed of the rotations</param>
        /// <param name="frequency">Frequency of the rotation. Higher values lead to wider spirals</param>
        /// <param name="depth">Indicates how much the tween should move along the spiral's axis</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener DOSpiral(this Rigidbody target, float duration, Vector3? axis = null, SpiralMode mode = SpiralMode.Expand, float speed = 1, float frequency = 10, float depth = 0, bool snapping = false)
        {
            if (Mathf.Approximately(speed, 0)) speed = 1;
            if (axis == null || axis == Vector3.zero) axis = Vector3.forward;

            TweenerCore<Vector3, Vector3, SpiralOptions> t = DOTween.To(SpiralPlugin.Get(), () => target.position, target.MovePosition, (Vector3)axis, duration)
                .SetTarget(target);

            t.plugOptions.mode = mode;
            t.plugOptions.speed = speed;
            t.plugOptions.frequency = frequency;
            t.plugOptions.depth = depth;
            t.plugOptions.snapping = snapping;
            return t;
        }

        #endregion

        #region Specific Options

        public static TweenerCore<Vector3, Path, PathOptions> SetOptions(this TweenerCore<Vector3, Path, PathOptions> t, bool closePath)
        {
            t.plugOptions.isClosedPath = closePath;
            return t;
        }

        public static TweenerCore<Vector3, Path, PathOptions> SetLookAt(this TweenerCore<Vector3, Path, PathOptions> t, Vector3 position, Vector3? forwardDirection = null, Vector3? up = null)
        {
            t.plugOptions.orientType = OrientType.LookAtPosition;
            t.plugOptions.lookAtPosition = position;
            SetPathForwardDirection(t, forwardDirection, up);
            return t;
        }

        public static TweenerCore<Vector3, Path, PathOptions> SetLookAt(this TweenerCore<Vector3, Path, PathOptions> t, Transform transform, Vector3? forwardDirection = null, Vector3? up = null)
        {
            t.plugOptions.orientType = OrientType.LookAtTransform;
            t.plugOptions.lookAtTransform = transform;
            SetPathForwardDirection(t, forwardDirection, up);
            return t;
        }

        public static TweenerCore<Vector3, Path, PathOptions> SetLookAt(this TweenerCore<Vector3, Path, PathOptions> t, float lookAhead, Vector3? forwardDirection = null, Vector3? up = null)
        {
            t.plugOptions.orientType = OrientType.ToPath;
            if (lookAhead < PathPlugin.MinLookAhead) lookAhead = PathPlugin.MinLookAhead;
            t.plugOptions.lookAhead = lookAhead;
            SetPathForwardDirection(t, forwardDirection, up);
            return t;
        }

        #endregion

        #region Private Methods

        static void SetPathForwardDirection(this TweenerCore<Vector3, Path, PathOptions> t, Vector3? forwardDirection = null, Vector3? up = null)
        {
            t.plugOptions.hasCustomForwardDirection = forwardDirection != null || up != null;
            if (forwardDirection == Vector3.zero) forwardDirection = Vector3.forward;
            if (t.plugOptions.hasCustomForwardDirection) {
                t.plugOptions.forward = Quaternion.LookRotation(
                    forwardDirection == null ? Vector3.forward : (Vector3)forwardDirection,
                    up == null ? Vector3.up : (Vector3)up
                );
            } else t.plugOptions.forward = Quaternion.identity;
        }

        #endregion
    }
}