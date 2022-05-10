using System;
using System.Collections.Generic;
using System.Threading;

namespace DimucaTheDev.Dino
{
    internal class Program
    {
        #region Переменные
        public static int long_terrain = 30;
        public static string clear_screen_cmd = "cls";
        public static int max_y = 5;
        public static bool god_mode = false;
        public static bool bot_mode = false;
        public static bool hard_mode = false;
        public static bool easy_mode = true;
        public static bool fly_mode = false;
        public static char dino = '>';
        public static char died_dino = 'X';
        public static char bird = '<';
        public static char cactus = '!';
        // Поменяйте на знак "=" если выводится "?";
        public static char floor = '=';
        public static char[] decorations_on_field = new char[] { '.', '_' };
        public static int chance = 5;
        public static int chance_birds = 1;
        public static int chance_decor = 2;
        public static double speed = 0.1;
        public static bool game_on = true;
        public static int jumps = 0;
        public static int score = 0;
        public static int y = 0;
        public static int walked = 0;
        public static string fields = new(' ', long_terrain);
        public static string sky = new(' ', long_terrain);
        public static Dictionary<string, bool> modes = new()
        {
            { "GOD", god_mode },
            { "BOT", bot_mode },
            { "HARD", hard_mode },
            { "EASY", easy_mode },
            { "FLY", fly_mode }
        };
        #endregion
        #region Методы
        static void output_screen(object y, string fields, object score, object jumps,
            char dino, object cactus, char floor, Dictionary<string, bool> modes, object sky, char died_dino)
        {
            //Вадим, че это :(
            string output_up;
            string output_down = fields;


            if ((int)y >= 1)
            {
                if (died_dino == dino)
                {
                    output_up = Program.sky[0] + died_dino.ToString() + Program.sky[1..];
                }
                else
                {
                    output_up = Program.sky[0] + dino.ToString() + Program.sky[1..];
                }
            }
            else
            {
                output_up = (string)sky;
                if (died_dino == dino)
                {
                    output_down = output_down[0] + died_dino.ToString() + output_down[1..];
                }
                else
                {
                    output_down = output_down[0] + dino.ToString() + output_down[1..];
                }
            }
            Console.SetCursorPosition(0,0);

            string modes_spisok = "";
            foreach (var item in modes)
            {
                if (item.Value)
                    modes_spisok += item.Key + " ";
            }
            string output =
                $"Score {score}  Jumps {jumps}\n" +
                $"\n" +
                $"{output_up}\n" +
                $"{output_down}\n" +
                $"{new string(floor, long_terrain)}\n" +
                $"Press Enter to Jump!\n\n" + modes_spisok;
            Console.WriteLine(output);
        }
        static void trigger_jump()
        {
            while (game_on)
            {
                try
                {
                    Console.ReadKey();
                    if (y == 0)
                        jump_execute();
                }
                catch
                {
                    game_on = false;
                    Environment.Exit(0);
                }
            }
        }
        static void jump_execute()
        {
            y = max_y;
            jumps += 1;
            score += 10;
        }
        static void gen_cactuses(int chance)
        {
            Random r = new Random();
            if (r.Next(0, 99) < chance)
                fields += cactus;
            else
                fields += " ";
        }
        static void gen_decor(int chance_decor)
        {
            Random r = new Random();
            if (r.Next(0, 99) < chance_decor && fields[fields.Length - 1] == ' ')
                fields = fields[0..(fields.Length - 1)] + choice(decorations_on_field);
        }
        static void gen_birds(int chance_birds)
        {
            Random r = new Random();
            if (r.Next(0, 99) < chance_birds && sky[sky.Length - 1] == ' ')
                sky = sky[0..(sky.Length - 1)] + bird;
            else
                sky += ' ';
        }
        static void bot_jump(int _y, string _fields)
        {
            if (_y == 0 && fields[2] == cactus)
                jump_execute();
        }        
        static object choice(char[] a)
        {
            Random r = new Random();
            return a[r.Next(0,a.Length)];
        }
#endregion
        static void Main(string[] args)
        {

            Console.CursorVisible = false;
            while (true)
            {

                Thread th = new(trigger_jump);
                th.Start();
                while (game_on)
                {
                    //Console.WriteLine("Game");
                    if (fly_mode)
                        y = 999;
                    if (bot_mode)
                        bot_jump(y, fields);
                    if (y >= 1)
                        y -= 1;
                    Random r = new Random();
                    fields = fields[1..];
                    gen_cactuses(chance);
                    gen_decor(chance_decor);
                    sky = sky[1..];
                    gen_birds(chance_birds);
                    if (fields[0] == cactus && y == 0 && !god_mode)
                    {
                        output_screen(y, fields, score, jumps, dino, cactus, floor, modes, sky, died_dino);
                        Console.WriteLine("Game Over");
                        Console.WriteLine($"{score} {jumps}");
                        Environment.Exit(0);

                    }
                    if (sky[0] == bird && y >= 1 && !god_mode)
                    {
                        output_screen(y, fields, score, jumps, dino, cactus, floor, modes, sky, died_dino);
                        Console.WriteLine("Game Over");
                        Console.WriteLine($"{score} {jumps}");
                        Environment.Exit(0);

                    }
                    if (hard_mode)
                    {
                        chance = r.Next(5, 12);
                        speed = r.Next(1, 10) / 10;
                    }
                    if (easy_mode)
                    {
                        chance = r.Next(0, 5);
                    }
                    score += 20;
                    walked += 1;
                    output_screen(y, fields, score, jumps, dino, cactus, floor, modes, sky, died_dino);
                    Thread.Sleep((int)(speed*1000));
                }
            }
        }
    }
}
