using System.Threading.Tasks;

namespace Common.Interfaces; 

public interface IGameDataProvider {
    int Priority();
}

public interface IGameDataProvider<T, T1> {
    Task<T1?> GetData(T dataType, string gameName);
}