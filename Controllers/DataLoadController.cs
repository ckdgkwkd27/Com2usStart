using CloudStructures.Structures;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace com2us_start.Controllers;

[Route("[controller]")]
[ApiController]
public class DataLoadController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IConfiguration _conf;
    private readonly IRealDbConnector _realDbConnector;

    public DataLoadController(ILogger<DataLoadController> logger, IConfiguration conf, IRealDbConnector realDbConnector)
    {
        _logger = logger;
        _conf = conf;
        _realDbConnector = realDbConnector;
    }

    [HttpPost]
    public async Task<LoadResponse> Post(LoadRequest request)
    {
        var response = new LoadResponse();

        try
        {
            //DB에서 체크
            var playerInfo = await _realDbConnector.SelectGamePlayer(request.PlayerID);
            if (playerInfo == null)
            {
                response.Result = ErrorCode.Load_Fail_Wrong_PlayerID;
                return response;
            }
            
            _logger.ZLogDebug($"Player Data Load Completed!!"); 
            _logger.ZLogDebug($"PlayerID: {playerInfo.PlayerID}");
            _logger.ZLogDebug($"ID: {playerInfo.ID}");
            _logger.ZLogDebug($"Level: {playerInfo.Level}");
            _logger.ZLogDebug($"Exp: {playerInfo.Exp}");
            _logger.ZLogDebug($"GameMoney: {playerInfo.GameMoney}");
        }
        catch (Exception ex)
        {
            _logger.ZLogError(ex.ToString());
            response.Result = ErrorCode.Load_Fail_Exception;
            return response;
        }

        return response;
    }
}

public class LoadRequest
{
    public string ID { get; set; }
    public string PlayerID { get; set; }
    public string AuthToken { get; set; }
}

public class LoadResponse
{
    public LoadResponse() { Result = ErrorCode.None;}

    public ErrorCode Result { get; set; }
}


