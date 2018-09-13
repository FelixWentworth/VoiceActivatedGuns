using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
	[SerializeField] private Text _text;

	public void Set(Color textColor, string score)
	{
		_text.color = textColor;
		_text.text = score;
	}
}
