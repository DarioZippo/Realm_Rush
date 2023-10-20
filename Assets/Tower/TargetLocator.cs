using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    //Li rendo serializzabili solo per vederne lo stato dall'inspector, quindi per debug
    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticles;
    //1 tile e mezzo
    [SerializeField] float range = 15f;
    Transform target;

    // Update is called once per frame
    void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }
    
    void FindClosestTarget(){
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach(Enemy enemy in enemies){
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
            
            if(targetDistance < maxDistance){
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }
        target = closestTarget;
    }

    void AimWeapon(){
        float targetDistance = Vector3.Distance(transform.position, target.position);

        weapon.LookAt(target);

        if(targetDistance < range){
            Attack(true);
        }
        else{
            Attack(false);
        }
    }

    void Attack(bool isActive){
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive; 
    }
}
