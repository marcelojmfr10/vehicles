using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class VehiclePhotoesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;

        public VehiclePhotoesController(DataContext context, IBlobHelper blobHelper)
        {
            _context = context;
            _blobHelper = blobHelper;
        }

        [HttpPost]
        public async Task<ActionResult<VehiclePhoto>> PostVehiclePhoto(VehiclePhotoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            Vehicle vehicle = await _context.Vehicles.FindAsync(request.VehicleId);
            if (vehicle == null)
            {
                return BadRequest("Vehículo no existe.");
            }

            Guid imageId = await _blobHelper.UploadBlobAsync(request.Image, "vehicles");
            //if (request.Image != null && request.Image.Length > 0)
            //{
            //    imageId = await _blobHelper.UploadBlobAsync(request.Image, "vehicles");
            //}

            VehiclePhoto vehiclePhoto = new()
            {
                ImageId = imageId,
                Vehicle = vehicle,
            };

            //List<VehiclePhoto> noimages = await _context.VehiclePhotos.Where(x => x.ImageId == Guid.Empty).ToListAsync();
            _context.VehiclePhotos.Add(vehiclePhoto);
            //_context.VehiclePhotos.RemoveRange(noimages);
            await _context.SaveChangesAsync();
            return Ok(vehiclePhoto);
            //return CreatedAtAction("GetVehiclePhoto", new { id = vehiclePhoto.Id }, vehiclePhoto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehiclePhoto(int id)
        {
            VehiclePhoto vehiclePhoto = await _context.VehiclePhotos.FindAsync(id);
            if (vehiclePhoto == null)
            {
                return NotFound();
            }

            await _blobHelper.DeleteBlobAsync(vehiclePhoto.ImageId, "vehicles");

            _context.VehiclePhotos.Remove(vehiclePhoto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
