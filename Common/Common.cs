using Newtonsoft.Json;
using ProjectManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Common
{
    public class CommonClass
    {
        /// <summary>
        /// 1. To Create or update an existing Project, this method will be called
        /// 2. New project will be created under Folder "Files//" under Solution folder 
        /// 3. ProjectDetails.json will be updated
        /// </summary>
        /// <param name="source"></param>
        /// <param name="model"></param>
        /// <param name="list"></param>
        public void AddOrUpdateProject(string source, ProjectInfo model, ManagerModel list,string contentRootPath)
        {
            string fileName = "ProjectDetails.json";
          //  string contentRootPath = _hostingEnvironment.ContentRootPath;
            string destinationPath = String.Format("{0}\\Files\\", contentRootPath);
            string destinationProjectFolder = String.Format("{0}\\", Path.Combine(destinationPath, model.ProjectName));

            if (!Directory.Exists(destinationPath + model.ProjectName))
                Directory.CreateDirectory(destinationProjectFolder);
            foreach (string dirPath in Directory.GetDirectories(source, "*",
            SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(source, destinationProjectFolder));

            //Copy all the files & Replaces any files with the same name

            //Bug Detected: In case source has existing nested folders then copying files under the Files Folder -- Shivani

            foreach (string newPath in Directory.GetFiles(source, "*.*",
                SearchOption.AllDirectories))
                System.IO.File.Copy(newPath, newPath.Replace(source, destinationProjectFolder), true);

            var jsonToOutput = JsonConvert.SerializeObject(list, Formatting.Indented);
            System.IO.File.WriteAllText(Path.Combine(destinationPath, fileName), jsonToOutput.ToString());
        }

        /// <summary>
        /// This method will be called to download Project folder. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>{ProjectName_Timestamp}.zip will be downloaded as complete project at Solution 'Files//' folder</returns>
        public static string DownloadFiles(string model,string contentRootPath)
        {
            string projectName = model;
            //string contentRootPath = _hostingEnvironment.ContentRootPath;
            string _timestamp = String.Format("{0}{1}{2}_{3}{4}{5}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            string path = String.Format(String.Format("{0}\\Files\\",contentRootPath), _timestamp);
            List<string> outputFileNames = new List<string>();

            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }


            string zipFileName = projectName + _timestamp + ".zip";
            string startPath = String.Format("{0}" + projectName + "\\", path);
            string zipPath = String.Format("{0}\\Files\\{1}", contentRootPath, zipFileName);//URL for your ZIP file

            ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
            return zipFileName;
        }

    }
}
