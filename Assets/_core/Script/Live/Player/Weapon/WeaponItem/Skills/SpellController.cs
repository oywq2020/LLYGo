using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    public float speed = 0.1f;

    public GameObject explodePreb;
    private SphereCollider _sphereCollider;
    public float _flightHeight = 1.5f;

    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        //get the ball initial height
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var gameObjectTag = other.gameObject.tag;
        if (!gameObjectTag.Equals("Player")
            &&!gameObjectTag.Equals("Sensor"))
        {
            Instantiate(explodePreb, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed);
        if (Physics.Raycast(transform.position,Vector3.down,out RaycastHit raycastHit))
        {
            if (raycastHit.distance<_flightHeight-0.1)
            {
                transform.Translate(Vector3.up*0.1f);
            }
            else if (raycastHit.distance>_flightHeight+0.1)
            {
                transform.Translate(Vector3.down*0.1f);
            }
        }
    }
    
    
}
