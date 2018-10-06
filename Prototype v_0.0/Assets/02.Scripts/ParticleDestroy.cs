using UnityEngine;
using System.Collections;

public class ParticleDestroy : MonoBehaviour
{

    public float DestroyTime = 2.0f;
    void Start()
    {
        Destroy(gameObject, DestroyTime);
    }
}


