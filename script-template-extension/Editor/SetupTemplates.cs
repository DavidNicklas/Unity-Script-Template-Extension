#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace ENSO.ScriptTemplateExtensions.Editor
{
    public class SetupTemplates
    {
        [MenuItem("ScriptExtensions/Setup Script Templates")]
        public static void SetupScriptTemplates()
        {
            string sourcePath = GetPackageScriptTemplatesPath("com.entertainment-software.script-template-extension");
            if (!string.IsNullOrEmpty(sourcePath) && Directory.Exists(sourcePath))
            {
                Debug.Log($"ScriptTemplates found at: {sourcePath}");
            }
            else
            {
                Debug.Log($"ScriptTemplates not found at: {sourcePath}");
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

        public static string GetPackageScriptTemplatesPath(string packageName)
        {
            // Request information about the installed packages
            ListRequest request = Client.List(true); // true includes local paths
            while (!request.IsCompleted)
            {
                System.Threading.Thread.Sleep(100); // Wait until the request is completed
            }

            if (request.Status == StatusCode.Success)
            {
                foreach (var package in request.Result)
                {
                    if (package.name == packageName)
                    {
                        // Return the path to the package's ScriptTemplates folder
                        string packagePath = package.resolvedPath;
                        return Path.Combine(packagePath, "ScriptTemplates");
                    }
                }
            }
            else
            {
                Debug.LogError($"Failed to retrieve package list: {request.Error.message}");
            }

            return null; // Package not found
        }
    }
}

#endif