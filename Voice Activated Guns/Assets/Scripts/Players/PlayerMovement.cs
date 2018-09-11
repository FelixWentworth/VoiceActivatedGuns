using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement 
{
	public int JoystickNumber { get; private set; }

	private Action<float> _move;
	private Action _stop;
	private Action _jump;
	private Action<bool> _shout;


	public PlayerMovement(int joystick, Action<float> move, Action stop, Action jump, Action<bool> shout)
	{
		JoystickNumber = joystick;
		_move = move;
		_stop = stop;
		_jump = jump;
		_shout = shout;
	}

	public void Tick()
	{
		var x = Input.GetAxis(GetJoystickName("Horizontal"));
		if (x > 0.05 || x < -0.05)
		{
			_move(x);
		}
		else
		{
			_stop();
		}
		if (Input.GetButtonDown(GetJoystickName("Jump")))
		{
			_jump();
		}
		if (Input.GetButton(GetJoystickName("Shout")))
		{
			_shout(true);
		}
		if (Input.GetButtonUp(GetJoystickName("Shout")))
		{
			_shout(false);
		}

		KeyboardControls();
	}

	private void KeyboardControls()
	{
		var x = Input.GetAxis("Horizontal");
		if (x > 0.05 || x < -0.05)
		{
			_move(x);
		}
		else
		{
			_stop();
		}
		if (Input.GetButtonDown("Jump"))
		{
			_jump();
		}
		if (Input.GetButton("Shout"))
		{
			_shout(true);
		}
		if (Input.GetButtonUp("Shout"))
		{
			_shout(false);
		}
	}



	private string GetJoystickName(string ext)
	{
		return "Joystick" + JoystickNumber + ext;
	}
}
