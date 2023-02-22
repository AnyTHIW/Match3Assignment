using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    Normal,
    Hole,
}

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


    public GameObject Board;
    public readonly Vector2 cellSize;

    private int spawnerNumber;

    private List<Vector2> cellsAreaInfo = new List<Vector2>();

    public GameObject[] tiles;
    public GameObject[,] grid;

    public void Awake()
    {
        Board = GameObject.FindWithTag("BOARD");
    }

    public void Start()
    {
        CheckBoardInfo();

        grid = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = Instantiate(tiles[Random.Range(0, tiles.Length)], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            }
        }

    }

    // 전반적인 길이, 높이, 스포너 등을 확인
    private void CheckBoardInfo()
    {
        spawnerNumber = GameObject.FindGameObjectsWithTag("SPAWNER").Length;
        CheckCellsAreaInfo();
        InitBoard(cellsAreaInfo);
    }

    private void CheckCellsAreaInfo()
    {
        GameObject[] tmp;

        tmp = GameObject.FindGameObjectsWithTag("CELLSAREA");

        for (int i = 0; i < tmp.Length; i++)
        {
            cellsAreaInfo.Add(tmp[i].GetComponent<BoxCollider2D>().size);
        }
    }

    private void InitBoard(List<Vector2> areaInfo)
    {
        foreach(Vector2 item in areaInfo)
        {
            int VerticalRowNumber = (int)(item.x / cellSize.x);
            int CellsNumberInVerticalRow = (int)(item.y / cellSize.y);

            SweetsCtrl.instance
        }







    }

    private Vector2 CalcCellsNumberInCellsArea(Vector2 size)
    {
        Vector2 cellCount = new Vector2 ((int)(size.x / cellSize.x), (int)(size.y / cellSize.y));
        return cellCount;
    }

    private void FillEmptyCell()
    {
        // 빈칸 생겼을때, 스포너에서 빈칸 채우기
    }

}
