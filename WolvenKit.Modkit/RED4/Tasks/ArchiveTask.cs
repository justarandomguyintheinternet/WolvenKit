using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WolvenKit.Common.Extensions;
using WolvenKit.RED4.Archive;

namespace CP77Tools.Tasks
{
    public partial class ConsoleFunctions
    {
        #region Methods

        public void ArchiveTask(string[] path, string pattern, string regex, bool diff, bool list)
        {
            if (path == null || path.Length < 1)
            {
                _loggerService.Warning("Please fill in an input path.");
                return;
            }

            Parallel.ForEach(path, file =>
            {
                ArchiveTaskInner(file, pattern, regex, diff, list);
            });
        }

        private void ArchiveTaskInner(string path, string pattern, string regex, bool diff, bool list)
        {
            if (!LoadArchives(path))
            {
                return;
            }

            foreach (var ar in _archiveManager.GetArchives())
            {
                // check search pattern then regex
                var finalmatches = ar.Files.Values.Cast<FileEntry>();
                if (!string.IsNullOrEmpty(pattern))
                {
                    finalmatches = ar.Files.Values.Cast<FileEntry>().MatchesWildcard(item => item.FileName, pattern);
                }

                if (!string.IsNullOrEmpty(regex))
                {
                    var searchTerm = new System.Text.RegularExpressions.Regex($@"{regex}");
                    var queryMatchingFiles =
                        from file in finalmatches
                        let matches = searchTerm.Matches(file.FileName)
                        where matches.Count > 0
                        select file;

                    finalmatches = queryMatchingFiles;
                }

                // list files in archive
                if (list)
                {
                    foreach (var finalmatch in finalmatches)
                    {
                        Console.WriteLine(finalmatch.Name);
                    }
                }

                //
                if (diff)
                {
                    var json = JsonSerializer.Serialize(ar, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        ReferenceHandler = ReferenceHandler.IgnoreCycles
                    });

                    Console.Write(json);
                }
            }
        }

        #endregion Methods
    }
}
