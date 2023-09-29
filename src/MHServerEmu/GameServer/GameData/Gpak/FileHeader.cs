﻿namespace MHServerEmu.GameServer.GameData.Gpak
{
    public readonly struct FileHeader
    {
        public string Magic { get; }    // File signature
        public byte Version { get; }    // 10 for 1.9 and 1.10, 11 for most "modern" versions (at least 1.23+)

        public FileHeader(string magic, byte version)
        {
            Magic = magic;
            Version = version;
        }
    }
}
