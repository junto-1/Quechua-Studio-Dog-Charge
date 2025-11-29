using System;
using UnityEngine;

public class DogMovement : MonoBehaviour {
  public float DogSpeed = 10f;
  private Rigidbody2D rb;
  private Animator animator;
  private bool isGrounded = true;
  private bool isJumping = false;

  private void Start() {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
  }


  void Update() {
    //Movimiento horizontal constante
    transform.position += new Vector3(DogSpeed, 0, 0) * Time.deltaTime;

    UpdateAnimator();
  }

  /*Si detecta que está dentro de "AreaDeSalto" hace un salto.
  "AreaDeSalto" debe ser un rango alrededor del obstáculo (sin colisiones) y con isTrigger true
  El rango necesita del tag para funcionar.
  El salto es automático.
  */
  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("JumpingArea")) {
      Jump();

    }
  }

  //Ejecuta el salto y (se supone) que cambia la animación
  void Jump() {
    rb.AddForce(Vector2.up * 1000);
    animator.SetBool("isGrounded", false);
  }

  private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.collider.CompareTag("Ground")) {
      isGrounded = true;
    }
  }


  private void OnCollisionExit2D(Collision2D collision) {
    if (collision.collider.CompareTag("Ground")) {
      isGrounded = false;
    }
  }

  private void UpdateAnimator() {
    animator.SetBool("isGrounded", isGrounded);


    animator.SetBool("running", isGrounded);


    animator.SetBool("isJumping", !isGrounded);
  }
}
