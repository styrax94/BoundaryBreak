using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMakerTest : MonoBehaviour
{
    //Prefab
    public GameObject tilePrefab;

    public float gridUnit;
    public GameObject[,] grid;
    public int gridSizeX;
    public int gridSizeZ;
    // Start is called before the first frame update
    void Start()
    {
        grid = new GameObject[gridSizeZ, gridSizeX];
       // Debug.Log(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int b = 0; b < transform.GetChild(i).childCount; b++)
            {
                grid[b, i] = transform.GetChild(i).GetChild(b).gameObject;
            }
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }

   

   [ContextMenu("SpawnGrid")]
   public void CreateGrid()
    {
        if(transform.childCount > 0)
        {
            ClearGrid();
        }

        grid = new GameObject[gridSizeX, gridSizeZ];

        for (int i = 0; i < gridSizeX; i++)
        {
            GameObject empty = new GameObject("Row " + i);
            empty.transform.parent = transform;
            for (int b = 0; b < gridSizeZ; b++)
            {
                GameObject tileOb = Instantiate(tilePrefab, empty.transform);
                tileOb.transform.position = new Vector3(transform.localScale.x * b, transform.position.y, transform.localScale.z * i);
                grid[i, b] = tileOb;
                            
            }
        }
    }


    public void ClearGrid()
    {
        for (int i = transform.childCount-1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public bool CheckInsideGrid(Vector2Int cood)
    {
        if (cood.x >= 0 && cood.x < grid.GetLength(0) && cood.y >= 0 && cood.y < grid.GetLength(1))
        {
            return true;
        }
        return false;
    }
}
