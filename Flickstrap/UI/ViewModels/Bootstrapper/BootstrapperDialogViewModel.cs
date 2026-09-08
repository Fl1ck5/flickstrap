using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shell;

using CommunityToolkit.Mvvm.Input;

namespace Flickstrap.UI.ViewModels.Bootstrapper
{
    public class BootstrapperDialogViewModel : NotifyPropertyChangedViewModel
    {
        private readonly IBootstrapperDialog _dialog;

        public ICommand CancelInstallCommand => new RelayCommand(CancelInstall);

        public string Title => App.Settings.Prop.BootstrapperTitle;
        // Use the dedicated high-res logo PNG only for the default Flickstrap icon selection -
        // that's the one case where we know exactly what the image should look like. For every
        // other selection (the classic Roblox-era icons, or a user's own uploaded custom icon),
        // respect what was actually picked rather than overriding it.
        public ImageSource Icon { get; set; } = App.Settings.Prop.BootstrapperIcon == BootstrapperIcon.IconFlickstrap
            ? new BitmapImage(new Uri("pack://application:,,,/Resources/FlickstrapLogo.png"))
            : App.Settings.Prop.BootstrapperIcon.GetIcon().GetImageSource();
        public string Message { get; set; } = "Please wait...";
        public bool ProgressIndeterminate { get; set; } = true;
        public int ProgressMaximum { get; set; } = 0;
        public int ProgressValue { get; set; } = 0;

        public TaskbarItemProgressState TaskbarProgressState { get; set; } = TaskbarItemProgressState.Indeterminate;
        public double TaskbarProgressValue { get; set; } = 0;

        public bool CancelEnabled { get; set; } = false;
        public Visibility CancelButtonVisibility => CancelEnabled ? Visibility.Visible : Visibility.Collapsed;

        [Obsolete("Do not use this! This is for the designer only.", true)]
        public BootstrapperDialogViewModel()
        {
            _dialog = null!;
        }

        public BootstrapperDialogViewModel(IBootstrapperDialog dialog)
        {
            _dialog = dialog;
        }

        private void CancelInstall()
        {
            _dialog.Bootstrapper?.Cancel();
            _dialog.CloseBootstrapper();
        }
    }
}
