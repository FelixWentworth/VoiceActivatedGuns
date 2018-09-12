using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutRange : MonoBehaviour
{

	public void Stop()
	{
		var maxDist = transform.localScale.x / 2;
		var speed = transform.localScale.x;
		RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, maxDist, Vector3.right, maxDist);
		foreach (var raycastHit in hit)
		{
			if (raycastHit.collider.tag == "Gun" && Vector3.Distance(transform.position, raycastHit.transform.position) < maxDist)
			{
				raycastHit.collider.GetComponent<Gun>().InRangeAction(speed);
			}
		}
	}
}
