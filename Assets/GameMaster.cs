using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	RuntimePlatform _platform = Application.platform;
	float _timerRate = 5.0f;
	float _lifeTimer = 5.0f;
	uint _touches = 0;
	private static uint _points = 0;
	uint _pointsPerTouch = 1;
	public Button _buttonPrefab;
	public PowerUp2xScript _powerUp2xPrefab;
	ArrayList _buttons = new ArrayList();
	MonoBehaviour _powerup = null;
	byte _buttonsPerSpawn = 1;
	ArrayList _pointsDigits = new ArrayList();
	public PointsDigit _digitsPrefab;
	public Sprite num0, num1, num2, num3, num4;
	public Sprite num5, num6, num7, num8, num9;
	private Sprite[] _numSprites;
	public PointsDigit _timeDigit1, _timeDigit2, _timeDigit3;

	// power up variables
	float _powerup2xTimer = 0.0f;

	void Start()
	{
		_numSprites = new Sprite[10] { num0, num1, num2,
										num3, num4, num5,
										num6, num7, num8,
										num9 };
		SpawnButtons();
		AddPointsDigit();
	}

	// Update is called once per frame
	void Update()
	{
		CheckTouch();
		CheckActivePowerups();
		CheckLifeTimer();
		PrintScore();
		PrintTime();
	}

	public static void ResetGame()
	{
		_points = 0;
	}

	public static uint GetPoints()
	{
		return _points;
	}

	private void CheckLifeTimer()
	{
		_lifeTimer -= Time.deltaTime;
		
		Camera.main.backgroundColor = Color.Lerp (Color.red, Color.white, _lifeTimer / _timerRate);
		
		if (_lifeTimer <= 0.0f)
		{
			// Game over :( You died
			Application.LoadLevel("GameOverScene");
		}
	}

	private void CheckActivePowerups()
	{
		if (_powerup2xTimer > 0.0f)
		{
			// 2x powerup is active, let's check its timer
			_powerup2xTimer -= Time.deltaTime;
			if (_powerup2xTimer <= 0.0f)
			{
				_pointsPerTouch = 1;
			}
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
		_points += _pointsPerTouch;
		CheckLevelVariables();
		_lifeTimer = _timerRate;

		DestroyAllButtons();
		DestroyInactivePowerup();

		SpawnButtons();

		SpawnPowerup();
	}

	private void SpawnButtons()
	{
		for (int i=0; i<_buttonsPerSpawn; i++)
		{
			CreateButton ();
		}
	}

	private void SpawnPowerup()
	{
		int rand = Random.Range (1, 10);

		if (rand == 3)
		{
			CreatePowerUp(Random.Range(0, 0));
		}
	}

	private void DestroyAllButtons()
	{
		while (_buttons.Count > 0)
		{
			Destroy (((Button)_buttons[0]).gameObject);
			_buttons.RemoveAt(0);
		}
	}

	private void DestroyInactivePowerup()
	{
		if (_powerup != null)
		{
			Destroy (_powerup.gameObject);
		}
	}

	private void CheckLevelVariables()
	{
		switch (_touches)
		{
		case 5:
			_timerRate = 4.0f;
			_buttonsPerSpawn++;
			break;
		case 10:
			_timerRate = 3.0f;
			_buttonsPerSpawn++;
			break;
		case 15:
			_timerRate = 2.0f;
			_buttonsPerSpawn++;
			break;
		case 25:
			_timerRate = 1.0f;
			_buttonsPerSpawn++;
			break;
		case 50:
			_timerRate = 0.5f;
			_buttonsPerSpawn++;
			break;
		default:
			break;
		}
	}

	private void CreateButton()
	{
		Vector3 newPos = new Vector3(Random.Range(-3.0f, 3.4f),
		                             Random.Range(-4.2f, 3.1f),
		                             -1.0f);
		
		_buttons.Add(Instantiate(_buttonPrefab, newPos, Quaternion.identity));
	}

	private void CreatePowerUp(int pupType)
	{
		Vector3 newPos = new Vector3(Random.Range (-3.0f, 3.4f),
		                             Random.Range (-4.2f, 3.1f),
		                             -1.0f);

		Object prefab = null;
		switch (pupType)
		{
			case 0:
			{
				prefab = _powerUp2xPrefab;
				break;
			}
		}

		_powerup = (MonoBehaviour)Instantiate(prefab, newPos, Quaternion.identity);
	}

	private void AddPointsDigit()
	{
		// calc coords offset based on number of existing digits
		float xOffset = _pointsDigits.Count * 0.3f;

		Vector3 pos = new Vector3 (-0.8f + xOffset, 4.801f, 0.0f);

		_pointsDigits.Add (Instantiate (_digitsPrefab, pos, Quaternion.identity));
	}

	public void OnButtonTouch()
	{
		Application.LoadLevel("GameOverScene");
	}

	public void OnPowerup2xTouch()
	{
		// make each touch worth 2x points
		_pointsPerTouch = 2;

		// start a timer for the powerup to last
		_powerup2xTimer = 5.0f;

		// also treat this as a background touch
		OnBackgroundTouch();
	}

	private void PrintScore()
	{
		if (_points <= 9)
		{
			((PointsDigit)_pointsDigits [0]).SetSprite (_numSprites[_points]);
		}
		else if (_points <= 99)
		{
			if (_pointsDigits.Count < 2)
			{
				AddPointsDigit();
			}

			((PointsDigit)_pointsDigits[0]).SetSprite(_numSprites[_points / 10]);
			((PointsDigit)_pointsDigits[1]).SetSprite(_numSprites[_points % 10]);
		}
		else
		{
			if (_pointsDigits.Count < 3)
			{
				AddPointsDigit();
			}

			((PointsDigit)_pointsDigits[0]).SetSprite(_numSprites[_points / 100]);
			((PointsDigit)_pointsDigits[1]).SetSprite(_numSprites[(_points % 100) / 10]);
			((PointsDigit)_pointsDigits[2]).SetSprite(_numSprites[_points % 10]);
		}
	}

	private void PrintTime()
	{
		int ones = (int)(_lifeTimer);
		int fractional = (int)((_lifeTimer - ones) * 100);
		int decs = fractional / 10;
		int hunds = fractional % 10;

		if (ones >= 0)
		{
			_timeDigit1.SetSprite (_numSprites [ones]);
			_timeDigit2.SetSprite (_numSprites [decs]);
			_timeDigit3.SetSprite (_numSprites [hunds]);
		}
	}
}
