﻿using System.Text.Json.Serialization;
using Gazillion;

namespace MHServerEmu.GameServer.Billing.Catalogs
{
    public class Catalog
    {
        private Dictionary<long, CatalogEntry> _entryDict;

        public long TimestampSeconds { get; set; }
        public long TimestampMicroseconds { get; set; }
        public CatalogEntry[] Entries { get; set; }
        public LocalizedCatalogUrls[] Urls { get; set; }
        public bool ClientMustDownloadImages { get; set; }

        [JsonConstructor]
        public Catalog(long timestampSeconds, long timestampMicroseconds, CatalogEntry[] entries, LocalizedCatalogUrls[] urls, bool clientMustDownloadImages)
        {
            TimestampSeconds = timestampSeconds;
            TimestampMicroseconds = timestampMicroseconds;
            Entries = entries;
            Urls = urls;
            ClientMustDownloadImages = clientMustDownloadImages;

            GenerateEntryDict();
        }

        public Catalog(NetMessageCatalogItems catalogItems)
        {
            TimestampSeconds = catalogItems.TimestampSeconds;
            TimestampMicroseconds = catalogItems.TimestampMicroseconds;
            Entries = catalogItems.EntriesList.Select(entry => new CatalogEntry(entry)).ToArray();
            Urls = catalogItems.UrlsList.Select(url => new LocalizedCatalogUrls(url)).ToArray();
            ClientMustDownloadImages = catalogItems.Clientmustdownloadimages;

            GenerateEntryDict();
        }

        public CatalogEntry GetEntry(long skuId)
        {
            if (_entryDict.TryGetValue(skuId, out CatalogEntry entry))
                return entry;
            else
                return null;
        }
        public NetMessageCatalogItems ToNetMessageCatalogItems(bool clientMustDownloadImages)
        {
            DateTimeOffset timestamp = (DateTimeOffset)DateTime.UtcNow;

            return NetMessageCatalogItems.CreateBuilder()
                .SetTimestampSeconds(timestamp.ToUnixTimeSeconds())
                .SetTimestampMicroseconds(timestamp.Millisecond * 1000)
                .AddRangeEntries(Entries.Select(entry => entry.ToNetStruct()))
                .AddRangeUrls(Urls.Select(url => url.ToNetStruct()))
                .SetClientmustdownloadimages(clientMustDownloadImages)
                .Build();
        }

        private void GenerateEntryDict()
        {
            _entryDict = new(Entries.Length);
            foreach (CatalogEntry entry in Entries)
                _entryDict.Add(entry.SkuId, entry);
        }
    }
}
