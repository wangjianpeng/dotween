// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/04 10:15
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.PathCore
{
    // Public so it can be used internally as a T2 for PathPlugin
    public class Path
    {
        // Static decoders stored to avoid creating new ones each time
        static CatmullRomDecoder _catmullRomDecoder;
        static LinearDecoder _linearDecoder;

        internal PathType type;
        internal int subdivisionsXSegment; // Subdivisions x each segment
        internal int subdivisions; // Stored by PathPlugin > total subdivisions for whole path (derived automatically from subdivisionsXSegment)
        internal Vector3[] wps; // Waypoints (modified by PathPlugin when setting relative end value and change value)
        internal float length; // Unit length of the path
        internal float[] wpLengths; // Unit length of each waypoint

        internal float[] timesTable; // Connected to lengthsTable, used for constant speed calculations
        internal float[] lengthsTable; // Connected to timesTable, used for constant speed calculations

        readonly ABSPathDecoder _decoder;

        // GIZMOS DATA ///////////////////////////////////////////////

        bool _changed; // Indicates that the path has changed (after an incremental loop moved on/back) and the gizmo points need to be recalculated
        Vector3[] _drawWps; // Used to store the path gizmo points when inside Unity editor
        internal Vector3 targetPosition; // Set by PathPlugin at each update
        internal Vector3? lookAtPosition; // Set by PathPlugin in case there's a lookAt active
        Color _gizmoColor = Color.white;

        // ***********************************************************************************
        // CONSTRUCTOR
        // ***********************************************************************************

        public Path(PathType type, Vector3[] waypoints, int subdivisionsXSegment, Color? gizmoColor = null)
        {
            this.type = type;
            this.subdivisionsXSegment = subdivisionsXSegment;
            int count = waypoints.Length;
            this.wps = new Vector3[count];
            for (int i = 0; i < count; ++i) this.wps[i] = waypoints[i];
            if (gizmoColor != null) this._gizmoColor = (Color)gizmoColor;

            switch (type) {
            case PathType.Linear:
                if (_linearDecoder == null) _linearDecoder = new LinearDecoder();
                _decoder = _linearDecoder;
                break;
            default: // Catmull-Rom
                if (_catmullRomDecoder == null) _catmullRomDecoder = new CatmullRomDecoder();
                _decoder = _catmullRomDecoder;
                break;
            }

            if (DOTween.isUnityEditor) DOTween.onDrawGizmos.Add(Draw);
        }

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        /// <summary>
        /// Gets the point on the path at the given percentage (0 to 1)
        /// </summary>
        /// <param name="perc">The percentage (0 to 1) at which to get the point</param>
        /// <param name="useConstantSpeed">If TRUE constant speed is taken into account, otherwise not</param>
        internal Vector3 GetPoint(float perc, bool useConstantSpeed = false)
        {
            if (useConstantSpeed) perc = ConvertToConstantPathPerc(perc);
            return _decoder.GetPoint(perc, wps, length, timesTable);
        }

        // If path is linear subdivisions is ignored
        // and wpLengths are stored here instead than when calling SetWaypointsLengths
        internal void SetTimeToLenTables()
        {
            _decoder.SetTimeToLengthTables(this, subdivisions);
        }

        // Stops drawing the path gizmo
        internal void Destroy()
        {
            if (DOTween.isUnityEditor) DOTween.onDrawGizmos.Remove(Draw);
            wps = null;
            wpLengths = timesTable = lengthsTable = null;
            _drawWps = null;
        }

        // ===================================================================================
        // METHODS ---------------------------------------------------------------------------

        // If path is linear wpLengths were stored when calling SetTimeToLenTables
        void StoreWaypointsLengths()
        {
            _decoder.SetWaypointsLengths(this, subdivisions);
        }

        // Converts the given raw percentage to the correct percentage considering constant speed
        float ConvertToConstantPathPerc(float perc)
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

        // Used in DOTween.OnDrawGizmos if we're inside Unity Editor
        void Draw()
        {
            if (timesTable == null) return;

            Color gizmosFadedCol = _gizmoColor;
            gizmosFadedCol.a = 0.5f;
            Gizmos.color = _gizmoColor;
            int wpsCount = wps.Length;

            Vector3 currPt;
            if (_changed || type != PathType.Linear && _drawWps == null) {
                _changed = false;
                if (type != PathType.Linear) {
                    // Store draw points
                    int gizmosSubdivisions = wpsCount * 5;
                    _drawWps = new Vector3[gizmosSubdivisions + 1];
                    for (int i = 0; i <= gizmosSubdivisions; ++i) {
                        float perc = i / (float)gizmosSubdivisions;
                        currPt = GetPoint(perc);
                        _drawWps[i] = currPt;
                    }
                }
            }

            // Draw path
            Vector3 prevPt;
            switch (type) {
            case PathType.Linear:
                prevPt = wps[1];
                for (int i = 1; i < wpsCount - 1; ++i) {
                    currPt = wps[i];
                    Gizmos.DrawLine(currPt, prevPt);
                    prevPt = currPt;
                }
                break;
            default: // Curved
                prevPt = _drawWps[0];
                int count = _drawWps.Length;
                for (int i = 1; i < count; ++i) {
                    currPt = _drawWps[i];
                    Gizmos.DrawLine(currPt, prevPt);
                    prevPt = currPt;
                }
                break;
            }

            Gizmos.color = gizmosFadedCol;

            // Draw path control points
            for (int i = 1; i < wpsCount - 1; ++i) Gizmos.DrawSphere(wps[i], 0.075f);

            // Draw eventual path lookAt
            if (lookAtPosition != null) {
//                Gizmos.color = gizmosFadedCol;
                Vector3 lookAtP = (Vector3)lookAtPosition;
                Gizmos.DrawLine(targetPosition, lookAtP);
            }
        }
    }
}