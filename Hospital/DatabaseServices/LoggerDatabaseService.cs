using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Configs;

namespace Hospital.DatabaseServices
{
    public class LoggerDatabaseService
    {
        private readonly Config _config;

        public LoggerDatabaseService()
        {
            _config = Config.GetInstance();
        }
    }
}
