using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	RuntimePlatform _platform = Application.platform;
	float _timerRate = 5.0f;
	float _lifeTimer = 5.0f;
	ulong _touches = 0;
	public Button _buttonPrefab;
	ArrayList _buttons = new ArrayList();

	void Start()
	{
		CreateButton();
	}

	// Update is called once per frame
	void Update()
	{
		CheckTouch();
		CheckLifeTimer();
	}

	private void CheckLifeTimer()
	{
		_lifeTimer -= Time.deltaTime;
		
		Camera.main.backgroundColor = Color.Lerp (Color.red, Color.white, _lifeTimer / _timerRate);
		
		if (_lifeTimer <= 0.0f)
		{
			// Game over :( You died
			Debug.Log("YOU'RE DEAD! :(");
			Application.Quit();
		}
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
	
	private bool HandleTouch(Vector3 touchPos)
	{
		touchPos = Camera.main.ScreenToWorldPoint(touchPos);
		Vector2 touchPos2D = new Vector2(touchPos.x, touchPos.y);
		
		Collider2D collider = Physics2D.OverlapPoint(touchPos2D);
		if (collider)
		{
			collider.transform.gameObject.SendMessage("OnTouch",0,SendMessageOptions.DontRequireReceiver);
			return true;
		}

		return false;
	}

	public void OnBackgroundTouch()
	{
		_touches++;
		_lifeTimer = _timerRate;

		Debug.Log ("" + _buttons.Count);

		while (_buttons.Count > 0)
		{
			Destroy (((Button)_buttons[0]).gameObject);
			_buttons.RemoveAt(0);
		}

		CreateButton();
	}

	private void CreateButton()
	{
		Vector3 newPos = new Vector3(Random.Range(-3.0f, 3.4f),
		                             Random.Range(-4.2f, 3.1f),
		                             -1.0f);
		
		_buttons.Add(Instantiate(_buttonPrefab, newPos, Quaternion.identity));
	}

	public void OnButtonTouch()
	{
		Debug.Log("Hit a button! Game over :(");
		Application.Quit();
	}

	void OnGUI()
	{
		GUI.Box (new Rect (10, 10, 50, 20), "" + _lifeTimer.ToString ("0"));
		GUI.Box (new Rect (100, 10, 50, 20), "" + _touches.ToString ("0"));
	}
}
