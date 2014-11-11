using DG.Tweening;
using UnityEngine;
using System.Collections;

public class MultipleAxisRotation : MonoBehaviour
{
	public Transform target;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			DOTween.Complete();
			target.DOLocalAxisRotate(new Vector3(0, 90, 0), 1).SetRelative();
		} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			DOTween.Complete();
			target.DOLocalAxisRotate(new Vector3(0, -90, 0), 1).SetRelative();
		} else if (Input.GetKeyDown(KeyCode.UpArrow)) {
			DOTween.Complete();
			target.DOLocalAxisRotate(new Vector3(90, 0, 0), 1).SetRelative();
		} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			DOTween.Complete();
			target.DOLocalAxisRotate(new Vector3(-90, 0, 0), 1).SetRelative();
		}
	}
}