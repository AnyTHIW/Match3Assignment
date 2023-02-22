using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SweetsType
{
    Yellow,
    Red,
    Green,
    Purple,
}

public enum SpecialSweetsType
{
    PoppingCandy,

}

public class SweetsCtrl : MonoBehaviour
{
    public static SweetsCtrl Instance
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
    private static SweetsCtrl instance;

    private void MakeSpecialSweet()
    {

    }

    private void MakeInitialSweets()
    {

    }

    private void GetRandomSweet()
    {

    }

}
