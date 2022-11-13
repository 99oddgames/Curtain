using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonoPoolConfig", menuName = "Tech/MonoPoolConfig")]
public class ObjectPoolConfig : ScriptableObject
{
    public PoolableObject[] PrewarmObjects;
}
