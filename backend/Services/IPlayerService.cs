namespace backend.Services
{
    public interface IPlayerService
    {
        Task SetGameProcessPos(int gameId, int teamId);
        Task SetTeamPos(int gameId, int teamId);
    }
}