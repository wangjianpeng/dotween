using DG.Tweening;
using UnityEngine;

public class ShortcutsBrain : MonoBehaviour
{
	public GameObject prefab;

	void Start()
	{
		////////////////////////////////////////////
		// Transform shortcuts /////////////////////

		// Position
		NewTransform().MoveTo(RandomVector3(), 1).Loops(-1, LoopType.Yoyo);
		// X Position
		NewTransform().MoveToX(Random.Range(-10f, 10f), 1).Loops(-1, LoopType.Yoyo);
		// Local position
		NewTransform().MoveToLocal(RandomVector3(), 1).Loops(-1, LoopType.Yoyo);
		// Rotation
		NewTransform().RotateTo(RandomVector3(720), 1).Loops(-1, LoopType.Yoyo);
		// Local rotation
		NewTransform().RotateToLocal(RandomVector3(720), 1).Loops(-1, LoopType.Yoyo);
		// Scale
		NewTransform().ScaleTo(RandomVector3(3), 1).Loops(-1, LoopType.Yoyo);
		// Color
		NewTransform().renderer.material.ColorTo(Color.green, 1).Loops(-1, LoopType.Yoyo);
		// Alpha
		NewTransform().renderer.material.FadeTo(0, 1).Loops(-1, LoopType.Yoyo);
	}

	Transform NewTransform(bool randomPos = true)
	{
		Transform t = ((GameObject)Instantiate(prefab)).transform;
		if (randomPos) t.position = RandomVector3();
		return t;
	}

	Vector3 RandomVector3(float limit = 10f)
	{
		return new Vector3(Random.Range(-limit, limit), Random.Range(-limit, limit), Random.Range(-limit, limit));
	}
}