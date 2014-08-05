using DG.Tweening;
using Holoville.DebugFramework.Components;
using UnityEngine;

public class BrainBase : MonoBehaviour 
{
	public bool forceFrameRate;
	public int forcedFrameRate = 10;

	[System.NonSerialized] public HOFpsGadget fpsGadget;

	virtual protected void Awake()
	{
		GameObject fpsGadgetGo = new GameObject("FPS");
		DontDestroyOnLoad(fpsGadgetGo);
		fpsGadget = fpsGadgetGo.AddComponent<HOFpsGadget>();
		if (forceFrameRate) fpsGadget.limitFrameRate = forcedFrameRate;
		fpsGadget.showMemory = true;

		DOTween.showUnityEditorReport = true;
	}
}