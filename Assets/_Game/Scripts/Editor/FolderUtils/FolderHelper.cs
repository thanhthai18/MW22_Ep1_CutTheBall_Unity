#if UNITY_EDITOR

using UnityEditor;

namespace GameEditor.FolderUtils
{
    public static class FolderHelper
    {
        #region Class Methods

        public static void CreateFolder(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                string[] folders = path.Split('/');
                string tempPath = "";
                for (int i = 0; i < folders.Length - 1; i++)
                {
                    tempPath += folders[i];
                    if (!AssetDatabase.IsValidFolder(tempPath + "/" + folders[i + 1]))
                    {
                        AssetDatabase.CreateFolder(tempPath, folders[i + 1]);
                        AssetDatabase.Refresh();
                    }
                    tempPath += "/";
                }
            }
        }

        public static string FindFolder(string folderName, string parentFolder)
        {
            string result = null;
            var folders = AssetDatabase.GetSubFolders("Assets");
            foreach (var folder in folders)
            {
                result = Recursive(folder, folderName, parentFolder);
                if (result != null)
                    return result;
            }
            return result;
        }

        private static string Recursive(string currentFolder, string folderToSearch, string parentFolder)
        {
            if (currentFolder.EndsWith($"{parentFolder}/{folderToSearch}"))
                return currentFolder;

            var folders = AssetDatabase.GetSubFolders(currentFolder);
            foreach (var fld in folders)
            {
                string result = Recursive(fld, folderToSearch, parentFolder);
                if (result != null)
                    return result;
            }

            return null;
        }

        #endregion Class Methods
    }
}

#endif