using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class ClientInfoService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ClientInfoService> _logger;

    public ClientInfoService(IHttpContextAccessor httpContextAccessor, ILogger<ClientInfoService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public string GetClientIpAddress()
    {
        var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress;
        return ip?.ToString()?.Replace("::1", "127.0.0.1");
    }
    public string GetHostNameAsync()
    {
        var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress;
        try
        {
            //var hostEntry = Dns.GetHostEntry(ip);
            //return hostEntry.HostName;
            return "";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving host name for IP: {Ip}", ip);
            return string.Empty;
        }
    }

    public string GetWindowsSessionName()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }
}