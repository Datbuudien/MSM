public interface IBotState
{
    void OnEnter(Bot bot);
    void OnExcute(Bot bot);
    void OnExit(Bot bot);
}