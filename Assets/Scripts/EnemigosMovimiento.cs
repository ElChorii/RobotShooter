using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemigosMovimiento : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform[] ruta;

    int rutaActual;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (ruta.Length > 0)
        {
            agent.SetDestination(ruta[rutaActual].position);
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SiguienteRuta();
        }

        if (PlayerController.instance.partidaPerdida == true)
        {
            agent.isStopped = true;
        }
    }

    private void FixedUpdate()
    {
        DetectarJugador();
    }

    void SiguienteRuta()
    {
        rutaActual = (rutaActual + 1) % ruta.Length;
        agent.SetDestination(ruta[rutaActual].position);
    }

    void DetectarJugador()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3 (transform.position.x, transform.position.y + 3f, transform.position.z), transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag("Player") && PlayerController.instance.partidaPerdida == false)
            {
                PlayerController.instance.partidaPerdida = true;
                UIScript.instance.PartidaPerdida();
            }
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z), transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bala"))
        {
            UIScript.instance.RestarEnemigo();
            Destroy(gameObject);
        }
    }
}
