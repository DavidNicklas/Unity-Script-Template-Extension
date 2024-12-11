#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace ENSO.ScriptTemplateExtensions.Editor
{
    public class SetupTemplates
    {
        [MenuItem("ScriptExtensions/Setup Script Templates")]
        public static void SetupScriptTemplates()
        {
            string sourcePath = Path.Combine(Application.dataPath, "../Packages/script-template-extension/ScriptTemplates");
            if (!Directory.Exists(sourcePath))
            {
                Debug.LogWarning("ScriptTemplates directory not found. Make sure the package is installed correctly.");
                return;
            }

            string destinationPath = Path.Combine(Application.dataPath, "ScriptTemplates");

            if (!Directory.Exists(destinationPath))
            {
                Debug.Log("Setting up [ScriptTemplates] directory ...");

                // Copy the templates
                Directory.CreateDirectory(destinationPath);
                foreach (var file in Directory.GetFiles(sourcePath, "*.cs.txt"))
                {
                    string fileName = Path.GetFileName(file);
                    string destFile = Path.Combine(destinationPath, fileName);
                    File.Copy(file, destFile, true);
                }

                AssetDatabase.Refresh();
                Debug.Log("ScriptTemplates successfully loaded.");
                Debug.LogWarning("Restart the Editor in order to see the templates in the [Create] menu.");
            }
            else
            {
                Debug.LogWarning("[ScriptTemplates] directory already exist.");
            }
        }
    }
}

#endif