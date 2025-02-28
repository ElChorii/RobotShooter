using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private Transform cameraTransform;

    private InputAction moveAction;
    private InputAction shootAction;

    [SerializeField]
    public float playerSpeed = 2;
    [SerializeField]
    private float rotationSpeed = 2f;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform barrelTransform;
    [SerializeField]
    private Transform bulletParent;
    [SerializeField]
    private float bulletHitMissDistance = 25f;

    private float energiaDelBot = 100;
    private float energiaDelBotRedondeada;
    private float tiempoDeRecarga = 0f;
    public TextMeshProUGUI textoDeEnergia;

    
    public float speed = 5f; // Velocidad del jugador
    public float tiltAmount = 10f; // Cuánto se inclina al moverse
    public Transform elementoARotar; // El objeto que se inclinará

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        moveAction = playerInput.actions["Move"];
        shootAction = playerInput.actions["Shoot"];
        
    }

    private void OnEnable()
    {
        shootAction.performed += _ => ShootGun();
    }

    private void OnDisable()
    {
        shootAction.performed -= _ => ShootGun();
    }

    private void ShootGun()
    {
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (energiaDelBot >= 33f)
        {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
            {
                bulletController.target = hit.point;
                bulletController.hit = true;
                energiaDelBot = energiaDelBot - 33.33f;
                tiempoDeRecarga = 0;
            }
            else
            {
                bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
                bulletController.hit = false;
                energiaDelBot = energiaDelBot - 33.33f;
                tiempoDeRecarga = 0;
            }
        }   
    }
    private void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);
        controller.Move(playerVelocity * Time.deltaTime);

        // Que el robot rote con la camara
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        //////
        // Capturar entrada del jugador
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Aplicar movimiento
        Vector3 movement = new Vector3(moveX, 0, moveZ) * speed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Aplicar inclinación en base a la velocidad
        Vector3 velocity = rb.velocity;
        if (velocity.magnitude > 0.1f) // Evitar inclinación cuando está quieto
        {
            float tiltAngle = Mathf.Clamp(velocity.x * tiltAmount, -tiltAmount, tiltAmount);
            elementoARotar.rotation = Quaternion.Euler(0, transform.eulerAngles.y, -tiltAngle);
        }

        // Tiempo hasta que el bot empiece a recargar
        tiempoDeRecarga = tiempoDeRecarga + 1 * Time.deltaTime;
        if (tiempoDeRecarga >= 10)
        {
            energiaDelBot = energiaDelBot + 3 * Time.deltaTime;
        }

        // Que la energia no sobrepase el 100% y deje de recargar
        if (energiaDelBot > 100)
        {
            energiaDelBot = 100;
            tiempoDeRecarga = 0;
        }
        energiaDelBotRedondeada = Mathf.Round(energiaDelBot);
        textoDeEnergia.text = (energiaDelBotRedondeada + "%");
    }
}
