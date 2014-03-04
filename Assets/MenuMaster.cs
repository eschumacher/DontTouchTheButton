using UnityEngine;
using System.Collections;

public class MenuMaster : MonoBehaviour {

	RuntimePlatform _platform = Application.platform;
	
	// Update is called once per frame
	void Update()
	{
		CheckTouch();
	}
	
	
	private void CheckTouch()
	{
		switch (_platform)
		{
		case RuntimePlatform.Android:
		case RuntimePlatform.IPhonePlayer:
		{
			if (Input.touchCount > 0)
			{
				if (Input.GetTouch(0).phase == TouchPhase.Ended)
				{
					HandleTouch(Input.GetTouch(0).position);
				}
			}
			break;
		}
		case RuntimePlatform.WindowsPlayer:
		case RuntimePlatform.OSXPlayer:
		case RuntimePlatform.WindowsEditor:
		{
			if (Input.GetMouseButtonUp(0))
			{
				HandleTouch(Input.mousePosition);
			}
			break;
		}
		default:
		{
			Debug.Log("Invalid Runtime Platform!");
			Application.Quit();
			break;
		}
		}
	}
	
	private void HandleTouch(Vector3 touchPos)
	{
		touchPos = Camera.main.ScreenToWorldPoint(touchPos);
		Vector2 touchPos2D = new Vector2(touchPos.x, touchPos.y);
		
		Collider2D collider = Physics2D.OverlapPoint(touchPos2D);
		if (collider)
		{
			Debug.Log("Touched " + collider.transform.gameObject.name);
			collider.transform.gameObject.SendMessage("OnTouch",0,SendMessageOptions.DontRequireReceiver);
		}
	}
}
