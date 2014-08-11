using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCompatibilityTest : MonoBehaviour
{
	public Transform multiCube;
	public TextMesh txtSuccess, txtSuccessAdd, txtVersion;

	bool success;
	Sequence twSuccess;

	IEnumerator Start()
	{
		DOTween.Init(true);
		txtVersion.text = "v" + DOTween.Version;
		txtSuccess.gameObject.SetActive(false);
		txtSuccessAdd.gameObject.SetActive(false);

		// Find and sort nested cubes from back to front, from TL clockwise
		Transform[] children = multiCube.GetComponentsInChildren<Transform>();
		List<Transform> ts = new List<Transform>(children.Length - 1);
		foreach (Transform t in children) if (t != multiCube) ts.Add(t);
		ts.Sort((x, y) => {
			if (x.position.z > y.position.z) return -1;
			if (x.position.z < y.position.z) return 1;
			if (x.position.y > y.position.y) return -1;
			if (x.position.y < y.position.y) return 1;
			if (x.position.x < y.position.y) return -1;
			if (x.position.x > y.position.y) return 1;
			return 0;
		});

		yield return new WaitForSeconds(1);

		// Create sequence
		Sequence seq = DOTween.Sequence().SetLoops(-1, LoopType.Restart).OnStepComplete(Success);
		seq.Append(multiCube.DORotate(new Vector3(0, 720, 360), 2.25f).SetEase(Ease.Linear));
		foreach (Transform trans in ts) {
			Transform t = trans;
			seq.Insert(0, t.DOScale(Vector3.one * 0.5f, 1f));
			seq.Insert(0, t.DOLocalMove(t.position * 8, 1f).SetEase(Ease.InQuint));
			seq.Insert(1, t.DOScale(Vector3.one * 0.5f, 1f));
			seq.Insert(1, t.DOLocalMove(t.position, 1f).SetEase(Ease.OutQuint));
		}

		// Create success tween
		twSuccess = DOTween.Sequence()
			.OnStart(()=> {
				txtSuccess.gameObject.SetActive(true);
				txtSuccessAdd.gameObject.SetActive(true);
			})
			.Pause();
		twSuccess.Append(DOTween.FromAlpha(()=> txtSuccess.color, x => txtSuccess.color = x, 0, 1.5f));
		twSuccess.Insert(0, DOTween.From(()=> txtSuccessAdd.text, x => txtSuccessAdd.text = x, "", 1.5f).SetOptions(true));
	}

	void Success()
	{
		if (success) return;

		success = true;
		twSuccess.Play();
	}
}