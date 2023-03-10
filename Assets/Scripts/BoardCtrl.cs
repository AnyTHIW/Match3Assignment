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

    public GameObject HolderChunk;
    public int[,] Holder;
    public GameObject[] SpawnerGroup;

    public int width = 7;
    public int height = 7;

    private void InspectHolderChunk()
    {

    }

    public void Awake()
    {

        InitializeHolder();
    }

    private void InitializeHolder()
    {
        Holder = new int[width, height];
        for (int i = 0; i < width; i ++)
        {
            for (int j = 0; j < height; j++)
            {
                FillSweets(i, j);
            }
        }
    }

    public void Start()
    {

    }

    public void Update()
    {
        Time.timeScale = 0F;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                GameObject selected = hit.collider.gameObject;

                int x = Mathf.RoundToInt(selected.transform.position.x);
                int y = Mathf.RoundToInt(selected.transform.position.y);

                GameObject target = null;

                //if (x > 0 && grid[x - 1, y] != null && grid[x - 1, y].tag == selected.tag)
                //{
                //    target = grid[x - 1, y];
                //}
                //if (x < width - 1 && grid[x + 1, y] != null && grid[x + 1, y].tag == selected.tag)
                //{
                //    target = grid[x + 1, y];
                //}
                //if (y > 0 && grid[x, y - 1] != null && grid[x, y - 1].tag == selected.tag)
                //{
                //    target = grid[x, y - 1];
                //}
                //if (y < height - 1 && grid[x, y + 1] != null && grid[x, y + 1].tag == selected.tag)
                //{
                //    target = grid[x, y + 1];
                //}
                if (target != null)
                {
                    Swap(selected, target);
                }
            }
        }
    }


    private void CheckMatch()
    {

    }

    private void Swap(GameObject a, GameObject b)
    {
        Vector3 temp = a.transform.position;
        a.transform.position = b.transform.position;
        b.transform.position = temp;
        //grid[(int)a.transform.position.x, (int)a.transform.position.y] = a;
        //grid[(int)b.transform.position.x, (int)b.transform.position.y] = b;
    }

    private void GetScore()
    {

    }

    private void UseMovement()
    {

    }

    private void FillSweets(int index_X, int index_Y)
    {
        GameObject select = null;

        SweetsType rand = (SweetsType)Random.Range(1, System.Enum.GetValues(typeof(SweetsType)).Length + 1);
        Holder[index_X, index_Y] = (int)rand;

        select = ObjectCtrl.Instance.GetObject(rand.ToString());

        Vector2 selectPos = new Vector2(index_X / HolderChunk.width, index_Y / HolderChunk.height);

    }

    private void DestroySweets()
    {

    }

    private void SwapSweets()
    {

    }

}
