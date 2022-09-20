using System.Threading.Tasks;

namespace Common.Interfaces {
    
    public interface IGameDataProvider {
        int Priority();
    }

    public interface IGameDataProvider<in TGameDataType> {
        Task<string> GetData(TGameDataType dataType, string gameName);
    }
    
}