using System;

namespace MonoFarming {

    static class Program {

        private static MonoFarming.Main game;

        [STAThread]
        static void Main() {

            game = new MonoFarming.Main();
            game.Run();
        }
    }
}