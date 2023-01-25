using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private Status owner;
    private ITarget target;

    public void Shoot(Status owner, ITarget target)
    {
        this.owner = owner;
        this.target = target;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.transform.position) <= 0.0f)
        {
            target.TakeDamage(owner);
            Destroy(gameObject);
        }
    }
}


