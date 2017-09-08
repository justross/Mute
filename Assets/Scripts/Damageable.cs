using System.Collections;
using UnityEngine;

public interface Damageable<T>
{
    void damage(T damageTaken);
}