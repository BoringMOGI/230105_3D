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
    [SerializeField] ParticleSystem hitPrefab;

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

        // �浹ü���� Status�� ���ų� attackMask�� layer�� ���ԵǾ����� ���� ���.
        // &�� And�������� ���� 0�̶�� ���� layerMask�� ���̾ ���ԵǾ����� �ʴ�.
        if (target == null || (1 << target.gameObject.layer & attackMask) == 0)
            return;

        // ���� ��󿡰� attackPower�� ������ �� ���� ������ ����.
        float damage = target.TakeDamage(attackPower);

        // �浹�� ������ ���� ������ ǥ��.
        Vector3 hitPoint = collision.contacts[0].point;
        DamageEffectUI.Instance.ShowDamage(hitPoint, damage);

        // �浹 ������ ��Ʈ ����Ʈ ����.
        ParticleSystem hitFx = Instantiate(hitPrefab);
        hitFx.transform.position = hitPoint;
    }
    public void OnAttack()
    {
        // ���ݼӵ���ŭ �ð��� �帣�� �ʾҴٸ� ������ �� ����.
        if (rateTime < attackRate)
            return;

        rateTime = 0f;

        // �ִϸ��̼� ���� ó��.
        anim.SetLayerWeight(1, 1f);
        anim.SetTrigger("onAttack");
    }

    public void OnStartAttack()
    {
        meleeCollider.enabled = true;
    }
    public void OnEndAttack()
    {
        meleeCollider.enabled = false;
    }
    public void OnEndUpperMask()
    {
        anim.SetLayerWeight(1, 0f);
    }
}
