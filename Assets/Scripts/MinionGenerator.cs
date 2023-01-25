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
    [SerializeField] Transform[] pathPivots;    // ��� ��ġ �迭.

    [Header("Time")]
    [SerializeField] float createRate;          // ���� �ӵ�.
    [SerializeField] float delayTime;           // ���� ������.

    [Header("Timer")]
    [SerializeField] float remainingTime;       // ���� ��� �ð�.

    List<Minion> minionQueue;
    WaitForSeconds waitRate;
    Vector3[] paths;
    
    private void Start()
    {
        // ������ �̴Ͼ��� ��⿭ ����Ʈ.
        minionQueue= new List<Minion>();

        for(int i = 0; i<3; i++)
            minionQueue.Add(MeleePrefab);

        for (int i = 0; i < 3; i++)
            minionQueue.Add(RangePrefab);

        // ���� �ӵ�(�ֱ�)
        waitRate = new WaitForSeconds(createRate);

        // ��� ��ġ�� �迭 (Ʈ������ �迭 > ����3 �迭)
        paths = pathPivots.Select(p => p.position).ToArray();
    }

    private void Update()
    {
        // ���� �ð��� ��� �� �̴Ͼ� ����.
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
