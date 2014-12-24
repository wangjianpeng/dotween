// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/12/24 13:37

using DG.DOTweenEditor.Core;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
    public class UtilityWindowProcessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            string[] dotweenEntries = System.Array.FindAll(importedAssets, name => name.Contains("DOTween") && !name.EndsWith(".meta") && !name.EndsWith(".jpg") && !name.EndsWith(".png"));
            bool dotweenImported = dotweenEntries.Length > 0;
            if (dotweenImported && EditorUtils.DOTweenSetupRequired()) {
                EditorUtility.DisplayDialog("DOTween", "DOTween needs to be setup, so that additional elements may be imported based on your Unity version.\nSelect \"Setup DOTween...\" in the Utility Panel that will open after you press OK.", "Ok");
                DOTweenUtilityWindow.Open();
            }
        }
    }

    class DOTweenUtilityWindow : EditorWindow
    {
        [MenuItem("Tools/" + _Title)]
        static void ShowWindow() { Open(); }
		
        const string _Title = "DOTween Utility";
        static readonly Vector2 _WinSize = new Vector2(300,310);
        public const string Id = "DOTweenVersion";
        public const string IdPro = "DOTweenProVersion";
        static readonly float _HalfBtSize = _WinSize.x * 0.5f - 6;

        Texture2D _headerImg, _footerImg;
        Vector2 _headerSize, _footerSize;
        string _innerTitle;
        bool _setupRequired;

        bool _guiStylesSet;
        GUIStyle _boldLabelStyle, _setupLabelStyle, _redLabelStyle, _btStyle, _btImgStyle;

        // If force is FALSE opens the window only if DOTween's version has changed
        // (set to FALSE by OnPostprocessAllAssets)
        public static void Open()
        {
            EditorWindow window = EditorWindow.GetWindow<DOTweenUtilityWindow>(true, _Title, true);
            window.minSize = _WinSize;
            window.maxSize = _WinSize;
            window.ShowUtility();
            EditorPrefs.SetString(Id, DOTween.Version);
            EditorPrefs.SetString(IdPro, EditorUtils.proVersion);
        }

        // ===================================================================================
        // UNITY METHODS ---------------------------------------------------------------------

        void OnHierarchyChange()
        { Repaint(); }

        void OnEnable()
        {
            _innerTitle = "DOTween v" + DOTween.Version + (DOTween.isDebugBuild ? " [Debug build]" : " [Release build]");
            if (EditorUtils.hasPro) _innerTitle += "\nDOTweenPro v" + EditorUtils.proVersion;
            else _innerTitle += "\nDOTweenPro not installed";

            if (_headerImg == null) {
                _headerImg = Resources.LoadAssetAtPath("Assets/" + EditorUtils.editorADBDir + "Imgs/Header.jpg", typeof(Texture2D)) as Texture2D;
                EditorUtils.SetEditorTexture(_headerImg, FilterMode.Bilinear, 512);
                _headerSize.x = _WinSize.x;
                _headerSize.y = (int)((_WinSize.x * _headerImg.height) / _headerImg.width);
                _footerImg = Resources.LoadAssetAtPath("Assets/" + EditorUtils.editorADBDir + (EditorGUIUtility.isProSkin ? "Imgs/Footer.png" : "Imgs/Footer_dark.png"), typeof(Texture2D)) as Texture2D;
                EditorUtils.SetEditorTexture(_footerImg, FilterMode.Bilinear, 256);
                _footerSize.x = _WinSize.x;
                _footerSize.y = (int)((_WinSize.x * _footerImg.height) / _footerImg.width);
            }

            _setupRequired = EditorUtils.DOTweenSetupRequired();
        }

        void OnGUI()
        {
            SetGUIStyles();

            Rect headerRect = new Rect(0, 0, _headerSize.x, _headerSize.y);
            GUI.DrawTexture(headerRect, _headerImg, ScaleMode.StretchToFill, false);
            GUILayout.Space(_headerSize.y + 2);
            GUILayout.Label(_innerTitle, DOTween.isDebugBuild ? _redLabelStyle : _boldLabelStyle);

            if (_setupRequired) {
                GUI.backgroundColor = Color.red;
                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label("DOTWEEN SETUP REQUIRED", _setupLabelStyle);
                GUILayout.EndVertical();
                GUI.backgroundColor = Color.white;
            } else GUILayout.Space(8);
            if (GUILayout.Button("Setup DOTween...", _btStyle)) {
                DOTweenSetupMenuItem.Setup();
                _setupRequired = EditorUtils.DOTweenSetupRequired();
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Documentation", _btStyle, GUILayout.Width(_HalfBtSize))) Application.OpenURL("http://dotween.demigiant.com/documentation.php");
            if (GUILayout.Button("Support", _btStyle, GUILayout.Width(_HalfBtSize))) Application.OpenURL("http://dotween.demigiant.com/support.php");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Changelog", _btStyle, GUILayout.Width(_HalfBtSize))) Application.OpenURL("http://dotween.demigiant.com/download.php");
            if (GUILayout.Button("Check Updates", _btStyle, GUILayout.Width(_HalfBtSize))) Application.OpenURL("http://dotween.demigiant.com/download.php?v=" + DOTween.Version);
            GUILayout.EndHorizontal();
            GUILayout.Space(14);
            if (GUILayout.Button(_footerImg, _btImgStyle)) Application.OpenURL("http://www.demigiant.com/");
        }

        // ===================================================================================
        // METHODS ---------------------------------------------------------------------------

        void SetGUIStyles()
        {
            if (!_guiStylesSet) {
                _boldLabelStyle = new GUIStyle(GUI.skin.label);
                _boldLabelStyle.fontStyle = FontStyle.Bold;
                _redLabelStyle = new GUIStyle(GUI.skin.label);
                _redLabelStyle.normal.textColor = Color.red;
                _setupLabelStyle = new GUIStyle(_boldLabelStyle);
                _setupLabelStyle.alignment = TextAnchor.MiddleCenter;

                _btStyle = new GUIStyle(GUI.skin.button);
                _btStyle.padding = new RectOffset(0, 0, 10, 10);

                _btImgStyle = new GUIStyle(GUI.skin.button);
                _btImgStyle.normal.background = null;
                _btImgStyle.imagePosition = ImagePosition.ImageOnly;
                _btImgStyle.padding = new RectOffset(0, 0, 0, 0);
                _btImgStyle.fixedWidth = _footerSize.x;
                _btImgStyle.fixedHeight = _footerSize.y;

                _guiStylesSet = true;
            }
        }
    }
}