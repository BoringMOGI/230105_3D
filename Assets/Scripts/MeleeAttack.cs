using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Vector3 attackOffset;
    [SerializeField] float attackRange;
    [SerializeField] float attackRate;
    [SerializeField] float attackPower;

    float rateTime;
    private void Update()
    {
        rateTime = Mathf.Clamp(rateTime -= Time.deltaTime, 0f, attackRate);
    }

    public void OnAttack()
    {
        if (rateTime > 0f)
            return;

        // 애니메이션 공격 처리.
        anim.SetLayerWeight(1, 1f);
        anim.SetTrigger("onAttack");


        // 실제 공격 처리.
        rateTime = attackRate;
        Vector3 offset = transform.forward * attackOffset.z + transform.right * attackOffset.x;
        Collider[] targets = Physics.OverlapSphere(transform.position + offset, attackRange);
        for (int i = 0; i < targets.Length; i++)
        {
            Status target = targets[i].GetComponent<Status>();
            if (target == null || target.gameObject == gameObject)
                continue;

            target.TakeDamage(attackPower);
        }
    }

    public void OnEndUpperMask()
    {
        anim.SetLayerWeight(1, 0f);
    }

    private void OnDrawGizmos()
    {
        Vector3 offset = transform.forward * attackOffset.z + transform.right * attackOffset.x;
        Gizmos.DrawWireSphere(transform.position + offset, attackRange);
    }
}
