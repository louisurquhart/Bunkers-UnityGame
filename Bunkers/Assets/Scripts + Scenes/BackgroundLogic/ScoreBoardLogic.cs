using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoardLogic : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text aiName;

    [SerializeField] TMP_Text playerScore;
    [SerializeField] TMP_Text aiScore;

    void Start() // Start is called before the first frame update
    {
        // Loads names + scores on start: 
        loadNames();
        loadScores();
    }
    private void loadNames() // Loads names on scoreboard to saved playerpref values
    {
        // Sets the name TMP objects text value to the values saved in playerprefs. If no value's saved, defaults of "You" + "AI" are loaded
        playerName.text = PlayerPrefs.GetString("PlayerName", "You");
        aiName.text = PlayerPrefs.GetString("AIName", "AI");
    }
    private void loadScores() // Loads scores on scoreboard to saved variable values
    {
        // Sets the score TMP objects text values to the score variables stored in CommonVariables + converts them to strings.
        playerScore.text = CommonVariables.PlayerScore.ToString();
        aiScore.text = CommonVariables.AIScore.ToString();
    }
}
