using UnityEngine;
[System.Serializable]
public class Level 
{
    private const int MAX_BOT=10;
    [SerializeField]private int[]totalsCharacter = {10,15};
    private int currentStage;
    private int remainingToSpawn;
    public int CurrentStage=> currentStage;
    public int RemainingToSpawn=>remainingToSpawn;
    public bool HasNextStage=>currentStage+1<totalsCharacter.Length;
    public Level(){}
    public Level (int []totals){totalsCharacter=totals;}
    public void StartStage(int index)
    {
        currentStage=Mathf.Clamp(index,0,totalsCharacter.Length-1);
        remainingToSpawn=totalsCharacter[currentStage];
    }
    public bool CanSpawnMore(int count) => remainingToSpawn>0 && count<MAX_BOT;
    public void OnBotSpawned()=>remainingToSpawn--;
    public bool IsStageCleared(int count) =>remainingToSpawn<=0 &&count<=0;
}
