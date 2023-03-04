using CustomDic;
using System.Collections.Generic;
using UnityEngine;
using KeyType = System.String;

/*Ǯ ���� ������ Ŭ����(���ÿ�����Ʈ�� ����Ʈ�� ����)*/
[System.Serializable]
public class ObjectData : List<GameObject>
{
    public const int INITIALIZER_COUNT = 5;
    public const int MAX_COUNT = 100;

    public string name;
    public string objectType;
    public KeyType objectName;
    public int maxObjectCount;

    /*Ǯ������ Ŭ���� '������'*/
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
    private static ObjectCtrl instance;  //  ���� Ŭ���� �̱��� ����ȭ

    [SerializeField]
    public SerializableDictionary<KeyType, GameObject> ObjectDict = new SerializableDictionary<KeyType, GameObject>();   //  �̸��� �׻��� ������Ʈ ��ųʸ�

    [SerializeField]
    private static Dictionary<KeyType, List<GameObject>> PoolDict = new Dictionary<KeyType, List<GameObject>>();    // �̸��� ������Ʈ Ǯ�� 1:1������Ų ��ųʸ�

    [SerializeField]
    private static Dictionary<KeyType, ObjectData> DataDict = new Dictionary<KeyType, ObjectData>(); //  �̸��� ������Ʈ �����͸� 1:1������Ų ��ųʸ�

    private GameObject ObjectPoolInScene;  //  ���� �� �� �� ���� ������Ʈ Ǯ
    private GameObject TypeObjectInScene;    //  ���� �� �� ������Ʈ Ǯ Ÿ��

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

    ///*poolType�� ���� Pool�� �� Ǯ �����͸� ���� ��ȯ*/
    //public ObjectData GetRandomOnePoolFromPoolType(string poolType)
    //{
    //    ObjectData[] SelectedPools = new ObjectData[ObjectDict.Count];    //  �������� ���� ������Ʈ �迭
    //    int selectedPoolsCounter = 0;   //  �ش��ϴ� ������Ʈ�� ����

    //    foreach (KeyValuePair<KeyType, GameObject> itemPool in ObjectDict.Values)
    //    {
    //        //  ���ϴ� Ǯ �±���'���'
    //        if (itemPool.poolType == poolType)
    //        {
    //            SelectedPools[selectedPoolsCounter] = itemPool;
    //            selectedPoolsCounter++;    //  ������� Ű ����
    //        }
    //    }

    //    return SelectedPools[Random.Range(0, selectedPoolsCounter)];
    //}

    /*���� ������Ʈ�� �̸����� Ȯ��*/
    public GameObject CheckDeactivatedObject(KeyType name)
    {
        GameObject select = null;   //  �ӽ� �����̳�

        // ������Ʈ��ųʸ�
        if (PoolDict.ContainsKey(name))
        {
            // Ǯ������ �˻�
            foreach (GameObject obj in PoolDict[name])
            {
                if (!obj.activeSelf)
                {
                    select = obj;   //  �����̳ʿ� ���õ� ������Ʈ �Ҵ�
                }

            }

            // �����̳ʿ� ������Ʈ�� ���õ��� �ʾ�'�ٸ�'
            if (select == null)
            {
#if SHOW_DEBUG_MESSAGE
                Debug.Log("��� �����, ���� �����ؾߵ�");
#endif
            }

        }
        else
        {
#if SHOW_DEBUG_MESSAGE
            Debug.Log("CheckDeactivatedObject Ű�� �߸���");
#endif
        }

        return select;
    }

    ///*�̸����� Ǯ �����͸� �����ϴ� �Լ�*/
    //public PoolData GetPool(string name)
    //{
    //    PoolData select = null; //  �ӽ� �����̳�

    //    foreach (PoolData pool in ObjectDict.Values)
    //    {
    //        if (name == pool.name)
    //        {
    //            select = pool;
    //        }
    //    }

    //    return select;
    //}

    //    /*poolType���� ���� ������Ʈ ���� Ȯ�� �� �ش� ������Ʈ ��ȯ*/
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
    //            PoolData newPoolData;   //  �ӽ�

    //            // ������Ʈ��ųʸ�
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
    //                        Debug.Log("���� ������Ʈ�� ����, ���� �����ؾߵ�");
    //#endif
    //                        return null;
    //                    }
    //                }
    //            }
    //        }

    //#if SHOW_DEBUG_MESSAGE
    //        Debug.Log("Ű�� �߸���");
    //#endif
    //        return null;
    //    }

    /*������ ����, ������ ����Ʈ���� ����*/
    public GameObject GetObject(KeyType key)
    {
        GameObject select = null;   //  �ӽ� �����̳�

        select = CheckDeactivatedObject(key);   //  

        // �����̳ʰ� �������'��'
        if (select == null)
        {
#if SHOW_DEBUG_MESSAGE
            Debug.Log("������Ʈ ����");
#endif
            select = CloneObject(ObjectDict.GetValueOrDefault(key));  //  ������Ʈ ���� �� �Ҵ�
        }

        select.transform.SetParent(gameObject.transform);   // �θ� �Ҵ�
        select.SetActive(true); //  Ȱ��ȭ
        return select;
    }

    /*������Ʈ�� �����ϴ� �Լ�*/
    public GameObject CloneObject(GameObject obj)
    {
        GameObject select;    //  �ӽ� �����̳�

        select = Instantiate(obj) as GameObject;

        select.transform.SetParent(TypeObjectInScene.transform);

        select.SetActive(false);

        return select;
    }

    public void OnGameCleared()
    {

    }

    /*Ư�� Ÿ���� Ǯ�� ����� �Լ�*/
    private void MakeObjectPool(string type)
    {
        foreach (KeyValuePair<KeyType, GameObject> item in ObjectDict)
        {
            ObjectData newObjectData = new ObjectData(type, item.Key);  //  ������Ʈ��ųʸ��� ���� ������Ʈ ������ ����
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