﻿using System.IO;
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

    public AiController SpawnLocalAi(int id)
    {
        SpriteRenderer ai = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabAi.gameObject, PrefabManager.aiIndex)
            .GetComponent<SpriteRenderer>();

        ai.name = StringDb.aiPrefabName + id;
        ai.sprite = Resources.LoadAll<Sprite>(Path.Combine(StringDb.rscSpriteFolder, StringDb.rscAiSpritePrefix))[0];

        //set spawning position, first time the cell and second the z position, in order to render correctly the character
        Vector3 position = ClassDb.regularPathfinder.listTileMap[0].layoutGrid.CellToWorld(new Vector3Int(StringDb.aiSpawn.x, StringDb.aiSpawn.y, 0));
        position = new Vector3(position.x, position.y, 0f);
        ai.transform.position = position;

        AiController aiController = ai.GetComponent<AiController>();

        aiController.isAttacker = true;

        return aiController;
    }

    public AiController SpawnFakeLocalAi(int id)
    {
        SpriteRenderer ai = ClassDb.prefabManager.GetComponent<PrefabManager>().GetPrefab(ClassDb.prefabManager.prefabAi.gameObject, PrefabManager.aiIndex)
            .GetComponent<SpriteRenderer>();

        ai.name = StringDb.aiPrefabName + id;
        ai.sprite = Resources.LoadAll<Sprite>(Path.Combine(StringDb.rscSpriteFolder, StringDb.rscAiSpritePrefix))[0];

        //set spawning position, first time the cell and second the z position, in order to render correctly the character
        Vector3 position = ClassDb.regularPathfinder.listTileMap[0].layoutGrid.CellToWorld(new Vector3Int(StringDb.aiSpawn.x, StringDb.aiSpawn.y, 0));
        position = new Vector3(position.x, position.y, 0f);
        ai.transform.position = position;

        AiController aiController = ai.GetComponent<AiController>();

        aiController.isAttacker = false;

        return aiController;
    }

    public AiController SpawnRemoteAi(int id)
    {
        SpriteRenderer ai = ClassDb.prefabManager.GetComponent<PrefabManager>().GetPrefab(ClassDb.prefabManager.prefabAi.gameObject, PrefabManager.aiIndex)
            .GetComponent<SpriteRenderer>();

        ai.name = StringDb.aiPrefabName + id;

        ai.enabled = false;

        AiController aiController = ai.GetComponent<AiController>();

        aiController.isAttacker = true;

        return aiController;
    }

    public AiController RepawnLocalAi(int id)
    {
        SpriteRenderer ai = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabAi.gameObject, PrefabManager.aiIndex)
            .GetComponent<SpriteRenderer>();

        ai.name = StringDb.aiPrefabName + id;
        ai.sprite = Resources.LoadAll<Sprite>(Path.Combine(StringDb.rscSpriteFolder, StringDb.rscAiSpritePrefix))[0];

        //set spawning position, first time the cell and second the z position, in order to render correctly the character
        Vector3 position = ClassDb.regularPathfinder.listTileMap[0].layoutGrid.CellToWorld(new Vector3Int(StringDb.aiSpawn.x, StringDb.aiSpawn.y, 0));
        position = new Vector3(position.x, position.y, 0f);
        ai.transform.position = position;

        AiController aiController = ai.GetComponent<AiController>();

        aiController.isAttacker = true;

        return aiController;
    }

    public AiController RepawnFakeLocalAi(int id)
    {
        SpriteRenderer ai = ClassDb.prefabManager.GetComponent<PrefabManager>().GetPrefab(ClassDb.prefabManager.prefabAi.gameObject, PrefabManager.aiIndex)
            .GetComponent<SpriteRenderer>();

        ai.name = StringDb.aiPrefabName + id;
        ai.sprite = Resources.LoadAll<Sprite>(Path.Combine(StringDb.rscSpriteFolder, StringDb.rscAiSpritePrefix))[0];

        //set spawning position, first time the cell and second the z position, in order to render correctly the character
        Vector3 position = ClassDb.regularPathfinder.listTileMap[0].layoutGrid.CellToWorld(new Vector3Int(StringDb.aiSpawn.x, StringDb.aiSpawn.y, 0));
        position = new Vector3(position.x, position.y, 0f);
        ai.transform.position = position;

        AiController aiController = ai.GetComponent<AiController>();

        aiController.isAttacker = false;

        return aiController;
    }

    public AiController RespawnRemoteAi(int id)
    {
        SpriteRenderer ai = ClassDb.prefabManager.GetComponent<PrefabManager>().GetPrefab(ClassDb.prefabManager.prefabAi.gameObject, PrefabManager.aiIndex)
            .GetComponent<SpriteRenderer>();

        ai.name = StringDb.aiPrefabName + id;

        ai.enabled = false;

        AiController aiController = ai.GetComponent<AiController>();

        aiController.isAttacker = true;

        return aiController;
    }

    public void RemoveAi(GameObject aiGameObject)
    {
        AiController aiController = aiGameObject.GetComponent<AiController>();

        aiController.DestroyAi();
    }
}
