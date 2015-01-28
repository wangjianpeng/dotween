using DG.Tweening;
using System.Collections;
using UnityEngine;

public class VirtualTweens : BrainBase
{
	float propSetViaVirtualTween;

	IEnumerator Start()
	{
		DOTween.Init(recycleTweens);
		yield return new WaitForSeconds(0.5f);

		yield return DOTween.To( x => propSetViaVirtualTween = x, 5f, 25f, 1f).SetAutoKill(false).Pause().WaitForCompletion();
		Debug.Log("Complete");
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("TogglePause")) DOTween.TogglePauseAll();
		if (GUILayout.Button("Rewind")) DOTween.RewindAll();
		if (GUILayout.Button("Rewind")) DOTween.CompleteAll();
		GUILayout.EndHorizontal();

		GUILayout.Label("Virtual tween result: " + propSetViaVirtualTween);

		DGUtils.EndGUI();
	}
}