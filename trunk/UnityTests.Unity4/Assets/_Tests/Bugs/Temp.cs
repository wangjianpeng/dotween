using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public Transform target;

	void Start()
	{
		target.DOMoveX(5, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear)
			.OnUpdate(()=> Debug.Log(Time.frameCount + " > " + Time.realtimeSinceStartup + " UPDATE"));
	}

	// void OnGUI()
	// {
	// 	if (GUILayout.Button("Pause")) {
	// 		Debug.Log(Time.frameCount + " > " + Time.realtimeSinceStartup + " PAUSE");
	// 		DOTween.PauseAll();
	// 	}
	// }

	public void TogglePause()
	{
		Debug.Log("<color=#00ff00>" + Time.frameCount + " > " + Time.realtimeSinceStartup + " TOGGLE PAUSE</color>");
			DOTween.TogglePauseAll();
	}
}