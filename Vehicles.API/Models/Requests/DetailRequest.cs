﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vehicles.API.Models.Requests
{
    public class DetailRequest
    {
        public int Id { get; set; }

        [Display(Name = "Historia")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int HistoryId { get; set; }

        [Display(Name = "Procedimiento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ProcedureId { get; set; }

        [Display(Name = "Precio Mano de Obra")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal LaborPrice { get; set; }

        [Display(Name = "Precio Repuestos")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal SparePartsPrice { get; set; }

        [Display(Name = "Observación")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
    }
}
