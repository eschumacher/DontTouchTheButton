using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

	ArrayList _pointsDigits = new ArrayList();
	public PointsDigit _digitsPrefab;
	public Sprite num0, num1, num2, num3, num4;
	public Sprite num5, num6, num7, num8, num9;
	private Sprite[] _numSprites;

	// Use this for initialization
	void Start ()
	{
		AddPointsDigit();
		PrintScore();
	}
	
	// Update is called once per frame
	void Update () {
	
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
