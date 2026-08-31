using NUnit.Framework;
public class TestLevel
{
    [Test]
    public void CanSPawnMore_AliveAtMax_ReturnsFalse()
    {
        Level level = new Level(new []{20});
        level.StartStage(0);
        Assert.IsFalse(level.CanSpawnMore(10));
    }
    [Test]
    public void CanSpawnMore_NoRemaining_ReturnsFalse()
    {
        Level level = new Level(new []{1});
        level.StartStage(0);
        level.OnBotSpawned();
        Assert.IsFalse(level.CanSpawnMore (0));

    }
    [Test]
    public void IsStageCleared_AllSpawnedAndAllDead_ReturnsTrue()
    {
        Level level = new Level(new []{2});
        level.StartStage(0);
        level.OnBotSpawned();
        level.OnBotSpawned();
        Assert.IsTrue(level.IsStageCleared  (0));

    }
    [Test]
    public void IsStageCleared_StillAlive_ReturnsFalse()
    {
        Level level = new Level(new []{2});
        level.StartStage(0);
        level.OnBotSpawned();
        level.OnBotSpawned();
        Assert.IsFalse(level.IsStageCleared  (1));

    }
    [Test]
    public void HasNextStage_LastStage_ReturnsFalse()
    {
        Level level = new Level(new []{5,5});
        level.StartStage(1);
        Assert.IsFalse(level.HasNextStage);

    }      
}