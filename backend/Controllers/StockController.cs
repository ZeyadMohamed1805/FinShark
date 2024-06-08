using backend.Data;
using backend.Dtos.Stock;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public StockController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _dbContext.Stocks.ToListAsync();
            var stocksDto = stocks.Select(stock => stock.ToStockDto());
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stock = await _dbContext.Stocks.FindAsync(id);
            if (stock == null)
                return NotFound();
            return Ok(stock.ToStockDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            await _dbContext.Stocks.AddAsync(stockModel);
            _dbContext.SaveChanges();
            return Ok(stockModel);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await _dbContext.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);
            if (stockModel == null)
                return NotFound();

            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            _dbContext.SaveChanges();

            return Ok(stockModel);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stock = _dbContext.Stocks.Find(id);
            if (stock == null)
                return NotFound();

            _dbContext.Stocks.Remove(stock);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}