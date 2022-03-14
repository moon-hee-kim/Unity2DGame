using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
	public GameObject ammoPrefab;
	static List<GameObject> ammoPool;
	public int poolSize;
	
	public float weaponVelocity;
	bool isFiring;
	
	Camera localCamera;
	
	[HideInInspector]
	public Animator animator;
	
	float positiveSlope;
	float negativeSlope;
	
	enum Quadrant
	{
	    East,
	    South,
	    West,
	    North
	}
	
	void Start()
	{
	    animator = GetComponent<Animator>();
	    isFiring = false;
	    localCamera = Camera.main;
	    
	    Vector2 lowerLeft = localCamera.ScreenToWorldPoint(new Vector2(0, 0));
	    Vector2 lowerRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0));
	    Vector2 upperLeft = localCamera.ScreenToWorldPoint(new Vector2(0, Screen.height));
	    Vector2 upperRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
	    
	    positiveSlope = GetSlope(lowerLeft, upperRight);
	    negativeSlope = GetSlope(upperLeft, lowerRight);
	}
	
	bool HigherThanPositiveSlopeLine()
	{
	    Vector2 playerPosition = gameObject.transform.position;
	    Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);
	    
	    float yIntercept = playerPosition.y - (positiveSlope * playerPosition.x);
	    float inputIntercept = mousePosition.y - (positiveSlope * mousePosition.x);
	    
	    return inputIntercept > yIntercept;
	}
	
	Quadrant GetQuadrant()
    {
        bool higherThanPositiveSlopeLine = HigherThanPositiveSlopeLine(Input.mousePosition);
        bool higherThanNegativeSlopeLine = HigherThanNegativeSlopeLine(Input.mousePosition);

        if (!higherThanPositiveSlopeLine && higherThanNegativeSlopeLine)
        {
            return Quadrant.East;
        }
        else if (!higherThanPositiveSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.South;
        }
        else if (higherThanPositiveSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.West;
        }
        else
        {
            return Quadrant.North;
        }
    }
	
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
	
	void Update()
	{
	    if (Input.GetMouseButtonDown(0))
	    {
	        isFiring = true;
	        FireAmmo();
	    }
	    
	    UpdateState();
	}
	
	float GetSlope(Vector2 point_a, Vector2 point_b)
	{
	    return (point_b.y - point_a.y) / (point_b.x - point_a.x);
	}
	
	GameObject SpawnAmmo(Vector3 location)
	{
	    foreach (GameObject ammo in ammoPool)
	    {
	        if (ammo.activeSelf == false)
	        {
	            ammo.SetActive(true);
	            ammo.transform.position = location;
	            
	            return ammo;
	        }
	    }
	    
	    return null;   
	}
	
	void UpdateState()
	{
        if (isFiring)
        {
            Vector2 quadrantVector;
            Quadrant quadEnum = GetQuadrant();

            switch (quadEnum)
            {
                case Quadrant.East:
                    quadrantVector = new Vector2(1.0f, 0.0f);
                    break;
                case Quadrant.South:
                    quadrantVector = new Vector2(0.0f, -1.0f);
                    break;
                case Quadrant.West:
                    quadrantVector = new Vector2(-1.0f, 0.0f);
                    break;
                case Quadrant.North:
                    quadrantVector = new Vector2(0.0f, 1.0f);
                    break;
                default:
                    quadrantVector = new Vector2(0.0f, 0.0f);
                    break;
            }

            animator.SetBool("isFiring", true);
            
            animator.SetFloat("fireXDir", quadrantVector.x);
            animator.SetFloat("fireYDir", quadrantVector.y);

            isFiring = false;
        }
        else
        {
            animator.SetBool("isFiring", false);
        }
	}
	
	void FireAmmo()
	{
	    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        GameObject ammo = SpawnAmmo(transform.position);
        
        if (ammo != null)
        {
            Arc arcScript = ammo.GetComponent<Arc>();
            
            float travelDuration = 1.0f / weaponVelocity;
            
            StartCoroutine(arcScript.TravelArc(mousePosition, travelDuration));
        }
	}
	
	void OnDestroy()
	{
	    ammoPool = null;
	}
	
}
