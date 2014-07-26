using DG.Tweening;
using System;
using UnityEngine;

public class RecycleBrain : BrainBase
{
	enum RecycleMode {
		RecreateAndUseInternalPooling,
		ChangeEndValue
	}

	public Transform target;

	RecycleMode recycleMode;
	string[] recycleModeList;
	Tweener tween;

	void Start()
	{
		recycleModeList = Enum.GetNames(typeof(RecycleMode));

		tween = DOTween.To(()=> target.position, x=> target.position = x, new Vector3(5, 5, 5), 2)
			.SetLoops(-1, LoopType.Yoyo).SetEase(EaseType.Linear);
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0)) {
			// Find mouse position to set as tween's new endValue
			Vector3 newEndValue = Input.mousePosition;
			newEndValue.z = UnityEngine.Random.Range(10f, 20f);
			newEndValue = Camera.main.ScreenToWorldPoint(newEndValue);
			switch (recycleMode) {
			case RecycleMode.RecreateAndUseInternalPooling:
				tween.Kill();
				tween = DOTween.To(()=> target.position, x=> target.position = x, newEndValue, 2)
			.SetLoops(-1, LoopType.Yoyo).SetEase(EaseType.Linear);
				break;
			case RecycleMode.ChangeEndValue:
				Debug.LogWarning("ChangeEndValue needs to be reimplemented");
				// tween.ChangeEndValue(newEndValue);
				break;
			}
		}
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		GUILayout.Label("Recycle Mode");
		recycleMode = (RecycleMode)GUILayout.Toolbar((int)recycleMode, recycleModeList);

		DGUtils.EndGUI();
	}
}