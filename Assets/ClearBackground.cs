using UnityEngine;
using System.Collections;

public class ClearBackground : MonoBehaviour {

	public GameMaster _gm;

	void OnTouch()
	{
		_gm.OnBackgroundTouch();
	}
}
