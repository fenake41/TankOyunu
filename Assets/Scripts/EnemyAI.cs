using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AForge.Fuzzy;

public class EnemyAI : MonoBehaviour
{
    Vector3 dir;
    float distance, dist, speed;

    FuzzySet fsNear, fsMed, fsFar;
    LinguisticVariable lvDistance;

    FuzzySet fsSlow, fsMedium, fsFast;
    LinguisticVariable lvSpeed;

    Database database;
    InferenceSystem infSystem;

    Animator fsm;
    Transform player;
    NavMeshAgent agent;
    public Transform[] waypoints;
    public Transform rayOrigin;
    Vector3[] wayPointsPos = new Vector3[3];
    int currentWayPointIndex = 0;
    float shootFreq = 5f;
    
    void Start()
    {
        Initialize();
        
        for(int i=0;i<waypoints.Length;i++)
        {
            wayPointsPos[i] = waypoints[i].position;
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        fsm = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(wayPointsPos[currentWayPointIndex]);


    }

    private void Initialize()
    {
        SetDistanceFuzzySets();
        SetSpeedFuzzySets();
        AddToDatabase();
    }

    private void SetDistanceFuzzySets()
    {
        fsNear = new FuzzySet("Near", new TrapezoidalFunction(20, 40, TrapezoidalFunction.EdgeType.Right));
        fsMed = new FuzzySet("Med", new TrapezoidalFunction(20, 40, 50, 70));
        fsFar = new FuzzySet("Far", new TrapezoidalFunction(50, 70, TrapezoidalFunction.EdgeType.Left));

        lvDistance = new LinguisticVariable("Distance", 0, 100);
        lvDistance.AddLabel(fsNear);
        lvDistance.AddLabel(fsMed);
        lvDistance.AddLabel(fsFar);
    }

    private void SetSpeedFuzzySets()
    {
        fsSlow = new FuzzySet("Slow", new TrapezoidalFunction(30, 50, TrapezoidalFunction.EdgeType.Right));
        fsMedium = new FuzzySet("Medium", new TrapezoidalFunction(30, 50, 80, 100));
        fsFast = new FuzzySet("Fast", new TrapezoidalFunction(80, 100, TrapezoidalFunction.EdgeType.Left));
        lvSpeed = new LinguisticVariable("Speed", 0, 120);
        lvSpeed.AddLabel(fsSlow);
        lvSpeed.AddLabel(fsMedium);
        lvSpeed.AddLabel(fsFast);

    }


    private void AddToDatabase()
    {
        database = new Database();
        database.AddVariable(lvDistance);
        database.AddVariable(lvSpeed);

        infSystem = new InferenceSystem(database, new CentroidDefuzzifier(120));
        infSystem.NewRule("Rule 1", "IF Distance IS Far THEN Speed IS Slow");
        infSystem.NewRule("Rule 2", "IF Distance IS Med THEN Speed IS Medium");
        infSystem.NewRule("Rule 3", "IF Distance IS Near THEN Speed IS Fast");
    }


   

    private void FixedUpdate()
    {
        Evaluate();

        //CheckVisibility
        CheckVisibility();

        //check Distance
        CheckDistance();

        //check CheckDistanceFromWayPoint
        CheckDistanceFromWayPoint();
    }

    private void Evaluate()
    {
        if(player!=null)
        {
            dist = Vector3.Distance(transform.position, player.position);
        }
        
        infSystem.SetInput("Distance", dist);
        speed = infSystem.Evaluate("Speed");
        agent.speed = speed*0.1f;
    }

    //    IEnumerator CheckPlayer()
    //{
    //    CheckVisibility(); 
    //    CheckDistance();
    //    CheckDistanceFromWayPoint();

    //    yield return new WaitForSeconds(0.1f);
    //    yield return CheckPlayer();
    //}

    private void CheckDistanceFromWayPoint()
    {
        float distanceFromWayPoint = Vector3.Distance(transform.position, wayPointsPos[currentWayPointIndex]);
        fsm.SetFloat("distanceFromWayPoint", distanceFromWayPoint);
    }

    private void CheckVisibility()
    {
        GetComponent<Perspective>().updateSense();

        //float maxDistance = 20;
        //Vector3 direction = (player.position - transform.position).normalized;
        //Debug.DrawRay(rayOrigin.position, direction * maxDistance, Color.red);

        //if (Physics.Raycast(rayOrigin.position, direction, out RaycastHit info, maxDistance))
        //{
        //    if (info.transform.tag == "Player")
        //    {
        //        fsm.SetBool("isVisible", true);
        //    }
        //    else
        //        fsm.SetBool("isVisible", false);
        //}
        //else
        //    fsm.SetBool("isVisible", false);

    }


    private void CheckDistance()
    {
        if (player != null)
        {
             distance = Vector3.Distance(player.position, transform.position);
        }
           
        fsm.SetFloat("Distance", distance);
    }

    public void SetNextWayPoint()
    {
        switch(currentWayPointIndex)
        {
            case 0:
                currentWayPointIndex = 1;
                break;
            case 1:
                currentWayPointIndex = 2;
                break;
            case 2:
                currentWayPointIndex = 0;
                break;
        }
        agent.SetDestination(wayPointsPos[currentWayPointIndex]);
    }

    public void Shoot()
    {
        GetComponent<ShootBehaviour>().Shoot(shootFreq);
    }

    public void Chase()
    {
        if(player!=null)
        agent.SetDestination(player.position);
    }

    public void Patrol()
    {
        
    }

    public void SetLookRotation()
    {
        if(player != null)
        {
            dir = (player.position - transform.position).normalized;
        }
         
        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.2f);  
    } 

}
