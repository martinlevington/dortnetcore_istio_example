using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace docker_helloWorld.Models
{
    public class ReadyHealthCheck : IHealthCheck
    {

        public static readonly string HealthCheckName = "ready_check";

        private  Task _task;



        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {

            _task = Task.Delay(2 * 1000);
            await _task;

            if (_task.IsCompleted)
            {
                return await Task.FromResult(HealthCheckResult.Healthy("Application is ready"));
            }

            return await Task.FromResult(new HealthCheckResult(
                status: context.Registration.FailureStatus,
                description: "Application is still initializing"));
        }

       
    }
}
