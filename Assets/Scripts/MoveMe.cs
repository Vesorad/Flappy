using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMe : MonoBehaviour
{
public float moveSpeed = 1.0f;
public Vector3 moveVecore;




void Start()
 {
  
  

 }
 void Update()
 {
   transform.Translate(moveVecore * moveSpeed * Time.deltaTime);
 }
}
