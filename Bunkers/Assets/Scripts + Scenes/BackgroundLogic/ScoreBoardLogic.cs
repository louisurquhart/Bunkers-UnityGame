using TMPro;
using UnityEngine;

public class ScoreBoardLogic : MonoBehaviour
{
    // References to player + ai's name objects
    [SerializeField] TMP_Text _playerName;
    [SerializeField] TMP_Text _aiName;
    // References to player + ai's score objects
    [SerializeField] TMP_Text _playerScore;
    [SerializeField] TMP_Text _aiScore;

    void Start() // Start is called before the first frame update
    {
        // Loads names + scores on start: 
        loadNames();
        loadScores();
    }
    private void loadNames() // Loads names on scoreboard to saved playerpref values
    {
        // Sets the name TMP objects text value to the values saved in playerprefs. If no value's saved, defaults of "You" + "AI" are loaded
        _playerName.text = PlayerPrefs.GetString("PlayerName", "You");
        _aiName.text = PlayerPrefs.GetString("AIName", "AI");
    }
    private void loadScores() // Loads scores on scoreboard to saved variable values
    {
        // Sets the score TMP objects text values to the score variables stored in CommonVariables + converts them to strings.
        _playerScore.text = CommonVariables.PlayerScore.ToString();
        _aiScore.text = CommonVariables.AIScore.ToString();
    }
}
