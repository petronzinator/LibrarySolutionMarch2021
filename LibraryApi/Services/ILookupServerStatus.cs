using LibraryApi.Controllers;

namespace LibraryApi.Services
{
    public interface ILookupServerStatus
    {
        StatusResponse GetStatusFor();
    }
}