using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ArrowShooter : Trap
{
    [SerializeField] [FormerlySerializedAs("m_ThingToShoot")] GameObject m_thingToShoot;
    [SerializeField] BowItem m_dispencerBow;
    [SerializeField] GameObject m_shotDestination;
    [SerializeField] [FormerlySerializedAs("m_shotOragin")]  GameObject m_shotOrigin;
    [Header("TimedShots")]
    [SerializeField][Tooltip("If true it will shoot an arrow ")] bool m_timedShots;
    private Coroutine m_ShootingCoroutine;
    [SerializeField] [FormerlySerializedAs("m_shotIntervall")] float m_shotInterval = 1;
    public override void EnterRoomEnabled()
    {
        if (m_timedShots)
        {
            m_ShootingCoroutine = StartCoroutine(ShootingCoroutine());
           
        }
    }

    public override void ExitRoomDisabled()
    {
        if (m_timedShots)
        {
            if (m_ShootingCoroutine != null)
            {
                StopCoroutine(m_ShootingCoroutine);
            }
        }
    }

    private IEnumerator ShootingCoroutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(m_shotInterval);
            Shoot();
        }
    }
    public void Shoot()
    {
        GameObject arrow = Instantiate(m_thingToShoot, m_shotOrigin.transform.position, transform.rotation);
        arrow.GetComponent<Arrow>().bowParent = m_dispencerBow;        
        arrow.GetComponent<Arrow>().EndPoint = m_shotDestination.transform.position;
    }
}
