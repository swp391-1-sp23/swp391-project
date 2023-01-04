namespace SWP391.Project.Extensions
{
    public static class ApplicationExtension
    {
        public static WebApplication UseMiddlewares(this WebApplication application)
        {
            IWebHostEnvironment environment = application.Environment;

            if (environment.IsDevelopment())
            {
                _ = application.UseSwagger();
                _ = application.UseSwaggerUI(setupAction: setupAction =>
                {
                    setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "SWP391 API");
                    setupAction.RoutePrefix = string.Empty;
                });
            }

            _ = application.UseCors("CorsPolicy");
            _ = application.UseAuthentication();
            _ = application.UseAuthorization();
            _ = application.MapControllers();

            return application;
        }
    }
}