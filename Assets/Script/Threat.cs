using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Threat : MonoBehaviour
{
    CameraShake camShake;
    private void Start()
    {
        Animator anim = GetComponent<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
        camShake = FindObjectOfType<CameraShake>();

        GameManager.Instance.AddThreat(this);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.transform.parent.GetComponent<PlayerController>();
            player.Hit();
            camShake.shakeDuration = 0.1f;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.transform.parent.GetComponent<PlayerController>();
            player.Hit();
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GameplayArea"))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveThreat(this);
    }
}
