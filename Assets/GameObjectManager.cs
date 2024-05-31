using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject storePrefab;
    public GameObject clearText;

    int[,] map; 
    GameObject[,] field; 

    bool IsClear()
    {
       
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 3)
                {
                    goals.Add(new Vector2Int(x, y));
                }   
            }
        }

        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];  

            if (f == null || f.tag != "Box")
            {
                return false;
            }  
        }

        return true;   
    }

  
    bool MoveNumber(Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0))
            return false;
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1))
            return false;

        if (field[moveTo.y, moveTo.x] != null
            && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;  
            bool success = MoveNumber(moveTo, moveTo + velocity);
            if (!success)
                return false;
        }  
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        Move move = field[moveTo.y, moveTo.x].GetComponent<Move>();
        move.MoveTo(new Vector3(moveTo.x, -1 * moveTo.y, 0));
        return true;
    }

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] != null
                    && field[y, x].tag == "Player")
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return new Vector2Int(-1, -1);  
    }

    void Start()
    {
        clearText.SetActive(false);
        map = new int[,]
        {
            {0,0,0,0},
            {0,3,1,0 },
            {0,2,0,0},
            {0,0,0,0 },
        }; 

        field = new GameObject
        [
            map.GetLength(0),
            map.GetLength(1)
        ];  

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    GameObject instance =Instantiate(playerPrefab,new Vector3(x, -1 * y, 0),Quaternion.identity);
                    field[y, x] = instance; 
                    
                } 
                else if (map[y, x] == 2)
                {
                    GameObject instance = Instantiate(boxPrefab,new Vector3(x, -1 * y, 0),Quaternion.identity);
                    field[y, x] = instance; 
                }  
                else if (map[y, x] == 3)
                {
                    GameObject instance = Instantiate(storePrefab,new Vector3(x, -1 * y, 0), Quaternion.identity);
                }   
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            var playerPosition = GetPlayerIndex();
            MoveNumber(playerPosition, new Vector2Int(playerPosition.x + 1, playerPosition.y));   

            if (IsClear())
                clearText.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            var playerPosition = GetPlayerIndex();
            MoveNumber(playerPosition, new Vector2Int(playerPosition.x - 1, playerPosition.y));   

            if (IsClear())
                clearText.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            var playerPosition = GetPlayerIndex();
            MoveNumber(playerPosition, new Vector2Int(playerPosition.x, playerPosition.y - 1));   

            if (IsClear())
                clearText.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            var playerPosition = GetPlayerIndex();
            MoveNumber(playerPosition, new Vector2Int(playerPosition.x, playerPosition.y + 1));   

            if (IsClear())
                clearText.SetActive(true);
        }
    }
}