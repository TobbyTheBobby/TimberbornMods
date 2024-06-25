using System.IO;
using System.Linq;
using ThunderKit.Core.Data;
using ThunderKit.Core.UIElements;
using ThunderKit.Markdown;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimberbornThunderkitExtension
{
    public class ModIoConfiguration : ThunderKitSetting
    {
        [SerializeField]
        public string ApiKeyFilePath;
        [SerializeField]
        public string AuthTokenFilePath;
        [SerializeField] 
        public int GameId;
        
        private SerializedObject _thunderKitSettingsSo;
        private MarkdownElement _markdown;

        public string ApiKey => File.ReadAllText(ApiKeyFilePath);
        
        public string AuthToken => File.ReadAllText(AuthTokenFilePath);

        public bool ConfiguredCorrectly()
        {
            return !string.IsNullOrEmpty(ApiKeyFilePath) && File.Exists(ApiKeyFilePath) && !string.IsNullOrEmpty(AuthTokenFilePath) && File.Exists(AuthTokenFilePath) && GameId > 0;
        }
        
        private MarkdownElement Markdown
        {
            get
            {
                if (_markdown != null) 
                    return _markdown;
                _markdown = new MarkdownElement
                {
                    Data = File.Exists("Assets/Plugins/TimberbornThunderkitExtension/ThunderkitModioSettingsWarning.md") ? "Assets/Plugins/TimberbornThunderkitExtension/ThunderkitModioSettingsWarning.md" : GetPath("ThunderkitModioSettingsWarning.md"),
                    MarkdownDataType = MarkdownDataType.Source
                };

                _markdown.AddToClassList("m4");
                _markdown.RefreshContent();
                _markdown.ToggleDisplayStyle(true);

                return _markdown;
            }
        }

        public override void CreateSettingsUI(VisualElement rootElement)
        {
            rootElement.Add(Markdown);
            UpdateMarkdown();
            
            var settingsElement = 
                TemplateHelpers.LoadTemplateInstance("Assets/Plugins/TimberbornThunderkitExtension/ModIoSettings.UXML") ??
                TemplateHelpers.LoadTemplateInstance(GetPath("Editor/ModIoSettings.UXML"));

            // settingsElement.AddEnvironmentAwareSheets(Constants.ThunderKitSettingsTemplatePath);
            rootElement.Add(settingsElement);
            
            var apiKeyBrowseButton = settingsElement.Q<Button>("apikey-browse-button");
            apiKeyBrowseButton.clickable.clicked -= () => BrowseForFile(ref ApiKeyFilePath);
            apiKeyBrowseButton.clickable.clicked += () => BrowseForFile(ref ApiKeyFilePath);
            
            var authTokenBrowseButton = settingsElement.Q<Button>("authtoken-browse-button");
            authTokenBrowseButton.clickable.clicked -= () => BrowseForFile(ref AuthTokenFilePath);
            authTokenBrowseButton.clickable.clicked += () => BrowseForFile(ref AuthTokenFilePath);
            
            var textFields = settingsElement.Query<TextField>("asset-name-field").ToList();
            foreach (var textField in textFields)
            {
                textField.UnregisterValueChangedCallback(_ => UpdateMarkdown());
                textField.RegisterValueChangedCallback(_ => UpdateMarkdown());
            }
            
            var integerFields = settingsElement.Query<IntegerField>("asset-integer-field").ToList();
            foreach (var integerField in integerFields)
            {
                integerField.UnregisterValueChangedCallback(_ => UpdateMarkdown());
                integerField.RegisterValueChangedCallback(_ => UpdateMarkdown());
            }

            _thunderKitSettingsSo ??= new SerializedObject(this);

            rootElement.Bind(_thunderKitSettingsSo);
        }

        private void UpdateMarkdown()
        {
            Markdown.ToggleDisplayStyle(!ConfiguredCorrectly());
        }
        
        private void BrowseForFile(ref string value)
        {
            LocateFile(ref value);
            UpdateMarkdown();
        }

        private void LocateFile(ref string value)
        {
            var foundExecutable = false;

            while (!foundExecutable)
            {
                var path = string.IsNullOrEmpty(value) ? Directory.GetCurrentDirectory() : value;
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        path = EditorUtility.OpenFilePanel("Open Game Executable", path, "txt");
                        break;
                    case RuntimePlatform.LinuxEditor:
                        path = EditorUtility.OpenFilePanel("Open Game Executable", path, "");
                        break;
                    default:
                        EditorUtility.DisplayDialog("Unsupported", "Your operating system is partially or completely unsupported. Contributions to improve this are welcome", "Ok");
                        return;
                }
                if (string.IsNullOrEmpty(path)) return;
                //For Linux, we will have to check the selected file to see if the GameExecutable_Data folder exists, we can use this to verify the executable was selected
                value = path;
                foundExecutable = Directory.GetFiles(Path.GetDirectoryName(path), Path.GetFileName(path)).Any();
            }
            EditorUtility.SetDirty(this);
        }

        private string GetPath(string packageFile)
        {
            return $"Packages/ModIoUpload/{packageFile}";
        }
    }
}