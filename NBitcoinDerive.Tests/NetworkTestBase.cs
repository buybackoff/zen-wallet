﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NBitcoin;
using NBitcoin.Protocol;
using NBitcoin.Protocol.Behaviors;
using NUnit.Framework;
using System.Linq;
using Infrastructure.Testing.Blockchain;

namespace NBitcoinDerive.Tests
{
	public abstract class NetworkTestBase
	{
		protected void WithBlockChains(int blockChains, byte[] genesisBlockHash, Action<BlockChain.BlockChain[]> action)
		{
			List<TestBlockChain> testBlockChains = new List<TestBlockChain>();

			for (int i = 0; i < blockChains; i++)
			{
				String dbName = "test-" + new Random().Next(0, 1000);
				testBlockChains.Add(new TestBlockChain(dbName, genesisBlockHash));
			}

			action(testBlockChains.Select(t => t.BlockChain).ToArray());

			foreach (var testBlockChain in testBlockChains)
			{
				testBlockChain.Dispose();
			}
		}

		protected void WithServerSet(int servers, Action<TestServerSet> action)
		{
			//		try
			//		{
			//	Task.Run(() =>
			//	{
			using (var Servers = new TestServerSet(servers))
			{
				action(Servers);
				Thread.Sleep(250);
			}
			//	}).Wait();
			//}
			//catch (Exception e)
			//{
			//	throw e;
			//}
		}

		//protected void WithChainMaintainingNodes(int count, Action<> action)
		//{
			
		//}

		protected Node Handshake(NodeServer originServer, NodeServer destServer)
		{
			Node node = Connect(originServer, destServer);

			node.Advertize = true;
			node.MyVersion.AddressFrom = originServer.ExternalEndpoint;
			node.VersionHandshake();

			Assert.AreEqual(NodeState.HandShaked, node.State);

			Trace.Information($"\n\nHandshaked from {originServer.ExternalEndpoint} to {destServer.ExternalEndpoint}\n\n");

			return node;
		}

		protected Node Connect(NodeServer originServer, NodeServer destServer)
		{
			NodeConnectionParameters nodeConnectionParameters = new NodeConnectionParameters();

			AddressManager addressManager = new AddressManager();

			AddressManagerBehavior addressManagerBehavior = new AddressManagerBehavior(addressManager);
			nodeConnectionParameters.TemplateBehaviors.Add(addressManagerBehavior);
			//	nodeConnectionParameters.AddressFrom = originServer.ExternalEndpoint;
			Node node = Node.Connect(destServer.Network, destServer.ExternalEndpoint, nodeConnectionParameters);

			Assert.AreEqual(NodeState.Connected, node.State);

			Trace.Information($"\n\nConncted from {originServer.ExternalEndpoint} to {destServer.ExternalEndpoint}\n\n");

			return node;
		}

		protected bool AddressManagerContains(Node node, IPEndPoint ipEndPoint)
		{
			return AddressManagerContains(AddressManagerBehavior.GetAddrman(node), ipEndPoint);
		}

		protected bool AddressManagerContains(NodeServer nodeServer, IPEndPoint ipEndPoint)
		{
			return AddressManagerContains(AddressManagerBehavior.GetAddrman(nodeServer.InboundNodeConnectionParameters.TemplateBehaviors), ipEndPoint);
		}

		private bool AddressManagerContains(AddressManager addressManager, IPEndPoint ipEndPoint)
		{
			foreach (NetworkAddress networkAddress in addressManager.GetAddr())
			{
				if (networkAddress.Endpoint.ToString() == ipEndPoint.ToString())
				{
					return true;
				}
			}

			return false;
		}
	}
}
