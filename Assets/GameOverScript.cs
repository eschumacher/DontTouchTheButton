using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

	// number display variables
	ArrayList _pointsDigits = new ArrayList();
	ArrayList _highScoreDigits = new ArrayList();
	public PointsDigit _digitsPrefab;
	public Sprite num0, num1, num2, num3, num4;
	public Sprite num5, num6, num7, num8, num9;
	private Sprite[] _numSprites;
	public RetryButtonScript _retryButton;
	public MenuButtonScript _menuButton;
	private float _buttonDisplayTimer = 1.2f;

	// touch variables
	RuntimePlatform _platform = Application.platform;

	// score variables
	uint _highScore = 0;
	uint _points = 0;

	// Use this for initialization
	void Start ()
	{
		_retryButton.renderer.enabled = false;
		_menuButton.renderer.enabled = false;
		_retryButton.collider2D.enabled = false;
		_menuButton.collider2D.enabled = false;

		_points = GameMaster.GetPoints();

		_highScore = (uint)PlayerPrefs.GetInt("HighScore", 0);
		UpdateHighScore();

		_numSprites = new Sprite[10] { num0, num1, num2,
			num3, num4, num5,
			num6, num7, num8,
			num9 };

		AddPointsDigit();
		AddHighScoreDigit();

		PrintScore();
		PrintHighScore();
	}
	
	// Update is called once per frame
	void Update()
	{
		_buttonDisplayTimer -= Time.deltaTime;

		if (_buttonDisplayTimer <= 0.0f)
		{
			_retryButton.renderer.enabled = true;
			_menuButton.renderer.enabled = true;
			_retryButton.collider2D.enabled = true;
			_menuButton.collider2D.enabled = true;
		}
		CheckTouch();
	}

	private void UpdateHighScore()
	{
		if (_points > _highScore)
		{
			_highScore = _points;
			PlayerPrefs.SetInt("HighScore", (int)_highScore);
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

	private void AddPointsDigit()
	{
		// calc coords offset based on number of existing digits
		float xOffset = _pointsDigits.Count * 0.3f;
		
		Vector3 pos = new Vector3 (1.2f + xOffset, 1.5191f, 0.0f);
		
		_pointsDigits.Add (Instantiate (_digitsPrefab, pos, Quaternion.identity));
	}

	private void AddHighScoreDigit()
	{
		// calc coords offset based on number of existing digits
		float xOffset = _highScoreDigits.Count * 0.3f;
		
		Vector3 pos = new Vector3 (1.2f + xOffset, 0.5f, 0.0f);
		
		_highScoreDigits.Add (Instantiate (_digitsPrefab, pos, Quaternion.identity));
	}

	private void PrintScore()
	{
		if (_points <= 9)
		{
			((PointsDigit)_pointsDigits [0]).SetSprite (_numSprites[_points]);
		}
		else if (_points <= 99)
		{
			while (_pointsDigits.Count < 2)
			{
				AddPointsDigit();
			}
			
			((PointsDigit)_pointsDigits[0]).SetSprite(_numSprites[_points / 10]);
			((PointsDigit)_pointsDigits[1]).SetSprite(_numSprites[_points % 10]);
		}
		else
		{
			while (_pointsDigits.Count < 3)
			{
				AddPointsDigit();
			}
			
			((PointsDigit)_pointsDigits[0]).SetSprite(_numSprites[_points / 100]);
			((PointsDigit)_pointsDigits[1]).SetSprite(_numSprites[(_points % 100) / 10]);
			((PointsDigit)_pointsDigits[2]).SetSprite(_numSprites[_points % 10]);
		}
	}

	private void PrintHighScore()
	{
		if (_highScore <= 9)
		{
			((PointsDigit)_highScoreDigits [0]).SetSprite (_numSprites[_highScore]);
		}
		else if (_highScore <= 99)
		{
			while (_highScoreDigits.Count < 2)
			{
				AddHighScoreDigit();
			}
			
			((PointsDigit)_highScoreDigits[0]).SetSprite(_numSprites[_highScore / 10]);
			((PointsDigit)_highScoreDigits[1]).SetSprite(_numSprites[_highScore % 10]);
		}
		else
		{
			while (_highScoreDigits.Count < 3)
			{
				AddHighScoreDigit();
			}

			((PointsDigit)_highScoreDigits[0]).SetSprite(_numSprites[_highScore / 100]);
			((PointsDigit)_highScoreDigits[1]).SetSprite(_numSprites[(_highScore % 100) / 10]);
			((PointsDigit)_highScoreDigits[2]).SetSprite(_numSprites[_highScore % 10]);
		}
	}
}
