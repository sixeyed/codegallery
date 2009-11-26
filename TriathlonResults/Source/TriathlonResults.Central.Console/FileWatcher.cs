using System;
using System.Collections.Generic;
using System.Text;
using TriathlonResults.Central.Console.Config;
using System.IO;
using TriathlonResults.Central.Services.DAL;

namespace TriathlonResults.Central.Console.FileWatcher
{
    /// <summary>
    /// Configurable file watcher - submits files found to SQL db
    /// </summary>
    public static class FileWatcher
    {
        private static FileSystemWatcher watcher;

        public static void Start()
        {
            //fire up the watcher if enabled:
            if (FileWatcherConfiguration.Current.Enabled)
            {
                watcher = new FileSystemWatcher();
                watcher.Path = FileWatcherConfiguration.Current.Path;
                watcher.Filter = FileWatcherConfiguration.Current.Filter;
                watcher.Created += new FileSystemEventHandler(watcher_Created);
                watcher.EnableRaisingEvents = true;
                System.Console.WriteLine("FileWatcher enabled. Watching path: {0}, with filter: {1}", FileWatcherConfiguration.Current.Path, FileWatcherConfiguration.Current.Filter);
            }
            else
            {
                System.Console.WriteLine("FileWatcher not enabled.");
            }
        }

        public static void Stop()
        {
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Created -= watcher_Created;
                watcher.Dispose();
                watcher = null;
            }
        }

        static void watcher_Created(object sender, FileSystemEventArgs e)
        {
            System.Console.WriteLine("Found file: {0}", e.FullPath);
            //read in the file contents:
            string content = File.ReadAllText(e.FullPath);
            //validate:
            string[] contentParts = content.Split(FileWatcherConfiguration.Current.FieldDelimiter.ToCharArray());
            if (!IsValid(contentParts))
            {
                File.Move(e.FullPath, string.Format("{0}.invalid", e.FullPath));
            }
            else
            {
                try
                {
                    SetSectorTime set = new SetSectorTime();
                    set.RaceId = int.Parse(contentParts[0]);
                    set.SectorId = int.Parse(contentParts[1]);
                    set.AthleteId = int.Parse(contentParts[2]);
                    set.Duration = int.Parse(contentParts[5]);
                    set.Execute();
                    string processedFilePath = string.Format("{0}.processed", e.FullPath);
                    File.Delete(processedFilePath);
                    File.Move(e.FullPath, processedFilePath);
                    System.Console.WriteLine("Processed.");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Error: {0}", ex.Message);
                    string errorFilePath = string.Format("{0}.errored", e.FullPath);
                    File.Delete(errorFilePath);
                    File.Move(e.FullPath, errorFilePath);
                }
            }
        }

        private static bool IsValid(string[] contentParts)
        {
            bool valid = contentParts.Length == 6;
            int temp;
            valid = valid && int.TryParse(contentParts[0], out temp);
            valid = valid && int.TryParse(contentParts[1], out temp);
            valid = valid && int.TryParse(contentParts[2], out temp);
            valid = valid && int.TryParse(contentParts[5], out temp);
            return valid;
        }
    }
}
