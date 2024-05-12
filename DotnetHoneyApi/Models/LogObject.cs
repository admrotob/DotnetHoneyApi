using Microsoft.Extensions.Primitives;

namespace DotnetHoneyApi.Models;

public class LogObject
{
    public DateTime RequestTime { get; set; }
    public string RequestHost { get; set; }
    public string RequestPath { get; set; }
    public string RequestMethod {  get; set; }
    public KeyValuePair<string, StringValues>[] RequestHeaders { get; set; }
    public string RequestBody { get; set; }
}
