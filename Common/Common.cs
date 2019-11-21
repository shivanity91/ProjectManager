using Newtonsoft.Json;
using ProjectManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Common
{
    public class CommonClass
    {
        public void AddOrUpdateProject(string source, ProjectInfo model, ManagerModel list)
        {
            string fileName = "ProjectDetails.json";
            string destinationPath = @"Files\";// String.Format("{0}Files\\", AppDomain.CurrentDomain.BaseDirectory);


            string destinationProjectFolder = Path.Combine(destinationPath, model.ProjectName);
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
    }
}
