using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	private Reaction _reaction;

	private Gun _gun;

	private PlayerMovement _playerMovement;

	void Awake()
	{
		_animator = GetComponentInChildren<Animator>();
		_rigidbody = GetComponent<Rigidbody2D>();
		_shout = GetComponentInChildren<Shout>();
		Alive = true;
		_startPos = transform.position;
		_startRot = transform.rotation;
		PickupGun(GetComponentInChildren<Gun>());
		_reaction = GetComponentInChildren<Reaction>();
	}

	void Start()
	{
		var playerNum = GameObject.Find("GameManager").GetComponent<GameManager>().JoinGame(this);

		_playerMovement = new PlayerMovement(playerNum, MoveAction, StopAction, JumpAction, ShoutAction);
	}

	void Update()
	{
		_playerMovement.Tick();
	}

	private void MoveAction(float x)
	{
		if (CanControl && Alive)
		{
			_animator.SetBool("Running", true);
			Move(x > 0);
		}
	}

	private void StopAction()
	{
		if (CanControl && Alive)
		{
			_animator.SetBool("Running", false);
		}
	}

	private void JumpAction()
	{
		if (CanControl && Alive)
		{
			_animator.SetBool("Running", false);
			_animator.SetTrigger("Jump");
			Jump();
		}
	}

	private void ShoutAction(bool shouting)
	{
		if (!_gun.Reloading && CanControl && Alive)
		{
			_shout.Activate(isShouting: shouting);
			if (!shouting)
			{
				GunShot();
			}
		}
	}

	public void Respawn()
	{
		Alive = true;
		transform.position = _startPos;
		transform.rotation = _startRot;
		StatusSprite.color = AliveColor;
		Stop();
	}

	public void PickupGun(Gun gun)
	{
		_gun = gun;
		_gun.GunShotAction = ResponseGunShot;
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

		var newPos = new Vector2(transform.position.x, transform.position.y) + (Vector2.right * x * _speed * Time.deltaTime);
		var dist = Vector2.Distance(transform.position, newPos);
		var rayHit = Physics2D.RaycastAll(transform.position, (newPos-new Vector2(transform.position.x, transform.position.y)), dist);

		Debug.DrawLine(transform.position, newPos, Color.red);

		// Cannot move if the movement will result in a wall hit
		if (rayHit == null || !rayHit.Any(r => r.collider.tag == "Level"))
		{
			transform.position = transform.position + (Vector3.right * x) * _speed * Time.deltaTime;
		}
		Debug.Log(rayHit.Length);
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
		_reaction.React(Reaction.ReactionType.Killed);
		StatusSprite.color = DeadColor;
		Alive = false;
	}

	public void GunShot()
	{
		_reaction.React(Reaction.ReactionType.FireGun);
	}

	public void ResponseGunShot()
	{
		_reaction.React(Reaction.ReactionType.ResponseFire);
	}


	public void LeaveGame()
	{
		GameObject.Find("GameManager").GetComponent<GameManager>().LeaveGame(this);
	}
}
