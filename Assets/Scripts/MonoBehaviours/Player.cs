using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("CanBePickedUp"))
		{
			Item hitObject = collision.gameObject.GetComponent<Consumable>().item;

			if (hitObject != null)
			{
				print("Hit: " + hitObject.objectName);
				collision.gameObject.SetActive(false);
			}
		}
	}
}
