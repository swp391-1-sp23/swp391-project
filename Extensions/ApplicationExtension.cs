namespace SWP391.Project.Extensions
{
    public static class ApplicationExtension
    {
        public static WebApplication UseMiddlewares(this WebApplication application)
        {
            IWebHostEnvironment environment = application.Environment;

            // if (environment.IsDevelopment())
            // {
            application.UseSwagger();
            application.UseSwaggerUI(setupAction: options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
            // }

            application.UseCors("CorsPolicy");
            application.UseAuthentication();
            application.UseAuthorization();
            application.MapControllers();

            return application;
        }
    }
}