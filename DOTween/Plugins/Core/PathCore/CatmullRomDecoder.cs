// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/04 11:01
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
    internal class CatmullRomDecoder : ABSPathDecoder
    {
        internal override void FinalizePath(Path p, Vector3[] wps, bool isClosedPath)
        {
            // Add starting and ending control points (uses only one vector per control point)
            int wpsLen = wps.Length;
            if (p.controlPoints == null || p.controlPoints.Length != 2) p.controlPoints = new ControlPoint[2];
            if (isClosedPath) {
                p.controlPoints[0] = new ControlPoint(wps[wpsLen - 2], Vector3.zero);
                p.controlPoints[1] = new ControlPoint(wps[1], Vector3.zero);
            } else {
                p.controlPoints[0] = new ControlPoint(wps[1], Vector3.zero);
                Vector3 lastP = wps[wpsLen - 1];
                Vector3 diffV = lastP - wps[wpsLen - 2];
                p.controlPoints[1] = new ControlPoint(lastP + diffV, Vector3.zero);
            }
            // Store total subdivisions
            p.subdivisions = (wpsLen + 2) * p.subdivisionsXSegment;
            // Store time to len tables
            SetTimeToLengthTables(p, p.subdivisions);
        }

        internal override Vector3 GetPoint(float perc, Vector3[] wps, Path p)
        {
            int numSections = wps.Length - 1; // Considering also control points
            int tSec = (int)Math.Floor(perc * numSections);
            int currPt = numSections - 1;
            if (currPt > tSec) currPt = tSec;
            float u = perc * numSections - currPt;

            Vector3 a = currPt == 0 ? p.controlPoints[0].a : wps[currPt - 1];
            Vector3 b = wps[currPt];
            Vector3 c = wps[currPt + 1];
            Vector3 d = currPt + 2 > wps.Length - 1 ? p.controlPoints[1].a : wps[currPt + 2];

            return .5f * (
                (-a + 3f * b - 3f * c + d) * (u * u * u)
                + (2f * a - 5f * b + 4f * c - d) * (u * u)
                + (-a + c) * u
                + 2f * b
            );
        }

        internal void SetTimeToLengthTables(Path p, int subdivisions)
        {
            float pathLen = 0;
            float incr = 1f / subdivisions;
            float[] timesTable = new float[subdivisions];
            float[] lengthsTable = new float[subdivisions];
            Vector3 prevP = GetPoint(0, p.wps, p);
            for (int i = 1; i < subdivisions + 1; ++i) {
                float perc = incr * i;
                Vector3 currP = GetPoint(perc, p.wps, p);
                pathLen += Vector3.Distance(currP, prevP);
                prevP = currP;
                timesTable[i - 1] = perc;
                lengthsTable[i - 1] = pathLen;
            }

            // Assign
            p.length = pathLen;
            p.timesTable = timesTable;
            p.lengthsTable = lengthsTable;
        }

        internal void SetWaypointsLengths(Path p, int subdivisions)
        {
            // Create a relative path between each waypoint,
            // with its start and end control lines coinciding with the next/prev waypoints.
            int count = p.wps.Length - 2;
            float[] wpLengths = new float[count];
            wpLengths[0] = 0;
            Vector3[] partialWps = new Vector3[4];
            for (int i = 2; i < count + 1; ++i) {
                // Create partial path
                partialWps[0] = p.wps[i - 2];
                partialWps[1] = p.wps[i - 1];
                partialWps[2] = p.wps[i];
                partialWps[3] = p.wps[i + 1];
                // Calculate length of partial path
                float partialLen = 0;
                float incr = 1f / subdivisions;
                Vector3 prevP = GetPoint(0, partialWps, p);
                for (int c = 1; c < subdivisions + 1; ++c) {
                    float perc = incr * c;
                    Vector3 currP = GetPoint(perc, partialWps, p);
                    partialLen += Vector3.Distance(currP, prevP);
                    prevP = currP;
                }
                wpLengths[i - 1] = partialLen;
            }

            // Assign
            p.wpLengths = wpLengths;
        }
    }
}