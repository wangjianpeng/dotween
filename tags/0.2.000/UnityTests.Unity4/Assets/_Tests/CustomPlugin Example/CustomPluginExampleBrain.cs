using DG.Tweening;
using DG.Tweening.Plugins;
using UnityEngine;

public class CustomPluginExampleBrain : BrainBase
{
	public Transform target;

	void Start()
	{
		// DOTween.To(()=>target.position, x=> target.position = x, new Vector3(4, 4, 0), 1.5f)
		DOTween.To(new PlugCustomPlugin(()=>target.position, x=> target.position = x, 4), 1.5f)
			.Delay(2).Relative().Loops(5, LoopType.Yoyo).AutoKill(false)
			.OnStart(()=> Debug.Log("Start"))
			.OnStepComplete(()=> Debug.Log("Step Complete"))
			.OnComplete(()=> Debug.Log("Complete"));
	}

	void OnGUI()
	{
		if (GUILayout.Button("Flip")) DOTween.Flip();
	}
}