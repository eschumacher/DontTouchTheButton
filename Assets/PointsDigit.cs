﻿using UnityEngine;
using System.Collections;

public class PointsDigit : MonoBehaviour
{
	private SpriteRenderer _renderer;

	void Start()
	{
		_renderer = gameObject.GetComponent<SpriteRenderer> ();
	}

	public void SetSprite(Sprite sprite)
	{
		if (sprite)
		{
			_renderer.sprite = sprite;
		}
	}
}
