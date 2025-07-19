using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{

public class EnemyWalk : EnemyBase
{
    [Header("Waypoints")]
    public GameObject[] waypoints;
    public float ninDistance = 1f;
    public float speed = 1f;
    private int _index = 0;

    public void Start()
    {
        FixarNoChao();
    }

    protected override void Update()
    {
        base.Update(); // Garante que o Update da EnemyBase tambem roda + //Inimigo olhar para o player
                
        if (Vector3.Distance(transform.position, waypoints[_index].transform.position) < ninDistance)
        {
              _index++;
              if(_index >= waypoints.Length)
              {
                  _index = 0;
              }
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[_index].transform.position, Time.deltaTime * speed);
        transform.LookAt(waypoints[_index].transform.position);
    }

        void FixarNoChao()
        {
            RaycastHit hit;
            Vector3 origem = transform.position + Vector3.up * 2f;

            if (Physics.Raycast(origem, Vector3.down, out hit, 10f, LayerMask.GetMask("Default")))
            {
                 transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
       }

}

}
