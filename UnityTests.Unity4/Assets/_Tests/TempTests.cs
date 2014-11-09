using DG.Tweening;
using UnityEngine;
using System.Collections;

public class TempTests : BrainBase
{
	public GUIText textMesh;
	public Transform target;
	Sequence slideSeq;

	public void Start()
    {
        target.DOMove(new Vector3(1, 1.5f, 1), 1).OnComplete(()=> Debug.Log(target.position));
    }
}