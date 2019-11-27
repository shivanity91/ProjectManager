using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectManager.Common;
using ProjectManager.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ProjectManager.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    //[RoutePrefix("api/Manager/")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ManagerController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// On load of landing page, this action will get called.
        /// </summary>
        /// <returns>From ProjectDetails.json, will get list of existing project details</returns>
        [HttpGet]
        public ManagerModel GetAllProjects()
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var fileData = System.IO.File.ReadAllText(contentRootPath + "/Files/ProjectDetails.json");
            ManagerModel data = JsonConvert.DeserializeObject<ManagerModel>(fileData);
            return data;
        }


        /// <summary>
        /// To create a new project. Project will be created in local app directory folder Files//
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        public void CreateProject(ProjectInfo model)
        {
            try
            {
                string contentRootPath = _hostingEnvironment.ContentRootPath;
                var fileData = System.IO.File.ReadAllText(contentRootPath + "/Files/ProjectDetails.json");
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
                addObject.AddOrUpdateProject(source, model, list, contentRootPath);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {

            }
        }


        /// <summary>
        /// To delete an existing project
        /// </summary>
        /// <param name="model"></param>        
        [HttpDelete]
        public void Delete(ProjectInfo model)
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var fileData = System.IO.File.ReadAllText(contentRootPath + "/Files/ProjectDetails.json");
            var list = JsonConvert.DeserializeObject<ManagerModel>(fileData);
            var itemToRemove = list.data.Single(r => r.ProjectName == model.ProjectName);
            list.data.Remove(itemToRemove);
            var jsonToOutput = JsonConvert.SerializeObject(list, Formatting.Indented);
            var newPath = String.Format("{0}\\Files\\ProjectDetails.json", contentRootPath);
            System.IO.File.WriteAllText(newPath, jsonToOutput.ToString());
        }

        /// <summary>
        /// To update an existing project
        /// </summary>
        /// <param name="model"></param>
        [HttpPut]
        public void Put(ProjectInfo model)
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var fileData = System.IO.File.ReadAllText(contentRootPath + "/Files/ProjectDetails.json");
            //string fileName = "ProjectDetails.json";
            //string path = String.Format("{0}Files\\", AppDomain.CurrentDomain.BaseDirectory);
            //string fileData = System.IO.File.ReadAllText(path + fileName);
            ManagerModel list = JsonConvert.DeserializeObject<ManagerModel>(fileData);
            var source = model.SourcePath.Replace(@"/", @"\\");
            var itemToUpdate = list.data.Single(r => r.ProjectName == model.ProjectName);
            itemToUpdate.ModifiedBy = model.ModifiedBy;
            itemToUpdate.ModifiedOn = DateTime.Now;
            itemToUpdate.SourcePath = model.SourcePath;
            itemToUpdate.Comments = model.Comments;
            CommonClass addObject = new CommonClass();
            addObject.AddOrUpdateProject(source, model, list, contentRootPath);
        }

        /// <summary>
        /// To download the selected project
        /// </summary>
        /// <param name="ProjectName"></param>
        /// <returns></returns>
        [Route("DownloadProject")]
        [HttpGet]
        public string DownloadProject([FromUri]string ProjectName)
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            string fileName = CommonClass.DownloadFiles(ProjectName, contentRootPath);
            return fileName;
        }


    }
}

