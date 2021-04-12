using System;

namespace Dummy
{
    [Route("dummy")]
    public class Dummy
    {
        
    }

    internal class RouteAttribute : Attribute
    {
        private readonly string _dummy;

        public RouteAttribute(string dummy)
        {
            _dummy = dummy;
        }
    }
}