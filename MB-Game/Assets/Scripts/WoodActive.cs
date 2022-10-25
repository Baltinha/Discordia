using System.Collections;
using UnityEngine;

public class WoodActive : Interactable
{
    public GameObject lantern, oldPlayerPosition;
    public GameManager manager;
    public Transform spotBrigeCamera;

    public PuzzleLabirinto puzzleLabirinto;
    public ParticleSystem Fire;

    private void Start()
    {
        manager = GameObject.FindObjectOfType<GameManager>();
        lantern = GameObject.FindGameObjectWithTag("Lantern");
        oldPlayerPosition = GameObject.FindGameObjectWithTag("Orientention");
    }
    public override void Interact()
    {
        Fire.Play();
        puzzleLabirinto.wood[puzzleLabirinto.cont].SetActive(true);
        puzzleLabirinto.cont++;
        StartCoroutine(CameraBridgeTime());
        this.gameObject.GetComponent<BoxCollider>().enabled = false;

        if (puzzleLabirinto.cont == 3)
        {
            puzzleLabirinto.LoadAct3.SetActive(true);
            puzzleLabirinto.invisibleWall.enabled = false;
        }

    }

    IEnumerator CameraBridgeTime()
    {
        manager.cameraAtual.GetComponent<Look>().canLook = false;
        manager.player.canMove = false;
        manager.cameraAtual.transform.position = spotBrigeCamera.position;
        manager.cameraAtual.transform.rotation = spotBrigeCamera.rotation;
        manager.cameraAtual.GetComponent<CameraFollow>().enabled = false;
        lantern.SetActive(false);
        yield return new WaitForSeconds(5f);
        manager.player.canMove = true;
        manager.cameraAtual.GetComponent<Look>().canLook = true;
        manager.cameraAtual.transform.position = oldPlayerPosition.transform.position;
        manager.cameraAtual.transform.rotation = oldPlayerPosition.transform.rotation;
        manager.cameraAtual.GetComponent<CameraFollow>().enabled = true;
        lantern.SetActive(true);
        yield return null;
    }
}
