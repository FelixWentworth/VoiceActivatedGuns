﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public bool CanControl;

	[HideInInspector]
	public bool Alive;

	[HideInInspector] public int Score;

	[SerializeField] private float _speed = 1f;
	[SerializeField] private float _jumpHeight = 1f;

	private Rigidbody2D _rigidbody;

	private Shout _shout;

	public SpriteRenderer StatusSprite;
	public Color AliveColor;
	public Color DeadColor;

	private Vector3 _startPos;
	private Quaternion _startRot;

	private Animator _animator;


	void Awake()
	{
		_animator = GetComponentInChildren<Animator>();
		_rigidbody = GetComponent<Rigidbody2D>();
		_shout = GetComponentInChildren<Shout>();
		Alive = true;
		_startPos = transform.position;
		_startRot = transform.rotation;
	}

	void Start()
	{
		GameObject.Find("GameManager").GetComponent<GameManager>().JoinGame(this);
	}

	public void Respawn()
	{
		Alive = true;
		transform.position = _startPos;
		transform.rotation = _startRot;
		Stop();
	}

	void Update()
	{
		_shout.enabled = CanControl && Alive;
		if (CanControl && Alive)
		{
			var x = Input.GetAxis("Horizontal");

			if (x > 0.05 || x < -0.05)
			{
				_animator.SetBool("Running", true);
				Move(x > 0);
			}
			else
			{
				_animator.SetBool("Running", false);
			}

			if (Input.GetKeyDown(KeyCode.Space))
			{
				_animator.SetBool("Running", false);
				_animator.SetTrigger("Jump");
				Jump();
			}
		}

		StatusSprite.color = Alive ? AliveColor : DeadColor;
	}

	private bool IsGrounded()
	{
		return _rigidbody.velocity.y <= 0.05 && _rigidbody.velocity.y >= -0.05;
	}

	private void Move(bool right)
	{
		var x = right ? 1 : -1;
		var yRot = right ? 0 : 180;
		transform.rotation = Quaternion.Euler(0, yRot, 0);
		transform.position = transform.position + (Vector3.right * x) * _speed * Time.deltaTime;
	}

	private void Stop()
	{
		_rigidbody.velocity = Vector2.zero;
	}

	private void Jump()
	{
		if (IsGrounded())
		{
			_rigidbody.AddForce(Vector2.up * _jumpHeight);
		}
	}

	public void Hit()
	{
		Stop();
		Alive = false;
	}

	public void LeaveGame()
	{
		GameObject.Find("GameManager").GetComponent<GameManager>().LeaveGame(this);
	}
}