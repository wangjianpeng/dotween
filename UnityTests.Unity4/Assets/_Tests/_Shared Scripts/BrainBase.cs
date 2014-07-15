using Holoville.DebugFramework.Components;
using UnityEngine;

public class BrainBase : MonoBehaviour 
{
	TestOptions options;
	[System.NonSerialized] public HOFpsGadget fpsGadget;

	virtual protected void Awake()
	{
		if (options != null) return;

		options = Resources.Load("TestOptions", typeof(TestOptions)) as TestOptions;
		GameObject fpsGadgetGo = new GameObject("FPS");
		DontDestroyOnLoad(fpsGadgetGo);
		fpsGadget = fpsGadgetGo.AddComponent<HOFpsGadget>();
		if (options.forceFrameRate) fpsGadget.limitFrameRate = options.forcedFrameRate;
		fpsGadget.showMemory = true;
	}
}