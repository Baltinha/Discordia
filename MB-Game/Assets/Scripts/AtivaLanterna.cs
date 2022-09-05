using UnityEngine;

public class AtivaLanterna : MonoBehaviour, IInteractable
{
    public GameObject lantern;

    public void Interact()
    {
        Ativa��o();
    }


    public void Ativa��o()
    {
        lantern.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
