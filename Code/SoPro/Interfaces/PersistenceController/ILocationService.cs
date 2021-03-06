﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sopro.ViewModels;
using System.Collections.Generic;

namespace Sopro.Interfaces.PersistenceController
{
    public interface ILocationService
    {
        public List<LocationExportImportViewModel> import(IFormFile file);

        public FileContentResult export(List<LocationExportImportViewModel> list);
    }
}
