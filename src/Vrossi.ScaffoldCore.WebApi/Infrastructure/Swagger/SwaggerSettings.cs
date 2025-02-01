namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Swagger
{
    public class SwaggerSettings
    {
        public bool Enabled { get; set; }
        public static SwaggerSettings Default = new();
        public List<SwaggerVersionOption> Versions { get; set; } = new List<SwaggerVersionOption>();
    }

    public class SwaggerVersionOption
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Endpoint { get; set; }
        public SwaggerVersionContactOption Contact { get; set; }
    }

    public class SwaggerVersionContactOption
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
