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
    private GameObject TypeObjectInScene = null;    //  ���� �� �� ������Ʈ Ǯ Ÿ��

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


    /*������ Ǯ�� ����� �Լ�*/
    private void MakePools(string objType)
    {
        foreach (KeyValuePair<KeyType, GameObject> item in ObjectDict)
        {
            ObjectData newObjectData = new ObjectData(objType, item.Key);  //  ������Ʈ��ųʸ��� ���� ������Ʈ ������ ����

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