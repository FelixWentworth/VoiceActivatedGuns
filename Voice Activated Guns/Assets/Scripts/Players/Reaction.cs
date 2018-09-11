using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction : MonoBehaviour
{

	public enum ReactionType
	{
		FireGun,
		ResponseFire,
		Killed,
	}


	[SerializeField] private GameObject _react;
	[SerializeField] private GameObject _fire;
	[SerializeField] private GameObject _killed;

	private Animation _animation;

	void Awake()
	{
		_animation = GetComponent<Animation>();
		DisableAll();
	}

	void Update()
	{
		transform.rotation = Quaternion.Euler(0,0, 0);
	}

	private void DisableAll()
	{
		_react.SetActive(false);
		_fire.SetActive(false);
		_killed.SetActive(false);
	}

	public void React(ReactionType reactionType)
	{
		DisableAll();
		switch (reactionType)
		{
			case ReactionType.FireGun:
				_fire.SetActive(true);
				break;
			case ReactionType.ResponseFire:
				_react.SetActive(true);
				break;
			case ReactionType.Killed:
				_killed.SetActive(true);
				break;
			default:
				throw new ArgumentOutOfRangeException("reactionType", reactionType, null);
		}

		_animation.Play();
	}

}
