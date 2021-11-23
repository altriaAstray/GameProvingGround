using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class SimplePool : SingleToneManager<SimplePool>
    {

        //对象池
        public List<ObjectsPool> ObjectsList = new List<ObjectsPool>();

        Transform thisTransform;
        int[] numberObjects;
        GameObject[][] stObjects;
        public int numObjectsList;

        public void Awake()
        {
            Instance = this;
            Init();
        }

        public void Start()
        {
            
        }

        void Init()
        {
            thisTransform = transform;
            numObjectsList = ObjectsList.Count;
            AddObjectsToPool();
        }

        //将对象添加到池
        void AddObjectsToPool()
        {
            numberObjects = new int[numObjectsList];
            stObjects = new GameObject[numObjectsList][];

            for (int num = 0; num < numObjectsList; num++)
            {
                numberObjects[num] = ObjectsList[num].numberObjects;
                stObjects[num] = new GameObject[numberObjects[num]];
                InstanInPool(ObjectsList[num].obj, stObjects[num]);
            }
        }

        //池中的实例
        void InstanInPool(GameObject obj, GameObject[] objs)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i] = Instantiate(obj);
                objs[i].SetActive(false);
                objs[i].transform.parent = thisTransform;
            }
        }

        /// <summary>
        /// 给出对象(用于提供对象)
        /// </summary>
        /// <param name="numElement">列表索引</param>
        /// <returns></returns>
        public GameObject GiveObj(int numElement)
        {
            for (int i = 0; i < numberObjects[numElement]; i++)
            {
                if (!stObjects[numElement][i].activeSelf)
                    return stObjects[numElement][i];
            }

            if(Tools.IsDebug())
                Debug.Log("池中的对象已结束!");
            return null;
        }

        //接收对象
        public void Takeobj(GameObject obj)
        {
            if (obj.activeSelf) obj.SetActive(false);
            if (obj.transform.parent != thisTransform) obj.transform.parent = thisTransform;
        }
    }

    [System.Serializable]

    public class ObjectsPool
    {
        public GameObject obj;
        public int numberObjects = 100;
    }
}
