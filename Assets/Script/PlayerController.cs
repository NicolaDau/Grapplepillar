using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [Header("Misc Stats:")]
    [SerializeField] float walkingSpeed;
    [SerializeField] float defaultAssSpeed;
    [SerializeField] float assSpeed()
    {
        return defaultAssSpeed + 100 * bodyParts.Count;
    }
    [SerializeField] int maxBodyParts;
    [SerializeField] int startingBodyParts;
    [SerializeField] float invincibilityFrames;
    [HideInInspector] bool invincible;
    [HideInInspector] bool flyingParts;

    [Header("Butterfly Stats:")]
    [SerializeField] float rallentyAmount;
    [SerializeField] float regenAmount;
    [SerializeField] float regenCooldown;
    [SerializeField] public float butterflyMaxTimer;
    [HideInInspector] public float butterflyCurrentTimer;
    [HideInInspector] bool inButterflyMode;
    [HideInInspector] bool cooldownOver;

    [Header("Prefabs:")]
    [SerializeField] GameObject bodyPrefab;
    [SerializeField] GameObject grapplePointPrefab;

    [Header("Other References:")]
    [SerializeField] Sprite[] bodySprites;
    [SerializeField] public GameObject head;
    [SerializeField] public GameObject lastBodyPart;
    [SerializeField] public List<GameObject> bodyParts = new List<GameObject>();
    [SerializeField] GrapplingRope grapplingRope;
    [SerializeField] PhysicsMaterial2D exBodyPartMaterial;
    Rigidbody2D rb;
    Rigidbody2D lastBodyPartRb;

    private void Awake()
    {
        for (int i = 0; i < startingBodyParts; i++)
        {
            AddBodyPart();
        }
    }
    private void Start()
    {  
        inButterflyMode = false;
        invincible = false;
        cooldownOver = true;
        butterflyCurrentTimer = butterflyMaxTimer;
        rb = head.gameObject.GetComponent<Rigidbody2D>();
        lastBodyPart = bodyParts.Last();
        lastBodyPartRb = lastBodyPart.gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
    if(Input.GetKeyDown(KeyCode.Q))
    {
        AddBodyPart();
     }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (flyingParts)
            {
                GameObject[] exBodyParts = GameObject.FindGameObjectsWithTag("ExBody");
                SpawnGrapplePoint(exBodyParts);
            }
             else if (lastBodyPart != head)
             { DetachBodyPart(); }
        }

       //if (!grapplingRope.enabled)
       //{
       //     Vector2 movementDirection = new Vector2(Input.GetAxis("Horizontal") * walkingSpeed * Time.deltaTime, 0);
       //    foreach(GameObject body in bodyParts)
       //    {
       //        body.gameObject.GetComponent<Rigidbody2D>().AddForce(movementDirection);
       //
       //    }
       //}

        if (grapplingRope.isGrappling && lastBodyPartRb != null)
        {
            Vector2 force = new Vector2(Input.GetAxis("Horizontal") * assSpeed() * Time.deltaTime, 0);
            lastBodyPartRb.AddForce(force);
        }

       // ToggleButterflyTime();
    }

    void ToggleButterflyTime()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && butterflyCurrentTimer > 0)
        {
            inButterflyMode = true;
            Time.timeScale = rallentyAmount;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) && inButterflyMode || butterflyCurrentTimer <= 0)
        {
            if(butterflyCurrentTimer <= 0)
            {
                butterflyCurrentTimer = regenAmount;
            }
            inButterflyMode = false;
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02F;
            StartCoroutine(ButterflyRechargeCooldown());
        }

        if (cooldownOver && !inButterflyMode && butterflyCurrentTimer <= butterflyMaxTimer)
        {
            butterflyCurrentTimer += regenAmount;
        }
        if(inButterflyMode)
        {
            butterflyCurrentTimer -= Time.deltaTime;
        }
    }

    IEnumerator ButterflyRechargeCooldown()
    {
        cooldownOver = false;
        yield return new WaitForSeconds(regenCooldown);
        cooldownOver = true;
    }
    public void AddBodyPart()
    {
        if(bodyParts.Count < maxBodyParts)
        {
            GameObject bodyPrefabClone = Instantiate(bodyPrefab, new Vector3(lastBodyPart.transform.position.x - 0.8f, lastBodyPart.transform.position.y, lastBodyPart.transform.position.z), Quaternion.identity) as GameObject;
            bodyPrefabClone.transform.parent = this.gameObject.transform;
            bodyPrefabClone.gameObject.GetComponent<SpringJoint2D>().connectedBody = lastBodyPart.gameObject.GetComponent<Rigidbody2D>();
            bodyPrefabClone.GetComponentInChildren<SpriteRenderer>().sprite = bodySprites[Random.Range(0, bodySprites.Count())];
            bodyParts.Add(bodyPrefabClone.gameObject);
            if (lastBodyPart != head)
            {
                lastBodyPart.GetComponentInChildren<TrailRenderer>().time = 0f;
            }
            LenghtUpdate();       
            StartCoroutine(Invincibility("Longer"));
        }
    }

   

    public void Hit()
    {
        if (!invincible)
        {
            if (bodyParts.Count > 1)
            {
                ParticleManager.Instance.SpawnOnce(ParticleManager.Instance.bloodSplatterBurstFX, lastBodyPart.transform.position);
                DetachBodyPart();
                AudioManager.Instance.PlayAudio(AudioManager.Instance.hit, 0.4f + 0.08f * bodyParts.Count);
                StartCoroutine(Invincibility("Hit"));
            }
            else
            {
                Die();
            }
        }
    }

    public void DetachBodyPart()
    {
            lastBodyPart.gameObject.tag = "ExBody";
            lastBodyPart.GetComponentInChildren<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
            Destroy(lastBodyPart.gameObject.GetComponent<SpringJoint2D>());
            if (lastBodyPart != head)
            {
                lastBodyPart.GetComponentInChildren<TrailRenderer>().time = 2f;
            }
            StartCoroutine(AutomaticDestruction(lastBodyPart));
            lastBodyPartRb.sharedMaterial = exBodyPartMaterial;
            bodyParts.Remove(bodyParts.Last());
            LenghtUpdate();
            flyingParts = true;
    }
    public void Die()
    {
        FindObjectOfType<CameraFollow>().targetPlayer = false;
        GameManager.Instance.EndLevel(SceneManager.GetActiveScene().buildIndex, "You Died!");
    }

    public void LenghtUpdate()
    {
        lastBodyPart = bodyParts.Last();
        lastBodyPartRb = lastBodyPart.GetComponent<Rigidbody2D>();
        lastBodyPart.GetComponentInChildren<TrailRenderer>().time = 0.2f;

    }
    void SpawnGrapplePoint(GameObject[] exBodyParts)
    {
        foreach (var item in exBodyParts)
        {
            Instantiate(grapplePointPrefab, item.transform.position, Quaternion.identity);
            grapplePointPrefab.transform.localScale = new Vector3(1, 1, 1);
            ParticleManager.Instance.SpawnOnce(ParticleManager.Instance.bloodSplatterBurstFX, item.transform.position);
            Destroy(item);
        }
        flyingParts = false;

    }

    IEnumerator Invincibility(string animName)
    {
        invincible = true;
        foreach (GameObject body in bodyParts)
        {
            body.GetComponent<Animator>().SetBool(animName, true);
        }

        yield return new WaitForSeconds(invincibilityFrames);
        foreach (GameObject body in bodyParts)
        {
            body.GetComponent<Animator>().SetBool(animName, false);
        }
        invincible = false;
    }
    IEnumerator AutomaticDestruction(GameObject exBodyPart)
    {
        yield return new WaitForSeconds(3);
        ParticleManager.Instance.SpawnOnce(ParticleManager.Instance.bloodSplatterBurstFX, exBodyPart.transform.position);
        Instantiate(grapplePointPrefab, exBodyPart.transform.position, Quaternion.identity);
        grapplePointPrefab.transform.localScale = new Vector3(1, 1, 1);
        Destroy(exBodyPart);
        flyingParts = false;
    }

}
