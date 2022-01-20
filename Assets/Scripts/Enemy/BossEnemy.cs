using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    bool phaseSwitch = false;
    // Start is called before the first frame update
    void Start()
    {
        m_health = 400;
        manager = new BossStateManager();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
