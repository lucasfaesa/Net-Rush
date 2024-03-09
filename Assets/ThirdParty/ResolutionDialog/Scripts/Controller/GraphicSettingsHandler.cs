using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using TMPro;

#pragma warning disable 0649

namespace NewResolutionDialog.Scripts.Controller
{
    public class GraphicSettingsHandler : MonoBehaviour
    {
        #region Fields and Stuff
        [SerializeField] TMP_Dropdown resolution;
        [SerializeField] TMP_Dropdown fullScreenMode;
        [SerializeField] TMP_Dropdown vSync;
        [SerializeField] TextMeshProUGUI vSyncNote;
        [SerializeField] TMP_Dropdown quality;

        static readonly string hzSuffix = " Hz";

        Dictionary<string, List<string>> refreshRates = new Dictionary<string, List<string>>();
        private List<DisplayInfo> displayInfos = new List<DisplayInfo>();
        bool updatingDialog = true;
        bool movingToDisplay = false;
        #endregion

        #region Dialog Getters
        string GetSelectedResolution()
        {
            return resolution.options[resolution.value].text;
        }
        string GetResolutionString()
        {
            return GetResolutionString(Screen.width, Screen.height);
        }
        string GetResolutionString(int w, int h)
        {
            return string.Format("{0}x{1}", w, h);
        }

        FullScreenMode GetSelectedFullScreenMode()
        {
            var mode = (FullScreenMode)fullScreenMode.value;

            // In the dropdown "MaximizedWindow" (#2 in enum) was removed because it ain't working, it is now "Windowed" (#3 in enum), hence this fix
            // see: https://issuetracker.unity3d.com/issues/fullscreen-mode-maximized-window-functionality-is-broken-and-any-built-player-changes-to-non-window-mode-when-maximizing
            if (mode == FullScreenMode.MaximizedWindow)
                mode = FullScreenMode.Windowed;

            return mode;
        }
        #endregion

        #region Debug
        void LogRefreshRates()
        {
            var sb = new StringBuilder("\n");
            foreach (var kvp in refreshRates)
            {
                sb.AppendFormat("{0} => ", kvp.Key);
                foreach (var hz in kvp.Value)
                {
                    sb.AppendFormat("{0}, ", hz);
                }
                sb.Append("\n");
            }

            sb.Append("\n");
            Debug.Log(sb);
        }
        #endregion

        #region Unity Startup
        void Awake()
        {
            #if UNITY_WEBGL
                resolution.transform.parent.gameObject.SetActive(false);
                fullScreenMode.transform.parent.gameObject.SetActive(false);
            #endif
            vSyncNote.gameObject.SetActive(false);

            /*var hz = PlayerPrefs.GetInt(prefsKey_RefreshRate, 0);
            if (hz != 0)
            {
                if (hz != Screen.currentResolution.refreshRate)
                {
                    SetResolution(Screen.width, Screen.height, Screen.fullScreenMode, PlayerPrefs.GetInt(prefsKey_RefreshRate, 0));
                    UpdateDialogAfterEndOfFrame();
                }
            }

            var vSyncCount = PlayerPrefs.GetInt(prefsKey_VSyncCount, QualitySettings.vSyncCount);
            if (vSyncCount != QualitySettings.vSyncCount)
                QualitySettings.vSyncCount = PlayerPrefs.GetInt(prefsKey_VSyncCount, QualitySettings.vSyncCount);*/
        }

        void OnEnable()
        {
            PopulateDropdowns();
            ApplyCurrentSettingsToDialog();
            UpdateDialogInteractability();
        }
        #endregion

        #region PopulateDropdowns
        void PopulateDropdowns()
        {
            PopulateResolutionsDropdown();
            PopulateQualityDropdown();
        }

        void PopulateResolutionsDropdown()
        {
            refreshRates.Clear();
            resolution.ClearOptions();
            var options = new List<TMP_Dropdown.OptionData>();
            var resolutions = Screen.resolutions;
            var isWindowed = Screen.fullScreenMode == FullScreenMode.Windowed;
            var isFullScreenWindow = Screen.fullScreenMode == FullScreenMode.FullScreenWindow;
            var systemWidth = Display.main.systemWidth;
            var systemHeight = Display.main.systemHeight;

            foreach (var res in resolutions)
            {
                var resParts = res.ToString().Split(new char[] { 'x', '@', 'H' });
                var width = int.Parse(resParts[0].Trim());
                var height = int.Parse(resParts[1].Trim());

                // skip resolutions that won't fit in windowed modes
                if (isWindowed && (width >= systemWidth || height >= systemHeight))
                    continue;
                if (isFullScreenWindow && (width > systemWidth || height > systemHeight))
                    continue;

                // resolution
                var resString = GetResolutionString(width, height);
                if (refreshRates.ContainsKey(resString) == false)
                {
                    refreshRates.Add(resString, new List<string>());
                    var option = new TMP_Dropdown.OptionData(resString);
                    options.Add(option);
                }

                // refresh rates without 'Hz' suffix
                var hz = resParts[2].Trim();

                refreshRates[resString].Add(hz);
            }

            resolution.AddOptions(options);
        }

        void PopulateQualityDropdown()
        {
            var options = new List<TMP_Dropdown.OptionData>();

            TMP_Dropdown.OptionData currentOption = null;
            var currentLevel = QualitySettings.GetQualityLevel();
            var qualityLevels = QualitySettings.names;
            foreach (var quality in qualityLevels)
            {
                var option = new TMP_Dropdown.OptionData(quality);
                options.Add(option);

                // remember initial quality level
                if (quality == qualityLevels[currentLevel])
                    currentOption = option;
            }

            quality.ClearOptions();
            quality.AddOptions(options);
        }


        /// <summary>
        /// Populate the display dropdown and select the currently active display
        /// </summary>

        #endregion

        #region UpdateDialogWithCurrentSettings
        void ApplyCurrentSettingsToDialog()
        {
            updatingDialog = true;

            SelectCurrentResolutionDropdownItem();
            SelectCurrentFullScreenModeDropdownItem();
            SelectCurrentVSyncCountDropdownItem();
            SelectCurrentQualityLevelDropdownItem();

            updatingDialog = false;
        }

        void SelectCurrentResolutionDropdownItem()
        {
            // select highest by default
            resolution.value = resolution.options.Count - 1;

            var res = GetResolutionString();
            for (int i = 0; i < resolution.options.Count; i++)
            {
                if (resolution.options[i].text == res)
                {
                    resolution.value = i;
                    break;
                }
            }
        }

        void SelectCurrentFullScreenModeDropdownItem()
        {
            var mode = Screen.fullScreenMode;
            if (Screen.fullScreenMode == FullScreenMode.MaximizedWindow)
                mode = FullScreenMode.FullScreenWindow;

            fullScreenMode.value = (int)mode;
        }

        void SelectCurrentVSyncCountDropdownItem()
        {
            vSync.value = QualitySettings.vSyncCount;

            // push a fair warning to users disabling vsync ignoring freesync/gsync users for whom this setting makes sense
            // in the majority of use cases, particularly on battery-powered devices, turning vsync off is detrimental to battery life and comfort (excess heat, noise)
            vSyncNote.gameObject.SetActive(QualitySettings.vSyncCount == 0);
        }

        void SelectCurrentQualityLevelDropdownItem()
        {
            quality.value = QualitySettings.GetQualityLevel();
        }
        #endregion

        #region UpdateInteractability
        void UpdateDialogInteractability()
        {
            if (Application.isEditor)
            {
                // in editor mode these settings are not applicable, some can be changed through the game view's settings (ie resolution)
                resolution.interactable = false; // change this through game view
                fullScreenMode.interactable = false; // not applicable, always "windowed"
                vSync.interactable = false; // not applicable, vsync has no effect in editor mode
                quality.interactable = quality.options.Count > 1; // interactable if there is more than one quality level to select from
            }
            else
            {
                resolution.interactable = true; // always interactable
                fullScreenMode.interactable = true; // always interactable
                vSync.interactable = true; // always interactable
                quality.interactable = quality.options.Count > 1; // interactable if there is more than one quality level to select from
            }
        }

        #endregion

        #region Event Handlers
        public void OnResolutionChanged()
        {
            if (updatingDialog)
                return;

            ApplySelectedResolution();
            UpdateDialogAfterEndOfFrame();
        }

        public void OnFullScreenModeChanged()
        {
            if (updatingDialog)
                return;

            var wasWindowed = Screen.fullScreenMode == FullScreenMode.Windowed;

            var mode = GetSelectedFullScreenMode();
            Screen.fullScreenMode = mode;
            PlayerPrefs.SetInt("windowMode", (int)mode);

            if (mode == FullScreenMode.Windowed)
            {
                var selectedRes = GetSelectedResolution();
                var resolution = selectedRes.Split(new char[] { 'x' });
                var width = int.Parse(resolution[0]);
                var height = int.Parse(resolution[1]);
                var screenWidth = Display.main.systemWidth;
                var screenHeight = Display.main.systemHeight;


                if (width >= screenWidth || height >= screenHeight)
                {
                    var closestWidth = screenWidth;
                    var closestHeight = screenHeight;
                    foreach (var res in Screen.resolutions)
                    {
                        if (res.width < screenWidth && res.height < screenHeight)
                        {
                            closestWidth = res.width;
                            closestHeight = res.height;
                        }
                    }

                    // set to resolution closest to desktop, just one below desktop res
                    SetResolution(closestWidth, closestHeight, mode, 0);
                }
                else
                {
                    ApplySelectedResolution();
                }
            }
            /*
        else if (wasWindowed)
        {
            // reset to native/desktop resolution
            SetResolution(Display.main.systemWidth, Display.main.systemHeight, mode, 0);
        }
        */
            else
            {
                ApplySelectedResolution();
            }

            UpdateDialogAfterEndOfFrame();
        }

        public void OnVSyncChanged()
        {
            if (updatingDialog)
                return;

            QualitySettings.vSyncCount = vSync.value;
            PlayerPrefs.SetInt("vSyncCount", vSync.value);
            UpdateDialogAfterEndOfFrame();
        }

        public void OnQualityLevelChanged()
        {
            if (updatingDialog)
                return;

            var selectedText = quality.options[quality.value].text;
            QualitySettings.SetQualityLevel(new List<string>(QualitySettings.names).IndexOf(selectedText), true);
            QualitySettings.vSyncCount = vSync.value; // reset vsync setting as it may be affected by quality level
            PlayerPrefs.SetString("graphicsSettings", quality.options[quality.value].text);

            UpdateDialogAfterEndOfFrame();
        }

        public void OnMonitorChanged(int index)
        {
            if (updatingDialog)
                return;

            // PB: Implemented display change method possible since Unity 2021.2
            StartCoroutine(MoveToDisplay(index));

        }
        #endregion

        #region Apply Changes
        void ApplySelectedResolution()
        {
            // in case resolution changed, we need to check whether the Hz selection still applies for the new resolution
            // if not we opt to go with the default '0' Hz
            var selectedRes = GetSelectedResolution();
            var availableRefreshRates = refreshRates[selectedRes];
            var selectedHz = "0";
            if (selectedHz.Equals("N/A") || availableRefreshRates.Contains(selectedHz) == false)
                selectedHz = "0";

            var resolution = selectedRes.Split(new char[] { 'x' });
            var width = int.Parse(resolution[0]);
            var height = int.Parse(resolution[1]);
            var hz = int.Parse(selectedHz);
            PlayerPrefs.SetInt("resolutionWidth", width);
            PlayerPrefs.SetInt("resolutionHeight", height);
            PlayerPrefs.SetInt("refreshRate", hz);
            SetResolution(width, height, GetSelectedFullScreenMode(), hz);
        }

        void SetResolution(int width, int height, FullScreenMode mode, int hz)
        {
            // prevent setting resolution multiple times when dialog is updated in the next frame
            //Debug.LogError("DESIRED res: " + GetResolutionString(width, height) + " @ " + hz + " Hz in " + mode);
            Screen.SetResolution(width, height, mode, hz);
            QualitySettings.vSyncCount = vSync.value; // reset vsync setting as it may be affected by resolution
        }
        #endregion

        #region Update After Frame
        void UpdateDialogAfterEndOfFrame()
        {
            StartCoroutine(UpdateDialogAfterEndOfFrameCoroutine());
        }

        IEnumerator UpdateDialogAfterEndOfFrameCoroutine()
        {
            // must wait for end of this AND next frame for the new resolution to be applied
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            //Debug.LogError("NEW Screen: " + Screen.width+ "x" + Screen.height + " (curRes: " + Screen.currentResolution.width + "x" + Screen.currentResolution.height +
            //    ", sysRes: " + Display.main.systemWidth + "x" + Display.main.systemHeight + ") @ " + Screen.currentResolution.refreshRate + " Hz in " + Screen.fullScreenMode);
            PopulateDropdowns();
            ApplyCurrentSettingsToDialog();
            UpdateDialogInteractability();
        }
        #endregion

        #region Move To Display
        private IEnumerator MoveToDisplay(int index)
        {
            movingToDisplay = true;

            try
            {
                var display = displayInfos[index];

                Debug.Log($"Moving window to display{index}: {display.name}");

                Vector2Int targetCoordinates = new Vector2Int(0, 0);
                if (Screen.fullScreenMode != FullScreenMode.Windowed)
                {
                    // Target the center of the display. Doing it this way shows off
                    // that MoveMainWindow snaps the window to the top left corner
                    // of the display when running in fullscreen mode.
                    targetCoordinates.x += display.width / 2;
                    targetCoordinates.y += display.height / 2;
                }

                var moveOperation = Screen.MoveMainWindowTo(display, targetCoordinates);
                yield return moveOperation;
            }
            finally
            {
                UpdateDialogAfterEndOfFrame();
                movingToDisplay = false;
            }
        }
        #endregion
    }
}
