using UnityEngine;
public class IdleState : IBotState
{
    private float timer;
    public void OnEnter(Bot bot)
    {
        timer= Random.Range(Constatnts.BOT_IDLE_MIN_TIME,Constatnts.BOT_IDLE_MAX_TIME);
        bot.StopMoving();
    }
    public void OnExcute(Bot bot)
    {
        if (bot.HasTarget)
        {
            bot.ChangeState(new AttackState());
            return;
        }
        timer -=Time.deltaTime;
        if(timer<= 0f) bot.ChangeState(new PatrolState());
    }
    public void OnExit(Bot bot)
    {
        
    }
}