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
    }

    void SiguienteRuta()
    {
        rutaActual = (rutaActual + 1) % ruta.Length;
        agent.SetDestination(ruta[rutaActual].position);
    }
}
