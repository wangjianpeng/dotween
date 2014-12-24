using DG.Tweening;
using UnityEngine;
using System.Collections;

public class TempTests : BrainBase
{
	public string str0 = "Some text to scramble blahblah";
	public string str1 = "Some text to scramble blahblah";
	public GUIText textMesh;
	public Transform target;
	public Transform uiImg;

	Sequence slideSeq;

	void Start()
    {
    	Sequence bob = DOTween.Sequence();
        Tween upDown = target.DOMoveY(-0.5f, 0.5f).SetRelative().SetEase(Ease.OutQuad);
        Tween downUp = target.DOMoveY(0.5f, 1f).SetRelative();
        Tween rest = target.DOMoveY(-0, 0.5f).SetRelative().SetEase(Ease.InQuad);
        bob.Append(upDown).Append(downUp).Append(rest).SetLoops(3);

        Sequence test = DOTween.Sequence();
        test.Insert(1, bob);
        test.Insert(1,target.DOMoveX(-2, 5).SetRelative());
    }

    void OnGUI()
    {
    	GUILayout.Label(str0);
    	GUILayout.Label(str1);
    }
}