using System.Collections.Generic;
using UnityEngine;

public class PuzzleTrigger : Interactable
{
    private GameManager m_manager;

    public GameObject Words;
    public GameObject door;
    private List<GameObject> puzzleInteract;

    private bool puzzleSpawn;

    public override void Interact()
    {
        if (m_manager.player.Lantern == null)
            return;
        else if (m_manager.player.Lantern.luzAtiva == true)
        {
            if (!puzzleSpawn)
            {
                this.gameObject.GetComponent<BoxCollider>().enabled = false;
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                Words.SetActive(true);
                puzzleSpawn = true;
            }
        }
        else
            return;

    }
    // Start is called before the first frame update
    void Start()
    {
        //puzzleInteract = new List<GameObject>();
        m_manager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleInteract.Count == 3)
        {
            Words.SetActive(false);
            puzzleSpawn = false;
            Destroy(door);
        }
    }

}

