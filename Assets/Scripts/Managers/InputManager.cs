using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	private string _currentKey = null;
	private float _heldTime = 0;

	// Singleton instance.
	public static InputManager Instance = null;

	// Initialize the singleton instance.
	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

    private void Update()
    {
		if (_currentKey == null && Input.anyKey)
		{
			if (Input.GetKey(InputNames.left))
				_currentKey = InputNames.left;
			else if (Input.GetKey(InputNames.right))
				_currentKey = InputNames.right;
			else if (Input.GetKey(InputNames.pause))
				_currentKey = InputNames.pause;
		} else if (_currentKey != null)
		{
			switch (Input.GetKey(_currentKey))
			{
				case true:
					_heldTime += Time.deltaTime;
					break;
				case false:
					_heldTime = 0;
					_currentKey = null;
					break;
			}
		}
	}

	public string GetcurrentKey(bool onlyPressed)
    {
		if (onlyPressed && _heldTime > 0)
			return null;
		return _currentKey;
	}

	public float GetHeldTime()
    {
		if (_currentKey == null)
			return -1;
		return _heldTime;
    }
}

public struct InputNames
{
	public static string left = "left";
	public static string right = "right";
	public static string pause = "escape";
}