using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shout : MonoBehaviour
{
	[SerializeField]
	private GameObject _rangeObject;

	[SerializeField] private float _rangeSpeed = 1f;

	private float _duration;

	[SerializeField] private float _minSpeed = 1f;
	[SerializeField] private float _maxSpeed = 10f;


	void Start()
	{
		Reset();
	}

	public void Reset()
	{
		_rangeObject.transform.localScale = Vector3.zero;
		_duration = 0f;
	}

	public void Activate(bool isShouting)
	{
		if (isShouting)
		{
			_duration += Time.deltaTime;
			_duration = Mathf.Clamp(_duration, _minSpeed, _maxSpeed);
			_rangeObject.transform.localScale = Vector3.one * _duration * _rangeSpeed;
		}
		else
		{
			_rangeObject.GetComponent<ShoutRange>().Stop();
			_duration = 0f;
			_rangeObject.transform.localScale = Vector3.zero;
		}
	}

}
