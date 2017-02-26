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
        public int Score; // текущий счет 
        public int MaxScore = 0; // наилучший счет (изначально равен нулю)
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
            IsDethmatch = false;                 // установка рекорда
            ClassScoreManager = ClassScoreManager.ReadScores();
            MaxScore = ClassScoreManager.Score.Value;
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
            MaxScore = ClassScoreManager.Score.Value;
        }
        public void WriteScore()
        {
            ClassScoreManager.Score.Value = ClassScoreManager.Score.Value + Score;
            ClassScoreManager.WriteScores();
        }
        
        public void WinGame()
        {
            IsWin = true;
            IsLose = false;
            IsGame = false;
            IsDethmatch = false;
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
        public void Classic()
        {
            IsWin = false;
            IsLose = false;
            IsGame = true;
            Score = 0;
            ReadScore();
            mobs = 10;
            countArrow = 0;        
            speedArrow = 5f;
            speedMobs = -100;
            timespawn = 0.5f;
        }
        public void Dethmatch()
        {
            IsWin = false;
            IsDethmatch = true;
            IsLose = false;
            IsGame = true;
            Score = 0;
            ReadScore();
            mobs = 100;
            countArrow = 0;
            speedArrow = 5f;
            speedMobs = -100;
            timespawn = 2f;
        }
    }
}