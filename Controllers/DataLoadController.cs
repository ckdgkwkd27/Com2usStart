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

    public DataLoadController(ILogger<DataLoadController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<LoadResponse> Post(LoadRequest request)
    {
        var response = new LoadResponse() { Result = ErrorCode.None };

        try
        {
            //DB에서 체크
            var playerInfo = await MysqlManager.SelectGamePlayerQuery(request.UUID);
            if (null == playerInfo)
            {
                response.Result = ErrorCode.Load_Fail_NotUser;
                return response;
            }
            
            _logger.ZLogInformation($"Player Data Load Completed!!"); 
            _logger.ZLogInformation($"UUID: {playerInfo.UUID}");
            _logger.ZLogInformation($"ID: {playerInfo.ID}");
            _logger.ZLogInformation($"Level: {playerInfo.Level}");
            _logger.ZLogInformation($"Exp: {playerInfo.Exp}");
            _logger.ZLogInformation($"GameMoney: {playerInfo.GameMoney}");

            response.Player = playerInfo;
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
    public string UUID { get; set; }
    public string AuthToken { get; set; }
}

public class LoadResponse
{
    public ErrorCode Result { get; set; }
    public GamePlayer? Player { get; set; }
}


