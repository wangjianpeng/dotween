using DG.Tweening;
using System.Text;
using UnityEngine;

public class SequencesBrain : BrainBase
{
	public LoopType loopType;
	public Transform target;

	Material mat;
	Sequence seq;
	int stepCompleteS, stepCompleteT1, stepCompleteT2, stepCompleteT3;
	int completeS, completeT1, completeT2, completeT3;
	StringBuilder sb = new StringBuilder();

	void Start()
	{
		mat = target.gameObject.renderer.material;

		seq = DOTween.Sequence().Loops(10001, loopType).AutoKill(false)
			.Id("Sequence")
			.OnStart(()=> DGUtils.Log("Sequence Start"))
			.OnStepComplete(()=> { stepCompleteS++; DGUtils.Log("SEQUENCE Step Complete"); })
			.OnComplete(()=> { completeS++; });

		seq.AppendInterval(0.5f);
		seq.Append(
			target.MoveTo(new Vector3(2, 2, 2), 1f).Loops(1, LoopType.Yoyo)
			.Id("Move")
			.OnStart(()=> DGUtils.Log("Move Start"))
			.OnStepComplete(()=> { stepCompleteT1++; DGUtils.Log("Move Step Complete"); })
			.OnComplete(()=> { completeT1++; })
		);
		seq.Append(
			target.RotateTo(new Vector3(0, 225, 2), 1)
			.Id("Rotate")
			.OnStart(()=> DGUtils.Log("Rotate Start"))
			.OnStepComplete(()=> { stepCompleteT2++; DGUtils.Log("Rotate Step Complete"); })
			.OnComplete(()=> { completeT2++; })
		);
		seq.Insert(
			0.5f, mat.ColorTo(Color.green, 1)
			.Id("Color")
			.OnStart(()=> DGUtils.Log("Color Start"))
			.OnStepComplete(()=> { stepCompleteT3++; DGUtils.Log("Color Step Complete"); })
			.OnComplete(()=> { completeT3++; })
		);
		seq.AppendInterval(0.5f);
	}

	void OnGUI()
	{
		DGUtils.OpenGUI();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Restart")) {
			ResetStepsCounters();
			seq.Restart();
		}
		if (GUILayout.Button("Rewind")) {
			ResetStepsCounters();
			seq.Rewind();
		}
		if (GUILayout.Button("Complete")) seq.Complete();
		if (GUILayout.Button("Flip")) seq.Flip();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("TogglePause")) seq.TogglePause();
		if (GUILayout.Button("PlayForward")) seq.PlayForward();
		if (GUILayout.Button("PlayBackwards")) seq.PlayBackwards();
		GUILayout.EndHorizontal();

		GUILayout.Space(10);
		sb.Remove(0, sb.Length);
		sb.Append("IsPlaying: ").Append(seq.IsPlaying());
		sb.Append("\nIsBackwards: ").Append(seq.IsBackwards());
		sb.Append("\nPosition: ").Append(seq.Position());
		sb.Append("\nElapsed: ").Append(seq.Elapsed());
		sb.Append("\nCompletedLoops: ").Append(seq.CompletedLoops());
		GUILayout.Label(sb.ToString());

		GUILayout.Space(10);
		sb.Remove(0, sb.Length);
		sb.Append("Sequence Steps/Complete: ").Append(stepCompleteS).Append("/").Append(completeS);
		sb.Append("\nMove Steps/Complete: ").Append(stepCompleteT1).Append("/").Append(completeT1);
		sb.Append("\nRotation Steps/Complete: ").Append(stepCompleteT2).Append("/").Append(completeT2);
		sb.Append("\nColor Steps/Complete: ").Append(stepCompleteT3).Append("/").Append(completeT3);
		GUILayout.Label(sb.ToString());

		DGUtils.CloseGUI();
	}

	void ResetStepsCounters()
	{
		stepCompleteS = stepCompleteT1 = stepCompleteT2 = stepCompleteT3 = completeS = completeT1 = completeT2 = completeT3 = 0;
	}
}