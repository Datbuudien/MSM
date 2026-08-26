public class AttackState: IBotState
{
    public void OnEnter(Bot bot)=> bot.StopMoving();
    
    public void OnExcute(Bot bot)
    {
        if(bot.HasTarget==false) bot.ChangeState(new IdleState());
    }
    public void OnExit(Bot bot)
    {
        
    }
}