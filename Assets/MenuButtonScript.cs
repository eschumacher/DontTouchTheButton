using UnityEngine;
using System.Collections;

public class MenuButtonScript : MonoBehaviour {

	void OnTouch()
	{
		GameMaster.ResetGame();

		Application.LoadLevel ("StartMenu");
	}
}
