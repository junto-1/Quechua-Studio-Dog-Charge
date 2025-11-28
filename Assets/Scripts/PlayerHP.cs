using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Segun chatGPT es buena practica, hay otras formas de declarar tipos pero es la unica que supe aplicar y que funcione
public struct Damage {
  public string Key { get; set; }
  public float Amount { get; set; }
}


public class PlayerHP : MonoBehaviour {
  public float CurrentHp = 1;

  private readonly Damage[] Damages = {
    new() {
      Key = "DamagingObject",
      Amount = 1f
    }
    // Aquí van mas daños que se agreguen en un futuro
  };


  private void Update() {
    //Manda a matar al jugador
    if (CurrentHp <= 0) {
      Die();
    }
  }

  /* Detecta si colisiona con un objeto que haga daño
 Si colisiona llama a la función de hacer daño */

  private void OnTriggerEnter2D(Collider2D other) {
    // Buscar si hay algun Damage con ese tag
    var damage = Damages.FirstOrDefault(d => other.CompareTag(d.Key));
    if (damage.Amount > 0f) {
      TakeDamage(damage.Amount);
    }
  }

  /*
   * IMPORTANTE: Mantener estos métodos publicos.
   */
  //Reduce la vida del jugador
  public void TakeDamage(float amount) {
    CurrentHp -= amount;
  }

  //Mata al jugador
  public void Die() {
    Destroy(gameObject);
    SceneManager.LoadSceneAsync(0);
  }
}
