using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shout : MonoBehaviour
{
	[SerializeField]
	private GameObject _rangeObject;

	[SerializeField] private float _rangeSpeed = 1f;

	private float _duration;

	void Start()
	{
		_rangeObject.transform.localScale = Vector3.zero;
	}

	public void Activate(bool isShouting)
	{
		if (isShouting)
		{
			_duration += Time.deltaTime;
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
