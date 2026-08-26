public class PatrolState : IBotState
{
    public void OnEnter(Bot bot)
    {
        if(bot.MoveToRandomPoint()==false) bot.ChangeState(new IdleState());
    }
    public void OnExcute(Bot bot)
    {
        if (bot.HasTarget)
        {
            bot.ChangeState(new AttackState());
            return;
        }
        if(bot.IsAtDestination()) bot.ChangeState(new IdleState());
    }
    public void OnExit(Bot bot) => bot.StopMoving();
}