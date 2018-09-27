
namespace SchemaPorter
{

    public class Settings_t
    {
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string WebRootPath { get; set; }
        public string ContentRootPath { get; set; }
        public string ProjectRootPath { get; set; }

        public Microsoft.Extensions.FileProviders.IFileProvider WebRootFileProvider { get; set; }
        public Microsoft.Extensions.FileProviders.IFileProvider ContentRootFileProvider { get; set; }
        public Microsoft.Extensions.FileProviders.IFileProvider ProjectRootFileProvider { get; set; }


        public Settings_t()
        {
            this.EnvironmentName = "Production";
            ApplicationName = typeof(Program).Assembly.GetName().Name;
            this.ContentRootPath = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);

            this.ProjectRootPath = this.ContentRootPath;
            this.ProjectRootPath = System.IO.Path.Combine(this.ProjectRootPath, "..", "..", "..");
            this.ProjectRootPath = System.IO.Path.GetFullPath(this.ProjectRootPath);


            this.WebRootPath = this.ProjectRootPath;

            this.ContentRootFileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
               this.ContentRootPath
            );

            this.ProjectRootFileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
               this.ProjectRootPath
            );

            this.WebRootFileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
               this.WebRootPath
            );


        }
    }

}
