using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectPool
{

    private int nOfObjects = 1;
    private GameObject archetype;
    private GameObject poolAvailableParent;
    
    private GameObject[] pool;
    private bool[] inPoolMask;
    
    public GameObjectPool(int maxNOfObjects, GameObject archetype)
    {
        this.poolAvailableParent = new GameObject();
        this.inPoolMask = new bool[maxNOfObjects];
        this.pool = new GameObject[maxNOfObjects];
        this.nOfObjects = maxNOfObjects;
        this.archetype = archetype;
        EnlargePool(nOfObjects);
    }

    public bool isEmpty()
    {
        return inPoolMask.Contains(true);
    }
    public GameObject GetFromPool()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if(inPoolMask[i]){
                inPoolMask[i] = false;
                pool[i].SetActive(true);
                return pool[i];
            }
        }

        return null;
    }
    public void ReturnToPool(GameObject obj)
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if(obj == pool[i]){
                pool[i].SetActive(false);
                pool[i].transform.parent = poolAvailableParent.transform;
                inPoolMask[i] = true;
            }
        }
    }

    public void EnlargePool(int by)
    {
        GeneratePoolObjects(by);       
    }

    private void GeneratePoolObjects(int n)
    {
        for (int i = 0; i < n; i++)
        {
            var obj = MonoBehaviour.Instantiate(archetype); 
            obj.SetActive(false);
            obj.transform.parent = poolAvailableParent.transform;
            pool[i] = obj;
            inPoolMask[i] = true;
        }   
    }
    
    

}
