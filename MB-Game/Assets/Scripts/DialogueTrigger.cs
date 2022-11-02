using System;
using System.Collections;
using UnityEngine;

public class DialogueTrigger : Interactable
{
    [Header("Dialogue Settings")]
    public DialogueStructure[] dialogue;
    public DialogueManager manager;
    public Camera targetCamera;
    public BoxCollider thisObj;
    public TriggersStructur[] needToSet;

    [Header("Automatic Dialogue")]
    public bool nextDialogue;
    public bool autoDialogue;

    [Header("Set Player")]
    public bool notMove;

    [Header("Have Animation")]
    public float waitTime = 0f;
    public bool activeAnimation;
    public string parameter;

    [Header("Have Thoughts")]
    public Thoughts thoughts;
    public float waitForThoughts = 0f;

    [Header("Final Level")]
    public bool isDialogueToFinal;
    public GameObject doorOne, doorTwo;

    [System.NonSerialized] public bool collided;

    private void Start()
    {
        thisObj = gameObject.GetComponent<BoxCollider>();
        manager = GameObject.FindObjectOfType<DialogueManager>();
    }

    public override void Interact()
    {
        if (waitTime > 0)
        {
            StartCoroutine(AfterEvent(waitTime));
        }
        TriggerDialogue(true);

        if (doorOne == null && doorTwo == null)
            return;
        else if (isDialogueToFinal)
        {
            doorOne.SetActive(false);
        }
        if (needToSet != null)
        {
            foreach (var set in needToSet)
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

    void Update()
    {
        waitTime = manager.time;
        if (collided && autoDialogue)
        {
            if (waitTime > 0)
            {
                StartCoroutine(AfterEvent(waitTime));
            }

            TriggerDialogue(true);

            if(thisObj == null)
                return;

            thisObj.gameObject.SetActive(false);

            if (needToSet != null)
            {
                foreach (var set in needToSet)
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

            collided = false;
        }
    }

    public void TriggerDialogue(bool value)
    {
        if (notMove == true)
            manager.Manager.SetMoving(false);

        if (activeAnimation && parameter != null)
            manager.Manager.animator.SetBool(parameter, true);

        if (thoughts != null)
            manager.thoughts = thoughts;

        manager.canNext = nextDialogue;
        manager.Dialogue(value, dialogue, this.gameObject, waitForThoughts);

        //manager.Manager.audioManager.Play(dialogue[0].audio);
        //foreach (var audio in dialogue)
        //{
        //    print("entrou");
        //    audio.audio.AudioSource.Play(audio.audio.Name);
        //    print("saiu");
        //}

    }

    IEnumerator AfterEvent(float value)
    {
        yield return new WaitForSeconds(value + 2f);
        TriggerDialogue(true);
        manager.time = 0f;
        yield return null;
    }

    #region OnTriggers
    private void OnTriggerEnter(Collider other)
    {
        collided = true;

        if (doorOne == null || doorTwo == null)
            return;
        else if (isDialogueToFinal)
        {
            doorOne.SetActive(false);
            doorTwo.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collided = false;
    }
    #endregion
}
