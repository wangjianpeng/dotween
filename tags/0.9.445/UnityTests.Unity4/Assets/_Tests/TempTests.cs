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
    	DOTween.To(()=> str0, x=> str0 = x, "Some changed text after the scramble", 3).SetOptions(true).SetLoops(-1);
    	DOTween.To(()=> str1, x=> str1 = x, "Some changed text after the scramble", 3).SetOptions(true, "0123456789").SetLoops(-1);
    }

    void OnGUI()
    {
    	GUILayout.Label(str0);
    	GUILayout.Label(str1);
    }
}