using UnityEngine;
using UnityEngine.Events;

public class Status : MonoBehaviour
{
    [System.Serializable]
    public struct StatusData
    {
        public float hp;
        public float mp;
        public float power;
        public float def;
        public float range;
        public float rate;
        public float critical;

        public static StatusData operator +(StatusData a, StatusData b)
        {
            a.hp += b.hp;
            a.mp += b.mp;
            a.power += b.power;
            a.def += b.def;
            a.range += b.range;
            a.rate += b.rate;
            a.critical += b.critical;
            return a;
        }
        public static StatusData operator *(StatusData a, int level)
        {
            a.hp *= level;
            a.mp *= level;
            a.power *= level;
            a.def *= level;
            a.range *= level;
            a.rate *= level;
            a.critical *= level;
            return a;
        }
    }

    [SerializeField] int level = 1;
    [SerializeField] StatusData origin;     // �⺻ ��ġ.
    [SerializeField] StatusData grow;       // ���� ��ġ.
    [SerializeField] StatusData final;      // ���� ��ġ.

    [Header("Event")]
    [SerializeField] UnityEvent onLevelUp;  // ������ �ö����� �Ҹ��� �Լ�.
    [SerializeField] UnityEvent onDead;     // �׾����� �Ҹ��� �̺�Ʈ �Լ�.

    const int MAX_LEVEL = 18;

    public float Hp { get; private set; }
    public float Mp { get; private set; }
    public float Exp { get; private set; }

    public float LevelExp => 80 + (level * 100);

    public bool IsDead => Hp <= 0;

    // �⺻ �������ͽ�.
    public float MaxHp => final.hp;
    public float MaxMp => final.mp;
    public float Power => final.power;
    public float Def => final.def;
    public float Range => final.range;
    public float Rate => final.rate;
    public float Critical => final.critical;

    private void Start()
    {
        // ���� ���� �Ҵ�.
        level = 1;
        final = origin + grow;
        Hp = MaxHp;
        Mp = MaxMp;
    }

    public void AddExp(float value)
    {
        // �ִ� ������ ��� ����ġ�� ȹ������ �ʴ´�.
        if (level >= MAX_LEVEL)
            return;

        Exp += value;

        // ���� ����ġ�� �䱸������ ���� ���.
        if (Exp >= LevelExp)
        {
            LevelUp();

            // �ִ� ���� �缺�� ���
            // ����ġ�� �ִ� ����ġ�� ����.
            if(level >= MAX_LEVEL)
                Exp = LevelExp;
        }
    }
    private bool LevelUp()
    {
        if (level >= MAX_LEVEL)
            return false;

        level += 1;
        final += grow;          // ���� ��ġ��ŭ ����.
        Hp += grow.hp;          // �ִ� ü�� ������ ��ŭ ü�� ȸ��.
        Mp += grow.mp;          // �ִ� ���� ������ ��ŭ ���� ȸ��.
        onLevelUp?.Invoke();    // ������ �̺�Ʈ ȣ��.
        return true;
    }

    public void TakeDamage(Status attacker)
    {
        // �̹� �׾��ٸ� �������� ���� �ʴ´�.
        if (IsDead)
            return;

        // ������ ����.
        float damage = attacker.Power * (100 / (100 + Def));
        Hp = Mathf.Clamp(Hp - damage, 0, MaxHp);

        if (IsDead)
            onDead?.Invoke();
    }
    public void Recovery(float value)
    {
        Hp = Mathf.Clamp(Hp + value, 0, MaxHp);
    }

    /// <summary>
    /// ���ϴ� �縸ŭ ����� ������ �ִ°�?
    /// �ִٸ� ����Ѵ�.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool UseMp(float value)
    {
        if (Mp < value)
            return false;

        Mp = Mathf.Clamp(Mp - value, 0, MaxMp);
        return true;
    }


#if UNITY_EDITOR
    public void EditUpdateFinal()
    {
        final = origin + grow;
    }
#endif
}
