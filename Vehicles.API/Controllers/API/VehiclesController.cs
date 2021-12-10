using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vehicles.API.Data;
using Vehicles.API.Data.Entities;
using Vehicles.API.Helpers;
using Vehicles.API.Models.Requests;

namespace Vehicles.API.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly IUserHelper _userHelper;

        public VehiclesController(DataContext context, IBlobHelper blobHelper, IUserHelper userHelper)
        {
            _context = context;
            _blobHelper = blobHelper;
            _userHelper = userHelper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
            Vehicle vehicle = await _context.Vehicles
                .Include(x => x.VehicleType)
                .Include(x => x.Brand)
                .Include(x => x.VehiclePhotos)
                .Include(x => x.Histories)
                .ThenInclude(x => x.Details)
                .ThenInclude(x => x.Procedure)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return vehicle;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(int id, VehicleRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VehicleType vehicleType = await _context.VehicleTypes.FindAsync(request.VehicleTypeId);
            if (vehicleType == null)
            {
                return BadRequest("El tipo de vehículo no existe.");
            }

            Brand brand = await _context.Brands.FindAsync(request.BrandId);
            if (brand == null)
            {
                return BadRequest("Esa marca no existe.");
            }

            Vehicle vehicle = await _context.Vehicles.FindAsync(request.Id);
            if (vehicle == null)
            {
                return BadRequest("Ese vehículo no existe.");
            }

            vehicle.Brand = brand;
            vehicle.Color = request.Color;
            vehicle.Line = request.Line;
            vehicle.Model = request.Model;
            vehicle.Plaque = request.Plaque;
            vehicle.Remarks = request.Remarks;
            vehicle.VehicleType = vehicleType;
            //_context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                _context.Vehicles.Update(vehicle);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un vehículo con esa placa.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Vehicle>> PostVehicle(VehicleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VehicleType vehicleType = await _context.VehicleTypes.FindAsync(request.VehicleTypeId);
            if (vehicleType == null)
            {
                return BadRequest("El tipo de vehículo no existe.");
            }

            Brand brand = await _context.Brands.FindAsync(request.BrandId);
            if (brand == null)
            {
                return BadRequest("Esa marca no existe.");
            }

            User user = await _userHelper.GetUserAsync(Guid.Parse(request.UserId));
            if (user == null)
            {
                return BadRequest("Ese usuario no existe.");
            }

            Vehicle vehicle = await _context.Vehicles.FirstOrDefaultAsync(x => x.Plaque.ToUpper() == request.Plaque.ToUpper());
            if (vehicle != null)
            {
                return BadRequest("Ya existe un vehículo con esa placa.");
            }

            Guid imageId = Guid.Empty;
            List<VehiclePhoto> vehiclePhotos = new();
            if (request.Image != null && request.Image.Length > 0)
            {
                imageId = await _blobHelper.UploadBlobAsync(request.Image, "vehicles");
                vehiclePhotos.Add(new VehiclePhoto
                {
                    ImageId = imageId
                });
            }

            vehicle = new Vehicle
            {
                Brand = brand,
                Color = request.Color,
                Histories = new List<History>(),
                Line = request.Line,
                Model = request.Model,
                Plaque = request.Plaque,
                Remarks = request.Remarks,
                User = user,
                VehicleType = vehicleType,
                VehiclePhotos = vehiclePhotos
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return Ok(vehicle);
            //return CreatedAtAction("GetVehicle", new { id = vehicle.Id }, vehicle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            Vehicle vehicle = await _context.Vehicles
                .Include(x => x.VehiclePhotos)
                .Include(x => x.Histories)
                .ThenInclude(x => x.Details)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
