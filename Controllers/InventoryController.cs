using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace com2us_start.Controllers;

[Route("[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IConfiguration _conf;
    private List<InventoryItem> _inventoryItems;

    public InventoryController(ILogger<InventoryController> logger)
    {
        _logger = logger;
        _inventoryItems = new List<InventoryItem>();
    }

    [HttpPost]
    public async Task<InventoryResponse> Post(InventoryRequest request)
    {
        var response = new InventoryResponse() { Result = ErrorCode.None };

        try
        {
            var invenList = await MysqlManager.SelectMultipleInventoryQuery(request.UUID);
            if (invenList.Count == 0)
            {
                _logger.ZLogError("Inventory is Empty!");
                response.Result = ErrorCode.Inventory_Fail_Empty;
                return response;
            }
            
            //item, Inventory 테이블간 Join
            var itemList = await MysqlManager.SelectAllInventoryItems(request.UUID);
            response.ItemList = itemList;
            return response;
        }
        catch (Exception ex)
        {
            _logger.ZLogError(ex.ToString());
            response.Result = ErrorCode.Inventory_Fail_Exception;
            return response;
        }
    }
}

public class InventoryRequest
{
    public string ID { get; set; }
    public string UUID { get; set; }
    public string AuthToken { get; set; }
}

public class InventoryResponse
{
    public ErrorCode Result { get; set; }
    public List<InventoryItem> ItemList { get; set; }
}
