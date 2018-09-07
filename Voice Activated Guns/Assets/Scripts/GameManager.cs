using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private float _timeAvailable;

	private List<Player> _players = new List<Player>();
	private float _timeElapsed;


	public void JoinGame(Player p)
	{
		if (!_players.Contains(p))
		{
			_players.Add(p);
		}
	}

	public void LeaveGame(Player p)
	{
		if (_players.Contains(p))
		{
			_players.Remove(p);
		}
	}

	private int GetPlayerCount()
	{
		return _players.Count;
	}

	private int GetAlivePlayers()
	{
		return _players.Count(p => p.Alive);
	}

	void Reset()
	{
		_timeElapsed = 0f;
		foreach (var player in _players)
		{
			player.Alive = true;
		}
	}

	private void Update()
	{
		_timeElapsed += Time.deltaTime;
		if (_timeElapsed >= _timeAvailable || GetAlivePlayers() <= 1)
		{
			var winningPlayer = _players.FirstOrDefault(p => p.Alive);
			StartCoroutine(GameWon(winningPlayer));

		}
	}

	private IEnumerator GameWon(Player winner)
	{
		yield return new WaitForSeconds(1.0f);
		// Display Winning Player
		// Show Score Increase
		if (winner != null)
		{
			winner.Score++;
		}

		// New Round
		NewRound();
	}

	private void NewRound()
	{
		// Load Level
		// Reset Time
		_timeElapsed = 0f;
		// Respawn Characters
		_players.ForEach(p => p.Respawn());

	}
}
