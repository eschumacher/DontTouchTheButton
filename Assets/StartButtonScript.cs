using UnityEngine;
using System.Collections;

public class StartButtonScript : MonoBehaviour {

	void OnTouch()
	{
		Application.LoadLevel ("GameScene");
	}
}
