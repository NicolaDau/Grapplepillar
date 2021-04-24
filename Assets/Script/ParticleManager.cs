using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] public GameObject coinFX;
    [SerializeField] public GameObject bloodSplatterBurstFX;
    [SerializeField] public GameObject shootHookFX;
    [SerializeField] public GameObject GrappledFX;
    [SerializeField] public GameObject endGrappleFX;

    public static ParticleManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SpawnOnce(GameObject fx, Vector3 pos)
    {
        Instantiate(fx, pos, Quaternion.identity);
    }
    public void SpawnOnce(GameObject fx, Vector3 pos, Quaternion direction)
    {
        Instantiate(fx, pos, direction);
    }
}
