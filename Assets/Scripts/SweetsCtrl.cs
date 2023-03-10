using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SweetsType
{
    Apple = 0,
    Banana,
    Blueberry,
    Grape,
    Munchkin,
    Orange,
    Pear,
    RainbowMarsh,
}

public enum SpecialSweetsType
{
    None,
    HorizontalWrapped,
    VerticalWrapped,
    Package,
    Big,
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
