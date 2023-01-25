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
    [SerializeField] StatusData origin;     // 기본 수치.
    [SerializeField] StatusData grow;       // 성장 수치.
    [SerializeField] StatusData final;      // 최종 수치.

    [Header("Event")]
    [SerializeField] UnityEvent onLevelUp;  // 레벨이 올랐을때 불리는 함수.
    [SerializeField] UnityEvent onDead;     // 죽었을때 불리는 이벤트 함수.

    const int MAX_LEVEL = 18;

    public float Hp { get; private set; }
    public float Mp { get; private set; }
    public float Exp { get; private set; }

    public float LevelExp => 80 + (level * 100);

    public bool IsDead => Hp <= 0;

    // 기본 스테이터스.
    public float MaxHp => final.hp;
    public float MaxMp => final.mp;
    public float Power => final.power;
    public float Def => final.def;
    public float Range => final.range;
    public float Rate => final.rate;
    public float Critical => final.critical;

    private void Start()
    {
        // 최초 값을 할당.
        level = 1;
        final = origin + grow;
        Hp = MaxHp;
        Mp = MaxMp;
    }

    public void AddExp(float value)
    {
        // 최대 레벨일 경우 경험치를 획득하지 않는다.
        if (level >= MAX_LEVEL)
            return;

        Exp += value;

        // 현재 경험치가 요구량보다 높을 경우.
        if (Exp >= LevelExp)
        {
            LevelUp();

            // 최대 레벨 당성의 경우
            // 경험치를 최대 경험치로 설정.
            if(level >= MAX_LEVEL)
                Exp = LevelExp;
        }
    }
    private bool LevelUp()
    {
        if (level >= MAX_LEVEL)
            return false;

        level += 1;
        final += grow;          // 성장 수치만큼 대입.
        Hp += grow.hp;          // 최대 체력 증가량 만큼 체력 회복.
        Mp += grow.mp;          // 최대 마나 증가량 만큼 마나 회복.
        onLevelUp?.Invoke();    // 레벨업 이벤트 호출.
        return true;
    }

    public void TakeDamage(Status attacker)
    {
        // 이미 죽었다면 데미지를 받지 않는다.
        if (IsDead)
            return;

        // 데미지 공식.
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
    /// 원하는 양만큼 충분한 마나가 있는가?
    /// 있다면 사용한다.
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
