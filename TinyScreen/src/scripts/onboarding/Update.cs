using System;
using ByteSizeLib;
using Godot;
using Godot.Collections;
using GodotOnReady.Attributes;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Services;

namespace TinyScreen.Scripts.Onboarding {
    public partial class Update : BaseRouter {
        
        [OnReadyGet] private Label _subtitle;
        [OnReadyGet] private Label _updateSize;
        [OnReadyGet] private Label _currentVersion;
        [OnReadyGet] private Label _latestVersion;
        [OnReadyGet] private Control _versionsList;
        [OnReadyGet] private Button _skipButton;
        [OnReadyGet] private Button _changelogButton;
        [OnReadyGet] private Button _updateButton;
        [OnReadyGet] private Button _tryAgainButton;
        [OnReadyGet] private ProgressBar _progressBar;

        [Inject] private IUpdateService _updateService;
        [Inject] private IHardwareService _hardwareService;
        [Inject] private ModalService _modalService;

        [OnReady] public void BindEvents() {
            base._Ready();

            // Bind buttons
            _skipButton.Connect("pressed", this, nameof(OnSkipPress));
            _changelogButton.Connect("pressed", this, nameof(OnChangelogPress));
            _updateButton.Connect("pressed", this, nameof(OnUpdatePress));
            _tryAgainButton.Connect("pressed", this, nameof(OnTryAgainPress));
        }
        
        [Route("check", true)]
        private async void CheckLatestVersion(string path) {
            
            // Update UI
            _subtitle.Text = "Checking is there is a new version of the application";
            _skipButton.Hide();
            _changelogButton.Hide();
            _updateButton.Hide();
            _versionsList.Hide();
            _tryAgainButton.Hide();
            _progressBar.Hide();
            
            if (!_hardwareService.IsOnline()) {
                Navigate("error", "Seems like you are not connected to internet");
                return;
            }

            // Compare versions
            Version latest = await _updateService.GetLatestVersion();
            Version current = _updateService.GetCurrentVersion();

            if (latest > current) {
                _currentVersion.Text = current.ToString();
                _latestVersion.Text = latest.ToString();

                // Show size of the update
                try {
                    var updateSize = await _updateService.GetUpdateSize();

                    if (updateSize != null) {
                        _updateSize.Text = ByteSize.FromBytes(updateSize.Value).ToString();
                        Navigate("confirm");
                    }
                }
                catch (Exception) {
                    Navigate("error", "Something went wrong");
                }
               
                return;
            }
            
            // Latest version, nothing to do
            OnSkipPress();
        }

        [Route("confirm")]
        private void Confirm(string path) {
            _subtitle.Text = "New version available, we strongly recommend to update";
            _skipButton.Show();
            _changelogButton.Show();
            _updateButton.Show();
            _versionsList.Show();
            _tryAgainButton.Hide();
        }

        [Route("error")]
        private async void Error(string path, string message = "Something went wrong") {
            _subtitle.Text = message;
            _progressBar.Hide();
            _versionsList.Hide();
            _skipButton.Show();
            _tryAgainButton.Show();
        }
        
        [Route("download")]
        private async void DownloadUpdate(string path) {
            
            // UI
            _subtitle.Text = "Downloading the latest version";
            _skipButton.Hide();
            _changelogButton.Hide();
            _updateButton.Hide();
            _versionsList.Hide();
            _tryAgainButton.Hide();
            _progressBar.Show();
            
            try {
                await _updateService.DownloadLatestUpdate((sender, progress) => _progressBar.Value = progress);
                _updateService.Install();
            }
            catch (Exception) {
                Navigate("error", "Error during downloading");
            }
        }

        private async void OnSkipPress() {
            if (await _modalService.Confirm(
                "Are you sure you want to skip update?\nOutdated version might not work correctly!",
                "Skip", "Back"))
                GetParent<BaseRouter>().Navigate("library");
        }

        private void OnUpdatePress() {
            Navigate("download");
        }

        private void OnTryAgainPress() {
            Navigate("check");
        }

        private void OnChangelogPress() {
            // TODO implement some kind of UI for that
            Navigate("changelog");
        }
        
    }
}