// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/04 11:02
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
    internal class LinearDecoder : ABSPathDecoder
    {
        internal override Vector3 GetPoint(float perc, Vector3[] wps, Path p = null)
        {
            if (perc <= 0) {
                p.linearWPIndex = 1;
                return wps[1];
            }

            int startPIndex = 0;
            int endPIndex = 0;
            int count = p.timesTable.Length;
            for (int i = 1; i < count; i++) {
                if (p.timesTable[i] >= perc) {
                    startPIndex = i - 1;
                    endPIndex = i;
                    break;
                }
            }
            float startPPerc = p.timesTable[startPIndex];
            float partialPerc = perc - startPPerc;
            float partialLen = p.length * partialPerc;
            Vector3 wp0 = wps[startPIndex];
            Vector3 wp1 = wps[endPIndex];
            p.linearWPIndex = endPIndex;
            return wp0 + Vector3.ClampMagnitude(wp1 - wp0, partialLen);
        }

        // Linear exception: also sets waypoints lengths and doesn't set lengthsTable since it's useless
        internal override void SetTimeToLengthTables(Path p, int subdivisions)
        {
            float pathLen = 0;
            int count = p.wps.Length;
            float[] wpLengths = new float[count];
            Vector3 prevP = p.wps[1];
            for (int i = 1; i < count; i++) {
                Vector3 currP = p.wps[i];
                float dist = Vector3.Distance(currP, prevP);
                if (i < count - 1) pathLen += dist;
                prevP = currP;
                wpLengths[i] = dist;
            }
            float[] timesTable = new float[count];
            float tmpLen = 0;
            for (int i = 2; i < count; i++) {
                tmpLen += wpLengths[i];
                timesTable[i] = tmpLen / pathLen;
            }

            // Assign
            p.length = pathLen;
            p.wpLengths = wpLengths;
            p.timesTable = timesTable;
        }

        internal override void SetWaypointsLengths(Path p, int subdivisions)
        {
            // Does nothing (waypoints lenghts were stored inside SetTimeToLengthTables)
        }
    }
}