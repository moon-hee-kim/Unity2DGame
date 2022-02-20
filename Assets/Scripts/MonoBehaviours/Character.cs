using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Character : MonoBehaviour
{
    public float maxHitPoints; // 체력 최대값
    public float startingHitPoints; // 최초 체력값
    
    public enum CharacterCategory
    {
        PLAYER,
        ENEMY
    }
    
    public CharacterCategory characterCategory;
    
    public virtual void KillCharacter()
    {
    	Destroy(gameObject);
    }
 
    public abstract void ResetCharacter();
    public abstract IEnumerator DamageCharacter(int damage, float interval);
    
    public virtual IEnumerator FlickerCharacter()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        
        GetComponent<SpriteRenderer>().color = Color.white;
    }    
}
