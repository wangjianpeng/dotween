using DG.Tweening;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Temp : MonoBehaviour
{
	public Transform target;

	void Start()
	{
		Tween rotateTween = target.DOLocalRotate(new Vector3(0f, 0f, -180f), 5f, RotateMode.Fast);
		rotateTween.SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
	}
}