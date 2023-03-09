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
    private GameObject TypeObjectInScene = null;    //  게임 씬 내 오브젝트 풀 타입

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
        MakePools("SWEETS");
    }

    private void Start()
    {
    }

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


    /*스위츠 풀을 만드는 함수*/
    private void MakePools(string objType)
    {
        foreach (KeyValuePair<KeyType, GameObject> item in ObjectDict)
        {
            ObjectData newObjectData = new ObjectData(objType, item.Key);  //  오브젝트딕셔너리를 토대로 오브젝트 데이터 생성

            DataDict.Add(item.Key, newObjectData);

            newObjectData.PoolLocation = new List<GameObject>();
            PoolDict.Add(newObjectData.objectName, newObjectData.PoolLocation);

            if (TypeObjectInScene == null)
            {
                TypeObjectInScene = new GameObject(newObjectData.objectType);
            }
            TypeObjectInScene.transform.SetParent(ObjectPoolInScene.transform);

            for (int i = 0; i < ObjectData.INITIALIZER_COUNT; i++)
            {
                GameObject newObj = instance.CloneObject(item.Value);
                newObj.name = item.Key;
                PoolDict[item.Key].Add(newObj);
            }
        }
    }

    public void OnGameCleared()
    {

    }


}