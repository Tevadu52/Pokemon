using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DresseurMove : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Dresseur dresseur;
    private Vector3 destination;
    public float range = 10.0f;

    private void Start()
    {
        if (RandomPoint(transform.position, range, out destination))
        {
            agent.destination = destination;
        }
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, destination) < 1)
        {
            if (RandomPoint(transform.position, range, out destination))
            {
                agent.destination = destination;
            }
        }
        else if(dresseur.getIsFighting())
        {
            agent.destination = this.transform.position;
        }
        else if(agent.destination != destination)
        {
            agent.destination = destination;
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
