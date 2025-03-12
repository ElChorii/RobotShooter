using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletController : MonoBehaviour
{

    [SerializeField]
    private GameObject bulletDecal;

    private float speed = 25f;
    private float timeToDestroy = 20f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    Vector3 frontOfObject;
    Rigidbody bulletRigidbody;
    [SerializeField] GameObject bulletView;

    // Start is called before the first frame update
    private void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletView = gameObject.transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        frontOfObject = transform.forward;
    }

    private void Update()
    {
        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy <= 0)
        {
            bulletView.transform.GetComponent<BulletDirection>().enemySelected = null;
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (bulletView.GetComponent<BulletDirection>().enemySelected == null)
        {
            bulletRigidbody.velocity = frontOfObject * speed;
        }
        else
        {
            bulletRigidbody.velocity = transform.up * -1 * 30;

            transform.rotation = Quaternion.Slerp(transform.rotation, bulletView.transform.rotation, 1f);
            bulletRigidbody.velocity = transform.forward * speed;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), other.collider);
            return;
        }
        
        if (other.gameObject.CompareTag("Mapa"))
        {
            ContactPoint contact = other.GetContact(0);
            GameObject.Instantiate(bulletDecal, contact.point, Quaternion.LookRotation(contact.normal));
        }
        Destroy(gameObject);
    }
}
