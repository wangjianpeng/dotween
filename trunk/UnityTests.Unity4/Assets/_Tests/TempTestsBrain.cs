using DG.Tweening;
using DG.Tweening.Plugins;
using UnityEngine;

public class TempTestsBrain : BrainBase
{
	public bool useSafeMode;
	public int loops = 3;
	public LoopType loopType = LoopType.Yoyo;
	public float delay = 1.5f;
	public Transform[] targets;

	Tween tween;

	void Start()
	{
		DOTween.Init();
		DOTween.useSafeMode = useSafeMode;
		Debug.Log("USE SAFE MODE " + DOTween.useSafeMode);

		tween = DOTween.ToAxis(()=>targets[0].position, x=> targets[0].position = x, 3f, 1.5f)
			// .Delay(delay).Relative()
			.SetLoops(loops, loopType).SetAutoKill(false)
			.OnStart(()=> Debug.Log("Start"))
			.OnStepComplete(()=> Debug.Log("Step Complete"))
			.OnComplete(()=> Debug.Log("Complete"))
			.Pause();
		DOTween.ToAxis(()=>targets[0].position, x=> targets[0].position = x, 3f, 1.5f)
			.SetOptions(AxisConstraint.Y, true)
			.SetRelative().SetLoops(loops, loopType).SetAutoKill(false)
			.Pause();
		DOTween.ToAxis(()=>targets[0].position, x=> targets[0].position = x, 30f, 1.5f)
			.SetOptions(AxisConstraint.Z)
			.SetRelative().SetLoops(loops, loopType).SetAutoKill(false)
			.Pause();

		// scale
		// targets[1].DOScale(Vector3.zero, 0);
		// targets[1].DOScale(Vector3.one * 2, 1);
		// DOTween.Sequence().Append(targets[1].DOScale(Vector3.zero, 0)).Append(targets[1].DOScale(Vector3.one * 2, 1));
		targets[1].DOScaleFrom(Vector3.zero, 1);
	}

	void OnGUI()
	{
		if (GUILayout.Button("Play")) DOTween.Play();
		if (GUILayout.Button("Pause")) DOTween.Pause();
		if (GUILayout.Button("Complete")) DOTween.Complete();
		if (GUILayout.Button("Goto 1000")) DOTween.Goto(1000);
		if (GUILayout.Button("Goto 1.5")) DOTween.Goto(1.5f);
		if (GUILayout.Button("Goto 2")) DOTween.Goto(2);
		if (GUILayout.Button("Goto 3")) DOTween.Goto(3);
		if (GUILayout.Button("Goto and Play 1.5")) DOTween.Goto(1.5f, true);
		if (GUILayout.Button("Goto and Play 2")) DOTween.Goto(2, true);
		if (GUILayout.Button("Goto and Play 3")) DOTween.Goto(3, true);
		if (GUILayout.Button("Goto and Play 4.5")) DOTween.Goto(4.5f, true);
		if (GUILayout.Button("Rewind w Delay")) {
			Debug.Log("Rewinded: " + DOTween.Rewind());
		}
		if (GUILayout.Button("Rewind without Delay")) {
			Debug.Log("Rewinded: " + DOTween.Rewind(false));
		}
		if (GUILayout.Button("Flip")) DOTween.Flip();

		GUILayout.Space(10);
		GUILayout.Label("Elapsed: " + tween.Elapsed(false));
		GUILayout.Label("FullElapsed: " + tween.Elapsed());
		GUILayout.Label("CompletedLoops: " + tween.CompletedLoops());
	}
}