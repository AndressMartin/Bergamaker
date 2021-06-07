using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyIA : MonoBehaviour
{
    public AIDestinationSetter aIDestinationSetter;
    public Transform target;
    public Player player;
    public AIPath aIPath;
    public float rangeAtaque;
    public float rangeView;
    public float maxSpeed=2;

    public enum State
    {
        Stop,
        Following,
        Attacking,
    }
    public State state;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        aIPath = GetComponent<AIPath>();

        rangeView = 6f;
        rangeAtaque = 2f;
        target = aIDestinationSetter.target;
      
    }

    public void Update()
    {

        if (Vector3.Distance(transform.position, player.transform.position) > rangeView && state !=State.Stop)
        {
            //muito longe da visão para
            Stop();
        }

        else if (Vector3.Distance(transform.position, player.transform.position) < rangeView)
        {
            //se esta dentro da visão persegue
            Following();
        }


        if (Vector3.Distance(transform.position, player.transform.position) < rangeAtaque)
        {
            //se tem distancia para atacar ataque
            Attacking();
        }

        
    }

    public void Attacking()
    {
        Debug.Log("Ataque");
        state = State.Following;
    }

    public void Following()
    {
        aIPath.maxSpeed = maxSpeed;
        Debug.Log("Seguindo");
        target = player.transform;
        state = State.Following;

    }

    public void Stop()
    {
        aIPath.maxSpeed = 0;
        target = null;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Debug.Log("parado");
        state = State.Stop;
    }
}
