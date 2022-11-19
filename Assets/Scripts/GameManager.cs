using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Transform emptySpace = null;

    private Camera rayCamera;

    [SerializeField] private TilesScript[] tiles;

    private int emptySpaceIndex = 8;

    private bool isFinished;

    

    [SerializeField] private GameObject endPanel, newScoreText;
    [SerializeField] private TextMeshProUGUI endPanelTimeText, bestScoreText;

    // Start is called before the first frame update
    void Start()
    {
        rayCamera= Camera.main;

        // caling the Shuffle() method to shuffle the tiles before the game begin.
        Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        // when player clicks the mouse button on the tile, the tile moves to the empty position
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = rayCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit && !isFinished)
            {
                if (Vector2.Distance(emptySpace.position, hit.transform.position) < 2.35)
                {
                    Vector2 lastEmptyPosition = emptySpace.position;
                    TilesScript thisTile = hit.transform.GetComponent<TilesScript>();
                    emptySpace.position = thisTile.targetPosition;
                    thisTile.targetPosition = lastEmptyPosition;

                    // find the index of the tile
                    int tileIndex = FindIndex(thisTile);

                    // after finding the position of the index SWAP THE POSITION WITH THE EMPTY SPACE
                    tiles[emptySpaceIndex] = tiles[tileIndex];
                    tiles[tileIndex] = null;
                    emptySpaceIndex = tileIndex;


                }
            }
        }
        if (!isFinished)
        {
            int correctTiles = 0;
            foreach (var a in tiles)
            {
                if (a != null)
                {
                    if (a.inRightPlace)
                    {
                        correctTiles++;
                    }
                }
            }

            if (correctTiles == tiles.Length - 1)
            {
                isFinished = true;
                endPanel.SetActive(true);
                var a = GetComponent<TimerScript>();
                    a.StopTimer();
                endPanelTimeText.text = (a.minutes < 10 ? "0" : "") + a.minutes +":" + (a.seconds < 10 ? "0" : "") + a.seconds;
               
            }
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // To change the position of each tile with another random tile
    public void Shuffle()
    {
        if (emptySpaceIndex != 8)
        {
            var tileOn09LastPos = tiles[8].targetPosition;
            tiles[8].targetPosition = emptySpace.position;
            emptySpace.position = tileOn09LastPos;
            tiles[emptySpaceIndex] = tiles[8];
            tiles[8] = null;
            emptySpaceIndex = 8;
        }
        int inversion;
        do
        {
            for (int i = 0; i < 8; i++)
            {
                // when we are shuffling our puzzle tile we don't want to move our empty space ; null element

                //to hold the current position
                var lastPos = tiles[i].targetPosition;

                //select random tile to swap their position
                int randomIndex = Random.Range(0, 8);
                tiles[i].targetPosition = tiles[randomIndex].targetPosition;
                tiles[randomIndex].targetPosition = lastPos;

                // Checking the solvability of the puzzle
                // by swaping the position of tile with [random index] with the with [i index]
                // when we shuffle the puzzle tiles we also change their position in the array
                var tile = tiles[i];
                tiles[i] = tiles[randomIndex];
                tiles[randomIndex] = tile;
            }
            inversion = GetInversions();
            
        }
        while(inversion%2 !=0);
    }


    //Method to find out the tile index in the array
    public int FindIndex(TilesScript ts)
    {
        for(int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null)
            {
                if (tiles[i] = ts)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    int GetInversions()
    {
        int inversionsSum = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            int thisTileInvertion = 0;
            for (int j = i; j < tiles.Length; j++)
            {
                if (tiles[j] != null)
                {
                    if (tiles[i].number > tiles[j].number)
                    {
                        thisTileInvertion++;
                    }
                }
            }
            inversionsSum += thisTileInvertion;
        }
        return inversionsSum;
    }
}
