using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	private GameMaster _gm;

	void Start()
	{
		_gm = (GameMaster) FindObjectOfType (System.Type.GetType("GameMaster"));
	}

	void OnTouch()
	{
		_gm.OnButtonTouch ();
	}
}
