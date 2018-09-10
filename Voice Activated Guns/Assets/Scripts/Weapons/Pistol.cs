using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun{

	protected override void Fire(float speed)
	{
		base.Fire(speed);
		// Play gun sound
	}
}
