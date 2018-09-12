using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
	[SerializeField]

	private float _baseSpeed = 2f;

	public TileBase Tile;

	private Vector2 _lastPos;

	public int debugTest = 1;

	private float _elapsedTime;
	private Player _player;

	public void Fire(float speed, Vector3 direction)
	{
		_player = GetComponentInParent<Player>();
		GetComponent<Rigidbody2D>().AddForce(direction * _baseSpeed * speed , ForceMode2D.Force);
	}


	void Start()
	{
		_lastPos = transform.position;
	}

	private void FixedUpdate()
	{
		_elapsedTime += Time.fixedDeltaTime;
		CalculateHit();
		_lastPos = transform.position;	
	}

	private void CalculateHit()
	{
		//if (transform.rotation.)
		var direction = new Vector2(transform.position.x, transform.position.y) - _lastPos;
		var distance = Vector2.Distance(transform.position, _lastPos);
		Debug.DrawRay(_lastPos, direction, Color.red, 0.25f);
		var ray = Physics2D.RaycastAll(_lastPos, direction, distance);

		if (ray.Length != 0)
		{
			for (var i = 0; i < ray.Length; i++)
			{
				if (ray[i].collider.tag == "Level")
				{
					var rayPoint = ray[i].point;

					// Get TileMap
					var tileMap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
					var tilePos = tileMap.WorldToCell(rayPoint + direction/10); // add in the correct direction to make sure we actually get the correct block
					tileMap.SetTile(tilePos, null);
					Destroy(this.gameObject);
				}
				else if (ray[i].collider.tag == "Bullet")
				{
					Destroy(ray[i].collider.gameObject);
					Destroy(this.gameObject);
				}
				else if (ray[i].collider.tag == "Player")
				{
					var player = ray[i].collider.GetComponent<Player>();
					if (player == _player && _elapsedTime <= 0.5f)
					{
						continue;
					}
					player.Hit();
					Destroy(this.gameObject);
				}
			}
			//if (ray.Any(r => r.collider.tag == "Gun"))
			//{
			//	ray.First(r => r.collider.tag == "Gun").collider.GetComponentInParent<Player>().Hit();
			//}
		}
	}



	private void CalculateHit(Vector2 contactPoint)
	{
		//if (transform.rotation.)
		var direction = contactPoint - _lastPos;

		Debug.DrawRay(_lastPos, direction, Color.red, 0.5f);
		var ray = Physics2D.Raycast(_lastPos, direction, 1f);

		if (ray.collider != null)
		{
			Debug.Log(ray.collider.tag);
			if (ray.collider.tag == "Level")
			{
				// Get TileMap
				var tileMap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
				var tilePos = tileMap.WorldToCell(ray.point + (direction/10)); // add in the correct direction to make sure we actually get the correct block
				tileMap.SetTile(tilePos, null);
			}
			if (ray.collider.tag == "Player")
			{
				ray.collider.GetComponent<Player>().Hit();
			}
			if (ray.collider.tag == "Gun")
			{
				ray.collider.GetComponentInParent<Player>().Hit();
			}
		}
	}

	//void OnCollisionEnter2D(Collision2D other)
	//{
	//	GetComponent<CircleCollider2D>().enabled = false;
	//	CalculateHit(other.contacts[0].point);
	//	//if (other.gameObject.tag == "Level")
	//	//{
	//	//	// Get TileMap
	//	//	var tileMap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
	//	//	var tilePos = tileMap.WorldToCell(other.contacts[0].point);
	//	//	tileMap.SetTile(tilePos, null);
	//	//}
	//	//if (other.gameObject.tag == "Player")
	//	//{
	//	//	other.gameObject.GetComponent<Player>().Hit();
	//	//}
	//	Destroy(this.gameObject);
	//}
}
