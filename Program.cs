using System;
using System.Threading;

class Block
{
    public bool IsVisible { get; set; }

    public Block()
    {
        IsVisible = true;
    }
}

class PinballGame
{
    static void Main(string[] args)
    {
        Console.Title = "Pinball Game";

        while (true)
        {
            ShowStartMessage();
            PlayGame();

            Console.WriteLine("¡Presiona 'R' para reiniciar o cualquier otra tecla para salir!");

            var key = Console.ReadKey(true).Key;
            if (key != ConsoleKey.R)
                break;
        }
    }

    static void ShowStartMessage()
    {
        Console.Clear();
        Console.WriteLine("¡Presiona cualquier tecla para empezar el juego!");
        Console.ReadKey(true);
    }

    static void PlayGame()
    {
        int ballX = Console.WindowWidth / 2;
        int ballY = 10;
        int ballSpeedX = 1;
        int ballSpeedY = 1;

        int paddleLength = 20;
        int paddlePosition = (Console.WindowWidth - paddleLength) / 2;

        int platformLength = 30;
        int platformPosition = (Console.WindowWidth - platformLength) / 2;

        bool gameOver = false;
        int score = 0;
        int platformTouches = 0;

        int gameSpeed = 100;

        Block[] blocks = new Block[Console.WindowWidth / 7];
        Random random = new Random();

        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i] = new Block();
        }

        while (!gameOver)
        {
            Console.Clear();

            Console.SetCursorPosition(paddlePosition, Console.WindowHeight - 1);
            Console.Write(new string('=', paddleLength));

            Console.SetCursorPosition(platformPosition, Console.WindowHeight - 2);
            Console.Write(new string('"', platformLength));

            DrawBlocks(blocks);

            Console.SetCursorPosition(ballX, ballY);
            Console.Write("⚽");

            ballX += ballSpeedX;
            ballY += ballSpeedY;

            // Check for collision with walls
            if (ballX <= 0 || ballX >= Console.WindowWidth - 1)
                ballSpeedX = -ballSpeedX;

            if (ballY <= 0)
                ballSpeedY = -ballSpeedY;

            // Check for collision with paddle
            if (ballY >= Console.WindowHeight - 2 && ballX >= platformPosition && ballX < platformPosition + platformLength)
            {
                ballSpeedY = -ballSpeedY;

                platformTouches++;
                if (platformTouches % 5 == 0)
                {
                    // Generate a new line of blocks every 5 platform touches
                    GenerateNewBlockLine(blocks);
                }
            }

            // Check for collision with blocks
            int blockIndex = ballX / 7;
            if (ballY >= 5 && ballY < 8 && blockIndex >= 0 && blockIndex < blocks.Length && blocks[blockIndex].IsVisible)
            {
                blocks[blockIndex].IsVisible = false;
                score++;
                ballSpeedY = -ballSpeedY;
            }

            // Display the score in the center of the screen
            string scoreText = "Puntos: " + score;
            Console.SetCursorPosition((Console.WindowWidth - scoreText.Length) / 2, Console.WindowHeight - 1);
            Console.Write(scoreText);

            if (ballY >= Console.WindowHeight - 1)
            {
                gameOver = true;
                Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2);
                Console.Write("Game Over! Puntuación: " + score);
            }

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.A && platformPosition > 0)
                    platformPosition -= 2;
                else if (key == ConsoleKey.D && platformPosition < Console.WindowWidth - platformLength)
                    platformPosition += 2;
            }

            Thread.Sleep(gameSpeed);
        }

        Console.ReadKey(true);
    }

    static void DrawBlocks(Block[] blocks)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].IsVisible)
            {
                Console.SetCursorPosition(i * 7, 5);
                Console.Write("⬛⬛⬛⬛⬛");
                Console.SetCursorPosition(i * 7, 6);
                Console.Write("⬛     ⬛");
                Console.SetCursorPosition(i * 7, 7);
                Console.Write("⬛⬛⬛⬛⬛");
            }
        }

        Console.ResetColor();
    }

    static void GenerateNewBlockLine(Block[] blocks)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].IsVisible = true;
        }
    }
}
