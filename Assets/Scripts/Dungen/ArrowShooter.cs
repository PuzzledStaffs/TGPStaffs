using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : Trap
{
    [SerializeField] GameObject m_ThingToShoot;
    [SerializeField] BowItem m_dispencerBow;
    [SerializeField] GameObject m_shotDestination;
    [SerializeField] GameObject m_shotOragin;
    [Header("TimedShots")]
    [SerializeField][Tooltip("If true it will shoot an arrow ")] bool m_timedShots;
    private Coroutine m_ShootingCoroutine;
    [SerializeField] float m_shotIntervall = 1;
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
            yield return new WaitForSecondsRealtime(m_shotIntervall);
            Shoot();
        }
    }
    public void Shoot()
    {
        GameObject arrow = Instantiate(m_ThingToShoot, m_shotOragin.transform.position, transform.rotation);
        arrow.GetComponent<Arrow>().bowParent = m_dispencerBow;        
        arrow.GetComponent<Arrow>().EndPoint = m_shotDestination.transform.position;
    }
}
