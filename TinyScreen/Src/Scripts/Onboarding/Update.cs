using System;
using ByteSizeLib;
using Godot;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Services;

namespace TinyScreen.Scripts.Onboarding; 

public partial class Update : BaseRouter {
    [Export] private Label _subtitle;
    [Export] private Label _updateSize;
    [Export] private Label _currentVersion;
    [Export] private Label _latestVersion;
    [Export] private Control _versionsList;
    [Export] private Button _skipButton;
    [Export] private Button _changelogButton;
    [Export] private Button _updateButton;
    [Export] private Button _tryAgainButton;
    [Export] private ProgressBar _progressBar;

    [Inject] private IUpdateService _updateService;
    [Inject] private IHardwareService _hardwareService;
    [Inject] private ModalService _modalService;

    public override partial void _Ready();
        
    [Ready]
    private void Start() {
        base._Ready();

        _skipButton.Pressed += async () => {
            if (await _modalService.Confirm(
                    "Are you sure you want to skip update?\nOutdated version might not work correctly!",
                    "Skip", "Back"))
                Navigate("/onboarding/library");
        };

        _changelogButton.Pressed += () => Navigate("changelog");
        _updateButton.Pressed += () => Navigate("download");
        _tryAgainButton.Pressed += () => Navigate("check");

    }


    [Route("check", true)]
    private async void Check(string path) {
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
        Navigate("/onboarding/library");
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
}