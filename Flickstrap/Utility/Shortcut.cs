using System.Windows;
using Flickstrap.Resources;

namespace Flickstrap.Utility
{
    internal static class Shortcut
    {
        private static GenericTriState _loadStatus = GenericTriState.Unknown;

        public static void Create(string exePath, string exeArgs, string lnkPath)
        {
            const string LOG_IDENT = "Shortcut::Create";

            try
            {
                // Always (re)write the shortcut rather than skipping when one already exists -
                // otherwise a stale shortcut from an older install (pointing at an old icon,
                // or before a rename/rebrand) sticks around forever and never gets refreshed.
                ShellLink.Shortcut.CreateShortcut(exePath, exeArgs, exePath, 0).WriteToFile(lnkPath);

                if (_loadStatus != GenericTriState.Successful)
                    _loadStatus = GenericTriState.Successful;
            }
            catch (Exception ex)
            {
                App.Logger.WriteLine(LOG_IDENT, $"Failed to create a shortcut for {lnkPath}!");
                App.Logger.WriteException(LOG_IDENT, ex);

                if (_loadStatus == GenericTriState.Failed)
                    return;

                _loadStatus = GenericTriState.Failed;

                Frontend.ShowMessageBox(Strings.Dialog_CannotCreateShortcuts, MessageBoxImage.Warning);
            }
        }
    }
}
