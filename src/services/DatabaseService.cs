using Godot;
using TinyScreen.framework.interfaces;

namespace TinyScreen.services {
    public class DatabaseService: IDatabaseService {

        public void Test() {
            GD.Print("Test from database");
        }
        
    }
}