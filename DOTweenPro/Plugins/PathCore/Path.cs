// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/04 10:15
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

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
        internal Vector3[] wps; // Waypoints (modified by PathPlugin when setting relative end value and change value)
        internal float length; // Unit length of the path
        internal float[] wpLengths; // Unit length of each waypoint

        internal float[] timesTable; // Connected to lengthsTable, used for constant speed calculations
        internal float[] lengthsTable; // Connected to timesTable, used for constant speed calculations

        readonly ABSPathDecoder _decoder;

        // ***********************************************************************************
        // CONSTRUCTOR
        // ***********************************************************************************

        public Path(PathType type, Vector3[] waypoints)
        {
            this.type = type;
            this.wps = waypoints;

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
        internal void SetTimeToLenTables(int subdivisions)
        {
            _decoder.SetTimeToLengthTables(this, subdivisions);
        }

        // ===================================================================================
        // METHODS ---------------------------------------------------------------------------

        // If path is linear wpLengths were stored when calling SetTimeToLenTables
        void StoreWaypointsLengths(int subdivisions)
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
    }
}