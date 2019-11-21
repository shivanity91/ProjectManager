using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectManager.Common;
using ProjectManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProjectManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {

        [HttpGet]
        public ManagerModel GetAllProjects()
        {
            string fileName = "ProjectDetails.json";
            // string path = String.Format("{0}Files\\", AppDomain.CurrentDomain.BaseDirectory);
            // string[] fileData = Directory.GetFiles(@"Files\", fileName);//
            string fileData = System.IO.File.ReadAllText(@"Files\" + fileName);
            ManagerModel data = JsonConvert.DeserializeObject<ManagerModel>(fileData);
            return data;
        }

        [HttpPost]
        public void CreateProject(ProjectInfo model)
        {
            try
            {
                string fileName = "ProjectDetails.json";
                //string path = String.Format("{0}Files\\", AppDomain.CurrentDomain.BaseDirectory);
                //string fileData = System.IO.File.ReadAllText(path + fileName);
                string fileData = System.IO.File.ReadAllText(@"Files\" + fileName);
                ManagerModel list = JsonConvert.DeserializeObject<ManagerModel>(fileData);
                var source = model.SourcePath.Replace(@"/", @"\\");
                list.data.Add(new ProjectInfo
                {
                    ProjectName = model.ProjectName,
                    CreatedBy = model.CreatedBy,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = model.CreatedBy,
                    ModifiedOn = DateTime.Now,
                    Comments = model.Comments,
                    SourcePath = source
                });

                CommonClass addObject = new CommonClass();
                addObject.AddOrUpdateProject(source, model, list);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {

            }
        }



        [HttpDelete]
        public void Delete(ProjectInfo model)
        {
            string fileName = "ProjectDetails.json";
            //string path = String.Format("{0}Files\\", AppDomain.CurrentDomain.BaseDirectory);
            //string fileData = System.IO.File.ReadAllText(path + fileName);
            string fileData = System.IO.File.ReadAllText(@"Files\" + fileName);
            var list = JsonConvert.DeserializeObject<ManagerModel>(fileData);
            var itemToRemove = list.data.Single(r => r.ProjectName == model.ProjectName);
            list.data.Remove(itemToRemove);
            var jsonToOutput = JsonConvert.SerializeObject(list, Formatting.Indented);
            System.IO.File.WriteAllText(Path.Combine(@"Files\", fileName), jsonToOutput.ToString());
        }

        [HttpPut]
        public void Put(ProjectInfo model)
        {
            string fileName = "ProjectDetails.json";
            //string path = String.Format("{0}Files\\", AppDomain.CurrentDomain.BaseDirectory);
            //string fileData = System.IO.File.ReadAllText(path + fileName);
            string fileData = System.IO.File.ReadAllText(@"Files\" + fileName);
            ManagerModel list = JsonConvert.DeserializeObject<ManagerModel>(fileData);
            var source = model.SourcePath.Replace(@"/", @"\\");
            var itemToUpdate = list.data.Single(r => r.ProjectName == model.ProjectName);
            itemToUpdate.ModifiedBy = model.ModifiedBy;
            itemToUpdate.ModifiedOn = DateTime.Now;
            itemToUpdate.SourcePath = model.SourcePath;
            CommonClass addObject = new CommonClass();
            addObject.AddOrUpdateProject(source, model, list);
        }

    }
}
