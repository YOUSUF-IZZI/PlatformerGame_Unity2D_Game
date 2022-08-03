using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    [SerializeField]
    Transform palyer;
    SpriteRenderer sr;

    private Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }


    void Update()
    {
        // make te Enemy diraction following the player diraction
        if (palyer.position.x > transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("isEagleDead", true);
        }
    }
}