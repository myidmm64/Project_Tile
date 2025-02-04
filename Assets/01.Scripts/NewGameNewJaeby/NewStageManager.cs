using UnityEngine;

public class NewStageManager : MonoSingleTon<NewStageManager>
{
    public NewStage stagePrefab = null; // ¿”§µ§”
    public NewStage curStage = null;

    private void Awake()
    {
        NewStage spawnedStage = Instantiate(stagePrefab);
        spawnedStage.InitializeStage();
        curStage = spawnedStage;
    }

}
