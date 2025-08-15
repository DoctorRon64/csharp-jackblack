using _Utils;
public class C : Singleton<C>, ISingleton {
    public void OnDestroy() {
    }

    public void OnInitialize() {
    }

    public static void Color(ConsoleColor color) => Console.ForegroundColor = color;
}
