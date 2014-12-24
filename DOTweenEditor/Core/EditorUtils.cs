// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/12/24 13:50

using System;
using System.Reflection;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor.Core
{
    public static class EditorUtils
    {
        public static bool hasPro { get { if (!_hasCheckedForPro) CheckForPro(); return _hasPro; } }
        public static string proVersion { get { if (!_hasCheckedForPro) CheckForPro(); return _proVersion; } }
        // Editor path from Assets (not included) with final slash, in AssetDatabase format
        public static string editorADBPath { get { if (string.IsNullOrEmpty(_editorADBPath)) StoreEditorADBPath(); return _editorADBPath; } }

        static bool _hasPro;
        static string _proVersion;
        static bool _hasCheckedForPro;
        static string _editorADBPath;

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------

        /// <summary>
        /// Checks that the given editor texture use the correct import settings,
        /// and applies them if they're incorrect.
        /// </summary>
        public static void SetEditorTexture(Texture2D texture, FilterMode filterMode = FilterMode.Point, int maxTextureSize = 32)
        {
            if (texture.wrapMode == TextureWrapMode.Clamp) return;

            string path = AssetDatabase.GetAssetPath(texture);
            TextureImporter tImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            tImporter.textureType = TextureImporterType.GUI;
            tImporter.npotScale = TextureImporterNPOTScale.None;
            tImporter.filterMode = filterMode;
            tImporter.wrapMode = TextureWrapMode.Clamp;
            tImporter.maxTextureSize = maxTextureSize;
            tImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
            AssetDatabase.ImportAsset(path);
        }

        // ===================================================================================
        // METHODS ---------------------------------------------------------------------------

        static void CheckForPro()
        {
            _hasCheckedForPro = true;
            try {
                Assembly additionalAssembly = Assembly.Load("DOTweenPro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                _proVersion = additionalAssembly.GetType("DG.Tweening.DOTweenPro").GetField("Version", BindingFlags.Static | BindingFlags.Public).GetValue(null) as string;
                _hasPro = true;
            } catch {
                // No DOTweenPro present
                _hasPro = false;
                _proVersion = "-";
            }
        }

        // AssetDatabase formatted path to DOTween's Editor folder
        static void StoreEditorADBPath()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(EditorUtils));
            UriBuilder uri = new UriBuilder(assembly.CodeBase);
            string fullPath = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            string adbPath = fullPath.Substring(Application.dataPath.Length + 1);
            _editorADBPath = adbPath.Replace("\\", "/") + "/";
        }
    }
}