using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Splat;
using WolvenKit.Common.FNV1A;
using WolvenKit.Core.Extensions;
using WolvenKit.RED4.Archive.CR2W;
using WolvenKit.RED4.IO;
using WolvenKit.RED4.Types;
using WolvenKit.RED4.Types.Compression;
using WolvenKit.RED4.Types.Exceptions;
using WolvenKit.Common.Services;

namespace WolvenKit.RED4.Archive.IO
{
    public partial class PackageReader
    {
        private PackageHeader header;
        private IHashService _hashService;

        public EFileReadErrorCodes ReadPackage(RedBuffer buffer)
        {
            _hashService = Locator.Current.GetService<IHashService>();

            var _chunks = new List<IRedClass>();
            _outputFile = buffer;

            var version = BaseReader.ReadInt16();
            BaseStream.Position -= 2;
            if (version != 4)
            {
                return EFileReadErrorCodes.UnsupportedVersion;
            }

            header = BaseStream.ReadStruct<PackageHeader>();
            buffer.Version = header.uk1;

            if (header.refPoolDescOffset != 0)
            {
                return EFileReadErrorCodes.NoCr2w;
            }

            if (header.numCruids0 != header.numCruids1)
            {
                return EFileReadErrorCodes.NoCr2w;
            }

            for (var i = 0; i < header.numCruids0; i++)
            {
                buffer.Cruids.Add(_reader.ReadUInt64());
            }

            var baseOff = BaseStream.Position;

            // read refs
            var refCount = (header.refPoolDataOffset - header.refPoolDescOffset) / 4;
            BaseStream.Position = baseOff + header.refPoolDescOffset;
            var refDesc = BaseStream.ReadStructs<PackageImportHeader>(refCount);

            foreach (var r in refDesc)
            {
                BaseStream.Position = baseOff + r.offset;
                importsList.Add(ReadImport(r));
            }

            // read strings
            var nameCount = (header.namePoolDataOffset - header.namePoolDescOffset) / 4;
            BaseStream.Position = baseOff + header.namePoolDescOffset;
            var nameDesc = BaseStream.ReadStructs<PackageNameHeader>(nameCount);

            foreach (var s in nameDesc)
            {
                BaseStream.Position = baseOff + s.offset;
                _namesList.Add(ReadName(s));
            }

            // read chunks
            var chunkCount = (header.chunkDataOffset - header.chunkDescOffset) / 8;
            BaseStream.Position = baseOff + header.chunkDescOffset;
            var chunkDesc = BaseStream.ReadStructs<PackageChunkHeader>(chunkCount);

            foreach (var c in chunkDesc)
            {
                BaseStream.Position = baseOff + c.offset;
                _chunks.Add(ReadChunk(c));
            }

            buffer.SetChunks(_chunks);

            buffer.IsPackage = true;

            return EFileReadErrorCodes.NoError;
        }

        private PackageImport ReadImport(PackageImportHeader r)
        {
            // needs header offset
            //Debug.Assert(BaseStream.Position == r.offset);

            var import = new PackageImport()
            {
                Flags = (InternalEnums.EImportFlags)(r.unk1 ? 0b10 : 0b00)
            };
            if (header.uk2 == 0)
            {
                var bytes = _reader.ReadBytes(r.size);
                import.DepotPath = Encoding.UTF8.GetString(bytes.ToArray());
                import.Hash = FNV1A64HashAlgorithm.HashString(import.DepotPath);
            }
            else
            {
                import.Hash = _reader.ReadUInt64();
                import.DepotPath = _hashService.Get(import.Hash);
            }
            return import;
        }
        private string ReadName(PackageNameHeader n)
        {
            var s = _reader.ReadNullTerminatedString();
            //Debug.Assert(s.Length == n.size);
            return s;
        }

        private IRedClass ReadChunk(PackageChunkHeader c)
        {
            // needs header offset
            //Debug.Assert(BaseStream.Position == c.offset);
            var redTypeName = GetStringValue((ushort)c.typeID);
            var (type, _) = RedReflection.GetCSTypeFromRedType(redTypeName);
            if (type == null)
            {
                throw new TypeNotFoundException(redTypeName);
            }

            return ReadClass(type);
        }
    }

    public class PackageImport : IRedImport
    {
        public string DepotPath { get; set; }

        public ulong Hash { get; set; }
        public InternalEnums.EImportFlags Flags { get; set;  }
    }
}