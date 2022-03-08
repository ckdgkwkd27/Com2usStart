using CloudStructures.Structures;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace com2us_start.Controllers;

[Route("[controller]")]
[ApiController]
public class PlayerCreateController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IConfiguration _conf;
    private readonly IRealDbConnector _realDbConnector;
    private readonly string _playerId;

    public PlayerCreateController(ILogger<PlayerCreateController> logger, IConfiguration conf, IRealDbConnector realDbConnector)
    {
        _logger = logger;
        _conf = conf;
        _realDbConnector = realDbConnector;
        //_playerId = Guid.NewGuid().ToString();
    }

    [HttpPost]
    public async Task<PlayerCreateResponse> Post(PlayerCreateRequest request)
    {
        var response = new PlayerCreateResponse() { Result = ErrorCode.None };
        
        try
        {
            var memberInfo = await _realDbConnector.SelectMember(request.ID);
            if (memberInfo == null)
            {
                response.Result = ErrorCode.Player_Fail_NotUser;
                return response;
            }

            //Player Data 생성
            var playerInsertCount = await _realDbConnector.InsertPlayer(request.ID,1, 0, gameMoney: 0);
            if (playerInsertCount != 1)
            {
                _logger.ZLogError("ERROR: Player Create Failed!");
                response.Result = ErrorCode.Player_Fail_Insertion;
                return response;
            }
        }
        catch (Exception ex)
        {
            _logger.ZLogError(ex.ToString());
            response.Result = ErrorCode.Player_Fail_Exception;
            return response;
        }
        
        _logger.ZLogInformation($"Character Create Success!! {_playerId}");
        return response;
    }
}

public class PlayerCreateRequest
{
    public string ID { get; set; }
    public string AuthToken { get; set; }
}

public class PlayerCreateResponse
{
    public PlayerCreateResponse() { Result = ErrorCode.None;}
    public ErrorCode Result { get; set; }
}