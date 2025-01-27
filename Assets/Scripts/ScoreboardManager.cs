using TMPro;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour
{
    private TextMeshProUGUI _scoreboardText;
    void Start()
    {
        _scoreboardText = GetComponent<TextMeshProUGUI>();
        _scoreboardText.text = UIManager.Instance.scoreboardString;
    }
}
