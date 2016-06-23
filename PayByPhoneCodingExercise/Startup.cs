using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PayByPhoneCodingExercise.Startup))]
namespace PayByPhoneCodingExercise
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
