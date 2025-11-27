using System;
using UnityEngine;

public class DogMovement : MonoBehaviour {
  public float DogSpeed = 10f;
  private Rigidbody2D rb;



  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }


  void Update()
  {
    //Movimiento horizontal constante
    transform.position += new Vector3(DogSpeed, 0, 0) * Time.deltaTime;
  }
  /*Si detecta que está dentro de "AreaDeSalto" hace un salto.
  "AreaDeSalto" debe ser un rango alrededor del obstáculo (sin colisiones) y con isTrigger true
  El rango necesita del tag para funcionar.
  El salto es automático.
  */
  private void  OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("JumpingArea"))
    {
      Jump();
    }
  }
  //Ejecuta el salto
  void Jump()
  {
    rb.AddForce(Vector2.up * 1000);
  }

}
