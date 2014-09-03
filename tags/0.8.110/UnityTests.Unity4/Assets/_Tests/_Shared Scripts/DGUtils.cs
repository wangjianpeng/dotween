using UnityEngine;

public static class DGUtils
{
	public static void Log(object o) {
		Debug.Log(Time.frameCount + "/" + Time.realtimeSinceStartup + " : " + o);
	}

	public static void BeginGUI()
	{
		GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();

		if (GUILayout.Button("RELOAD SCENE")) Application.LoadLevel(Application.loadedLevel);
	}

	public static void EndGUI()
	{
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
}