using System.Threading.Tasks;

namespace Common.Interfaces {
    
    public interface IGameDataProvider {
        int Priority();
    }

    public interface IGameDataProvider<T> {
        Task<string> GetData(T dataType, string gameName);
    }
    
}