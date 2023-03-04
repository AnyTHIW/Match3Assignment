using CustomDic;
using System.Collections.Generic;
using UnityEngine;
using KeyType = System.String;

/*풀 정보 데이터 클래스(샘플오브젝트와 리스트를 포함)*/
[System.Serializable]
public class ObjectData : List<GameObject>
{
    public const int INITIALIZER_COUNT = 5;
    public const int MAX_COUNT = 100;

    public string name;
    public string objectType;
    public KeyType objectName;
    public int maxObjectCount;

    /*풀데이터 클래스 '생성자'*/
    public ObjectData(string type, KeyType keyName)
    {
        name = string.Format("{0}_Data", keyName);
        objectType = type;
        objectName = keyName;
        maxObjectCount = MAX_COUNT;
    }

    public List<GameObject> PoolLocation;
}

[DisallowMultipleComponent]
public class ObjectCtrl : MonoBehaviour
{
    public static ObjectCtrl Instance
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
    private static ObjectCtrl instance;  //  현재 클래스 싱글턴 패턴화

    [SerializeField]
    public SerializableDictionary<KeyType, GameObject> ObjectDict = new SerializableDictionary<KeyType, GameObject>();   //  이름과 그샘플 오브젝트 딕셔너리

    [SerializeField]
    private static Dictionary<KeyType, List<GameObject>> PoolDict = new Dictionary<KeyType, List<GameObject>>();    // 이름과 오브젝트 풀을 1:1대응시킨 딕셔너리

    [SerializeField]
    private static Dictionary<KeyType, ObjectData> DataDict = new Dictionary<KeyType, ObjectData>(); //  이름과 오브젝트 데이터를 1:1대응시킨 딕셔너리

    private GameObject ObjectPoolInScene;  //  게임 씬 내 빈 게임 오브젝트 풀
    private GameObject TypeObjectInScene;    //  게임 씬 내 오브젝트 풀 타입

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        ObjectPoolInScene = new GameObject("ObjectPoolingContainer");
        MakeObjectPool("SWEET");
    }

    private void Start()
    {
    }

    ///*poolType을 가진 Pool들 중 풀 데이터를 랜덤 반환*/
    //public ObjectData GetRandomOnePoolFromPoolType(string poolType)
    //{
    //    ObjectData[] SelectedPools = new ObjectData[ObjectDict.Count];    //  랜덤으로 뽑을 오브젝트 배열
    //    int selectedPoolsCounter = 0;   //  해당하는 오브젝트들 갯수

    //    foreach (KeyValuePair<KeyType, GameObject> itemPool in ObjectDict.Values)
    //    {
    //        //  원하는 풀 태그일'경우'
    //        if (itemPool.poolType == poolType)
    //        {
    //            SelectedPools[selectedPoolsCounter] = itemPool;
    //            selectedPoolsCounter++;    //  순서대로 키 저장
    //        }
    //    }

    //    return SelectedPools[Random.Range(0, selectedPoolsCounter)];
    //}

    /*꺼진 오브젝트를 이름으로 확인*/
    public GameObject CheckDeactivatedObject(KeyType name)
    {
        GameObject select = null;   //  임시 컨테이너

        // 오브젝트딕셔너리
        if (PoolDict.ContainsKey(name))
        {
            // 풀데이터 검색
            foreach (GameObject obj in PoolDict[name])
            {
                if (!obj.activeSelf)
                {
                    select = obj;   //  컨테이너에 선택된 오브젝트 할당
                }

            }

            // 컨테이너에 오브젝트가 선택되지 않았'다면'
            if (select == null)
            {
#if SHOW_DEBUG_MESSAGE
                Debug.Log("모두 사용중, 새로 생성해야됨");
#endif
            }

        }
        else
        {
#if SHOW_DEBUG_MESSAGE
            Debug.Log("CheckDeactivatedObject 키가 잘못됨");
#endif
        }

        return select;
    }

    ///*이름으로 풀 데이터를 리턴하는 함수*/
    //public PoolData GetPool(string name)
    //{
    //    PoolData select = null; //  임시 컨테이너

    //    foreach (PoolData pool in ObjectDict.Values)
    //    {
    //        if (name == pool.name)
    //        {
    //            select = pool;
    //        }
    //    }

    //    return select;
    //}

    //    /*poolType으로 꺼진 오브젝트 종류 확인 및 해당 오브젝트 반환*/
    //    public GameObject CheckDeactivatedObjectWithPoolType(string poolType)
    //    {
    //        List<string> keyNameList = new();

    //        foreach (ObjectData item in DataDict.Values)
    //        {
    //            if (item.objectType == poolType)
    //            {
    //                keyNameList.Add(item.objectType);
    //            }
    //        }

    //        foreach (string keyName in keyNameList)
    //        {
    //            PoolData newPoolData;   //  임시

    //            // 오브젝트딕셔너리
    //            if (ObjectDict.TryGetValue(keyName, out newPoolData))
    //            {
    //                foreach (GameObject obj in newPoolData.Pool)
    //                {
    //                    if (!obj.activeSelf)
    //                    {
    //                        return obj;
    //                    }
    //                    else
    //                    {
    //#if SHOW_DEBUG_MESSAGE
    //                        Debug.Log("꺼진 오브젝트가 없음, 새로 생성해야됨");
    //#endif
    //                        return null;
    //                    }
    //                }
    //            }
    //        }

    //#if SHOW_DEBUG_MESSAGE
    //        Debug.Log("키가 잘못됨");
    //#endif
    //        return null;
    //    }

    /*없으면 생성, 있으면 리스트에서 선택*/
    public GameObject GetObject(KeyType key)
    {
        GameObject select = null;   //  임시 컨테이너

        select = CheckDeactivatedObject(key);   //  

        // 컨테이너가 비어있으'면'
        if (select == null)
        {
#if SHOW_DEBUG_MESSAGE
            Debug.Log("오브젝트 생성");
#endif
            select = CloneObject(ObjectDict.GetValueOrDefault(key));  //  오브젝트 복사 후 할당
        }

        select.transform.SetParent(gameObject.transform);   // 부모 할당
        select.SetActive(true); //  활성화
        return select;
    }

    /*오브젝트를 복사하는 함수*/
    public GameObject CloneObject(GameObject obj)
    {
        GameObject select;    //  임시 컨테이너

        select = Instantiate(obj) as GameObject;

        select.transform.SetParent(TypeObjectInScene.transform);

        select.SetActive(false);

        return select;
    }

    public void OnGameCleared()
    {

    }

    /*특정 타입의 풀을 만드는 함수*/
    private void MakeObjectPool(string type)
    {
        foreach (KeyValuePair<KeyType, GameObject> item in ObjectDict)
        {
            ObjectData newObjectData = new ObjectData(type, item.Key);  //  오브젝트딕셔너리를 토대로 오브젝트 데이터 생성
            DataDict.Add(item.Key, newObjectData);
            newObjectData.PoolLocation = new List<GameObject>();
            PoolDict.Add(newObjectData.objectType, newObjectData.PoolLocation);

            TypeObjectInScene = new GameObject(newObjectData.objectType);
            TypeObjectInScene.transform.SetParent(ObjectPoolInScene.transform);

            for (int i = 0; i < ObjectData.INITIALIZER_COUNT; i++)
            {
                GameObject newObj = instance.CloneObject(item.Value);
                PoolDict[item.Key].Add(newObj);
            }
        }

    }

}