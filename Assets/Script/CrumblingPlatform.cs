using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField] private float crumbleTimer = 2;

    public event Action<bool> crumbled;

    SpriteRenderer spriteRenderer;
    Collider2D col;


    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        col = gameObject.GetComponent<Collider2D>();
    }
    void HitByRay()
    {
        StartCoroutine(Crumble());
    }
     IEnumerator Crumble()
    {
        yield return new WaitForSeconds(crumbleTimer);
        spriteRenderer.enabled = false;
        col.enabled = false;
        crumbled(true);
        yield return new WaitForSeconds(crumbleTimer);
        spriteRenderer.enabled = true;
        col.enabled = true;
    }
}
