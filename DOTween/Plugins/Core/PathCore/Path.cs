// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/04 10:15
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Core.PathCore
{
    // Public so it can be used internally as a T2 for PathPlugin.
    // Also used by DOTweenPath and relative Inspector to set up visual paths.
    public class Path
    {
        // Static decoders stored to avoid creating new ones each time
        static CatmullRomDecoder _catmullRomDecoder;
        static LinearDecoder _linearDecoder;

        internal PathType type;
        internal int subdivisionsXSegment; // Subdivisions x each segment
        internal int subdivisions; // Stored by PathPlugin > total subdivisions for whole path (derived automatically from subdivisionsXSegment)
        internal Vector3[] wps; // Waypoints (modified by PathPlugin when setting relative end value and change value) - also modified by DOTweenPathInspector
        internal float length; // Unit length of the path
        internal float[] wpLengths; // Unit length of each waypoint (CURRENTLY UNUSED)

        internal float[] timesTable; // Connected to lengthsTable, used for constant speed calculations
        internal float[] lengthsTable; // Connected to timesTable, used for constant speed calculations
        internal int linearWPIndex = -1; // Waypoint towards which we're moving (only stored for linear paths, when calling GetPoint)

        ABSPathDecoder _decoder;

        // GIZMOS DATA ///////////////////////////////////////////////

        bool _changed; // Indicates that the path has changed (after an incremental loop moved on/back) and the gizmo points need to be recalculated
        Vector3[] _nonLinearDrawWps; // Used to store non-linear path gizmo points when inside Unity editor
        internal Vector3 targetPosition; // Set by PathPlugin at each update
        internal Vector3? lookAtPosition; // Set by PathPlugin in case there's a lookAt active
        Color _gizmoColor = new Color(1, 1, 1, 0.7f);

        #region Main

        // ***********************************************************************************
        // CONSTRUCTOR
        // ***********************************************************************************

        public Path(PathType type, Vector3[] waypoints, int subdivisionsXSegment, Color? gizmoColor = null)
        {
            this.type = type;
            this.subdivisionsXSegment = subdivisionsXSegment;
            if (gizmoColor != null) _gizmoColor = (Color)gizmoColor;
            AssignWaypoints(waypoints, true);
            AssignDecoder(type);

            if (DOTween.isUnityEditor) DOTween.GizmosDelegates.Add(Draw);
        }

        // Needs to be called once waypoints and decoder are assigned, to setup path data.
        // If path is linear subdivisions is ignored and wpLengths are stored here instead than when calling SetWaypointsLengths (CURRENTLY UNUSED)
        internal void Setup()
        {
            _decoder.SetTimeToLengthTables(this, subdivisions);
        }

        /// <summary>
        /// Gets the point on the path at the given percentage (0 to 1)
        /// </summary>
        /// <param name="perc">The percentage (0 to 1) at which to get the point</param>
        /// <param name="convertToConstantPerc">If TRUE constant speed is taken into account, otherwise not</param>
        internal Vector3 GetPoint(float perc, bool convertToConstantPerc = false)
        {
            if (convertToConstantPerc) perc = ConvertToConstantPathPerc(perc);
            return _decoder.GetPoint(perc, wps, this);
        }

        // Converts the given raw percentage to the correct percentage considering constant speed
        internal float ConvertToConstantPathPerc(float perc)
        {
            if (type == PathType.Linear) return perc;

            if (perc > 0 && perc < 1) {
                float tLen = length * perc;
                // Find point in time/length table
                float t0 = 0, l0 = 0, t1 = 0, l1 = 0;
                int count = lengthsTable.Length;
                for (int i = 0; i < count; ++i) {
                    if (lengthsTable[i] > tLen) {
                        t1 = timesTable[i];
                        l1 = lengthsTable[i];
                        if (i > 0) l0 = lengthsTable[i - 1];
                        break;
                    }
                    t0 = timesTable[i];
                }
                // Find correct time
                perc = t0 + ((tLen - l0) / (l1 - l0)) * (t1 - t0);
            }

            // Clamp value because path has limited range of 0-1
            if (perc > 1) perc = 1;
            else if (perc < 0) perc = 0;

            return perc;
        }

        // Refreshes the waypoints used to draw non-linear gizmos and the PathInspector scene view.
        // Called by Draw method or by DOTweenPathInspector
        internal static void RefreshNonLinearDrawWps(Path p)
        {
            int wpsCount = p.wps.Length;

            int gizmosSubdivisions = wpsCount * 5;
            if (p._nonLinearDrawWps == null || p._nonLinearDrawWps.Length != gizmosSubdivisions + 1)
                p._nonLinearDrawWps = new Vector3[gizmosSubdivisions + 1];
            for (int i = 0; i <= gizmosSubdivisions; ++i) {
                float perc = i / (float)gizmosSubdivisions;
                Vector3 wp = p.GetPoint(perc);
                p._nonLinearDrawWps[i] = wp;
            }
        }

        // Stops drawing the path gizmo
        internal void Destroy()
        {
            if (DOTween.isUnityEditor) DOTween.GizmosDelegates.Remove(Draw);
            wps = null;
            wpLengths = timesTable = lengthsTable = null;
            _nonLinearDrawWps = null;
        }

        #endregion

        // Deletes the previous waypoints and assigns the new ones
        // (newWps length must be at least 1).
        // Internal so DOTweenPathInspector can use it
        internal void AssignWaypoints(Vector3[] newWps, bool cloneWps = false)
        {
            if (cloneWps) {
                int count = newWps.Length;
                wps = new Vector3[count];
                for (int i = 0; i < count; ++i) wps[i] = newWps[i];
            } else wps = newWps;
        }

        // Internal so DOTweenPathInspector can use it
        internal void AssignDecoder(PathType pathType)
        {
            switch (pathType) {
            case PathType.Linear:
                if (_linearDecoder == null) _linearDecoder = new LinearDecoder();
                _decoder = _linearDecoder;
                break;
            default: // Catmull-Rom
                if (_catmullRomDecoder == null) _catmullRomDecoder = new CatmullRomDecoder();
                _decoder = _catmullRomDecoder;
                break;
            }
        }

        // If path is linear wpLengths were stored when calling Setup
        void StoreWaypointsLengths()
        {
            _decoder.SetWaypointsLengths(this, subdivisions);
        }

        // Used in DOTween.OnDrawGizmos if we're inside Unity Editor
        void Draw() { Draw(this); }
        static void Draw(Path p)
        {
            if (p.timesTable == null) return;

            Color gizmosFadedCol = p._gizmoColor;
            gizmosFadedCol.a *= 0.5f;
            Gizmos.color = p._gizmoColor;
            int wpsCount = p.wps.Length;

            if (p._changed || p.type != PathType.Linear && p._nonLinearDrawWps == null) {
                p._changed = false;
                if (p.type != PathType.Linear) {
                    // Store draw points
                    RefreshNonLinearDrawWps(p);
                }
            }

            // Draw path
            Vector3 currPt;
            Vector3 prevPt;
            switch (p.type) {
            case PathType.Linear:
                prevPt = p.wps[1];
                for (int i = 1; i < wpsCount - 1; ++i) {
                    currPt = p.wps[i];
                    Gizmos.DrawLine(currPt, prevPt);
                    prevPt = currPt;
                }
                break;
            default: // Curved
                prevPt = p._nonLinearDrawWps[0];
                int count = p._nonLinearDrawWps.Length;
                for (int i = 1; i < count; ++i) {
                    currPt = p._nonLinearDrawWps[i];
                    Gizmos.DrawLine(currPt, prevPt);
                    prevPt = currPt;
                }
                break;
            }

            Gizmos.color = gizmosFadedCol;
            const float spheresSize = 0.075f;

            // Draw path control points
            for (int i = 1; i < wpsCount - 1; ++i) Gizmos.DrawSphere(p.wps[i], spheresSize);

            // Draw eventual path lookAt
            if (p.lookAtPosition != null) {
                Vector3 lookAtP = (Vector3)p.lookAtPosition;
                Gizmos.DrawLine(p.targetPosition, lookAtP);
                Gizmos.DrawWireSphere(lookAtP, spheresSize);
            }
        }
    }
}