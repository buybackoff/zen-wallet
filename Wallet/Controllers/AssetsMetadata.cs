﻿﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using BlockChain.Data;
using Infrastructure;
using Newtonsoft.Json;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;
using Wallet.core;

namespace Wallet
{
	public class AssetMetadata
	{
		public byte[] Asset;
		public string Display;
	}

	class AssetMetadataJson
	{
		public string name;

		//TODO: image and versioning
		//public string imageUrl;
		//public string version;
	}

    public class AssetsMetadata
    {
        static class Utils
        {
            public static string Config(string key) { return ConfigurationManager.AppSettings.Get(key); }
            public static string PathCombine(params string[] values)
            {
                var value = Path.Combine(values);
                if (!Directory.Exists(Path.GetDirectoryName(value)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(value));
                }
                return value;
            }
        }

        readonly ConcurrentDictionary<string, AssetMetadata> _Cache = new ConcurrentDictionary<string, AssetMetadata>();
        readonly JsonLoader<Dictionary<string, AssetMetadataJson>> _CacheJsonStore = JsonLoader<Dictionary<string, AssetMetadataJson>>.Instance;
        public static readonly string ZEN = "Zen";

        public event Action<AssetMetadata> AssetMatadataChanged;

        public AssetsMetadata(WalletManager wallet)
        {
            try
            {
                string directory = Utils.PathCombine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), Utils.Config("assetsDir"));
                _CacheJsonStore.FileName = Utils.PathCombine(directory, "metadata.json");
                _CacheJsonStore.Value.ToList().ForEach(t => _Cache.TryAdd(t.Key, new AssetMetadata() { Asset = Convert.FromBase64String(t.Key), Display = t.Value.name }));
                _Cache.TryAdd(Convert.ToBase64String(Consensus.Tests.zhash), new AssetMetadata() { Asset = Consensus.Tests.zhash, Display = ZEN });

				wallet.OnItems += OnItems;
			} catch (Exception e)
            {
                InfrastructureTrace.Error("AssetsMetadata ctor", e);
            }
        }

		void OnItems(List<TxDelta> txDeltas)
		{
            foreach (var txDelta in txDeltas)
            {
                foreach (var asset in txDelta.AssetDeltas.Keys)
                {
                    var key = Convert.ToBase64String(asset);

                    if (!_Cache.ContainsKey(key))
                    {
                        GetMetadata(key, asset);
                    }
                }
            }
		}

		//public async Task GetMetadata(byte[] asset)
		//{
		//    var key = Convert.ToBase64String(asset);

		//    if (_Cache.ContainsKey(key))
		//        return;

		//    var assetMetadata = new AssetMetadata() { Asset = asset, Display = key };

		//    _Cache.TryAdd(key, assetMetadata);
		//    AssetMatadataChanged(assetMetadata); // firstly, with initial Display
		//    await GetAssetMatadataAsync(assetMetadata);
		//    AssetMatadataChanged(assetMetadata); // again, now with updated Display
		//}

		public void GetMetadata(string key, byte[] asset)
        {
            Task.Factory.StartNew(async () => {
				var assetMetadata = new AssetMetadata() { Asset = asset, Display = key };

				_Cache.TryAdd(key, assetMetadata);

				await GetAssetMatadataAsync(assetMetadata);

				try
				{
					if (AssetMatadataChanged != null)
						AssetMatadataChanged(assetMetadata);
				}
				catch (Exception e)
				{
					InfrastructureTrace.Information($"Error invoking subscriber: {e.Message}");
				}
            });
        }

        public ICollection<AssetMetadata> GetAssetMatadataList()
        {
            return _Cache.Values;
        }

        public string TryGetValue(byte[] asset)
        {
            var key = Convert.ToBase64String(asset);

            if (_Cache.ContainsKey(key))
            {
                return _Cache[key].Display;
            }
            else
            {
                GetMetadata(key, asset);
                return key;
            }
        }

        object _lock = new object();

        async Task GetAssetMatadataAsync(AssetMetadata assetMetadata)
        {
            var uri = new Uri(string.Format($"http://{Utils.Config("assetsDiscovery")}/AssetMetadata/Index/" + HttpServerUtility.UrlTokenEncode(assetMetadata.Asset)));
			
            InfrastructureTrace.Information($"Loading asset metadata from web: {uri.AbsoluteUri}");
            using (var response = await new HttpClient().GetAsync(uri.AbsoluteUri))
            {
				if (response.IsSuccessStatusCode)
                {
                    var remoteString = await response.Content.ReadAsStringAsync();

					try
					{
						//Version currentVersion = null;
						//Version remoteVersion = null;

						var remoteJson = JsonConvert.DeserializeObject<AssetMetadataJson>(remoteString);

                        //currentVersion = _Cache.Value.ContainsKey(_hash) ? new Version(_Cache.Value[_hash].version) : new Version();
                        //remoteVersion = remoteJson.version == null ? new Version() : new Version(remoteJson.version);

                        //TODO: check if newer version
                        //if (remoteVersion > currentVersion)

                        //TODO: fetch image

                        if (string.IsNullOrWhiteSpace(remoteJson.name))
                        {
                           //assetMetadata.Display += " (No metadada)";
                        }
                        else
                        {
                            lock (_lock)
                            {
                                assetMetadata.Display = remoteJson.name;
                                _CacheJsonStore.Value[Convert.ToBase64String(assetMetadata.Asset)] = remoteJson;
                                _CacheJsonStore.Save();
                            }
						}
					}
					catch (Exception e)
					{

                        InfrastructureTrace.Information($"Error fetching asset metadata from url: {uri.AbsoluteUri}");
                        assetMetadata.Display += " (Metadata error)";
					}
                }
                else
                {
                    assetMetadata.Display += " (Network error)";
                }
            }
        }
    }

    //async Task ProcessImageAsync(byte[] hash, AssetMetadata metadata)
    //{
    //    var imageFile = LocalImage(hash, metadata.imageUrl);

    //    using (var response = await _HttpClient.GetAsync(metadata.imageUrl))
    //    {
    //        response.EnsureSuccessStatusCode();

    //        using (var inputStream = await response.Content.ReadAsStreamAsync())
    //        {
    //            using (var fileStream = File.Create(imageFile))
    //            {
    //                inputStream.CopyTo(fileStream);
    //            }
    //        }
    //    }

    //    WalletTrace.Information($"Image downloaded for asset: {metadata.name}");
    //    Update(hash, metadata.name, imageFile);
    //}

    //void Update(byte[] hash, string name, string image)
    //{
    //    this[hash] = new AssetType(name, image);

    //    if (AssetChanged != null)
    //    {
    //        AssetChanged(hash);
    //    }
    //}
}