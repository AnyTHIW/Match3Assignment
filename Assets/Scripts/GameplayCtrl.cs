using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCtrl : MonoBehaviour
{
    public static GameplayCtrl Instance
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
    private static GameplayCtrl instance;

    private void Start()
    {

    }

    private void Update()
    {

    }

}
