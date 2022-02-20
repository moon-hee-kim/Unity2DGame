using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public GameObject ammoPrefab;
	static List<GameObject> ammoPool;
	public int poolSize;
	
	void Awake()
	{
		if (ammoPool == null)
		{
			ammoPool = new List<GameObject>();
		}
		
		for (int i = 0; i < poolSize ; i++)
		{
		    GameObject ammoObject = Instantiate(ammoPrefab);
		    ammoObject.SetActive(false);
		    ammoPool.Add(ammoObject);
		}
	}
}
