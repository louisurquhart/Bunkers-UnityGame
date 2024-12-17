using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class AILogic : MonoBehaviour
{
    List<int> previouslyHitBunkers = new List<int>();

    private void Start()
    {
        
    }

    public static void EasyAI(GridManager gridManager) // Easy AI is very basic and only guesses random board positions
    {
        bool noPositionGenerated = true;

        if (CommonVariables.PlayerTurn == false)
        {

        }

        int iterations = 0;

        while (noPositionGenerated && iterations <= 10)
        {
            iterations++;

            Tile randomTile = GeneralBackgroundLogic.GenerateRandomTile(gridManager); // Uses GenerateRandomTile function to find a random tile 

            int tileHitSuccess;

            if (!randomTile.isPreviouslyHit) // Checks that the randomTile isn't previously hit + that the function hasn't failed and iterated more than 10 times
            {
                tileHitSuccess = gridManager.OnTileHit(randomTile);

                switch (tileHitSuccess) // Initiates the OnTileHit function for the random tile to tell it has been hit. It also evaluates if the bool return value wasn't successful for remedial action 
                {
                    case 0: // If 0 is returned it means the OnTileHit function was carried out successfully
                        return; // Exits out the function

                    case 1: // If 1 is returned it means the OnTileHit functions validation failed due to the tile already having been hit
                        Debug.LogError("AI is trying to hit a tile which has already been hit");
                        continue; // Takes remedial action continuing the function to try find a tile which hasn't been hit. (infinite loop is prevented by iteration limit)

                    case 2: // If 2 is returned it means the OnTileHit functions validation failed due to the it not being the AI's turn
                        Debug.LogError("AI is trying to hit a tile when it's not their turn");
                        return; // Exits out the function
                }
            }
            else
            {
                continue;
            }
        }
    }

    public static void MediumAI() // Medium AI is a moderately advanced AI which guesses around where previous bunkers have been hit but not fully destroyed until its destroyed the whole bunker.
    {

    }

    public static void HardAI() // Hard AI is a very advanced AI which guesses around where previous bunkers + countinues lines until a bunker has been fully destroyed.
    {

    }




}
