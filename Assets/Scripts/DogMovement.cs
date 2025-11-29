using System;
using UnityEngine;

public class DogMovement : MonoBehaviour {
  public float DogSpeed = 10f;
  private Rigidbody2D rb;
  private Animator animator;
  private bool isInJumpArea  = false;
  private bool WantsToChange = false;

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
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("JumpingArea"))
    {
      isInJumpArea = true;
      Jump();
    }
  }

  //Ejecuta el salto y (se supone) que cambia la animación
  void Jump() {
    rb.AddForce(Vector2.up * 1000);
  }


  private void OnTriggerExit2D(Collider2D other) {
    if (other.CompareTag("JumpingArea"))
    {
      isInJumpArea = false;
    }
  }

  private void UpdateAnimator()
  {
    animator.SetBool("isInJumpArea", isInJumpArea);
    if (!isInJumpArea)
    {
            changeTheAnimation();
    }
  }

  private void changeTheAnimation()
  {
  animator.SetBool("Cambia", WantsToChange);
  }
}
