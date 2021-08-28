using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vehicles.API.Data.Entities;

namespace Vehicles.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            this._context = context;
        }

        public async Task SeedAsync()
        {
            // asegurarse que la bd exista
            await _context.Database.EnsureCreatedAsync();
            // garantizar que existan los tipos de vehículos
            await CheckVehicleTypeAsync();
            // garantizar que existan las marcas
            await CheckBrandsAsync();
            await CheckDocumentTypesAsync();
            await CheckProceduresAsync();
        }

        private async Task CheckProceduresAsync()
        {
            if (!_context.Procedures.Any())
            {
                if (!_context.Procedures.Any())
                {
                    _context.Procedures.Add(new Procedure { Price = 500, Description = "Alineación" });
                    _context.Procedures.Add(new Procedure { Price = 10000, Description = "Lubricación de suspención delantera" });
                    _context.Procedures.Add(new Procedure { Price = 10000, Description = "Lubricación de suspención trasera" });
                    _context.Procedures.Add(new Procedure { Price = 10000, Description = "Frenos delanteros" });
                    _context.Procedures.Add(new Procedure { Price = 10000, Description = "Frenos traseros" });
                    _context.Procedures.Add(new Procedure { Price = 50, Description = "Líquido frenos delanteros" });
                    _context.Procedures.Add(new Procedure { Price = 10000, Description = "Líquido frenos traseros" });
                    _context.Procedures.Add(new Procedure { Price = 10000, Description = "Calibración de válvulas" });
                    _context.Procedures.Add(new Procedure { Price = 500, Description = "Alineación carburador" });
                    _context.Procedures.Add(new Procedure { Price = 10000, Description = "Aceite motor" });
                    _context.Procedures.Add(new Procedure { Price = 1000, Description = "Aceite caja" });
                    _context.Procedures.Add(new Procedure { Price = 250, Description = "Filtro de aire" });
                    _context.Procedures.Add(new Procedure { Price = 10000, Description = "Sistema eléctrico" });
                    _context.Procedures.Add(new Procedure { Price = 800, Description = "Guayas" });
                    _context.Procedures.Add(new Procedure { Price = 10000, Description = "Cambio llanta delantera" });
                    _context.Procedures.Add(new Procedure { Price = 360, Description = "Cambio llanta trasera" });
                    _context.Procedures.Add(new Procedure { Price = 10000, Description = "Reparación de motor" });
                    _context.Procedures.Add(new Procedure { Price = 450, Description = "Kit arrastre" });
                    _context.Procedures.Add(new Procedure { Price = 10000, Description = "Banda transmisión" });
                    _context.Procedures.Add(new Procedure { Price = 10000, Description = "Cambio batería" });
                    _context.Procedures.Add(new Procedure { Price = 150, Description = "Lavado sistema de inyección" });
                    _context.Procedures.Add(new Procedure { Price = 10000, Description = "Lavada de tanque" });
                    _context.Procedures.Add(new Procedure { Price = 600, Description = "Cambio de bujia" });
                    _context.Procedures.Add(new Procedure { Price = 900, Description = "Cambio rodamiento delantero" });
                    _context.Procedures.Add(new Procedure { Price = 10000, Description = "Cambio rodamiento trasero" });
                    _context.Procedures.Add(new Procedure { Price = 10000, Description = "Accesorios" });
                    await _context.SaveChangesAsync();
                }
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckDocumentTypesAsync()
        {
            if (!_context.DocumentTypes.Any())
            {
                _context.DocumentTypes.Add(new DocumentType { Description = "DPI" });
                _context.DocumentTypes.Add(new DocumentType { Description = "NIT" });
                _context.DocumentTypes.Add(new DocumentType { Description = "Pasaporte" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckBrandsAsync()
        {
            if (!_context.Brands.Any())
            {
                _context.Brands.Add(new Brand { Description = "Toyota" });
                _context.Brands.Add(new Brand { Description = "Mazda" });
                _context.Brands.Add(new Brand { Description = "Suzuki" });
                _context.Brands.Add(new Brand { Description = "Honda" });
                _context.Brands.Add(new Brand { Description = "Ducati" });
                _context.Brands.Add(new Brand { Description = "Renault" });
                _context.Brands.Add(new Brand { Description = "BMW" });
                _context.Brands.Add(new Brand { Description = "Yamaha" });
                _context.Brands.Add(new Brand { Description = "Ford" });
                _context.Brands.Add(new Brand { Description = "Chevrolet" });
                _context.Brands.Add(new Brand { Description = "Harley Davidson" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckVehicleTypeAsync()
        {
            if (!_context.VehicleTypes.Any())
            {
                _context.VehicleTypes.Add(new VehicleType { Description = "Carro" });
                _context.VehicleTypes.Add(new VehicleType { Description = "Moto" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
