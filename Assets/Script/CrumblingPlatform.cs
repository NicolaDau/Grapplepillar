using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField] private float crumbleTimer = 2;

    public event Action<bool> crumbled;

    [SerializeField]SpriteRenderer PlatformSprite, CursorSprite;
    Collider2D col;


    private void Start()
    {
        col = gameObject.GetComponent<Collider2D>();
        CursorSprite.enabled = false;

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "PlayerDistance")
        {
            CursorSprite.enabled = true;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "PlayerDistance")
        {
            CursorSprite.enabled = false;
        }
    }

    public void HitByRay()
    {
        //StartCoroutine(Crumble());
    }
     IEnumerator Crumble()
    {
        yield return new WaitForSeconds(crumbleTimer);
        if(crumbled != null)
        {
            crumbled(true);
        }
        PlatformSprite.enabled = false;
        col.enabled = false;
        yield return new WaitForSeconds(crumbleTimer);
        PlatformSprite.enabled = true;
        col.enabled = true;
        if (crumbled != null)
        {
            crumbled(false);
        }
    }
}
