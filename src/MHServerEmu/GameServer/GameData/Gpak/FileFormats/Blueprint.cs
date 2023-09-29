﻿using MHServerEmu.Common.Extensions;

namespace MHServerEmu.GameServer.GameData.Gpak.FileFormats
{
    public class Blueprint
    {
        public FileHeader Header { get; }                               // BPT
        public string RuntimeBinding { get; }                           // Name of the C++ class that handles prototypes that use this blueprint
        public ulong DefaultPrototypeId { get; }                        // .defaults prototype file id
        public BlueprintReference[] Parents { get; }
        public BlueprintReference[] ContributingBlueprints { get; }
        public BlueprintMember[] Members { get; }                       // Field definitions for prototypes that use this blueprint  

        public Blueprint(byte[] data)
        {
            using (MemoryStream stream = new(data))
            using (BinaryReader reader = new(stream))
            {
                Header = reader.ReadHeader();
                RuntimeBinding = reader.ReadFixedString16();
                DefaultPrototypeId = reader.ReadUInt64();

                Parents = new BlueprintReference[reader.ReadUInt16()];
                for (int i = 0; i < Parents.Length; i++)
                    Parents[i] = new(reader);

                ContributingBlueprints = new BlueprintReference[reader.ReadInt16()];
                for (int i = 0; i < ContributingBlueprints.Length; i++)
                    ContributingBlueprints[i] = new(reader);

                Members = new BlueprintMember[reader.ReadUInt16()];
                for (int i = 0; i < Members.Length; i++)
                    Members[i] = new(reader);
            }
        }

        public BlueprintMember GetMember(ulong id)
        {
            return Members.First(member => member.FieldId == id);
        }
    }

    public class BlueprintReference
    {
        public ulong Id { get; }
        public byte Field1 { get; }

        public BlueprintReference(BinaryReader reader)
        {
            Id = reader.ReadUInt64();
            Field1 = reader.ReadByte();
        }
    }

    public class BlueprintMember
    {
        public ulong FieldId { get; }
        public string FieldName { get; }
        public CalligraphyValueType ValueType { get; }
        public CalligraphyContainerType ContainerType { get; }
        public ulong Subtype { get; }

        public BlueprintMember(BinaryReader reader)
        {
            FieldId = reader.ReadUInt64();
            FieldName = reader.ReadFixedString16();
            ValueType = (CalligraphyValueType)reader.ReadByte();
            ContainerType = (CalligraphyContainerType)reader.ReadByte();

            switch (ValueType)
            {
                // Only these types have subtypes
                case CalligraphyValueType.A:
                case CalligraphyValueType.C:
                case CalligraphyValueType.P:
                case CalligraphyValueType.R:
                    Subtype = reader.ReadUInt64();
                    break;
            }
        }
    }
}
