using System.Threading.Tasks;

namespace Common.Interfaces; 

public interface IGameDataProvider {
    int Priority();
}

public interface IGameDataProvider<T, T1> {
    Task<bool> GetData(T dataType, string gameName, out T1 response);
}