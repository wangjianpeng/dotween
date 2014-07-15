using DG.Tweening;
using System;
using UnityEngine;

public class RecycleBrain : BrainBase
{
	enum RecycleMode {
		ChangeAll,
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
			.Loops(-1, LoopType.Yoyo).Ease(EaseType.Linear);
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0)) {
			// Find mouse position to set as tween's new endValue
			Vector3 newEndValue = Input.mousePosition;
			newEndValue.z = UnityEngine.Random.Range(10f, 20f);
			newEndValue = Camera.main.ScreenToWorldPoint(newEndValue);
			switch (recycleMode) {
			case RecycleMode.ChangeAll:

				break;
			case RecycleMode.ChangeEndValue:
				tween.ChangeEndValue(newEndValue);
				break;
			}
		}
	}

	void OnGUI()
	{
		DGUtils.OpenGUI();

		GUILayout.Label("Recycle Mode");
		recycleMode = (RecycleMode)GUILayout.Toolbar((int)recycleMode, recycleModeList);

		DGUtils.CloseGUI();
	}
}