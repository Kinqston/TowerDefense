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
        public bool IsWin; // ���� �� ������ (� ���� ����� ���������� �����, �� ������ �������� �����)
        public bool IsLose; // ���� �� ��������� (� ���� ����� ���������� �����, �� ������ �������� �����)
        public bool IsGame; // ���� �� ������� ������� (� ���� ����� ���������� �����, �� ������ �������� �����)
        public bool IsPause; // �������� �� �����
        public int Score; // ������� ���� 
        public int MaxScore = 0; // ��������� ���� (���������� ����� ����)
        ClassScoreManager ClassScoreManager = new ClassScoreManager(); // �������� �����
        //public List<Enemy> Enemies = new List<Enemy>(); // ����������
        //public List<Enemy> BadEnemies = new List<Enemy>(); // ������ ����������
         // ������������� ������
        public GameProcess()
        {
            IsWin = false; // ������ �� ����
            IsLose = false; // ��������� �� ����
            IsGame = false; // ������� ������� ��� �� ����
            IsGame = false; // ����� �� ��������
            // ��������� �������
            ClassScoreManager = ClassScoreManager.ReadScores();
            MaxScore = ClassScoreManager.Score.Value;
            // ���������� ������
            //Enemies.Add(new Enemy(100, 300));
            //Enemies.Add(new Enemy(500, 400));
            //Enemies.Add(new Enemy(300, 400));
            //Enemies.Add(new Enemy(200, 350));
            //Enemies.Add(new Enemy(600, 100));
            //Enemies.Add(new Enemy(150, 200));
            //Enemies.Add(new Enemy(400, 150));
            //// ���������� ����������
            //BadEnemies.Add(new Enemy(200, 500));
            //BadEnemies.Add(new Enemy(610, 200));
            //BadEnemies.Add(new Enemy(200, 510));
            //BadEnemies.Add(new Enemy(500, 350));
            //BadEnemies.Add(new Enemy(100, 420));
        }
        // ��������� ����� ���� (������������� ��������� ���������� ������)
        public void NewGame()
        {
            IsWin = false;
            IsLose = false;
            IsGame = false;
            ClassScoreManager.ReadScores();
        }
        // ��������� ������
        public void WinGame()
        {
            IsWin = true;
            IsLose = false;
            IsGame = false;
            ClassScoreManager.Score.Value = MaxScore;
            ClassScoreManager.WriteScores();
        }
        // ��������� ���������
        public void LoseGame()
        {
            IsWin = false;
            IsLose = true;
            IsGame = false;
        }
    }
}