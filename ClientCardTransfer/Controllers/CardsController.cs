using ClientCardTransfer.Models;
using ClientCardTransfer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ClientCardTransfer.Controllers
{
    // Контроллер Web API для работы с картами
    [ApiController]
    [Route("api/cards")]
    public class CardsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CardsController> _logger;
        public CardsController(IUnitOfWork unitOfWork,ILogger<CardsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCards()
        {
            try
            {
                var cards = await _unitOfWork.CardRepository.GetAll();
                return Ok(cards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                // Обработка ошибки
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCard(int id)
        {
            try
            {
                var card = await _unitOfWork.CardRepository.GetById(id);
                if (card == null)
                {
                    return NotFound();
                }
                return Ok(card);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                // Обработка ошибки
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCard(Card card)
        {
            try
            {
                _unitOfWork.CardRepository.Add(card);
                await _unitOfWork.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCard), new { id = card.Id }, card);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                // Обработка ошибки
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCard(int id, Card card)
        {
            try
            {
                if (id != card.Id)
                {
                    return BadRequest();
                }
                _unitOfWork.CardRepository.Update(card);
                await _unitOfWork.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                // Обработка ошибки
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(int id)
        {
            try
            {
                var card = await _unitOfWork.CardRepository.GetById(id);
                if (card == null)
                {
                    return NotFound();
                }
                _unitOfWork.CardRepository.Delete(card);
                await _unitOfWork.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                // Обработка ошибки
                return StatusCode(500, "Internal Server Error");
            }
        }
    }

}
