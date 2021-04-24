using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrapplingGun : MonoBehaviour
{
    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Scripts Ref:")]
    public GrapplingRope grappleRope;
    GameObject hitGameobject;
    PlayerController player;

    [Header("Layers Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int grappableLayerNumber = 9;

    [Header("Main Camera:")]
    public UnityEngine.Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float defaultMaxDistance;
    [HideInInspector] private float maxDistance()
    {
        return defaultMaxDistance + 0.5f * player.bodyParts.Count;
    }
    [SerializeField] GameObject gunPointer;

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;

    [Header("Miscellanea")]
    [SerializeField] private float grappleKeyRememberTime;
    [HideInInspector] private float grappleKeyCurrentRemember;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private float mouseWheelScale = 1;
    [SerializeField] private float defaultRopeLenghtSpeed = 3;
    private float ropeLengthSpeed()
    {
        return defaultRopeLenghtSpeed + (0.2f * player.bodyParts.Count);
    }

    [HideInInspector] private bool canGrapple;
    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        canGrapple = true;
        m_camera = UnityEngine.Camera.main;
    }

    private void Update()
    {
        grappleKeyCurrentRemember -= Time.deltaTime;
        SetGrapplePoint();

        if(Input.GetKey(KeyCode.S))
        {
            m_springJoint2D.distance += ropeLengthSpeed() * Time.deltaTime;
        }

        else if (Input.GetKey(KeyCode.W))
        {
            m_springJoint2D.distance -= ropeLengthSpeed() * Time.deltaTime;
        }

        else 
        {
            m_springJoint2D.distance -= Input.mouseScrollDelta.y * mouseWheelScale * Time.deltaTime;
        }

        m_springJoint2D.distance = Mathf.Clamp(m_springJoint2D.distance, 2, maxDistance());

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            grappleKeyCurrentRemember = grappleKeyRememberTime;

        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (grappleRope.enabled)
            {
                RotateGun(grapplePoint, false);
            }
            else
            {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }

            if (launchToPoint && grappleRope.isGrappling)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistnace;
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            EndGrapple(true);
        }
        else
        {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            RotateGun(mousePos, true);
        }

    }

    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void SetGrapplePoint()
    {
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
            if (_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
            {
                gunPointer.transform.position = m_camera.ScreenToWorldPoint(Input.mousePosition);
                if (Vector2.Distance(_hit.point, firePoint.position) <= maxDistance() || !hasMaxDistance)
                {
                    gunPointer.transform.position = _hit.transform.gameObject.transform.position; // _hit.point;

                    if(grappleKeyCurrentRemember > 0 && Input.GetKey(KeyCode.Mouse0))
                    {
                        grappleKeyCurrentRemember = 0;
                        grapplePoint = _hit.transform.gameObject.transform.position; // _hit.point;
                        grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                        grappleRope.enabled = true;
                        ParticleManager.Instance.SpawnOnce(ParticleManager.Instance.shootHookFX, gunPivot.transform.position + gunPivot.transform.right * 0.7f, gunPivot.transform.rotation);
                        
                        hitGameobject = _hit.transform.gameObject;
                        ParticleManager.Instance.SpawnOnce(ParticleManager.Instance.GrappledFX, hitGameobject.transform.position);
                        AudioManager.Instance.PlayAudio(AudioManager.Instance.grappled, 0.8f, 1.2f);                       
                        hitGameobject.GetComponent<GrapplePoint>().HitByRay();
                        hitGameobject.GetComponent<GrapplePoint>().crumbled += EndGrapple;
                        
                    }
                }
            }           
        }
    }

    public void Grapple()
    {
        canGrapple = false;
        m_springJoint2D.autoConfigureDistance = false;
        if (!launchToPoint && !autoConfigureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequncy;
        }
        if (!launchToPoint)
        {
            if (autoConfigureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }

            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }
        else
        {
            switch (launchType)
            {
                case LaunchType.Physics_Launch:
                    m_springJoint2D.connectedAnchor = grapplePoint;

                    Vector2 distanceVector = firePoint.position - gunHolder.position;

                    m_springJoint2D.distance = distanceVector.magnitude;
                    m_springJoint2D.frequency = launchSpeed;
                    m_springJoint2D.enabled = true;
                    break;
                case LaunchType.Transform_Launch:
                    m_rigidbody.gravityScale = 0;
                    m_rigidbody.velocity = Vector2.zero;
                    break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistance());
        }
    }

    public void EndGrapple(bool crumble)
    {
        if (!canGrapple)
        {
            StartCoroutine(GrappleCooldown());
        }

       if(hitGameobject.TryGetComponent(out GrapplePoint crumblingPlatform))
        {
            crumblingPlatform.EndGrapple();
            crumblingPlatform.crumbled -= EndGrapple;
        }

        hitGameobject = null;
        AudioManager.Instance.PlayAudio(AudioManager.Instance.leaveGrapple, 0.8f, 1.4f);
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        m_rigidbody.gravityScale = 1;
    }



    IEnumerator GrappleCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canGrapple = true;
    }

}
