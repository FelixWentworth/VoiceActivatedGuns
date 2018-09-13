using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls 
{
	public int JoystickNumber { get; private set; }

	private readonly Action<float> _move;
	private readonly Action _stop;
	private readonly Action _jump;
	private readonly Action<bool> _shout;
	private readonly Action<int> _look;

	private bool _isShouting;

	public PlayerControls(int joystick, Action<float> move, Action stop, Action jump, Action<bool> shout, Action<int> look)
	{
		JoystickNumber = joystick;
		_move = move;
		_stop = stop;
		_jump = jump;
		_shout = shout;
		_look = look;
	}

	public void Tick()
	{
		// Movement
		var x = Input.GetAxis(GetJoystickName("Horizontal"));
		if (x > 0.05 || x < -0.05)
		{
			_move(x);
		}
		else
		{
			_stop();
		}

		// Looking
		var lookX = Input.GetAxis(GetJoystickName("LookHorizontal"));
		var lookY = Input.GetAxis(GetJoystickName("LookVertical"));
		var absX = Mathf.Abs(lookX);
		var absY = Mathf.Abs(lookY);
		if (absY > 0 || absX > 0)
		{
			// Get the axis that has the higher value
			if (absY > absX)
			{
				// up/down	
				var zRot = lookY > 0 ? 90 : -90;
				_look(zRot);
			}
			else
			{
				// left/righ
				var zRot = lookX > 0 ? 0 : 180;
				_look(zRot);
			}
		}

		// Actions
		var shouting = Input.GetAxis(GetJoystickName("Shout"));
		if (shouting > 0)
		{
			_shout(true);
			_isShouting = true;
		}
		else if (_isShouting)
		{
			_isShouting = false;
			_shout(false);
		}
		if (Input.GetButtonDown(GetJoystickName("Jump")))
		{
			_jump();
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
