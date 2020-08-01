﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emulator.Interfaces;

namespace Emulator
{
	public class Emulation
	{
		private readonly IWorldMapProvider mapProvider;
		private readonly IWorldMapFiller mapFiller;
		private readonly IGenerationBuilder generationBuilder;
		private readonly EmulationConfig config;
		private WorldMap map;
		private IEnumerable<Bot> bots;

		public Emulation(IWorldMapProvider mapProvider, 
			IWorldMapFiller mapFiller,
			IGenerationBuilder generationBuilder,
			EmulationConfig config)
		{
			this.mapProvider = mapProvider;
			this.mapFiller = mapFiller;
			this.generationBuilder = generationBuilder;
			this.config = config;
		}

		public void Start()
		{
			if (map == null || bots == null)
				Prepare();
			while (true)
			{
				mapFiller.FillBots(map, bots);
				RunGeneration();
				var survivedBots = bots.Where(bot => !bot.IsDead).ToArray();
				bots = generationBuilder.BuildNew(survivedBots);
				mapFiller.RemoveObjectsFromMap(survivedBots, map);
			}
		}

		private void Prepare()
		{
			bots = generationBuilder.CreateInitial();
			map = mapProvider.GetMap();
			mapFiller.FillItems(map);
			map.CellChanged += AddItem;
		}

		private void RunGeneration()
		{
			var aliveBotsCount = config.GenerationSize;
			while (aliveBotsCount > config.ParentsCount)
			{
				foreach (var bot in bots.Where(bot => !bot.IsDead))
				{
					Command command;
					do
					{
						command = bot.CurrentCommand;
						command.Execute(bot, map);
					} while (!command.IsFinal);

					if (bot.IsDead)
						aliveBotsCount--;
				}
			}
		}

		private void AddItem(WorldMapChangedEvent eventArgs)
		{
			var currentObjType = map[eventArgs.Coordinates.X, eventArgs.Coordinates.Y].Type;
			var prevObjType = eventArgs.PreviousCellObject.Type;
			if (prevObjType == currentObjType)
				return;
			IWorldObject objFactory(Point pos) => new WorldObject(pos, prevObjType);
			mapFiller.PlaceObject(objFactory, 1, map);
		}
	}
}