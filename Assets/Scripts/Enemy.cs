using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private BoxCollider boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AttackRange")
        {            
            StartCoroutine(Hit());
        }
    }

    private IEnumerator Hit()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }
}