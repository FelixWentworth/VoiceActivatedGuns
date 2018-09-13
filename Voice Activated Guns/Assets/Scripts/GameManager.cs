using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public Color[] PlayerColors;

	[SerializeField] private Text _gameTimeText;
	[SerializeField] private float _timeAvailable;

	private List<Player> _players = new List<Player>();
	private float _timeElapsed;

	private bool _roundComplete;

	public int JoinGame(Player p)
	{
		if (!_players.Contains(p))
		{
			_players.Add(p);
			p.Score = 0;
			p.SetColor(PlayerColors[_players.Count-1]);
		}
		return _players.Count;
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
		if (_timeElapsed >= _timeAvailable || GetAlivePlayers() <= 1 && !_roundComplete)
		{
			_roundComplete = true;
			var winningPlayer = _players.FirstOrDefault(p => p.Alive);
			StartCoroutine(GameWon(winningPlayer));
		}
		else
		{
			SetUI();
		}
	}

	private void SetUI()
	{
		_gameTimeText.text = (_timeAvailable - _timeElapsed).ToString("0") + "s";
	}

	private IEnumerator GameWon(Player winner)
	{
		// Update Scores 
		if (winner != null)
		{
			winner.Score++;
		}
		yield return new WaitForSeconds(1.0f);
		// Display Winning Player
		
		// New Round
		NewRound();
	}

	private void NewRound()
	{
		
		// Load Level
		
		// Respawn Characters
		_players.ForEach(p => p.Respawn());
		_roundComplete = false;
	}

	private void NewGame()
	{
		// Reset Time
		_timeElapsed = 0f;
		// Respawn Characters
		_players.ForEach(p => p.Respawn());
		_roundComplete = false;
	}
}
