using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

	[SerializeField]
	private GameObject _bullet;

	[SerializeField]
	private Transform _bulletSpawnPosition;

	public Action<float> InRangeAction;

	void Awake()
	{
		InRangeAction = Fire;
	}

	public void Fire(float speed)
	{
		var bullet = Instantiate(_bullet, _bulletSpawnPosition.position, Quaternion.identity).GetComponent<Bullet>();
		bullet.Fire(speed, transform.right);
	}
}

