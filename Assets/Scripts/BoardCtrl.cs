using System.Collections.Generic;
using UnityEngine;

public class BoardCtrl : MonoBehaviour
{
    public static BoardCtrl Instance
    {
        get
        {
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static BoardCtrl instance;

    private int spawnerNumber;
    private int cellsAreaCount;

    private Vector2 CellsAreaOffset;
    private Vector2 CellsAreaSize;

    public GameObject CellReference;
    public GameObject SpawningPointGroup;
    public GameObject[,] grid;

    public void Awake()
    {
        CellsAreaSize = gameObject.GetComponent<BoxCollider2D>().size;
        CellsAreaOffset = gameObject.GetComponent<BoxCollider2D>().offset;
    }

    public void Start()
    {
        //CheckBoardInfo();

        //int cellNumberX = cellsAreaSize / cell;
        //int cellNumberY = cellsAreaSize / cellsAreaSize;
        //int width = (int)cellSize.x;
        //int heigth = (int)cellSize.y;
        //grid = new GameObject[width, heigth];

        //for (int x = 0; x < width; x++)
        //{
        //    for (int y = 0; y < heigth; y++)
        //{
        //    //grid[x, y] = Instantiate(tiles[Random.Range(0, tiles.Length)], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        //}
        //}

    }

    //private void MakeBoard()
    //{

    //}

    // 전반적인 길이, 높이, 스포너 등을 확인
    private void CheckBoardInfo()
    {
        spawnerNumber = GameObject.FindGameObjectsWithTag("SPAWNER").Length;
        CheckCellsAreaInfo();
        //InitBoard(cellsAreaInfo);
    }

    private void CheckCellsAreaInfo()
    {
        GameObject[] tmp;

        tmp = GameObject.FindGameObjectsWithTag("CELLSAREA");

        for (int i = 0; i < tmp.Length; i++)
        {
            //cellsAreaInfo.Add(tmp[i].GetComponent<BoxCollider2D>().size);
        }
    }

    private void InitBoard(List<Vector2> areaInfo)
    {
        foreach (Vector2 item in areaInfo)
        {
            //int VerticalRowNumber = (int)(item.x / cellSize.x);
            //int CellsNumberInVerticalRow = (int)(item.y / cellSize.y);

            //SweetsCtrl.instance
        }


    }

    //private Vector2 CalcCellsNumberInCellsArea(Vector2 size)
    //{
    //    //Vector2 cellCount = new Vector2 ((int)(size.x / cellSize.x), (int)(size.y / cellSize.y));
    //    //return cellCount;
    //}

    private void FillEmptyCell()
    {
        // 빈칸 생겼을때, 스포너에서 빈칸 채우기
    }

}
