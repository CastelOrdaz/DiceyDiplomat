using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBurglar : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float swapTime;

    private float swapCoolDown = 0;

    public void Update()
    {
        if (swapCoolDown < 0)
        {
            swapCoolDown = swapTime;
            sprite.flipX = !sprite.flipX;
        }

        swapCoolDown -= Time.deltaTime;
    }
}
