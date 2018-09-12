using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
	public bool Reloading;

	[SerializeField] protected int ClipSize;
	[SerializeField] protected int Ammo; // unused


	[SerializeField] protected float _timeBetweenShots;
	[SerializeField] protected float _reloadTime;

	private int _shotsAvailable;
	protected float _requiredCooldownTime;
	protected float _cooldownTime;


	[SerializeField]
	protected GameObject _bullet;

	[SerializeField]
	protected Transform _bulletSpawnPosition;

	public Action<float> InRangeAction;

	public Action GunShotAction;

	void Awake()
	{
		InRangeAction = Fire;
		_shotsAvailable = ClipSize;
	}

	void Update()
	{
		Reloading = _cooldownTime < _requiredCooldownTime;

		if (Reloading)
		{
			_cooldownTime += Time.deltaTime;			
		}
	}

	protected virtual void Fire(float speed)
	{
		var bullet = Instantiate(_bullet, _bulletSpawnPosition.position, Quaternion.identity).GetComponent<Bullet>();
		bullet.transform.SetParent(transform, true);
		bullet.Fire(speed, transform.right);
		_shotsAvailable--;

		// if anything is listening to this action, call this
		if (GunShotAction != null)
		{
			GunShotAction();
		}

		if (_shotsAvailable <= 0)
		{
			// need to reload
			_cooldownTime = 0f;
			_requiredCooldownTime = _reloadTime;
			_shotsAvailable = ClipSize;
		}
		else
		{
			// need to wait some cooldown between shots
			_cooldownTime = 0f;
			_requiredCooldownTime = _timeBetweenShots;
		}
	}
}

