using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    public Animator EnemyAnim;

    private int MaxHealth = 100;
    private int CurrentHealth;
    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        EnemyAnim.Play("Hurt NoEffect");
        if (CurrentHealth <= 0) 
        {
            Die();
        }
    }

    void Die()
    {
        //Debug.Log("Died");
        EnemyAnim.SetBool("IsDead", true);
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }


}
