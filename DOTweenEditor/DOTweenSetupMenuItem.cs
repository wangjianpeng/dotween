// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/30 11:59
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
    class DOTweenSetupMenuItem
    {
        [MenuItem("Tools/" + _Title)]
        static void StartSetup() { Setup(); }

        const string _Title = "DOTween Setup";
		
        static void Setup()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string addonsDir = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            string pathSeparator = addonsDir.IndexOf("/") != -1 ? "/" : "\\";
            addonsDir = addonsDir.Substring(0, addonsDir.LastIndexOf(pathSeparator) + 1);

            if (Directory.GetFiles(addonsDir, "*.addon").Length > 0) {
                string msg = "Based on your Unity version (" + Application.unityVersion + "), DOTween will now import additional DOTween elements for more recent Unity features, if available.";
                if (!EditorUtility.DisplayDialog(_Title, msg, "Ok", "Cancel")) return;
            } else {
                string msg = "This project has already been setup for your version of DOTween.\nUpdate to a new DOTween version before running the setup again.";
                EditorUtility.DisplayDialog(_Title, msg, "Ok");
                return;
            }

            EditorUtility.DisplayProgressBar(_Title, "Importing additional DOTween elements based on your Unity version...", 0.25f);

            int totImported = 0;
            string[] vs = Application.unityVersion.Split("."[0]);
            int majorVersion = Convert.ToInt32(vs[0]);
            int minorVersion = Convert.ToInt32(vs[1]);
            if (majorVersion < 4) SetupComplete(addonsDir, totImported);
            else if (majorVersion == 4) {
                if (minorVersion < 3) SetupComplete(addonsDir, totImported);
                else {
                    totImported += ImportDll(43, addonsDir);
                    if (minorVersion >= 6) totImported += ImportDll(46, addonsDir);
                }
            } else {
                // 5.x
                totImported += ImportDll(43, addonsDir);
                totImported += ImportDll(46, addonsDir);
            }
            SetupComplete(addonsDir, totImported);
        }

        static void SetupComplete(string addonsDir, int totImported)
        {

            // Delete all remaining addon files
            string[] leftoverAddonFiles = Directory.GetFiles(addonsDir, "*.addon");
            if (leftoverAddonFiles.Length > 0) {
                EditorUtility.DisplayProgressBar(_Title, "Removing " + leftoverAddonFiles.Length + " unused additional files...", 0.5f);
                foreach (string leftoverAddonFile in leftoverAddonFiles) File.Delete(leftoverAddonFile);
            }
            // Delete all remaining addon meta files
            leftoverAddonFiles = Directory.GetFiles(addonsDir, "*.addon.meta");
            if (leftoverAddonFiles.Length > 0) {
                EditorUtility.DisplayProgressBar(_Title, "Removing " + leftoverAddonFiles.Length + " unused additional meta files...", 0.75f);
                foreach (string leftoverAddonFile in leftoverAddonFiles) File.Delete(leftoverAddonFile);
            }

            EditorUtility.DisplayProgressBar(_Title, "Refreshing...", 0.9f);
            AssetDatabase.Refresh();

            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog(_Title, "DOTween setup is now complete.", "Ok");
        }

        // Removes relative .addon extension thus activating files
        static int ImportDll(int version, string addonsDir)
        {
            int totImported = 0;
            string[] filenames = new[] {
                "DOTween" + version + ".dll",
                "DOTween" + version + ".xml",
                "DOTween" + version + ".dll.mdb"
            };

            foreach (string filename in filenames) {
                string addonFilepath = addonsDir + filename + ".addon";
                string finalFilepath = addonsDir + filename;
                if (File.Exists(addonFilepath)) {
                    // Delete eventual existing final file
                    if (File.Exists(finalFilepath)) File.Delete(finalFilepath);
                    // Rename addon file to final
                    File.Move(addonFilepath, finalFilepath);
                    totImported++;
                }
            }

            return totImported;
        }
    }
}