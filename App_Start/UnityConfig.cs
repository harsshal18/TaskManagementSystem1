using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using Unity.Lifetime;

using TaskManagementSystem1.Repositories;


namespace TaskManagementSystem1
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();        
            container.RegisterType<ITaskRepository, TaskRepository>();


            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}