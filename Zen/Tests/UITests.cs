﻿using NUnit.Framework;
using System;
using System.Linq;
using Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Infrastructure.Testing;
using Consensus;

namespace Zen
{
	[TestFixture()]
	public class UITests
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}

		[Test(), Order(1)]
		public void CanAquireGenesisOutputs()
		{
			App app = new App();

		//	app.Settings.EndpointOptions.EndpointOption = NBitcoinDerive.EndpointOptions.EndpointOptionsEnum.NoNetworking;
			app.Init();

			app.AddGenesisBlock();

			JsonLoader<Outputs>.Instance.Value.Values.ForEach(o => app.ImportKey(o.Key));

			app.Start();

			new Thread(() =>
			{
				Thread.Sleep(2000);
				app.CloseGUI();
				app.Stop();
			}).Start();

			app.GUI();
		}

		[Test(), Order(2)]
		public async Task CanSendAmounts()
		{
			App app = new App();


		//	app.Settings.EndpointOptions.EndpointOption = NBitcoinDerive.EndpointOptions.EndpointOptionsEnum.NoNetworking;
			app.Init();

			app.AddGenesisBlock();

			JsonLoader<Outputs>.Instance.Value.Values.ForEach(o => app.ImportKey(o.Key));

			app.Start();

			Task.Run(() =>
			{
				Thread.Sleep(1000);
				Assert.That(app.Spend(2), Is.True);
				Thread.Sleep(1000);
				Assert.That(app.Spend(3), Is.True);
				Thread.Sleep(1000);
				Assert.That(app.Spend(4), Is.True);
				Thread.Sleep(1000);
				Assert.That(app.Spend(500), Is.False);
			});

			Task.Run(() =>
			{
				Thread.Sleep(6000);
				app.CloseGUI();
				app.Stop();
			});

			app.GUI();
		//	Thread.Sleep(6000);
		}

		[Test(), Order(2)]
		public async Task ShouldInvalidate()
		{
			App app = new App();


			//	app.Settings.EndpointOptions.EndpointOption = NBitcoinDerive.EndpointOptions.EndpointOptionsEnum.NoNetworking;
			app.Init();
			app.AddGenesisBlock();
			
	//		var _NewTx = Infrastructure.Testing.Utils.GetTx().AddOutput(app.GetUnusedKey().Address, Tests.zhash, 1);


			JsonLoader<Outputs>.Instance.Value.Values.ForEach(o => app.ImportKey(o.Key));

			app.Start();

			Task.Run(() =>
			{
				Types.Transaction tx;
				Thread.Sleep(1000);
				Assert.That(app.Spend(2, out tx), Is.True);
				Thread.Sleep(1000);

				var block = app.GenesisBlock.Value.Child().AddTx(tx);
				app.AddBlock(block);

				block = app.GenesisBlock.Value.Child();
				app.AddBlock(block);

				block = block.Child();
				Thread.Sleep(1000);

				app.AddBlock(block);
				Thread.Sleep(1000);
			});

			Task.Run(() =>
			{
				Thread.Sleep(5000);
				app.CloseGUI();
			});

			app.GUI();
			Thread.Sleep(1000); // sleep on main

			Task.Run(() =>
			{
				Thread.Sleep(5000);
				app.CloseGUI();
				app.Stop();
			});

			app.GUI();
		}
	}
}
