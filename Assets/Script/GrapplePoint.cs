using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    [SerializeField] private float crumbleTimer = 2;
    public event Action<bool> crumbled;
    Vector3 targetSize;
    [SerializeField]SpriteRenderer PlatformSprite, CursorSprite;
    Collider2D col;
    Animator anim;
  
    ParticleSystem parSys;

    private void Start()
    {
        col = gameObject.GetComponent<Collider2D>();
        anim = gameObject.GetComponent<Animator>();
        parSys = gameObject.GetComponentInChildren<ParticleSystem>();
        GameManager.Instance.AddGrapplePoint(this);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "PlayerDistance")
        {
            anim.SetBool("PlayerGrappling", true);              
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "PlayerDistance")
        {
            anim.SetBool("PlayerGrappling", false);
        }

        if (collider.gameObject.CompareTag("GameplayArea"))
        {
            Destroy(this.gameObject);
        }
    }

    public void HitByRay()
    {
        //StartCoroutine(Crumble());
        var main = parSys.main;
        main.startSize = 0.15f;
        var em = parSys.emission;
        em.rateOverTime = 30;
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

    public void EndGrapple()
    {
        //StartCoroutine(Crumble());
        ParticleManager.Instance.SpawnOnce(ParticleManager.Instance.endGrappleFX, transform.position);

        var main = parSys.main;
        main.startSize = 0.1f;
        var em = parSys.emission;
        em.rateOverTime = 3;
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveGrapplePoint(this);
    }
}
