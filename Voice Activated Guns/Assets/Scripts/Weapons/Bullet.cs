using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField]
	private float _baseSpeed = 2f;

	public void Fire(float speed, Vector3 direction)
	{
		GetComponent<Rigidbody2D>().AddForce(direction * _baseSpeed * speed , ForceMode2D.Force);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<Player>().Hit();
		}
		Destroy(gameObject);
	}
}
