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
    [SerializeField] LayerMask attackMask;

    Collider meleeCollider;
    float rateTime;

    private void Start()
    {
        meleeCollider = GetComponent<Collider>();
    }
    private void Update()
    {
        rateTime = Mathf.Clamp(rateTime + Time.deltaTime, 0f, attackRate);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Status target = collision.gameObject.GetComponent<Status>();

        // 충돌체에게 Status가 없거나 attackMask에 layer가 포함되어있지 않을 경우.
        // &는 And연산으로 값이 0이라는 것은 layerMask에 레이어가 포함되어있지 않다.
        if (target == null || (target.gameObject.layer & attackMask) == 0)
            return;

        // 공격 대상에게 attackPower를 전달한 후 최종 데미지 리턴.
        float damage = target.TakeDamage(attackPower);

        // 충돌한 지점에 대해 데미지 표기.
        Vector3 hitPoint = collision.contacts[0].point;
        DamageEffectUI.Instance.ShowDamage(hitPoint, damage);
    }
    public void OnAttack()
    {
        // 공격속도만큼 시간이 흐르지 않았다면 공격할 수 없다.
        if (rateTime < attackRate)
            return;

        rateTime = 0f;

        // 애니메이션 공격 처리.
        anim.SetLayerWeight(1, 1f);
        anim.SetTrigger("onAttack");
    }

    private void OnStartAttack()
    {
        meleeCollider.enabled = true;
    }
    private void OnEndAttack()
    {
        meleeCollider.enabled = false;
    }

    private void OnEndUpperMask()
    {
        anim.SetLayerWeight(1, 0f);
    }


}
