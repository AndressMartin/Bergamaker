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

    public Transform transformInicial;
    public Transform transformFinal;

    public enum State
    {
        Stop,
        Following,
        Attacking,
        BackPatrolling,
    }
    public enum Mode
    {
        BackToPatrol,
        WaitnAttack,
    }

    private bool attacking;
    public State state;
    public Mode mode;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        aIPath = GetComponent<AIPath>();

        rangeView = 6f;
        rangeAtaque = 2f;
        target = aIDestinationSetter.target;

        transformInicial = transform;

     
    }

    public void Update()
    {
        if (state == State.Stop || state == State.Attacking)
            aIPath.canMove = false;

        else if (state == State.Following)
            aIPath.canMove = true;


        if (mode == Mode.WaitnAttack)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > rangeView && state != State.Stop)
            {
                //muito longe da visão para
                Stop();
            }

            else if (Vector3.Distance(transform.position, player.transform.position) < rangeAtaque)
            {
                //se tem distancia para atacar ataque
                Attacking();
            }

            else if (Vector3.Distance(transform.position, player.transform.position) < rangeView )
            {
                //se esta dentro da visão persegue
                Following();
            }           
        }
        else if (mode == Mode.BackToPatrol)
        {
          
            if (Vector3.Distance(transform.position, player.transform.position) > rangeView && state != State.BackPatrolling)
            {
                //muito longe da visão para
                BackToPatrol();
            }

            else if (Vector3.Distance(transform.position, player.transform.position) < rangeView && state != State.Attacking)
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


    }

    public void BackToPatrol()
    {
        //Debug.Log("DeVoltaOrigem");
        state = State.BackPatrolling;
        target = transformInicial;
    }

    public void Attacking()
    {
        state = State.Attacking;
        if (transform.GetComponent<AtaqueInimigo>().activated == false)
        {
            transform.GetComponent<AtaqueInimigo>().Activate(transform.GetComponent<EntityModel>());
        }
        
    }


    public void Following()
    {
        state = State.Following;
        target = player.transform;
    }

    public void Stop()
    {
        target = null;
        state = State.Stop;
    }
}
