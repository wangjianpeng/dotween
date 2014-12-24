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
            if (EditorPrefs.GetString(DOTweenUtilityWindow.Id) != DOTween.Version
                || EditorPrefs.GetString(DOTweenUtilityWindow.IdPro) != EditorUtils.proVersion)
                DOTweenUtilityWindow.Open(true);
        }
    }

    class DOTweenUtilityWindow : EditorWindow
    {
        [MenuItem("Tools/" + _Title)]
        static void ShowWindow() { Open(); }
		
        const string _Title = "DOTween Utility";
        static readonly Vector2 _WinSize = new Vector2(300,300);
        public const string Id = "DOTweenVersion";
        public const string IdPro = "DOTweenProVersion";
        static readonly float _HalfBtSize = _WinSize.x * 0.5f - 6;

        Texture2D _headerImg, _footerImg;
        Vector2 _headerSize, _footerSize;
        string _innerTitle;

        bool _guiStylesSet;
        GUIStyle _boldLabelStyle, _redLabelStyle, _btStyle, _btImgStyle;

        // If force is FALSE opens the window only if DOTween's version has changed
        // (set to FALSE by OnPostprocessAllAssets)
        public static void Open(bool startSetupAutomatically = false)
        {
            EditorWindow window = EditorWindow.GetWindow<DOTweenUtilityWindow>(true, _Title, true);
            window.minSize = _WinSize;
            window.maxSize = _WinSize;
            window.ShowUtility();
            EditorPrefs.SetString(Id, DOTween.Version);
            EditorPrefs.SetString(IdPro, EditorUtils.proVersion);

            if (startSetupAutomatically) DOTweenSetupMenuItem.Setup(true);
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

            _headerImg = Resources.LoadAssetAtPath("Assets/" + EditorUtils.editorADBPath + "Imgs/DOTween_header.jpg", typeof(Texture2D)) as Texture2D;
            EditorUtils.SetEditorTexture(_headerImg, FilterMode.Bilinear, 512);
            _headerSize.x = _WinSize.x;
            _headerSize.y = (int)((_WinSize.x * _headerImg.height) / _headerImg.width);
            _footerImg = Resources.LoadAssetAtPath("Assets/" + EditorUtils.editorADBPath + (EditorGUIUtility.isProSkin ? "Imgs/DOTween_footer.png" : "Imgs/DOTween_footer_dark.png"), typeof(Texture2D)) as Texture2D;
            EditorUtils.SetEditorTexture(_footerImg, FilterMode.Bilinear, 512);
            _footerSize.x = _WinSize.x;
            _footerSize.y = (int)((_WinSize.x * _footerImg.height) / _footerImg.width);
        }

        void OnGUI()
        {
            SetGUIStyles();

            Rect headerRect = new Rect(0, 0, _headerSize.x, _headerSize.y);
            GUI.DrawTexture(headerRect, _headerImg, ScaleMode.StretchToFill, false);
            GUILayout.Space(_headerSize.y + 2);
            GUILayout.Label(_innerTitle, DOTween.isDebugBuild ? _redLabelStyle : _boldLabelStyle);
            GUILayout.Space(8);
            if (GUILayout.Button("Setup DOTween...", _btStyle)) DOTweenSetupMenuItem.Setup();
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