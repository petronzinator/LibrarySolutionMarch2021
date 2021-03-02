using LibraryApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public class WillsHealthCheckServerStatus : ILookupServerStatus
    {
        public StatusResponse GetStatusFor()
        {
            return new StatusResponse
            {
                Message = "Everything is going great. Thanks for asking!",
                LastChecked = DateTime.Now
            };
        }
    }
}
