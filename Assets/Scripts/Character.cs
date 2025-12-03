using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public string job;
    public int year;
    private NavMeshAgent agent;
    private bool Hunger = false;
    private bool Tired = false;
    private bool Joy = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if (Hunger == false)
        {
                Destroy(gameObject);
        }
        if (Tired == false)
        {
            Joy = false;
        }
    }
}
