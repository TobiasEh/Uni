﻿using Sopro.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.PersistenceController
{
    public interface IScenarioService : IScenarioRepository
    {
        public List<IScenario> import(string path);

        public void export(List<IScenario> list, string path);
    }
}
