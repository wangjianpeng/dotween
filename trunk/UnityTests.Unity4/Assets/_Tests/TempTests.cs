using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
 
public class TempTests : BrainBase
{
    public void OnClick(Transform t)
    {
    	t = null;
    	t.DOMoveX(2, 1);
    }
}