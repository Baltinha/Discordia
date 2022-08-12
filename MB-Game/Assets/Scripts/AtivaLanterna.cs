using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivaLanterna : MonoBehaviour, IInteractable
{
    public GameObject lantern;
    public void Interact() 
    {
        Ativa��o();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ativa��o() 
    {
        lantern.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
