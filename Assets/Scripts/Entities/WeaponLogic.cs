using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WeaponLogic : MonoBehaviour
{
    [HideInInspector]
    new public BoxCollider collider;
    private List<GameObject> targetList = new List<GameObject>();
    public string targetTag;

    private CharacterModel model;

    private WeaponConfig hitModel;

    private void Awake() {
        collider = GetComponent<BoxCollider>();
        collider.enabled = false;
    }
    
    public void Init(CharacterModel model)
    {
        this.model = model;
        Deactivate();
    }

    public void Activate(WeaponConfig hitModel)
    {
        this.hitModel = hitModel;
        collider.enabled = true;
    }

    public void Deactivate()
    {
        collider.enabled = false;
        targetList.Clear();
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
            other.GetComponent<DamageHandler>().Take(hitModel.hardTime, model.transform, hitModel.repelVelocity, hitModel.repelTransitionTime, hitModel.damageValue);
            if (hitModel.hitEffect != null)
            {
                // Apply hit effect
                Vector3 hitpoint = other.ClosestPointOnBounds(transform.position);
                SpawnObject(hitModel.hitEffect.spawn, hitpoint);
                if (hitModel.hitEffect.sound != null) 
                    model.PlayAudio(hitModel.hitEffect.sound);
                
                model.Spawn(hitModel.spawn);
            }

        }
    }

    private void SpawnObject(SpawnConfig data, Vector3 pos)
    {
        if (data != null && data.prefab != null)
        {
            var obj = Instantiate(data.prefab);
            obj.transform.position = pos + data.offset;
            obj.transform.LookAt(Camera.main.transform);
            obj.transform.eulerAngles += data.rotation;
            model.PlayAudio(data.sound);
        }
    }
}
