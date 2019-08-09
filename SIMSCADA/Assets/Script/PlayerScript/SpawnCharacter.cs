using System.IO;
using UnityEngine;

public class SpawnCharacter : MonoBehaviour
{

    // SpawnPlayer is called before the first frame update
    public void SpawnPlayer()
    {
        SpriteRenderer player = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabPlayer.gameObject, PrefabManager.playerIndex)
            .GetComponent<SpriteRenderer>();

        player.name = StringDb.playerPrefabName;

        Vector3 position = ClassDb.regularPathfinder.listTileMap[0].layoutGrid.CellToWorld(new Vector3Int(StringDb.playerSpawn.x, StringDb.playerSpawn.y, 0));
        position = new Vector3(position.x, position.y, 0f);
        player.transform.position = position;
    }

    public AiController SpawnAi(int id, bool isAttacker)
    {
        SpriteRenderer ai = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabAi.gameObject, PrefabManager.aiIndex)
            .GetComponent<SpriteRenderer>();

        ai.name = StringDb.aiPrefabName + id;

        //set spawning position, first time the cell and second the z position, in order to render correctly the character
        Vector3 position = ClassDb.regularPathfinder.listTileMap[0].layoutGrid.CellToWorld(new Vector3Int(StringDb.aiSpawn.x, StringDb.aiSpawn.y, 0));
        position = new Vector3(position.x, position.y, 0f);

        ai.transform.position = position;

        AiController aiController = ai.GetComponent<AiController>();

        aiController.isAttacker = isAttacker;

        return aiController;
    }

    public AiController RespawnAi(SerializableAiController controller)
    {
        SpriteRenderer ai = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabAi.gameObject, PrefabManager.aiIndex)
            .GetComponent<SpriteRenderer>();

        ai.name = StringDb.aiPrefabName + controller.aiId;

        ai.transform.position = ClassDb.regularPathfinder.listTileMap[0].layoutGrid.CellToWorld(controller.aiCellPos);

        AiController aiController = ai.GetComponent<AiController>();

        aiController = controller.DeserializeAiController(aiController);

        return aiController;
    }

    public void RemoveAi(GameObject aiGameObject)
    {
        AiController aiController = aiGameObject.GetComponent<AiController>();

        aiController.DestroyAi();
    }
}
