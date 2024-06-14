using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class ArrowLogic : MonoBehaviour
{
    protected GameObject player;

    private List<GameObject> targetList = new List<GameObject>();
    public string targetTag;

    [SerializeField]
    float speed;
    float lifeTime;


    void Awake()
    {
        Debug.Log("Arrow created");
        player = GameObject.FindWithTag("Player");
        // Make sure the player is found.
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();
            Debug.Log("Player position is " + player.transform.position + "Arrow position is " + transform.position);
            GetComponent<Rigidbody>().velocity = direction * speed;
            // Make arrow face enemy.
            transform.LookAt(player.transform.position);
            transform.Rotate(-90, 0, 0, Space.Self);
        }
        else
        {
            Debug.LogWarning("Player Transform is not assigned.");
        }

        // Destroy(gameObject, lifeTime);
    }

    
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        /// NOTE: Make sure the "Include Layers" in the collider is set to the correct layer 
        ///       so that the weapon will only hit the target we want

        // Make sure that a damage will only apply once

        // if other is target and this is the first time we hit him
        if (other.CompareTag(targetTag) && !targetList.Contains(other.gameObject))
        {
            targetList.Add(other.gameObject);
            // Apply real damage
            Debug.Log("Apply Damage");
            Destroy(gameObject);
            // other.GetComponent<DamageHandler>().Take();
        }
    }

}
