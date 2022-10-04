using System;
using System.Collections.Generic;
using System.IO;
using WolvenKit.Common.FNV1A;
using WolvenKit.Common.Model.Arguments;
using WolvenKit.RED4.Archive;
using WolvenKit.RED4.Archive.CR2W;
using WolvenKit.RED4.Archive.IO;
using WolvenKit.RED4.Types;
using WolvenKit.RED4.Types.Exceptions;

namespace WolvenKit.Modkit.RED4;

public partial class ModTools
{
    private enum FindFileResult
    {
        NoError,
        FileNotFound,
        NoCR2W
    }

    private class FindFileEntry
    {
        public CR2WFile File { get; }
        public List<IRedImport> Imports { get; }

        public FindFileEntry(CR2WFile file, List<IRedImport> imports)
        {
            File = file;
            Imports = imports;
        }
    }

    private FindFileResult TryFindFile(CName path, out FindFileEntry result, bool includeCustomArchives = true)
    {
        var status = InternalTryFindFile(path, out result, includeCustomArchives);

        var pathStr = path.GetResolvedText();
        if (status == FindFileResult.FileNotFound)
        {
            if (string.IsNullOrEmpty(pathStr))
            {
                _loggerService?.Warning($"The file with the hash \"{(ulong)path}\" could not be found!");
            }
            else
            {
                _loggerService?.Warning($"The file \"{pathStr}\" could not be found!");
            }
        }

        if (status == FindFileResult.NoCR2W)
        {
            if (string.IsNullOrEmpty(pathStr))
            {
                _loggerService?.Error($"The file with the hash \"{(ulong)path}\" could not be parsed!");
            }
            else
            {
                _loggerService?.Error($"The file \"{pathStr}\" could not be parsed!");
            }
        }

        return status;
    }

    private FindFileResult InternalTryFindFile(ulong hash, out FindFileEntry result, bool includeCustomArchives = true)
    {
        result = null;

        foreach (var archive in _archiveManager.GetArchives(includeCustomArchives, includeCustomArchives))
        {
            if (archive.Files.TryGetValue(hash, out var gameFile))
            {
                var ms = new MemoryStream();
                gameFile.Extract(ms);
                ms.Seek(0, SeekOrigin.Begin);

                using var reader = new CR2WReader(ms);
                reader.ParsingError += args => args is InvalidDefaultValueEventArgs;

                if (reader.ReadFile(out var file) != EFileReadErrorCodes.NoError)
                {
                    return FindFileResult.NoCR2W;
                }

                result = new FindFileEntry(file, reader.ImportsList);

                return FindFileResult.NoError;
            }
        }

        return FindFileResult.FileNotFound;
    }

    private bool UncookFile(string path, string matRepo, GlobalExportArgs args, bool excludeCustomArchives = false)
    {
        return UncookFile(FNV1A64HashAlgorithm.HashString(path), matRepo, args, excludeCustomArchives);
    }

    private bool UncookFile(ulong hash, string matRepo, GlobalExportArgs args, bool excludeCustomArchives = false)
    {
        foreach (var archive in _archiveManager.Archives.Items)
        {
            if (excludeCustomArchives && archive is not Archive)
            {
                continue;
            }

            if (archive.Files.TryGetValue(hash, out var gameFile))
            {
                var di = new DirectoryInfo(matRepo);
                if (!di.Exists)
                {
                    di.Create();
                }

                return UncookSingle((ICyberGameArchive)archive, gameFile.Key, di, args);
            }
        }

        return false;
    }
}
