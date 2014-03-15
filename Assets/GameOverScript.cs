using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

	// number display variables
	ArrayList _pointsDigits = new ArrayList();
	public PointsDigit _digitsPrefab;
	public Sprite num0, num1, num2, num3, num4;
	public Sprite num5, num6, num7, num8, num9;
	private Sprite[] _numSprites;

	// touch variables
	RuntimePlatform _platform = Application.platform;

	// Use this for initialization
	void Start ()
	{
		_numSprites = new Sprite[10] { num0, num1, num2,
			num3, num4, num5,
			num6, num7, num8,
			num9 };

		AddPointsDigit();
		PrintScore();
	}
	
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

	private void PrintScore()
	{
		uint points = GameMaster.GetPoints();

		if (points <= 9)
		{
			((PointsDigit)_pointsDigits [0]).SetSprite (_numSprites[points]);
		}
		else if (points <= 99)
		{
			if (_pointsDigits.Count < 2)
			{
				AddPointsDigit();
			}
			
			((PointsDigit)_pointsDigits[0]).SetSprite(_numSprites[points / 10]);
			((PointsDigit)_pointsDigits[1]).SetSprite(_numSprites[points % 10]);
		}
		else
		{
			if (_pointsDigits.Count < 3)
			{
				AddPointsDigit();
			}
			
			((PointsDigit)_pointsDigits[0]).SetSprite(_numSprites[points / 100]);
			((PointsDigit)_pointsDigits[1]).SetSprite(_numSprites[(points % 100) / 10]);
			((PointsDigit)_pointsDigits[2]).SetSprite(_numSprites[points % 10]);
		}
	}
}
