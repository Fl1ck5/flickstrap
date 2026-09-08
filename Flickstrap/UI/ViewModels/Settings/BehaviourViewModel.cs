using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

namespace Flickstrap.UI.ViewModels.Settings
{
    public class BehaviourViewModel : NotifyPropertyChangedViewModel
    {
        public bool ConfirmLaunches
        {
            get => App.Settings.Prop.ConfirmLaunches;
            set => App.Settings.Prop.ConfirmLaunches = value;
        }

        public bool DisableRobloxCrashHandler
        {
            get => App.Settings.Prop.DisableRobloxCrashHandler;
            set => App.Settings.Prop.DisableRobloxCrashHandler = value;
        }

        public IEnumerable<CleanerFrequency> CleanerFrequencies { get; } = Enum.GetValues(typeof(CleanerFrequency)).Cast<CleanerFrequency>();

        public CleanerFrequency CleanerFrequency
        {
            get => App.Settings.Prop.CleanerFrequency;
            set => App.Settings.Prop.CleanerFrequency = value;
        }

        public ICommand CleanNowCommand => new RelayCommand(CleanNow);

        private void CleanNow()
        {
            var result = Cleaner.RunCleanup();
            App.State.Prop.LastCleanupTime = DateTime.Now;
            App.State.Save();

            double mb = result.BytesFreed / 1024.0 / 1024.0;
            Frontend.ShowMessageBox(
                $"Cleaned up {mb:0.#} MB across {result.FilesDeleted} file(s).",
                MessageBoxImage.Information
            );
        }

        public bool BackgroundUpdates
        {
            get => App.Settings.Prop.BackgroundUpdatesEnabled;
            set => App.Settings.Prop.BackgroundUpdatesEnabled = value;
        }

        public bool IsRobloxInstallationMissing => !App.IsPlayerInstalled && !App.IsStudioInstalled;

        public bool ForceRobloxReinstallation
        {
            get => App.State.Prop.ForceReinstall || IsRobloxInstallationMissing;
            set => App.State.Prop.ForceReinstall = value;
        }
    }
}
