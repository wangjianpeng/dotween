using DG.Tweening;
using DG.Tweening.Plugins;
using UnityEngine;

public class TempTestsBrain : BrainBase
{
	public bool useSafeMode;
	public int loops = 3;
	public LoopType loopType = LoopType.Yoyo;
	public float delay = 1.5f;
	public Transform target;

	Tween tween;

	void Start()
	{
		DOTween.Init();
		DOTween.useSafeMode = useSafeMode;
		Debug.Log("USE SAFE MODE " + DOTween.useSafeMode);

		// PlugVector3X
		// Using NEW
		tween = DOTween.To(new PlugVector3X(()=>target.position, x=> target.position = x, 3f, new PlugVector3X.Options(false)), 1.5f)
			// .Delay(delay).Relative()
			.Loops(loops, loopType).AutoKill(false)
			.OnStart(()=> Debug.Log("Start"))
			.OnStepComplete(()=> Debug.Log("Step Complete"))
			.OnComplete(()=> Debug.Log("Complete"))
			.Pause();
		// Using Plug shortcuts (and no delays)
		DOTween.To(Plug.Vector3Y(()=>target.position, x=> target.position = x, 3f, Plug.Vector3YOptions(true)), 1.5f)
			.Relative().Loops(loops, loopType).AutoKill(false)
			.Pause();
		DOTween.To(Plug.Vector3Z(()=>target.position, x=> target.position = x, 3f), 1.5f)
			.Relative().Loops(loops, loopType).AutoKill(false)
			.Pause();
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
		GUILayout.Label("Position: " + tween.Position());
		GUILayout.Label("Elapsed: " + tween.Elapsed());
		GUILayout.Label("CompletedLoops: " + tween.CompletedLoops());
	}
}