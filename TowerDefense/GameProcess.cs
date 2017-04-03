using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TowerDefense
{
    class GameProcess
    {
        public bool IsWin; // была ли победа (к полю можно обратиться извне, но нельзя изменить извне)
        public bool IsLose; // было ли поражение (к полю можно обратиться извне, но нельзя изменить извне)
        public bool IsGame; // идет ли игровой процесс (к полю можно обратиться извне, но нельзя изменить извне)
        public bool IsPause; // включена ли пауза
        public bool IsDethmatch;
        public bool IsClassic;
        public bool IsHard;
        public bool IsBallista;
        public bool IsUpgrate;
        public bool IsShop;
        public int Score; // текущий счет 
        public int MaxScore = 0; // наилучший счет (изначально равен нулю)
        public int HP_mobs;
        public int Arrow_strength;
        public int lvl_game;
        public double arrow_spawn;

        public int lvl_game_hard;             //hard
        public int mobs_hard;
        public int speedMobs_hard;
        public float timespawn_hard;
        public int total_arrows;

        ClassScoreManager ClassScoreManager = new ClassScoreManager(); // менеджер счета
        public int mobs; //Количество мобов за раунд
        public int countArrow;   //Количество стрел
        public float speedArrow; //Скорость стрелы
        public int speedMobs;    //Скорость мобов
        public float timespawn;
        //public List<Enemy> Enemies = new List<Enemy>(); // привидения
        //public List<Enemy> BadEnemies = new List<Enemy>(); // желтые шестеренки
         // инициализация класса
        public GameProcess()
        {
            IsWin = false; // победы не было
            IsLose = false; // поражения не было
            IsGame = false; // игровой процесс еще не идет
            IsPause = false; // пауза не включена
            IsClassic = false;
            IsUpgrate = false;
            IsDethmatch = false;                 // установка рекорда
            ClassScoreManager = ClassScoreManager.ReadScores();
            MaxScore = ClassScoreManager.Score.Value;
            HP_mobs = ClassScoreManager.Score.Hp;
            speedArrow = ClassScoreManager.Score.speed_arrow;
            Arrow_strength = ClassScoreManager.Score.arrow_strenght;
            speedMobs = ClassScoreManager.Score.speed_mobs;
            timespawn = ClassScoreManager.Score.time_spawn;
            mobs = ClassScoreManager.Score.count_mobs;
            lvl_game = ClassScoreManager.Score.lvl_game;
            
            // добавление врагов
            //Enemies.Add(new Enemy(100, 300));
            //Enemies.Add(new Enemy(500, 400));
            //Enemies.Add(new Enemy(300, 400));
            //Enemies.Add(new Enemy(200, 350));
            //Enemies.Add(new Enemy(600, 100));
            //Enemies.Add(new Enemy(150, 200));
            //Enemies.Add(new Enemy(400, 150));
            //// добавление шестеренок
            //BadEnemies.Add(new Enemy(200, 500));
            //BadEnemies.Add(new Enemy(610, 200));
            //BadEnemies.Add(new Enemy(200, 510));
            //BadEnemies.Add(new Enemy(500, 350));
            //BadEnemies.Add(new Enemy(100, 420));
        }
        // установка новой игры (подразумевает отрисовку начального экрана)
        public void NewGame()
        {
            IsWin = false;
            IsLose = false;
            IsGame = false;
            ClassScoreManager.ReadScores();
        }
        // установка победы

        public void ReadScore()
        {
            ClassScoreManager = ClassScoreManager.ReadScores();
            speedArrow = ClassScoreManager.Score.speed_arrow;
            Arrow_strength = ClassScoreManager.Score.arrow_strenght;
            if (IsClassic)
            {
                HP_mobs = ClassScoreManager.Score.Hp;
                speedArrow = ClassScoreManager.Score.speed_arrow;
                Arrow_strength = ClassScoreManager.Score.arrow_strenght;
                speedMobs = ClassScoreManager.Score.speed_mobs;
                timespawn = ClassScoreManager.Score.time_spawn;
                mobs = ClassScoreManager.Score.count_mobs;
                lvl_game = ClassScoreManager.Score.lvl_game;
                countArrow = ClassScoreManager.Score.count_arrow;
                arrow_spawn = ClassScoreManager.Score.arrow_spawn;
            }
            if (IsHard)
            {
                speedArrow = ClassScoreManager.Score.speed_arrow_hard;
                speedMobs = ClassScoreManager.Score.speed_mobs_hard;
                timespawn = ClassScoreManager.Score.time_spawn_hard;
                mobs = ClassScoreManager.Score.count_mobs_hard;
                lvl_game = ClassScoreManager.Score.lvl_game_hard;
                countArrow = ClassScoreManager.Score.count_arrows_hard;
                HP_mobs = ClassScoreManager.Score.Hp_hard;
                total_arrows = ClassScoreManager.Score.count_arrows_hard;
            }
            if (IsBallista)
            {
                speedMobs = ClassScoreManager.Score.speed_mobs_ballista;             
                timespawn =  ClassScoreManager.Score.time_spawn_ballista;
                mobs = ClassScoreManager.Score.count_mobs_ballista;
                lvl_game = ClassScoreManager.Score.lvl_game_ballista;
                speedArrow = ClassScoreManager.Score.speed_arrow_ballista;
                countArrow = ClassScoreManager.Score.count_arrows_ballista;               
            }
            if (IsDethmatch)
            {
                speedMobs = ClassScoreManager.Score.speed_mobs_dethmatch;
                timespawn = ClassScoreManager.Score.time_spawn_dethmatch;
                mobs = ClassScoreManager.Score.count_mobs_dethmatch;
                lvl_game = ClassScoreManager.Score.lvl_game_dethmatch;
                speedArrow = ClassScoreManager.Score.speed_arrow_dethmatch;
                countArrow = ClassScoreManager.Score.count_arrows_dethmatch;
                arrow_spawn = ClassScoreManager.Score.arrow_spawn_dethmatch;
                HP_mobs = 1;
            }
            MaxScore = ClassScoreManager.Score.Value;
        }
        public void WriteScore()
        {
            if (IsClassic)
            {
                ClassScoreManager.Score.Hp = HP_mobs;
                ClassScoreManager.Score.lvl_game = lvl_game;
                ClassScoreManager.Score.count_mobs = mobs;
                ClassScoreManager.Score.speed_mobs = speedMobs;
                ClassScoreManager.Score.speed_arrow = speedArrow;
                ClassScoreManager.Score.time_spawn = timespawn;
                ClassScoreManager.Score.count_arrow = countArrow;
                ClassScoreManager.Score.arrow_strenght = Arrow_strength;
            }
            if (IsHard)
            {
                ClassScoreManager.Score.Hp_hard = HP_mobs;
                ClassScoreManager.Score.lvl_game_hard = lvl_game;
                ClassScoreManager.Score.count_mobs_hard = mobs;
                ClassScoreManager.Score.speed_mobs_hard = speedMobs;
                ClassScoreManager.Score.speed_arrow_hard= speedArrow;
                ClassScoreManager.Score.time_spawn_hard = timespawn;
                ClassScoreManager.Score.count_arrows_hard = total_arrows;
            }
            if (IsBallista)
            {
                ClassScoreManager.Score.speed_mobs_ballista = speedMobs;
                ClassScoreManager.Score.time_spawn_ballista = timespawn;
                ClassScoreManager.Score.count_mobs_ballista = mobs;
                ClassScoreManager.Score.lvl_game_ballista = lvl_game;
                ClassScoreManager.Score.speed_arrow_ballista = speedArrow;
                ClassScoreManager.Score.count_arrows_ballista = countArrow;
            }
            ClassScoreManager.Score.Value = ClassScoreManager.Score.Value + Score;
            ClassScoreManager.WriteScores();
        }
        public void Upgrate(int count,int buy, string what) {
            ClassScoreManager.Score.Value = ClassScoreManager.Score.Value - buy;
            if (what == "strength")
                ClassScoreManager.Score.arrow_strenght += count;
            else
                ClassScoreManager.Score.speed_arrow += (count/10f);
            ClassScoreManager.WriteScores();
        }        
        public void WinGame()
        {
            IsWin = true;
            IsLose = false;
            IsGame = false;
            if (IsClassic)
            {
                lvl_game += 1;
                if(lvl_game%5 == 0)
                {
                    HP_mobs += 2;
                }
                else
                {
                    if (lvl_game % 3 == 0)
                    {
                        mobs = mobs + 2;
                    }
                    else
                    {
                        if(lvl_game%2==0)
                            speedMobs = speedMobs - 5;
                    }
                }
            }
            if (IsHard)
            {
                lvl_game += 1;
                if (lvl_game % 30 == 0)
                {
                    HP_mobs += 1;
                    total_arrows = 10;
                    timespawn = 0.6f;
                    speedMobs = -100;
                    speedArrow = 8f;
                    mobs = 20;
                }
                else
                {
                    if (lvl_game % 5 == 0)
                    {
                        total_arrows -= 3;
                        if (total_arrows  < 1)
                        {
                            total_arrows = 10;
                        }
                    }
                    else
                    {
                        if (lvl_game % 3 == 0)
                        {
                            timespawn -= 0.2f;
                            speedArrow -= 0.2f;
                        }
                        else
                        {                           
                            speedMobs = speedMobs - 10;                           
                            mobs += 1;                            
                        }
                    }                                 
                }
            }
            if (IsBallista)
            {
                lvl_game += 1;
                if (lvl_game % 15 == 0)
                {
                    speedArrow -= 0.1f;
                    if ((speedArrow == 4f) || (speedArrow == 3f)|| (speedArrow == 2f)|| (speedArrow == 1f))
                    {
                        countArrow++;
                    }
                    mobs = 20;
                    speedMobs = -50;
                }
                else
                {
                    if (lvl_game % 2 == 0)
                    {
                        mobs += 1;
                    }
                    else
                    {
                        speedMobs -= 2;
                    }
                }
            }
                WriteScore();
        }
        // установка поражения
        public void LoseGame()
        {
            IsWin = false;
            IsLose = true;
            IsGame = false;
            IsPause = false;
            WriteScore();
        }
        public void first_game()
        {
            ClassScoreManager.Score.Value = 0;                      //Classic
            ClassScoreManager.Score.Hp = 1;
            ClassScoreManager.Score.speed_arrow = 3f;
            ClassScoreManager.Score.arrow_strenght = 1;
            ClassScoreManager.Score.speed_mobs = -100;
            ClassScoreManager.Score.time_spawn = 0.5f;
            ClassScoreManager.Score.count_mobs = 10;
            ClassScoreManager.Score.lvl_game = 1;
            ClassScoreManager.Score.arrow_spawn = 0.5;

            ClassScoreManager.Score.speed_mobs_hard = -100;             //hard
            ClassScoreManager.Score.time_spawn_hard = 1f;
            ClassScoreManager.Score.count_mobs_hard = 20;
            ClassScoreManager.Score.lvl_game_hard = 1;
            ClassScoreManager.Score.Hp_hard = 1;
            ClassScoreManager.Score.speed_arrow_hard = 10f;
            ClassScoreManager.Score.count_arrows_hard = 10;
           

            ClassScoreManager.Score.speed_mobs_ballista = -50;             //Ballista
            ClassScoreManager.Score.time_spawn_ballista = 0.5f;
            ClassScoreManager.Score.count_mobs_ballista = 20;
            ClassScoreManager.Score.lvl_game_ballista = 1;
            ClassScoreManager.Score.speed_arrow_ballista = 5.5f;
            ClassScoreManager.Score.count_arrows_ballista = 1;

            ClassScoreManager.Score.speed_mobs_dethmatch = -80;             //Dethmatch
            ClassScoreManager.Score.time_spawn_dethmatch = 2f;
            ClassScoreManager.Score.count_mobs_dethmatch = 1000;
            ClassScoreManager.Score.lvl_game_dethmatch = 1;
            ClassScoreManager.Score.speed_arrow_dethmatch = 7f;
            ClassScoreManager.Score.count_arrows_dethmatch = 10;
            ClassScoreManager.Score.arrow_spawn_dethmatch = 0.3;

            ClassScoreManager.WriteScores();
        }
        public void Classic()
        {
            IsClassic = true;          
            ReadScore();
            if (lvl_game == 0)
            {
                first_game();
                ReadScore();
            }          
            IsWin = false;
            IsLose = false;
            IsGame = true;            
            Score = 0;
            countArrow = 20;
        }
        public void Hard()
        {
            IsHard = true;
            ReadScore();          
            if (lvl_game == 0)
            {
                first_game();
                ReadScore();
            }
            IsWin = false;
            IsLose = false;
            IsGame = true;           
            Score = 0;
        }
        public void Ballista()
        {
            IsBallista = true;
            ReadScore();
            if (lvl_game == 0)
            {
                first_game();
                ReadScore();
            }
            IsWin = false;
            IsLose = false;
            IsGame = true;
            Score = 0;
        }
        public void Dethmatch()
        {
            IsDethmatch = true;
            ReadScore();
            if (lvl_game == 0)
            {
                first_game();
                ReadScore();
            }
            IsWin = false;
            IsLose = false;
            IsGame = true;
            Score = 0;
        }
    }
}