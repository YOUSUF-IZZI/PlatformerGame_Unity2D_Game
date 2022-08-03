using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    [SerializeField]
    float speed;

    // used to make Right and left movment for the Enemy
    bool isRight = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {

    }

    // used to apply with velocity insted of using Update func.
    private void FixedUpdate()
    {
        if (Mathf.Abs(rb.velocity.x) <= 0.1)
        {
            isRight = !isRight;
            sr.flipX = !sr.flipX;
        }

        if (isRight)
            rb.velocity = new Vector2(Time.fixedDeltaTime * speed, rb.velocity.y);
        else
            rb.velocity = new Vector2(Time.fixedDeltaTime * speed * -1, rb.velocity.y);
    }

}