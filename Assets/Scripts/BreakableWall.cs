using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IHealth
{
    public int GetHealth() { return 0; }

    void Start()
    {
        if (PersistentPrefs.GetInstance().m_currentSaveFile.HasFlag(gameObject.scene.name + "_WallBroken_" + gameObject.transform.parent.parent.name + "_" + gameObject.name))
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public virtual void TakeDamage(IHealth.Damage damage)
    {
        if (damage.type == IHealth.DamageType.BOMB)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            PersistentPrefs.GetInstance().m_currentSaveFile.AddFlag(gameObject.scene.name + "_WallBroken_" + gameObject.transform.parent.parent.name + "_" + gameObject.name);
        }
    }

    public bool IsDead() { return false; }
}
        