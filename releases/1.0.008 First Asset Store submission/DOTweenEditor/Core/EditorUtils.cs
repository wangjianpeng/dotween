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
        // Editor path from Assets (not included) with final slash, in AssetDatabase format (/)
        public static string editorADBDir { get { if (string.IsNullOrEmpty(_editorADBDir)) StoreEditorADBDir(); return _editorADBDir; } }
        // With final slash (system based)
        public static string dotweenDir { get { if (string.IsNullOrEmpty(_dotweenDir)) StoreDOTweenDirs(); return _dotweenDir; } }
        // With final slash (system based)
        public static string dotweenProDir { get { if (string.IsNullOrEmpty(_dotweenProDir)) StoreDOTweenDirs(); return _dotweenProDir; } }
        public static bool isOSXEditor { get; private set; }
        public static string pathSlash { get; private set; } // for full paths
        public static string pathSlashToReplace { get; private set; } // for full paths

        static bool _hasPro;
        static string _proVersion;
        static bool _hasCheckedForPro;
        static string _editorADBDir;
        static string _dotweenDir; // with final slash
        static string _dotweenProDir; // with final slash

        static EditorUtils()
        {
            isOSXEditor = Application.platform == RuntimePlatform.OSXEditor;
            bool useWindowsSlashes = Application.platform == RuntimePlatform.WindowsEditor;
            pathSlash = useWindowsSlashes ? "\\" : "/";
            pathSlashToReplace = useWindowsSlashes ? "/" : "\\";
        }

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

        /// <summary>
        /// Returns TRUE if addons setup is required.
        /// </summary>
        public static bool DOTweenSetupRequired()
        {
            if (!Directory.Exists(dotweenDir)) return false; // Can happen if we were deleting DOTween
            return Directory.GetFiles(dotweenDir, "*.addon").Length > 0 || hasPro && Directory.GetFiles(dotweenProDir, "*.addon").Length > 0;
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
        static void StoreEditorADBDir()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string fullPath = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            string adbPath = fullPath.Substring(Application.dataPath.Length + 1);
            _editorADBDir = adbPath.Replace("\\", "/") + "/";
        }

        static void StoreDOTweenDirs()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            _dotweenDir = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            string pathSeparator = _dotweenDir.IndexOf("/") != -1 ? "/" : "\\";
            _dotweenDir = _dotweenDir.Substring(0, _dotweenDir.LastIndexOf(pathSeparator) + 1);

            _dotweenProDir = _dotweenDir.Substring(0, _dotweenDir.LastIndexOf(pathSeparator));
            _dotweenProDir = _dotweenProDir.Substring(0, _dotweenProDir.LastIndexOf(pathSeparator) + 1) + "DOTweenPro" + pathSeparator;
        }
    }
}