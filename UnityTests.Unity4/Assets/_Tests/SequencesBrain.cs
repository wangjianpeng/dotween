using DG.Tweening;
using UnityEngine;

public class SequencesBrain : MonoBehaviour
{
	public LoopType loopType;
	public Transform target;

	Material mat;

	void Start()
	{
		mat = target.gameObject.renderer.material;

		Sequence seq = DOTween.Sequence().Loops(-1, loopType);
		seq.Append(target.MoveTo(new Vector3(2, 2, 2), 1f));
		seq.Append(target.RotateTo(new Vector3(0, 225, 2), 1f));
		seq.Insert(0, mat.ColorTo(Color.green, 1));
	}
}