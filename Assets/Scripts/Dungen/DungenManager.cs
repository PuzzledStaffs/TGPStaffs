using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class DungenManager : MonoBehaviour
{
    [SerializeField][FormerlySerializedAs("m_DungenCam")] public Camera m_dungeonCam;
    //[SerializeField] float m_cameraSpeed;
    float m_cameraTransitionTime = 1.0f;
    [SerializeField][FormerlySerializedAs("m_KeyCountText")] TextMeshProUGUI m_keyCountText;
    [SerializeField] private Canvas m_keyCanvas;
    [FormerlySerializedAs("m_CameraRB")] private Rigidbody m_cameraRB;
    public int m_keysCollected { get; protected set; }
    [SerializeField][FormerlySerializedAs("m_StartingKeys")] int m_startingKeys;
    [SerializeField] public Light m_playerLight;
    [Header("restart")]
    [SerializeField] PlayerController m_player;
    //[SerializeField] string m_scene;
    [Header("Room Start Info")]
    private DungenRoom m_startingRoom;
    [SerializeField] string m_dungenEnterText;
    [SerializeField] Canvas m_welcomeCanvas;
    [SerializeField][FormerlySerializedAs("m_TitalText")] TextMeshProUGUI m_titleText;
    private Animator m_animator;
    private float m_animTime = 2.0f;

    private void Start()
    {
        m_cameraRB = m_dungeonCam.transform.GetComponent<Rigidbody>();
        if (PersistentPrefs.GetInstance().m_currentSaveFile.m_saveLoaded)
        {
            if (PersistentPrefs.GetInstance().m_currentSaveFile.m_currentDungeonRoom != "null")
                SetStartingRoom(GameObject.Find(PersistentPrefs.GetInstance().m_currentSaveFile.m_currentDungeonRoom).GetComponent<DungenRoom>());
        }
        if (PersistentPrefs.GetInstance().m_currentSaveFile.HasIntFlag(gameObject.scene.name + "_Keys"))
            m_keysCollected = PersistentPrefs.GetInstance().m_currentSaveFile.GetIntFlag(gameObject.scene.name + "_Keys");
        else
            m_keysCollected = m_startingKeys;
        PersistentPrefs.GetInstance().m_currentSaveFile.m_currentDungeonRoom = m_startingRoom == null ? "null" : m_startingRoom.name;

        UpdateKeyUI();
        if (m_keysCollected == 0)
        {
            m_keyCanvas.enabled = false;
        }
        m_animator = GetComponent<Animator>();
        m_welcomeCanvas.enabled = false;
        m_player.m_Death2 += PlayerDeath;
        m_playerLight = gameObject.GetComponentInChildren<Light>();

        GameObject.FindObjectOfType<PlayerController>().enabled = false;
        //m_TitalText.text = m_dungenEnterText;
        m_welcomeCanvas.enabled = true;
        m_animator.SetTrigger("Start");
    }

    public void SetStartingRoom(DungenRoom room)
    {
        if (m_startingRoom == null)
        {
            m_startingRoom = room;
            m_startingRoom.m_playerStartingRoom = true;
            m_startingRoom.m_playerInRoom = true;
            m_startingRoom.RoomEntered();
            m_dungeonCam.transform.position = room.m_origin.transform.position;
            if (m_startingRoom.m_roomType == RoomType.BIG)
                m_dungeonCam.GetComponent<DungeonCameraController>().UpdateForBigRoom();
            SetTitalText(m_startingRoom.gameObject.scene.name.ToString());
            //m_startingRoom.UnFrezeRoom();
        }
    }

    public void Update()
    {
        if (m_animTime > 0.0f)
        {
            m_animTime -= Time.deltaTime;
            if (m_animTime <= 0.0f)
                JoinAnimationEnd();
        }

        m_playerLight.transform.position = m_player.transform.position + new Vector3(0, 5, 0);
    }

    public void SetTitalText(string text)
    {
        m_titleText.text = text;
    }

    public void JoinAnimationEnd()
    {
        GameObject.FindObjectOfType<PlayerController>().enabled = true;
        m_startingRoom.StartRoom();
        m_welcomeCanvas.enabled = false;
    }

    private void PlayerDeath()
    {
        int sceneCount = SceneManager.sceneCount;
        Scene[] scenes = new Scene[sceneCount];
        for (int i = 0; i < sceneCount; i++)
        {
            scenes[i] = SceneManager.GetSceneAt(i);
        }
        SceneManager.LoadScene(scenes[0].name, LoadSceneMode.Single);
        SceneManager.LoadScene(scenes[1].name, LoadSceneMode.Additive);
        for (int i = 1; i < sceneCount; i++)
        {

        }
    }

    public IEnumerator MoveCameraCoroutine(Vector3 TargetLocation)
    {
        Vector3 initialPosition = m_dungeonCam.transform.position;
        //Vector3 toMove = TargetLocation - m_DungenCam.transform.position;
        float time = 0.0f;
        //while (Mathf.Pow(toMove.x, 2) + Mathf.Pow(toMove.z, 2) > 0.4) //Mathf.Pow(toMove.x, 2) + Mathf.Pow(toMoveCheck.z, 2) > 0.4
        while (time < m_cameraTransitionTime) //Mathf.Pow(toMove.x, 2) + Mathf.Pow(toMoveCheck.z, 2) > 0.4
        {
            //toMove = TargetLocation - m_DungenCam.transform.position;
            m_dungeonCam.transform.position = Vector3.Lerp(initialPosition, TargetLocation, time);
            //m_CameraRB.velocity = toMove.normalized * m_cameraSpeed;
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }
        m_cameraRB.velocity = Vector3.zero;
        m_dungeonCam.transform.position = TargetLocation;
    }

    public void AddKey()
    {
        m_keysCollected++;
        PersistentPrefs.GetInstance().m_currentSaveFile.SetIntFlag(gameObject.scene.name + "_Keys", m_keysCollected);
        UpdateKeyUI();
    }

    public bool UseKey()
    {
        if (m_keysCollected > 0)
        {
            m_keysCollected--;
            PersistentPrefs.GetInstance().m_currentSaveFile.SetIntFlag(gameObject.scene.name + "_Keys", m_keysCollected);
            UpdateKeyUI();
            return true;
        }
        else
        {
            return false;
        }
    }


    private void UpdateKeyUI()
    {
        m_keyCountText.text = "x" + m_keysCollected.ToString();
        if (m_keysCollected > 0)
        {
            m_keyCanvas.enabled = true;
        }
        else
        {
            m_keyCanvas.enabled = false;
        }
    }
}
