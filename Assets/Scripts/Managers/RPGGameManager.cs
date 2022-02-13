using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGGameManager : MonoBehaviour
{
    public static RPGGameManager sharedInstance = null;
    
    void Awake()
    {
    	if (sharedInstance != null && sharedInstance != this)
    	{
    		Destroy(gameObject);
    	}
    	else
    	{
    		sharedInstance = this;
    	}
    }
    
    void Start()
    {
    	SetupScene();
    }
    
    public void SetupScene()
    {
    }
}
