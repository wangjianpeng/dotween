using DG.Tweening;
using System.Collections;
using UnityEngine;

public class TempBugTests : BrainBase
{
	public Transform[] targets;

	public Sequence seqMain, seqA, seqB;

	void Start()
	{
		seqMain = DOTween.Sequence().SetId("seqMain");
        seqA = DOTween.Sequence().SetId("seqA");
        seqB = DOTween.Sequence().SetId("seqB");
 
        seqA.Append(targets[0].DOMove(new Vector3(), 10f).SetId("seqA tween 0"));
        seqA.Append(targets[0].DOScale(new Vector3(3, 3, 3), 10f).SetId("seqA tween 1"));
 
        seqB.Append(targets[2].DOMove(new Vector3(), 2f).SetId("seqB tween 0"));
        seqB.AppendCallback(Fin);
 
        seqMain.Append(seqA);
        seqMain.Insert(0, seqB);
	}

	void Fin() {
        seqMain.Complete();
    }
}