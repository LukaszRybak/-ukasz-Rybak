namespace PrescriptionHandler.Middlewares
{
    public class ErrorHandlerMiddleware
    {

        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            { 
            await _next(httpContext);
            }
            catch (Exception exc)
            {

                using (StreamWriter sw = File.AppendText(Directory.GetCurrentDirectory() + "\\log.txt"))
                {
                    DateTime currentDate = DateTime.Now;
                    string dateString = currentDate.ToString("MM/dd/yyyy HH:mm:ss");
                    sw.WriteLine($"[{dateString}] {exc.Message}");
                }

                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync(exc.Message);
            }
        }

    }
}
