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
        objectType = type;
        objectName = keyName;
        maxObjectCount = MAX_COUNT;
    }

    public GameObject PoolLocation;
    public List<GameObject> Pool;
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
    private SerializableDictionary<KeyType, GameObject> ObjectDict = new SerializableDictionary<KeyType, GameObject>();   //  ������ ������Ʈ ���� ��ųʸ�

    [SerializeField]
    private GameObject ObjectPool;
    private List<ObjectData> ObjectDataList;

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

        ObjectPool = new GameObject("ObjectPoolingContainer");
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
    public GameObject CheckDeactivatedObject(string keyName)
    {
        ObjectData select = null;   //  �ӽ� �����̳�

        // ������Ʈ��ųʸ�
        if (ObjectDict.ContainsKey(keyName))
        {
            // Ǯ������ �˻�
            foreach (GameObject obj in ObjectDict[keyName].Pool)
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

    /*�̸����� Ǯ �����͸� �����ϴ� �Լ�*/
    public PoolData GetPool(string name)
    {
        PoolData select = null; //  �ӽ� �����̳�

        foreach (PoolData pool in ObjectDict.Values)
        {
            if (name == pool.name)
            {
                select = pool;
            }
        }

        return select;
    }

    /*poolType���� ���� ������Ʈ ���� Ȯ�� �� �ش� ������Ʈ ��ȯ*/
    public GameObject CheckDeactivatedObjectWithPoolType(string poolType)
    {
        List<string> keyNameList = new();

        foreach (PoolData pool in ObjectDict.Values)
        {
            if (pool.poolType == poolType)
            {
                keyNameList.Add(pool.name);
            }
        }

        foreach (string keyName in keyNameList)
        {
            PoolData newPoolData;   //  �ӽ�

            // ������Ʈ��ųʸ�
            if (ObjectDict.TryGetValue(keyName, out newPoolData))
            {
                foreach (GameObject obj in newPoolData.Pool)
                {
                    if (!obj.activeSelf)
                    {
                        return obj;
                    }
                    else
                    {
#if SHOW_DEBUG_MESSAGE
                        Debug.Log("���� ������Ʈ�� ����, ���� �����ؾߵ�");
#endif
                        return null;
                    }
                }
            }
        }

#if SHOW_DEBUG_MESSAGE
        Debug.Log("Ű�� �߸���");
#endif
        return null;
    }

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
            select = CloneObject(ObjectDict.GetValueOrDefault(key), ObjectDataList);  //  ������Ʈ ���� �� �Ҵ�
        }

        select.transform.SetParent(gameObject.transform);   // �θ� �Ҵ�
        select.SetActive(true); //  Ȱ��ȭ
        return select;
    }

    /*������Ʈ�� �����ϴ� �Լ�*/
    public GameObject CloneObject(GameObject obj, ObjectData data)
    {
        GameObject select;    //  �ӽ� �����̳�

        select = Instantiate(obj) as GameObject;
        data.Pool.Add(select);
        select.transform.SetParent(data.PoolLocation.transform);

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
            ObjectData newObjectData = new ObjectData(type, item.Key);
            newObjectData.objectName = item.Key;

            GameObject TypePool = new GameObject(type);
            ObjectPool.transform.SetParent(TypePool.transform);

            newObjectData.PoolLocation = TypePool;


            for (int i = 0; i < ObjectData.INITIALIZER_COUNT; i++)
            {
                TypePool.transform.SetParent(instance.CloneObject(item.Value, newObjectData).transform);
            }

            ObjectDataList.Add(newObjectData);
        }

    }


}