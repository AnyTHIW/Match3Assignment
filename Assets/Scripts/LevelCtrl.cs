using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCtrl : MonoBehaviour
{
    public static LevelCtrl Instance
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
    private static LevelCtrl instance;




    private int goalCounter;
    private int moveCounter;

    private void Awake()
    {



    }

    private void Start()
    {
        CheckLevelInfo();
    }







    private void CheckLevelInfo()
    {

    }

}
