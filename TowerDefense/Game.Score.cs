using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace TowerDefense
{
	[Serializable]
	public class ClassScoreManager
	{
		// класс "счет"
		[Serializable]
		public class ScoreItem
		{
			public int Value = 0;                       //Classic
            public int Hp = 1;
            public float speed_arrow = 3f;
            public int arrow_strenght = 1;
            public int speed_mobs = -50;
            public float time_spawn = 0.5f;
            public int count_mobs = 10;
            public int lvl_game = 1;
            public int count_arrow = 5;
            public double arrow_spawn = 0.5;

            public int Hp_hard = 1;                 //Hard
            public int lvl_game_hard = 1;
            public float time_spawn_hard = 0.5f;
            public int speed_mobs_hard = -100;
            public float speed_arrow_hard = 3f;
            public int count_mobs_hard = 10;
            public int count_arrows_hard = 5;


            public int lvl_game_ballista= 1;                //Ballista
            public float time_spawn_ballista = 0.5f;
            public int speed_mobs_ballista = -50;
            public float speed_arrow_ballista = 5.5f;
            public int count_mobs_ballista = 20;
            public int count_arrows_ballista = 1;

            public int lvl_game_dethmatch = 1;                //Dethmatch
            public float time_spawn_dethmatch = 2f;
            public int speed_mobs_dethmatch = -80;
            public float speed_arrow_dethmatch = 7f;
            public int count_mobs_dethmatch =1000;
            public int count_arrows_dethmatch = 10;
            public double arrow_spawn_dethmatch = 0.2;
        }
        public ScoreItem Score;

		// прочитать счет обоих режимов из файла
		public ClassScoreManager ReadScores()
		{
			try
			{
				var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
				var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Best Studio", "tower_defense.xml");
				FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
				var myBinaryFormatter = new BinaryFormatter();
				var mc = (ClassScoreManager) myBinaryFormatter.Deserialize(fStream);
				fStream.Close();
				return mc;
			}
			catch (Exception e)
			{
				Score = new ScoreItem ();
				return this;
			}
		}

		// записать счет обоих режимов в файл
		public void WriteScores()
		{
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			if (!Directory.Exists(sdCardPath +"/Application")) Directory.CreateDirectory (sdCardPath +"/Application");
			if (!Directory.Exists(sdCardPath +"/Application/Best Studio")) Directory.CreateDirectory (sdCardPath + "/Application/Best Studio");
			var filePath = System.IO.Path.Combine(sdCardPath+ "/Application/Best Studio", "tower_defense.xml");
			FileStream fStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			var myBinaryFormatter = new BinaryFormatter();
			myBinaryFormatter.Serialize(fStream, this);
			fStream.Close();
		}
	}
}

