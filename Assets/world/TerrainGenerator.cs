using System.Collections.Generic;
using UnityEngine;


// Generador de terreno procedural infinito.
// Genera chunks adelante del jugador y destruye los que quedan atrás.
public class TerrainGenerator : MonoBehaviour {
    [Header("References")]
    [Tooltip("Transform del jugador a seguir")]
    public Transform player;

    [Tooltip("Prefab del chunk de terreno")]
    public GameObject chunkPrefab;

    [Header("Generation Settings")]
    [Tooltip("Cantidad de chunks a generar al inicio")]
    public int chunksPreload = 6;

    [Tooltip("Ancho de cada chunk en unidades")]
    public float chunkWidth = 16f;

    [Tooltip("Distancia adelante del jugador para generar nuevos chunks")]
    public float spawnOffset = 20f;

    [Tooltip("Distancia atrás del jugador para destruir chunks")]
    public float destroyOffset = 20f;

    [Header("Terrain Variation")]
    [Tooltip("Variación vertical del terreno")]
    public float heightVariation = 1f;

    [Tooltip("¿Activar variación de altura?")]
    public bool enableHeightVariation = true;

    [Header("Sprite Adaptation")]
    [Tooltip("Rotar el chunk para que su sprite siga la pendiente calculada")]
    public bool rotateChunkToSlope = true;

    [Header("Slope Settings (Pendientes)")]
    [Tooltip("Ángulo máximo de pendiente en grados (ej: 15 = subida/bajada moderada)")]
    [Range(0f, 45f)]
    public float maxSlopeAngle = 15f;

    [Tooltip("Mínimo de chunks consecutivos con la misma pendiente")]
    public int minSlopeLength = 3;

    [Tooltip("Máximo de chunks consecutivos con la misma pendiente")]
    public int maxSlopeLength = 8;

    [Tooltip("¿Activar pendientes/ángulos en el terreno?")]
    public bool enableSlopes = true;

    [Header("Collision Settings")]
    [Tooltip("¿Generar colisión suave automáticamente?")]
    public bool generateSmoothCollision = true;

    [Tooltip("GameObject padre para los colliders (opcional)")]
    public GameObject collisionParent;

    private Queue<GameObject> activeChunks = new Queue<GameObject>();
    private Queue<Vector3> chunkPositions = new Queue<Vector3>(); // Guarda posiciones para colisión suave
    private EdgeCollider2D terrainCollider;
    private float lastChunkX = 0f;
    private float lastChunkY = 0f;
    private bool isInitialized = false;

    // Variables para controlar las pendientes
    private float currentSlope = 0f; // Ángulo actual en grados
    private int slopeChunksRemaining = 0; // Chunks restantes con la misma pendiente

    void Start() {
        InitializeTerrain();
    }

    void Update() {
        if (!isInitialized || player == null) {
            return;
        }

        GenerateChunksAhead();
        RemoveChunksBehind();
    }


    // Inicializa el terreno generando los chunks iniciales
    private void InitializeTerrain() {
        if (player == null) {
            Debug.LogError("TerrainGenerator: ¡Player no asignado! Asigna el Transform del jugador en el Inspector.");
            return;
        }

        if (chunkPrefab == null) {
            Debug.LogError("TerrainGenerator: ¡ChunkPrefab no asignado! Asigna un prefab de chunk en el Inspector.");
            return;
        }

        // Crea el collider suave si está activado
        if (generateSmoothCollision) {
            CreateSmoothCollider();
        }

        // Empieza generando chunks desde antes de la posición del jugador
        lastChunkX = player.position.x - (chunkWidth * (chunksPreload / 2f));
        lastChunkY = player.position.y;

        // Inicializa la primera pendiente
        if (enableSlopes) {
            GenerateNewSlope();
        }

        // Pre-genera los chunks iniciales
        for (int i = 0; i < chunksPreload; i++) {
            SpawnChunk();
        }

        isInitialized = true;
        Debug.Log($"TerrainGenerator: {chunksPreload} chunks generados exitosamente.");
    }


    // Genera nuevos chunks adelante del jugador
    private void GenerateChunksAhead() {
        // Genera chunks si el jugador se acerca al último chunk
        while (player.position.x + spawnOffset > lastChunkX) {
            SpawnChunk();
        }
    }

    // Remueve chunks que quedaron muy atrás del jugador
    private void RemoveChunksBehind() {
        // Destruye chunks que están muy atrás del jugador
        while (activeChunks.Count > 0) {
            GameObject frontChunk = activeChunks.Peek();

            // Verifica si el chunk está muy atrás
            if (frontChunk != null && frontChunk.transform.position.x + chunkWidth < player.position.x - destroyOffset) {
                Destroy(activeChunks.Dequeue());

                // Remueve la posición del queue también
                if (chunkPositions.Count > 0) {
                    chunkPositions.Dequeue();

                    // Actualiza el collider suave
                    if (generateSmoothCollision && terrainCollider != null) {
                        UpdateSmoothCollider();
                    }
                }
            } else {
                break; // Sale del loop si el chunk aún está cerca
            }
        }
    }



    // Genera un nuevo chunk de terreno
    private void SpawnChunk() {
        float prevYForAngle = lastChunkY;

        // Avanza en X (siempre horizontal)
        lastChunkX += chunkWidth;

        // Calcula la altura del nuevo chunk basado en la pendiente
        float yPos = lastChunkY;

        if (enableSlopes) {
            // Calcula el cambio de altura basado en la pendiente
            float heightChange = CalculateHeightChange();
            yPos += heightChange;

            // Verifica si necesita generar una nueva pendiente
            slopeChunksRemaining--;
            if (slopeChunksRemaining <= 0) {
                GenerateNewSlope();
            }
        } else if (enableHeightVariation) {
            // Variación aleatoria simple (sistema antiguo)
            yPos = Random.Range(-heightVariation, heightVariation);
        }

        // Actualiza la última posición Y
        lastChunkY = yPos;

        // Posición del chunk (sin rotación)
        Vector3 position = new Vector3(lastChunkX, yPos, 0f);

        // Guarda la posición para el collider suave
        chunkPositions.Enqueue(position);

        // Instancia el chunk SIN rotación - los chunks se colocan uno tras otro
        // La pendiente se crea por el cambio de altura entre chunks, no por rotación
        GameObject newChunk = Instantiate(chunkPrefab, position, Quaternion.identity, transform);
        // Rota el chunk para que su sprite siga la pendiente entre el chunk anterior y éste
        if (rotateChunkToSlope) {
            float angleDeg = Mathf.Atan2(lastChunkY - prevYForAngle, chunkWidth) * Mathf.Rad2Deg;
            newChunk.transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);
        }

        // Desactiva el collider del chunk individual si usamos collider suave
        if (generateSmoothCollision) {
            Collider2D chunkCollider = newChunk.GetComponent<Collider2D>();
            if (chunkCollider != null) {
                chunkCollider.enabled = false;
            }
        }

        activeChunks.Enqueue(newChunk);

        // Actualiza el collider suave
        if (generateSmoothCollision && terrainCollider != null) {
            UpdateSmoothCollider();
        }
    }

    // Crea el Edge Collider 2D para colisión suave
    private void CreateSmoothCollider() {
        GameObject colliderObject;

        if (collisionParent != null) {
            colliderObject = collisionParent;
        } else {
            colliderObject = new GameObject("TerrainCollider");
            colliderObject.transform.SetParent(transform);
        }

        terrainCollider = colliderObject.GetComponent<EdgeCollider2D>();
        if (terrainCollider == null) {
            terrainCollider = colliderObject.AddComponent<EdgeCollider2D>();
        }

        // Configura el collider
        terrainCollider.edgeRadius = 0.1f; // Radio pequeño para suavizar

        Debug.Log("TerrainGenerator: Collider suave creado.");
    }

    // Actualiza el Edge Collider con las posiciones de los chunks
    private void UpdateSmoothCollider() {
        if (terrainCollider == null || chunkPositions.Count == 0) return;

        // Convierte las posiciones a un array de Vector2
        List<Vector2> points = new List<Vector2>();

        foreach (Vector3 pos in chunkPositions) {
            // Añade el punto superior del chunk (superficie)
            points.Add(new Vector2(pos.x - chunkWidth / 2f, pos.y));
        }

        // Añade el último punto (esquina derecha del último chunk)
        if (chunkPositions.Count > 0) {
            Vector3 lastPos = chunkPositions.ToArray()[chunkPositions.Count - 1];
            points.Add(new Vector2(lastPos.x + chunkWidth / 2f, lastPos.y));
        }

        // Asigna los puntos al collider
        terrainCollider.points = points.ToArray();
    }    // Genera una nueva pendiente aleatoria para los próximos chunks
    private void GenerateNewSlope() {
        // Decide cuántos chunks tendrán esta pendiente
        slopeChunksRemaining = Random.Range(minSlopeLength, maxSlopeLength + 1);

        // Decide el ángulo de la pendiente
        // 50% plano, 25% subida, 25% bajada
        float random = Random.value;
        if (random < 0.5f) {
            currentSlope = 0f; // Terreno plano
        } else if (random < 0.75f) {
            currentSlope = Random.Range(maxSlopeAngle * 0.3f, maxSlopeAngle); // Subida
        } else {
            currentSlope = Random.Range(-maxSlopeAngle, -maxSlopeAngle * 0.3f); // Bajada
        }

        Debug.Log($"Nueva pendiente: {currentSlope:F1}° por {slopeChunksRemaining} chunks");
    }

    // Calcula el cambio de altura basado en el ángulo de la pendiente
    private float CalculateHeightChange() {
        // Convierte el ángulo a radianes
        float angleRad = currentSlope * Mathf.Deg2Rad;

        // Calcula el cambio de altura usando trigonometría
        // altura = tan(ángulo) × distancia horizontal
        float heightChange = Mathf.Tan(angleRad) * chunkWidth;

        return heightChange;
    }


    // Limpia todos los chunks activos (útil para reiniciar)
    public void ClearAllChunks() {
        while (activeChunks.Count > 0) {
            GameObject chunk = activeChunks.Dequeue();
            if (chunk != null) {
                Destroy(chunk);
            }
        }

        chunkPositions.Clear();

        if (terrainCollider != null) {
            terrainCollider.points = new Vector2[0];
        }

        lastChunkX = 0f;
        lastChunkY = 0f;
        currentSlope = 0f;
        slopeChunksRemaining = 0;
        isInitialized = false;
    }


    // Reinicia el generador de terreno
    public void ResetTerrain() {
        ClearAllChunks();
        InitializeTerrain();
    }

    // Dibuja gizmos en el editor para visualizar el sistema
    void OnDrawGizmos() {
        if (player == null) return;

        // Zona de spawn (verde)
        Gizmos.color = Color.green;
        Vector3 spawnZone = new Vector3(player.position.x + spawnOffset, player.position.y, 0f);
        Gizmos.DrawWireSphere(spawnZone, 2f);

        // Zona de destrucción (rojo)
        Gizmos.color = Color.red;
        Vector3 destroyZone = new Vector3(player.position.x - destroyOffset, player.position.y, 0f);
        Gizmos.DrawWireSphere(destroyZone, 2f);

        // Visualiza la pendiente actual
        if (Application.isPlaying && enableSlopes) {
            Gizmos.color = Color.yellow;
            Vector3 slopeStart = new Vector3(lastChunkX, lastChunkY, 0f);
            float heightChange = CalculateHeightChange() * slopeChunksRemaining;
            Vector3 slopeEnd = new Vector3(lastChunkX + (chunkWidth * slopeChunksRemaining), lastChunkY + heightChange, 0f);
            Gizmos.DrawLine(slopeStart, slopeEnd);
        }
    }
}
