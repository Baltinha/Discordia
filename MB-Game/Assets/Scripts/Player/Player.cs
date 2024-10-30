using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Lantern Lantern;
    public Transform LanterRef;

    [SerializeField] Transform Orientetion;

    [Header("Canvas")]
    [SerializeField]private Image m_crossHair;

    [Header("Objetos")]
    private GameManager m_gameManager;
    private CharacterController m_controller;
    public InputLanternMode lanternMode;
    private new Camera camera;

    [Header("Moviment")]
    [SerializeField]private float m_gravity = -9.81f;
    private Vector3 velocity;


    [Header("Speed")]
    //Velocidades
    [SerializeField]private float m_speed = 12;
    [SerializeField] private float m_speedCrounch = 3f;
    //[SerializeField] private float m_speedRunning = 15; //talvez aplicar a corrida em outro momento
    [SerializeField] private float m_normalSpeed = 8;
    [SerializeField] private float m_currentScale;
    private float m_scalePlayer, m_currentSpeed;

    [Header("Bool")]
    //Boleana
    private bool m_crouch = false;
    private bool m_running;
    //private bool hasRegenStamina;

    /*[Header("Stamina UI elements")]
    [SerializeField] private Image staminaProgressUI;
    [SerializeField] private CanvasGroup sliderCanvasGroup;
    [Header("Running")]
    [SerializeField] private float m_stamina = 100f;
    [SerializeField] private float m_maxStamina = 100f;*/

    [Header("Interact")]
    [SerializeField] private float m_distanceToInteract = 4f;

    private void Start()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_controller = GetComponent<CharacterController>();
        camera = Camera.main;
        camera = m_gameManager.GetCamera();
        m_normalSpeed = m_speed;
        m_currentSpeed = m_normalSpeed;
    }

    private void Update()
    {
        if (canMove)
        {
            Movimente();
        }

        //Correr();
        Abaixar();
        Interacte();
        m_gameManager.Breathing();
        m_speed = m_currentSpeed;
    }

    private void Interacte()
    {
        RaycastHit hitInfo;

        var objInteract = Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInfo, m_distanceToInteract, LayerMask.GetMask("Interact"));

        CrosshairImageChange(objInteract);

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInfo, m_distanceToInteract, LayerMask.GetMask("Interact")))
        {
            if (hitInfo.transform.TryGetComponent<Interactable>(out Interactable obj))
            {
                if (obj.InputLanternMode == InputLanternMode.OnClick)
                {
                    if (!Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        return;
                    }
                }
        
                obj.Interact();
        
            }
        }
    }

    private void CrosshairImageChange(bool objInteract)
    {
        if (objInteract)
        {
            m_crossHair.color = Color.red;
            //crossHair.sprite = nova imagem
        }
        else
        {
            m_crossHair.color = Color.white;
            //crossHair.sprite = volta para a outra imagem
        }
    }

    #region Moviment


    private void Movimente()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        velocity.y += m_gravity * Time.deltaTime;
        Vector3 move = new Vector3();

        if (Mathf.Abs(x) < 0.1 && Mathf.Abs(z) < 0.1)
        {
            m_gameManager.audioManager.Play("Pasos");
            isMoving = false;
        }
        else
        {
            move = Orientetion.right * x + Orientetion.forward * z;
            move = new Vector3(move.x, velocity.y, move.z);
            move = new Vector3(move.x, 0, move.z).normalized;
            isMoving = true;

        }
        m_controller.Move(move * m_speed * Time.deltaTime);
        m_controller.Move(velocity * Time.deltaTime);
    }
    private void Abaixar()
    {
        bool estaEmBaixo = false;
        RaycastHit hit;

        if (Physics.Raycast(Orientetion.transform.position, Orientetion.transform.up, out hit, LayerMask.GetMask("Ground")))
        {
            if (hit.collider.gameObject.tag == "ObsCabeca")
            {
                estaEmBaixo = true;
            }
            else
            {
                estaEmBaixo = false;
            }
        }

        if (Input.GetKey(KeyCode.LeftControl) && !m_running && !m_crouch)
        {
            m_crouch = true;
            m_scalePlayer = 0.5f;
            m_currentSpeed = m_speedCrounch;
            //Orientetion.transform.position = new Vector3(0, 0.5f, 0);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            m_crouch = false;
            m_scalePlayer = m_currentScale;
            //Orientetion.transform.position = new Vector3(0, 3.5f, 0);
        }
        if (m_crouch && m_controller.height > m_scalePlayer)
        {
            m_controller.height = Mathf.Lerp(m_controller.height, m_scalePlayer, 4 * Time.deltaTime);
        }
        else if (!m_crouch && m_controller.height < m_scalePlayer && !estaEmBaixo)
        {
            m_controller.height = Mathf.Lerp(m_controller.height, m_scalePlayer, 4 * Time.deltaTime);
        }
        if (m_controller.height >= 3.4f && !m_running && !m_crouch)
        {
            m_currentSpeed = m_normalSpeed;
        }

        Orientetion.localPosition = new Vector3(0, m_scalePlayer == 0.5f ? 0.5f : 2f, 0);
    }

    //private void Correr()
    //{
    //    if (Input.GetKey(KeyCode.LeftShift) && !m_crouch && m_stamina > 0.1f && m_controller.height >= 3.4f)
    //    {
    //        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1)
    //        {
    //            m_currentSpeed = m_speedRunning;
    //            m_running = true;
    //            drainStamina();
    //            updateStamina(1);
    //        }
    //        else
    //        {
    //            m_currentSpeed = m_normalSpeed;
    //            m_running = false;
    //            gainStamina();

    //            if (m_stamina >= m_maxStamina - 0.1f)
    //            {
    //                updateStamina(0);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        gainStamina();

    //        if (m_stamina >= m_maxStamina - 0.1f)
    //        {
    //            updateStamina(0);
    //        }
    //    }
    //    if (Input.GetKeyUp(KeyCode.LeftShift) && m_controller.height >= 3.4f || m_stamina < 0.1f)
    //    {
    //        m_currentSpeed = m_normalSpeed;
    //        m_running = false;
    //    }
    //}
    //private void drainStamina()
    //{
    //    if (m_running)
    //    {
    //        m_stamina -= decriStamina * Time.deltaTime;
    //    }
    //}

    //private void gainStamina()
    //{
    //    if (!m_running && m_stamina <= m_maxStamina - 0.01f)
    //    {
    //        m_stamina += m_staminaRegen * Time.deltaTime;
    //        updateStamina(1);
    //    }
    //}

    /*private void updateStamina(int value)
    {
        staminaProgressUI.fillAmount = m_stamina / m_maxStamina;

        if (value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }*/

    #endregion

    #region Get & Set
    //private float Speed { get => m_speed; set => m_speed = value; }
    public bool isMoving { get; set; }
    public bool canMove { get; set; } = true;


    #endregion
}

[Serializable]
public enum InputLanternMode { Automatic, OnClick }