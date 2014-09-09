using DG.Tweening;
using System.Collections;
using UnityEngine;

public class TempBugTests : BrainBase
{
	public GameObject[] targets;

	public Sequence seqMain, seqA, seqB;

	void Start()
	{
		FadeIn(0);
	}

    void FadeIn(int id)
    {
        if (id > 2) return;

        DOTween.FromAlpha (
            () => targets[id].renderer.material.color,
            x => targets[id].renderer.material.color = x,
            0f, 0.5f)
        .OnComplete(()=> FadeIn(id + 1));
    }

	// void OnShowCompleteCallback()
 //    {
 //       ProcessToNextSequence();
 //    }

 //    void ProcessToNextSequence()
 //    {
 //       DOTween.Complete();
 //       // Some show and fade in
 //    }

    void OnGUI()
    {
        if (GUILayout.Button("Complete")) DOTween.Complete();
    }
}