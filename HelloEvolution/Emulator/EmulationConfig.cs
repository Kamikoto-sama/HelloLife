﻿using System;

namespace Emulator
{
	public class EmulationConfig
	{
		public int GenerationSize { get; set; } = 64;
		public int GenomeSize { get; set; } = 64;
		public int ParentsCount { get; set; } = 8;
		public int EachParentCopiesCount { get; set; } = 7;
		public int MutationsCount { get; set; } = 8;
		
		public int BotInitialHealth { get; set; } = 40;
		public Directions BotInitialDirection { get; set; } = Directions.Up;
		public int BotMaxHealth { get; set; } = 90;
		public int FoodHealthIncrease { get; set; } = 10;

		public int PoisonCountInMap { get; set; } = 50;
		public int FoodCountInMap { get; set; } = 50;
		public string TxtMapFilePath { get; set; } = "map.txt";

		public int GoalGenerationLifeCount { get; set; } = 1000;
		public DelayTypes DelayType { get; set; } = DelayTypes.None;
		public TimeSpan IterationDelay { get; set; } = TimeSpan.FromMilliseconds(500);
	}
}