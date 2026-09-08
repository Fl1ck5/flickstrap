namespace Flickstrap.Utility
{
    // Clears out disk space that accumulates over time: cached Roblox package downloads
    // (kept around normally to speed up future installs/updates, at the cost of disk space)
    // and old Flickstrap log files. This is separate from CleanupVersionsFolder, which already
    // runs automatically and removes old Roblox client version folders - this instead targets
    // the download cache and logs specifically, which build up independently of that.
    public static class Cleaner
    {
        public readonly record struct CleanupResult(long BytesFreed, int FilesDeleted);

        public static CleanupResult RunCleanup(bool keepCurrentLog = true)
        {
            const string LOG_IDENT = "Cleaner::RunCleanup";

            long bytesFreed = 0;
            int filesDeleted = 0;

            // Roblox's own cached downloaded package files
            if (Directory.Exists(Paths.Downloads))
            {
                foreach (string file in Directory.GetFiles(Paths.Downloads))
                {
                    try
                    {
                        var info = new FileInfo(file);
                        long size = info.Length;
                        info.Delete();
                        bytesFreed += size;
                        filesDeleted++;
                    }
                    catch (Exception ex)
                    {
                        App.Logger.WriteLine(LOG_IDENT, $"Failed to delete cached download '{file}'");
                        App.Logger.WriteException(LOG_IDENT, ex);
                    }
                }
            }

            // Flickstrap's own old log files
            if (Directory.Exists(Paths.Logs))
            {
                string? currentLogFile = keepCurrentLog ? App.Logger.FileLocation : null;

                foreach (string file in Directory.GetFiles(Paths.Logs))
                {
                    if (currentLogFile is not null && string.Equals(file, currentLogFile, StringComparison.OrdinalIgnoreCase))
                        continue;

                    try
                    {
                        var info = new FileInfo(file);
                        long size = info.Length;
                        info.Delete();
                        bytesFreed += size;
                        filesDeleted++;
                    }
                    catch (Exception ex)
                    {
                        App.Logger.WriteLine(LOG_IDENT, $"Failed to delete log file '{file}'");
                        App.Logger.WriteException(LOG_IDENT, ex);
                    }
                }
            }

            App.Logger.WriteLine(LOG_IDENT, $"Cleanup freed {bytesFreed} bytes across {filesDeleted} files");

            return new CleanupResult(bytesFreed, filesDeleted);
        }

        public static bool IsCleanupDue()
        {
            var frequency = App.Settings.Prop.CleanerFrequency;

            if (frequency == CleanerFrequency.Never)
                return false;

            DateTime? last = App.State.Prop.LastCleanupTime;

            if (last is null)
                return true;

            TimeSpan interval = frequency switch
            {
                CleanerFrequency.Daily => TimeSpan.FromDays(1),
                CleanerFrequency.Weekly => TimeSpan.FromDays(7),
                CleanerFrequency.Monthly => TimeSpan.FromDays(30),
                _ => TimeSpan.MaxValue
            };

            return DateTime.Now - last.Value >= interval;
        }

        public static void RunScheduledCleanupIfDue()
        {
            const string LOG_IDENT = "Cleaner::RunScheduledCleanupIfDue";

            if (!IsCleanupDue())
                return;

            App.Logger.WriteLine(LOG_IDENT, $"Running scheduled cleanup (frequency: {App.Settings.Prop.CleanerFrequency})");

            RunCleanup();

            App.State.Prop.LastCleanupTime = DateTime.Now;
            App.State.Save();
        }
    }
}
