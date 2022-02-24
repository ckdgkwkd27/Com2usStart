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
        var response = new LoadResponse() { Result = ErrorCode.None };

        try
        {
            //DB에서 체크
            using MysqlManager manager = new MysqlManager(_conf, _realDbConnector);
            await manager.GetDbConnection();
            
            var playerInfo = await manager.SelectGamePlayerQuery(request.UUID);
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


