using DG.Tweening;
using UnityEngine;

public class CloneBrain : BrainBase
{
	public GameObject prefab;
	public AnimationCurve animCurve;

	void Start()
	{
		Transform t = NewTransform();
		Tween tween = t.MoveToY(5f, 1)
			.Delay(2f)
			.Relative()
			.Ease(animCurve)
			.OnStart(()=> Debug.Log("OnStart"))
			.Loops(-1, LoopType.Yoyo);
		for (int i = 0; i < 4; ++i) {
			t = NewTransform();
			Transform t2 = t;
			t2.MoveToY(5f, 1).SetAs(tween);
		}
	}

	Transform NewTransform()
	{
		Transform t = ((GameObject)Instantiate(prefab)).transform;
		t.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
		return t;
	}
}