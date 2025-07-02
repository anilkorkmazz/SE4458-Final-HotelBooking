public interface IAgentCoordinatorService
{
    Task<string> HandleMessageAsync(string message); // bu isimle eşleştir
}
