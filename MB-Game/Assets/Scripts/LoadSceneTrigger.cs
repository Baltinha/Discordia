using UnityEngine;

public class LoadSceneTrigger : MonoBehaviour
{
    private GameManager m_manager;
    [SerializeField] private string m_level;
    [SerializeField] private GameObject m_door;

    private void Start()
    {
        m_manager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_manager.LoadLevel(m_level);
            if (m_door == null)
                return;
            m_door.SetActive(false);
        }
    }
}
