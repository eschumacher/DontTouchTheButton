using UnityEngine;
using System.Collections;

public class RetryButtonScript : MonoBehaviour {

	void OnTouch()
	{
		GameMaster.ResetGame();
		
		Application.LoadLevel("GameScene");
	}
}
