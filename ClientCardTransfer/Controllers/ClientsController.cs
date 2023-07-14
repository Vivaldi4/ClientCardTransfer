using ClientCardTransfer.Models;
using ClientCardTransfer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ClientCardTransfer.Controllers
{
    // Контроллер Web API для работы с клиентами
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(IUnitOfWork unitOfWork, ILogger<ClientsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            try
            {
                var clients = await _unitOfWork.ClientRepository.GetAll();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                // Обработка ошибки
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
        {
            try
            {
                var client = await _unitOfWork.ClientRepository.GetById(id);
                if (client == null)
                {
                    return NotFound();
                }
                return Ok(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                // Обработка ошибки
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(Client client)
        {
            try
            {
                _unitOfWork.ClientRepository.Add(client);
                await _unitOfWork.SaveChangesAsync();
                return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                // Обработка ошибки
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, Client client)
        {
            try
            {
                if (id != client.Id)
                {
                    return BadRequest();
                }
                _unitOfWork.ClientRepository.Update(client);
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
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                var client = await _unitOfWork.ClientRepository.GetById(id);
                if (client == null)
                {
                    return NotFound();
                }
                _unitOfWork.ClientRepository.Delete(client);
                await _unitOfWork.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                // Запись информации об исключении в журнал
                _logger.LogError(ex, "An error occurred while processing the request.");
                // Отправка уведомления разработчикам
                //emailService.SendEmail("developers@example.com", "Error Notification", ex.ToString());
                // Обработка ошибки
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}