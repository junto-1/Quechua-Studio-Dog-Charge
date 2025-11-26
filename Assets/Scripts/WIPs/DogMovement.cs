using System;
using UnityEngine;

public class DogMovement : MonoBehaviour {
  public float DogSpeed = 10f;
  private Rigidbody2D rb;
  public float JumpForce;


  private void Start() {
    rb = GetComponent<Rigidbody2D>();
  }


  void Update() {
    //Movimiento horizontal constante
    transform.position += new Vector3(DogSpeed, 0, 0) * Time.deltaTime;

    //Esto es de prueba nada más, luego se puede borrar
    //funciona mejor si gravity scale = 3 (Dentro de Rigidbody2D en Unity)
    if (Input.GetKey(KeyCode.Space))
    {
      rb.AddForce(Vector2.up * 100);
    }
  /*Si detecta que está dentro de "AreaDeSalto" hace un salto
   la idea es que "AreaDeSalto" sea un rango alrededor del obstáculo (sin colisiones)
   Entonces al entrar en contacto ejecuta el salto automáticamente
   */
    void OnTriggerEnter2D(Collider2D other) {
      if (other.CompareTag("JumpingArea")) {
        Jump();
      }
    }

    //Ejecuta el salto
    void Jump()
    {
      rb.AddForce(Vector2.up * 100);
    }

  }
}
