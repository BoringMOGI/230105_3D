using UnityEngine;

public interface ITarget
{
    public TEAM Team { get; }
    public Transform transform { get; }
    public void TakeDamage(Status attacker);
}