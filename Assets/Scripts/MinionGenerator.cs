using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinionGenerator : MonoBehaviour
{
    [Header("Minion")]
    [SerializeField] TEAM team;
    [SerializeField] Minion MeleePrefab;
    [SerializeField] Minion RangePrefab;

    [Header("Path")]
    [SerializeField] Transform[] pathPivots;    // 경로 위치 배열.

    [Header("Time")]
    [SerializeField] float createRate;          // 생성 속도.
    [SerializeField] float delayTime;           // 생성 딜레이.

    [Header("Timer")]
    [SerializeField] float remainingTime;       // 남은 대기 시간.

    List<Minion> minionQueue;
    WaitForSeconds waitRate;
    Vector3[] paths;
    
    private void Start()
    {
        // 생성할 미니언의 대기열 리스트.
        minionQueue= new List<Minion>();

        for(int i = 0; i<3; i++)
            minionQueue.Add(MeleePrefab);

        for (int i = 0; i < 3; i++)
            minionQueue.Add(RangePrefab);

        // 생성 속도(주기)
        waitRate = new WaitForSeconds(createRate);

        // 경로 위치값 배열 (트랜스폼 배열 > 벡터3 배열)
        paths = pathPivots.Select(p => p.position).ToArray();
    }

    private void Update()
    {
        // 일정 시간을 대기 후 미니언 생성.
        remainingTime = Mathf.Clamp(remainingTime - Time.deltaTime, 0f, delayTime);
        if (remainingTime <= 0.0f)
        {
            StartCoroutine(IECreateMinion());
            remainingTime = delayTime;
        }
    }
    private IEnumerator IECreateMinion()
    {
        for (int i = 0; i < minionQueue.Count; i++)
        {
            Minion newMinion = Instantiate(minionQueue[i], transform.position, Quaternion.identity);
            newMinion.Setup(team, paths);
            yield return waitRate;
        }
    }
}
