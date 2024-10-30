using UnityEngine;
using UnityEngine.UI;

public class OpenDoor : Interactable
{
    [Header("Porta")]
    [SerializeField] private GameObject m_door;
    [SerializeField] private GameObject m_needHideAnything;
    private bool m_needToInteract;
    private bool m_interacted = false;


    [Header("Setting Triggers")]
    [SerializeField] private TriggersStructur[] m_needToSet;

    [Header("Som")]
    public string m_nomeSom;
    private AudioManager m_som;

    private void Start()
    {
        m_som = GameObject.FindObjectOfType<AudioManager>();

    }
    public override void Interact()
    {
        m_interacted = true;
        DestroyDoor();
    }

    public void DestroyDoor()
    {
        Destroy(m_door);
        if (!m_needToInteract)
        {
            Destroy(this.gameObject);
            return;
        }

        if (m_som != null)
            m_som.Play(m_nomeSom);

        if (m_needHideAnything != null)
            m_needHideAnything.SetActive(false);

        if (m_needToSet != null && m_interacted)
        {
            foreach (var set in m_needToSet)
            {
                if (set.elemento != null)
                {
                    set.elemento.enabled = set.setValueBoxCollider;
                }
                if (set.gameObject != null)
                {
                    set.gameObject.SetActive(set.setValueGameObject);
                }
            }
        }
    }
}
