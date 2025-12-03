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
    private enum State {Yes,No}
    private State Hunger = State.No;
    private State Tired = State.No;

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
