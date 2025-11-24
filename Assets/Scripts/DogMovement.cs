using UnityEngine;

public class DogMovement : MonoBehaviour {
  public float DogSpeed = 10f;
//Wip, todavia no esta funcionando
    void Update()
    {
    transform.position = new Vector3(DogSpeed, 0, 0) *  Time.deltaTime;
    }
}
