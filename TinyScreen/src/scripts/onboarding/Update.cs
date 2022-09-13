using System;
using System.Net.Http;
using ByteSizeLib;
using Godot;
using GodotOnReady.Attributes;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Framework.Extensions;

namespace TinyScreen.Scripts.Onboarding {
    public partial class Update : Control {
        
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

        private enum State {
            CheckingLatestVersion,
            NoInternet,
            Confirmation,
            Updating,
            Error,
            Install
        }

        private State _currentState = State.CheckingLatestVersion;
        
        [Inject] private IUpdateInterface _updateService;
        [Inject] private IHardwareService _hardwareService;

        
        [OnReady] public void BindEvents() {
            base._Ready();

            // Bind buttons
            _skipButton.Connect("pressed", this, nameof(OnSkipPress));
            _changelogButton.Connect("pressed", this, nameof(OnChangelogPress));
            _updateButton.Connect("pressed", this, nameof(OnUpdatePress));
            _tryAgainButton.Connect("pressed", this, nameof(OnTryAgainPress));
            
            SetState(_currentState);
        }

        private void SetState(State newState) {

            switch (newState) {
                case State.CheckingLatestVersion:
                    _subtitle.Text = "Checking is there is a new version of the application";
                    _skipButton.Hide();
                    _changelogButton.Hide();
                    _updateButton.Hide();
                    _versionsList.Hide();
                    _tryAgainButton.Hide();
                    _progressBar.Hide();
                    CheckLatestVersion();
                    break;
                
                case State.NoInternet:
                    _subtitle.Text = "Seems like you are not connected to internet";
                    _skipButton.Show();
                    _tryAgainButton.Show();
                    break;
                
                case State.Confirmation:
                    _subtitle.Text = "New version available, we strongly recommend to update";
                    _skipButton.Show();
                    _changelogButton.Show();
                    _updateButton.Show();
                    _versionsList.Show();
                    _tryAgainButton.Hide();
                    break;
                
                case State.Updating:
                    _subtitle.Text = "Downloading the latest version";
                    _skipButton.Hide();
                    _changelogButton.Hide();
                    _updateButton.Hide();
                    _versionsList.Hide();
                    _tryAgainButton.Hide();
                    _progressBar.Show();
                    DownloadUpdate();
                    break;
                
                case State.Error:
                    _subtitle.Text = "Something went wrong";
                    _progressBar.Hide();
                    _versionsList.Hide();
                    _skipButton.Show();
                    _tryAgainButton.Show();
                    break;

            }
            
            _currentState = newState;
        }

        private async void CheckLatestVersion() {
            
            if (!_hardwareService.IsOnline()) {
                SetState(State.NoInternet);
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
                        SetState(State.Confirmation);
                    }
                }
                catch (Exception) {
                    SetState(State.Error);
                }
               
                return;
            }
            
            // Latest version, nothing to do
            OnSkipPress();
        }

        private async void DownloadUpdate() {
            try {
                await _updateService.DownloadLatestUpdate((object sender, float progress) => {
                    _progressBar.Value = progress;
                });
                
                _updateService.Install();
            }
            catch (Exception) {
                SetState(State.Error);
            }
        }

        private void OnSkipPress() {
            GetParent<Onboarding>().NextStage();
        }

        private void OnUpdatePress() {
            SetState(State.Updating);
        }

        private void OnTryAgainPress() {
            SetState(State.CheckingLatestVersion);
        }

        private void OnChangelogPress() {
            // TODO implement some kind of UI for that
        }
        
    }
}